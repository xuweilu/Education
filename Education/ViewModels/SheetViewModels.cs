using Education.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Education.ViewModels
{
    public class SheetViewModel : PaperViewModel
    {
        public new string TeacherName { get; set; }
        [DisplayName("考试名")]
        public string ExamName { get; set; }

        [DisplayName("答题人姓名")]
        public string StudentName { get; set; }
        public new List<SheetTrueOrFalseQuestionViewModel> TrueOrFalseQuestions { get; set; }
        public new List<SheetSingleQuestionViewModel> SingleQuestions { get; set; }
        public new List<SheetMultipleQuestionViewModel> MultipleQuestions { get; set; }
    }
    public abstract class SheetQuestionViewModel
    {
        public Guid Id { get; set; }
        [DisplayName("题目类型")]
        public QuestionType Type { get; set; }

        [DisplayName("题目内容")]
        public string Content { get; set; }
    }
    public class SheetTrueOrFalseQuestionViewModel : SheetQuestionViewModel
    {
        [DisplayName("是否正确？")]
        [Required(ErrorMessage = "请选择是否正确")]
        public bool IsCorrect { get; set; }
        public SheetTrueOrFalseQuestionViewModel()
        {
            Type = QuestionType.判断题;
        }
    }
    public class SheetSingleQuestionViewModel : SheetQuestionViewModel
    {
        public List<SheetOptionViewModel> Options { get; set; }
        [Required(ErrorMessage = "请选择一个正确选项")]
        public int CorrectAnswer { get; set; }
        public SheetSingleQuestionViewModel()
        {
            Type = QuestionType.单选题;
            Options = new List<SheetOptionViewModel>();
            Options.Add(new SheetOptionViewModel { OptionId = OptionType.选项A });
            Options.Add(new SheetOptionViewModel { OptionId = OptionType.选项B });
            Options.Add(new SheetOptionViewModel { OptionId = OptionType.选项C });
            Options.Add(new SheetOptionViewModel { OptionId = OptionType.选项D });
        }
    }
    public class SheetMultipleQuestionViewModel : SheetQuestionViewModel
    {
        public List<SheetMultipleOptionViewModel> Options { get; set; }
        public SheetMultipleQuestionViewModel()
        {
            Type = QuestionType.多选题;
            Options = new List<SheetMultipleOptionViewModel>();
            Options.Add(new SheetMultipleOptionViewModel { OptionId = OptionType.选项A });
        }
    }
    public class SheetOptionViewModel
    {
        [DisplayName("选项号")]
        public OptionType OptionId { get; set; }

        [DisplayName("选项内容")]
        public string OptionProperty { get; set; }
    }
    public class SheetMultipleOptionViewModel
    {
        [DisplayName("选项号")]
        public OptionType OptionId { get; set; }

        [DisplayName("选项内容")]
        public string OptionProperty { get; set; }
        [DisplayName("选项是否正确？")]
        public bool IsCorrect { get; set; }
    }
}