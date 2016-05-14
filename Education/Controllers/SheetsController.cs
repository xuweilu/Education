using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Education.Models;
using Education.ViewModels;

namespace Education.Controllers
{
    public class SheetsController : BaseController
    {
        private const double Point = 3;
        public ActionResult List()
        {
            var stu = GetCurrentUser() as Student;
            var model = stu.Exams.ToList();
            return View(model);
        }
        // GET: Sheets/Create
        public async Task<ActionResult> Create(Guid id)
        {
            var currentStudent =(await GetCurrentUserAsync()) as Student;
            var currentExam = await DB.Exams.FirstOrDefaultAsync(e => e.Id == id);
            var currentPaper = currentExam.Paper;
            SheetViewModel model = new SheetViewModel();
            model.Id = id;
            model.ExamName = currentExam.ExamName;
            model.StudentName = currentStudent.TrueName;
            model.TeacherName = currentPaper.Teacher.TrueName;
            model.MultipleQuestions = currentPaper.Questions.Where(q => q.Type == QuestionType.多选题).Select(q => new SheetMultipleQuestionViewModel
            {
                Id = q.Id,
                Content = q.Content,
                Type = q.Type,
                Options = (q as ChoiceQuestion).Options.OrderBy(o => o.OptionId).Select(o => new SheetMultipleOptionViewModel
                {
                    OptionId = o.OptionId,
                    OptionProperty = o.OptionProperty,
                }).ToList()
            }).ToList();
            model.SingleQuestions = currentPaper.Questions.Where(q => q.Type == QuestionType.单选题).Select(q => new SheetSingleQuestionViewModel
            {
                Id = q.Id,
                Content = q.Content,
                Type = q.Type,
                Options = (q as ChoiceQuestion).Options.OrderBy(o => o.OptionId).Select(o => new SheetOptionViewModel
                {
                    OptionId = o.OptionId,
                    OptionProperty = o.OptionProperty
                }).ToList()
            }).ToList();
            model.TrueOrFalseQuestions = currentPaper.Questions.Where(q => q.Type == QuestionType.判断题).Select(q => new SheetTrueOrFalseQuestionViewModel
            {
                Id = q.Id,
                Content = q.Content,
                Type = q.Type,
            }).ToList();
            return View(model);
        }

        // POST: Sheets/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SheetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sheet = new Sheet();
                sheet.AnswerOn = DateTime.Now;
                sheet.Student = (GetCurrentUser()) as Student;
                sheet.ExamId = model.Id;
                double score = 0;
                foreach(var questioninfo in model.TrueOrFalseQuestions)
                {
                    sheet.Answers.Add(new Answer
                    {
                        AnswerType = QuestionType.判断题,
                        QuestionId = questioninfo.Id,
                        Content = questioninfo.IsCorrect.ToString()
                    });
                    //var rightAnswer = ((await DB.Questions.FirstOrDefaultAsync(q => q.Id == questioninfo.Id)) as TrueOrFalseQuestion).IsCorrect;
                    //if (questioninfo.IsCorrect == rightAnswer)
                    //    score++;
                }
                foreach(var questioninfo in model.SingleQuestions)
                {
                    sheet.Answers.Add(new Answer
                    {
                        AnswerType = QuestionType.单选题,
                        QuestionId = questioninfo.Id,
                        Content = questioninfo.CorrectAnswer.ToString(),
                    });
                    //var rightAnswer =(int)((await DB.Questions.FirstOrDefaultAsync(q => q.Id == questioninfo.Id)) as ChoiceQuestion).Options.Where(o => o.IsCorrect == true).FirstOrDefault().OptionId;
                    //if (questioninfo.CorrectAnswer == rightAnswer)
                    //    score++;
                }
                foreach(var questioninfo in model.MultipleQuestions)
                {
                    sheet.Answers.Add(new Answer
                    {
                        AnswerType = QuestionType.多选题,
                        QuestionId = questioninfo.Id,
                        Content = string.Join(",", questioninfo.Options.Where(o => o.IsCorrect == true).Select(o => o.OptionId))
                    });
                    //var rightOptions = (await DB.Questions.FirstOrDefaultAsync(q => q.Id == questioninfo.Id) as ChoiceQuestion).Options.Where(o => o.IsCorrect == true).ToList();
                    //var selectedOptions = questioninfo.Options.Where(o => o.IsCorrect == true).ToList();
                    //if(rightOptions == selectedOptions)
                    //{
                    //    score++;
                    //}
                }
                DB.Sheets.Add(sheet);
                await DB.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
