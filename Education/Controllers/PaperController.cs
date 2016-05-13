using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Education.Models;
using PagedList;
using Education.Abstract;
using Education.Concrete;
using System.Threading.Tasks;
using Education.ViewModels;
using MvcContrib.Filters;
using System.Data.Entity;

namespace Education.Controllers
{
    public class PaperController : BaseController
    {
        private const int PageSize = 10;
        private IEntityRepository<Paper> repository;
        public PaperController(IEntityRepository<Paper> repository)
        {
            this.repository = repository;
        }

        public ActionResult List(int page = 1)
        {
            return View(DB.Papers.OrderByDescending(p => p.EditOn).ToPagedList(page, PageSize));
        }

        [ModelStateToTempData]
        public ActionResult Create()
        {
            if (TempData["LastPostModel"] == null)
            {
                var model = new PaperViewModel();
                return View(model);
            }
            return View(TempData["LastPostModel"] as PaperViewModel);
        }

        [HttpPost]
        [ModelStateToTempData]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PaperViewModel paperInfo)
        {
            foreach (var item in paperInfo.SingleQuestions)
            {
                for (int i = 0; i < 4; i++)
                {
                    item.Options[i].OptionId = (OptionType)(i + 1);
                }
            }
            foreach (var item in paperInfo.MultipleQuestions)
            {
                for (int i = 0; i < item.Options.Count; i++)
                {
                    item.Options[i].OptionId = (OptionType)(i + 1);
                }
            }
            if (ModelState.IsValid)
            {
                //var user = await GetCurrentUserAsync() as Teacher;
                //var teacher = DB.Users.FirstOrDefault(t => t.Id == user.Id) as Teacher;
                var teacher = GetCurrentUser() as Teacher;
                try
                {
                    Paper paper = new Paper();
                    paper.EditOn = DateTime.Now;
                    paper.Teacher = teacher;
                    var trueOrFalseQuestionList = new List<TrueOrFalseQuestion>();
                    foreach (TrueOrFalseQuestionViewModel questioninfo in paperInfo.TrueOrFalseQuestions)
                    {
                        trueOrFalseQuestionList.Add(new TrueOrFalseQuestion
                        {
                            Type = questioninfo.Type,
                            Content = questioninfo.Content,
                            IsCorrect = questioninfo.IsCorrect
                        });
                    };
                    var singleQuestionList = new List<ChoiceQuestion>();
                    foreach (SingleQuestionViewModel questioninfo in paperInfo.SingleQuestions)
                    {
                        ChoiceQuestion c = new ChoiceQuestion();
                        c.Type = questioninfo.Type;
                        c.Content = questioninfo.Content;
                        for (int i = 0; i < 4; i++)
                        {
                            Option o = new Option
                            {
                                OptionProperty = questioninfo.Options[i].OptionProperty,
                                OptionId = questioninfo.Options[i].OptionId
                                //OptionId = (OptionType)(i + 1),
                            };
                            if ((i + 1) == questioninfo.CorrectAnswer)  //view中正确选项是从1开始的，和OptionId的枚举一致，所以这里要加一才能和正确选项相等。
                            {
                                o.IsCorrect = true;
                            }
                            c.Options.Add(o);
                        }
                        singleQuestionList.Add(c);
                    }
                    var multipleQuestionList = new List<ChoiceQuestion>();
                    foreach (MultipleQuestionViewModel questioninfo in paperInfo.MultipleQuestions)
                    {
                        ChoiceQuestion c = new ChoiceQuestion();
                        c.Type = questioninfo.Type;
                        c.Content = questioninfo.Content;
                        for (int i = 0; i < questioninfo.Options.Count; i++)
                        {
                            c.Options.Add(new Option
                            {
                                OptionProperty = questioninfo.Options[i].OptionProperty,
                                IsCorrect = questioninfo.Options[i].IsCorrect,
                                OptionId = questioninfo.Options[i].OptionId
                                //OptionId = (OptionType)(i + 1)
                            });
                        }
                        multipleQuestionList.Add(c);
                    }
                    //paper.Questions = new List<Question>();
                    paper.Questions.AddRange(trueOrFalseQuestionList);
                    paper.Questions.AddRange(singleQuestionList);
                    paper.Questions.AddRange(multipleQuestionList);
                    DB.Papers.Add(paper);
                    await DB.SaveChangesAsync();
                    //repository.Add(paper);
                    //await repository.SaveAsync();
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            };

            TempData["LastPostModel"] = paperInfo;
            return RedirectToAction("Create");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            Paper paper = await DB.Papers.FirstOrDefaultAsync(p => p.Id == id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            else
            {
                PaperViewModel model = new PaperViewModel();
                model.Id = paper.Id;
                try
                {
                    model.MultipleQuestions = paper.Questions.Where(q => q.Type == QuestionType.多选题).Select(q => new MultipleQuestionViewModel
                    {
                        Id = q.Id,
                        Content = q.Content,
                        Type = q.Type,
                        Options = (q as ChoiceQuestion).Options.OrderBy(o => o.OptionId).Select(o => new MultipleOptionViewModel
                        {
                            OptionId = o.OptionId,
                            OptionProperty = o.OptionProperty,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                    }).ToList();
                    model.SingleQuestions = paper.Questions.Where(q => q.Type == QuestionType.单选题).Select(q => new SingleQuestionViewModel
                    {
                        Id = q.Id,
                        Content = q.Content,
                        Type = q.Type,
                        CorrectAnswer = (int)((q as ChoiceQuestion).Options.Where(o => o.IsCorrect == true).FirstOrDefault().OptionId),
                        Options = (q as ChoiceQuestion).Options.OrderBy(o => o.OptionId).Select(o => new OptionViewModel
                        {
                            OptionId = o.OptionId,
                            OptionProperty = o.OptionProperty
                        }).ToList()
                    }).ToList();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                model.TrueOrFalseQuestions = paper.Questions.Where(q => q.Type == QuestionType.判断题).Select(q => new TrueOrFalseQuestionViewModel
                {
                    Id = q.Id,
                    Content = q.Content,
                    Type = q.Type,
                    IsCorrect = (q as TrueOrFalseQuestion).IsCorrect
                }).ToList();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, PaperViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            Paper oldPaper = await DB.Papers.FindAsync(id);
            oldPaper.EditOn = DateTime.Now;
            foreach (var question in oldPaper.Questions)
            {
                switch (question.Type)
                {
                    case QuestionType.判断题:
                        var editedTq = model.TrueOrFalseQuestions.Find(t => t.Id == question.Id);
                        question.Content = editedTq.Content;
                        (question as TrueOrFalseQuestion).IsCorrect = editedTq.IsCorrect;
                        DB.Entry(question).State = EntityState.Modified;
                        break;
                    case QuestionType.单选题:
                        var editedSq = model.SingleQuestions.Find(s => s.Id == question.Id);
                        question.Content = editedSq.Content;
                        foreach (var op in (question as ChoiceQuestion).Options)
                        {
                            op.OptionProperty = editedSq.Options.Find(o => o.OptionId == op.OptionId).OptionProperty;
                            if ((int)op.OptionId == editedSq.CorrectAnswer)
                            {
                                op.IsCorrect = true;
                            }
                            else
                            {
                                op.IsCorrect = false;
                            }
                        }
                        DB.Entry(question).State = EntityState.Modified;
                        break;
                    case QuestionType.多选题:
                        var editedMq = model.MultipleQuestions.Find(m => m.Id == question.Id);
                        question.Content = editedMq.Content;
                        foreach (var op in (question as ChoiceQuestion).Options)
                        {
                            op.OptionProperty = editedMq.Options.Find(o => o.OptionId == op.OptionId).OptionProperty;
                            op.IsCorrect = editedMq.Options.Find(o => o.OptionId == op.OptionId).IsCorrect;
                        }
                        DB.Entry(question).State = EntityState.Modified;
                        break;
                }
            }
            DB.Entry(oldPaper).State = EntityState.Modified;
            await DB.SaveChangesAsync();
            return RedirectToAction("List");
        }

        public async Task<ActionResult> Details(Guid id)
        {
            //var paper = await DB.Papers.FirstOrDefaultAsync(p => p.Id == id);
            var paper = await DB.Papers.FindAsync(id);
            PaperViewModel model = new PaperViewModel();
            model.TeacherName = paper.Teacher.TrueName;
            model.EditOn = paper.EditOn.ToShortDateString();
            model.Id = id;
            model.TrueOrFalseQuestions = paper.Questions.Where(q => q.Type == QuestionType.判断题).Select(q => new TrueOrFalseQuestionViewModel
            {
                Content = q.Content,
                IsCorrect = (q as TrueOrFalseQuestion).IsCorrect,
                Type = QuestionType.判断题
            }).ToList();
            model.SingleQuestions = paper.Questions.Where(q => q.Type == QuestionType.单选题).Select(q => new SingleQuestionViewModel
            {
                Content = q.Content,
                Type = QuestionType.单选题,
                Options = (q as ChoiceQuestion).Options.OrderBy(o => o.OptionId).Select(o => new OptionViewModel { OptionId = o.OptionId, OptionProperty = o.OptionProperty }).ToList(),
                CorrectAnswer = (int)((q as ChoiceQuestion).Options.Where(o => o.IsCorrect == true).FirstOrDefault().OptionId),
            }).ToList();
            model.MultipleQuestions = paper.Questions.Where(q => q.Type == QuestionType.多选题).Select(q => new MultipleQuestionViewModel
            {
                Content = q.Content,
                Type = QuestionType.多选题,
                Options = (q as ChoiceQuestion).Options.OrderBy(o => o.OptionId).Select(o => new MultipleOptionViewModel { OptionId = o.OptionId, OptionProperty = o.OptionProperty, IsCorrect = o.IsCorrect }).ToList(),
            }).ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            //var paper = await DB.Papers.FindAsync(id);
            var paper = await DB.Papers.FirstOrDefaultAsync(p => p.Id == id);
            if(paper == null)
            {
                return HttpNotFound();
            }
            paper.Teacher = null;
            DB.Papers.Remove(paper);
            await DB.SaveChangesAsync();
            return new EmptyResult();
        }
        //[HttpPost]
        //public async Task<ActionResult> Edit(Guid id, FormCollection collection)
        //{

        //    var paperToUpdate = await DB.Papers.FirstOrDefaultAsync(p => p.Id == id);
        //    if (TryUpdateModel(paperToUpdate, "", collection.AllKeys, new string[] { "Id," }) && ModelState.IsValid)
        //    {
        //        await DB.SaveChangesAsync();
        //    }
        //    return View(paperToUpdate);
        //}
    }
}