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
                var teacher = await GetCurrentUserAsync() as Teacher;
                Paper paper = new Paper();
                paper.EditOn = DateTime.Now;
                paper.Teacher = teacher;
                var trueOrFalseQuestionList = new List<TrueOrFalseQuestion>();
                foreach(TrueOrFalseQuestionViewModel questioninfo in paperInfo.TrueOrFalseQuestions)
                {
                    trueOrFalseQuestionList.Add(new TrueOrFalseQuestion
                    {
                        Type = QuestionType.判断题,
                        Content = questioninfo.Content,
                        IsCorrect = questioninfo.IsCorrect
                    });
                };
                var singleQuestionList = new List<ChoiceQuestion>();
                foreach(SingleQuestionViewModel questioninfo in paperInfo.SingleQuestions)
                {
                    ChoiceQuestion c = new ChoiceQuestion();
                    c.Type = QuestionType.单选题;
                    c.Content = questioninfo.Content;
                    for(int i = 0; i < 4; i++)
                    {
                        Option o = new Option
                        {
                            OptionProperty = questioninfo.Options[i].OptionProperty,
                            OptionId = (OptionType)(i + 1),
                        };
                        if ((i + 1) == questioninfo.CorrectAnswer)
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
                    c.Type = QuestionType.多选题;
                    c.Content = questioninfo.Content;
                    foreach (MultipleOptionViewModel optioninfo in questioninfo.Options)
                    {
                        c.Options.Add(new Option
                        {
                            OptionProperty = optioninfo.OptionProperty,
                            IsCorrect = optioninfo.IsCorrect
                        });
                    }
                    multipleQuestionList.Add(c);
                }
                paper.Questions.AddRange(trueOrFalseQuestionList);
                paper.Questions.AddRange(singleQuestionList);
                paper.Questions.AddRange(multipleQuestionList);
                repository.Add(paper);
                await repository.SaveAsync();
                return RedirectToAction("List");
            };
            return RedirectToAction("Create");
        }
    }
}