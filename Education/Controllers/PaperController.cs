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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
                    foreach(OptionViewModel optioninfo in questioninfo.Options)
                    {
                        c.Options.Add(new Option
                        {
                            OptionProperty = optioninfo.OptionProperty,
                            IsCorrect = optioninfo.IsCorrect,
                        });
                    }
                    singleQuestionList.Add(c);
                }
                var multipleQuestionList = new List<ChoiceQuestion>();
                foreach (MultipleQuestionViewModel questioninfo in paperInfo.MultipleQuestions)
                {
                    ChoiceQuestion c = new ChoiceQuestion();
                    c.Type = QuestionType.多选题;
                    c.Content = questioninfo.Content;
                    foreach (OptionViewModel optioninfo in questioninfo.Options)
                    {
                        c.Options.Add(new Option
                        {
                            OptionProperty = optioninfo.OptionProperty,
                            IsCorrect = optioninfo.IsCorrect,
                        });
                    }
                    multipleQuestionList.Add(c);
                }
                paper.Questions.AddRange(trueOrFalseQuestionList);
                paper.Questions.AddRange(singleQuestionList);
                paper.Questions.AddRange(multipleQuestionList);
                DB.Papers.Add(paper);
                await DB.SaveChangesAsync();
                return View();
            };
            return View(paperInfo);
        }
    }
}