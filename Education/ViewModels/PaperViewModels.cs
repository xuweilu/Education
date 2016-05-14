using Education.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Education.ViewModels
{
    public class PaperViewModel
    {
        public Guid Id { get; set; }
        [DisplayName("出题人姓名")]
        public string TeacherName { get; set; }
        [DisplayName("最近一次修改时间")]
        public string EditOn { get; set; }
        public List<TrueOrFalseQuestionViewModel> TrueOrFalseQuestions { get; set; }
        public List<SingleQuestionViewModel> SingleQuestions { get; set; }
        public List<MultipleQuestionViewModel> MultipleQuestions { get; set; }
        public PaperViewModel()
        {
            TrueOrFalseQuestions = new List<TrueOrFalseQuestionViewModel>();
            SingleQuestions = new List<SingleQuestionViewModel>();
            MultipleQuestions = new List<MultipleQuestionViewModel>();
            //默认有一个判断题、一个单选题、一个多选题，并且一个单选题默认有四个选项，一个多选题默认有一个选项。
            TrueOrFalseQuestions.Add(new TrueOrFalseQuestionViewModel());
            SingleQuestions.Add(new SingleQuestionViewModel());
            MultipleQuestions.Add(new MultipleQuestionViewModel());
        }
    }
    public abstract class QuestionViewModel
    {
        public Guid Id { get; set; }
        [DisplayName("题目类型")]
        public QuestionType Type { get; set; }

        [DisplayName("题目内容")]
        [Required(ErrorMessage = "请输入题目内容！")]
        public string Content { get; set; }
    }
    public class TrueOrFalseQuestionViewModel : QuestionViewModel
    {
        [DisplayName("是否正确？")]
        [Required(ErrorMessage ="请选择是否正确")]
        public bool IsCorrect { get; set; }
        public TrueOrFalseQuestionViewModel()
        {
            Type = QuestionType.判断题;
        }
    }
    public class SingleQuestionViewModel : QuestionViewModel
    {
        public List<OptionViewModel> Options { get; set; }
        [Required(ErrorMessage = "请选择一个正确选项")]
        public int CorrectAnswer { get; set; }
        public SingleQuestionViewModel()
        {
            Type = QuestionType.单选题;
            Options = new List<OptionViewModel>();
            Options.Add(new OptionViewModel { OptionId = OptionType.选项A });
            Options.Add(new OptionViewModel { OptionId = OptionType.选项B });
            Options.Add(new OptionViewModel { OptionId = OptionType.选项C });
            Options.Add(new OptionViewModel { OptionId = OptionType.选项D });
        }
    }
    public class MultipleQuestionViewModel : QuestionViewModel
    {
        public List<MultipleOptionViewModel> Options { get; set; }
        public MultipleQuestionViewModel()
        {
            Type = QuestionType.多选题;
            Options = new List<MultipleOptionViewModel>();
            Options.Add(new MultipleOptionViewModel { OptionId = OptionType.选项A });
        }
    }

    public class OptionViewModel
    {
        [DisplayName("选项号")]
        public OptionType OptionId { get; set; }

        [DisplayName("选项内容")]
        [Required(ErrorMessage = "请输入选项内容！")]
        public string OptionProperty { get; set; }
    }
    public class MultipleOptionViewModel
    {
        [DisplayName("选项号")]
        public OptionType OptionId { get; set; }

        [DisplayName("选项内容")]
        [Required(ErrorMessage = "请输入选项内容！")]
        public string OptionProperty { get; set; }
        [DisplayName("选项是否正确？")]
        public bool IsCorrect { get; set; }
    }
}