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
        public List<TrueOrFalseQuestionViewModel> TrueOrFalseQuestions { get; set; }
        public List<SingleQuestionViewModel> SingleQuestions { get; set; }
        public List<MultipleQuestionViewModel> MultipleQuestions { get; set; }
        public PaperViewModel()
        {
            TrueOrFalseQuestions = new List<TrueOrFalseQuestionViewModel>();
            SingleQuestions = new List<SingleQuestionViewModel>();
            MultipleQuestions = new List<MultipleQuestionViewModel>();
        }
    }
    public abstract class QuestionViewModel
    {
        [DisplayName("题目类型")]
        public QuestionType Type { get; set; }

        [DisplayName("题目内容")]
        [Required(ErrorMessage = "请输入题目内容！")]
        public string Content { get; set; }
    }
    public class TrueOrFalseQuestionViewModel : QuestionViewModel
    {
        public bool IsCorrect { get; set; }
    }
    public class SingleQuestionViewModel : QuestionViewModel
    {
        public List<OptionViewModel> Options { get; set; }
        public SingleQuestionViewModel()
        {
            Options = new List<OptionViewModel>();
        }
    }
    public class MultipleQuestionViewModel : QuestionViewModel
    {
        public List<OptionViewModel> Options { get; set; }
        public MultipleQuestionViewModel()
        {
            Options = new List<OptionViewModel>();
        }
    }

    public class OptionViewModel
    {
        [DisplayName("选项号")]
        public OptionType OptiondId { get; set; }

        [DisplayName("选项内容")]
        [Required(ErrorMessage = "请输入选项内容！")]
        public string OptionProperty { get; set; }

        [DisplayName("选项是否正确？")]
        public bool IsCorrect { get; set; }
    }
}