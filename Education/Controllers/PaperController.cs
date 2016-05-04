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
            if(TempData["LastPostModel"] == null)
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
                                OptionId = (OptionType)(i + 1),
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
                                OptionId = (OptionType)(i + 1)
                            });
                        }
                        multipleQuestionList.Add(c);
                    }
                    //paper.Questions = new List<Question>();
                    paper.Questions.AddRange(trueOrFalseQuestionList);
                    paper.Questions.AddRange(singleQuestionList);
                    paper.Questions.AddRange(multipleQuestionList);
                    //DB.Papers.Add(paper);
                    //await DB.SaveChangesAsync();
                    repository.Add(paper);
                    await repository.SaveAsync();
                    return RedirectToAction("Index","Home");
                }
                catch(Exception ex)
                {
                    throw ex;
                }

            };
            foreach(var item in paperInfo.SingleQuestions)
            {
                for (int i = 0; i < 4; i++)
                {
                    item.Options[i].OptiondId = (OptionType)(i + 1);
                }
            }
            foreach(var item in paperInfo.MultipleQuestions)
            {
                for(int i = 0; i < item.Options.Count; i++)
                {
                    item.Options[i].OptiondId = (OptionType)(i + 1);
                }
            }
            TempData["LastPostModel"] = paperInfo;
            return RedirectToAction("Create");
        }
    }
}