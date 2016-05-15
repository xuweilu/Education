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
        private const double tqPoint = 1;
        private const double sqPoint = 1;
        private const double mqPoint = 1;
        public ActionResult List()
        {
            var stu = GetCurrentUser() as Student;
            var model = stu.Exams ?? new List<Exam>();
            return View(model);
        }
        // GET: Sheets/Create
        public async Task<ActionResult> Create(Guid id)
        {
            var currentStudent = (await GetCurrentUserAsync()) as Student;
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
                int rightTqCount = 0;
                int rightSqCount = 0;
                int rightmqCount = 0;
                foreach (var questioninfo in model.TrueOrFalseQuestions)
                {
                    sheet.Answers.Add(new Answer
                    {
                        AnswerType = QuestionType.判断题,
                        QuestionId = questioninfo.Id,
                        Content = questioninfo.IsCorrect.ToString()
                    });
                    var rightAnswer = ((await DB.Questions.FirstOrDefaultAsync(q => q.Id == questioninfo.Id)) as TrueOrFalseQuestion).IsCorrect;
                    if (questioninfo.IsCorrect == rightAnswer)
                        rightTqCount++;
                }
                foreach (var questioninfo in model.SingleQuestions)
                {
                    sheet.Answers.Add(new Answer
                    {
                        AnswerType = QuestionType.单选题,
                        QuestionId = questioninfo.Id,
                        Content = questioninfo.CorrectAnswer.ToString(),
                    });
                    var rightAnswer = (int)((await DB.Questions.FirstOrDefaultAsync(q => q.Id == questioninfo.Id)) as ChoiceQuestion).Options.Where(o => o.IsCorrect == true).FirstOrDefault().OptionId;
                    if (questioninfo.CorrectAnswer == rightAnswer)
                        rightSqCount++;
                }
                foreach (var questioninfo in model.MultipleQuestions)
                {
                    sheet.Answers.Add(new Answer
                    {
                        AnswerType = QuestionType.多选题,
                        QuestionId = questioninfo.Id,
                        Content = string.Join(",", questioninfo.Options.Where(o => o.IsCorrect == true).Select(o => o.OptionId))
                    });
                    var rightOptions = (await DB.Questions.FirstOrDefaultAsync(q => q.Id == questioninfo.Id) as ChoiceQuestion).Options.Where(o => o.IsCorrect == true).OrderBy(o => o.OptionId).ToList();
                    var selectedOptions = questioninfo.Options.Where(o => o.IsCorrect == true).OrderBy(o => o.OptionId).ToList();
                    bool answerRight = true;
                    for (int i = 0; i < selectedOptions.Count; i++)
                    {
                        if (selectedOptions[i].OptionId != rightOptions[i].OptionId)
                        {
                            answerRight = false;
                        }
                    }
                    if (answerRight)
                    {
                        rightmqCount++;
                    }
                }
                score = rightTqCount * tqPoint + rightSqCount * sqPoint + rightmqCount * mqPoint;
                sheet.Score = score;
                DB.Sheets.Add(sheet);
                await DB.SaveChangesAsync();
                var rightQuestionsCount = rightTqCount + rightSqCount + rightmqCount;
                var totalQuestionsCount = model.MultipleQuestions.Count + model.SingleQuestions.Count + model.MultipleQuestions.Count;
                double grade = rightQuestionsCount/(double)totalQuestionsCount;
                string comment = "";
                if (grade < 0.6)
                {
                    comment = "答来答去的答案都图样！爱慕安规！";
                }
                else if (grade >= 0.6 && grade < 0.8)
                {
                    comment = "还要继续学习一个！";
                }
                else
                {
                    comment = "跟美国的华莱士一样不知道高到哪里去了！";
                }
                var mark = new MarkViewModel();
                mark.Comment = comment;
                mark.Grade = ((int)(grade*100)).ToString();
                return RedirectToAction("Score","Sheets", mark);
            }
            return View(model);
        }
        public ActionResult Score(MarkViewModel mark)
        {
            return View(mark);
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
