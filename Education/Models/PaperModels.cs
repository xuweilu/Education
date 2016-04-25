using Education.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Education.Models
{
    public class Paper : IEntity
    {
        public Guid Id { get; set; }
        public DateTime? EditOn { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual List<Question> Questions { get; set; }

        public Paper()
        {
            Exam = new Exam();
            Teacher = new Teacher();
            Questions = new List<Question>();
        }

    }
    public abstract class Question : IEntity
    {
        public Guid Id { get; set; }
        public QuestionType Type { get; set; }
        public string Content { get; set; }

        [ForeignKey("Paper")]
        public Guid PaperId { get; set; }
        public virtual Paper Paper { get; set; }
        
        public virtual List<Answer> Answers { get; set; }

        public Question()
        {
            Paper = new Paper();
            Answers = new List<Answer>();
        }
    }
    public class TrueOrFalseQuestion : Question
    {
        [Column("TruseOrFalseCorrectAnswer")]
        public bool? IsCorrect { get; set; }

    }
    public class SingleQuestion : Question
    {
        [Column("SingleCorrectAnswer")]
        public OptionType CorrectOption { get; set; }
        public virtual List<SingleOption> SingleOptions { get; set; }
        public SingleQuestion()
        {
            SingleOptions = new List<SingleOption>();
        }

    }
    public class MultipleQuestion : Question
    {
        public virtual List<MultipleOption> MultipleOptions { get; set; }
        public MultipleQuestion()
        {
            MultipleOptions = new List<MultipleOption>();
        }

    }
    public abstract class Option
    {
        [Key, ForeignKey("Question")]
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        //public int Id { get; set; }
        public OptionType OptionId { get; set; }
        public string OptionProperty { get; set; }
    }
    public class SingleOption : Option
    {
        //[Key, ForeignKey("SingleQuestion")]
        //public Guid SingleQuestionId { get; set; }
        //public virtual SingleQuestion SingleQuestion { get; set; }

        public SingleOption()
        {
            base.Question = new SingleQuestion();
            //SingleQuestion = new SingleQuestion();
        }
    }
    public class MultipleOption : Option
    {
        public bool? IsCorrect { get; set; }

        //[Key, ForeignKey("MultipleQuestion")]
        //public Guid MultipleQuestionId { get; set; }
        //public virtual MultipleQuestion MultipleQuestion { get; set; }

        public MultipleOption()
        {
            base.Question = new MultipleQuestion();
            //MultipleQuestion = new MultipleQuestion();
        }
    }
    public enum QuestionType { 判断题 = 1, 单选题, 多选题 };
    public enum OptionType { 选项A = 1, 选项B, 选项C, 选项D, 选项E, 选项F, 选项G, 选项H, 选项I, 选项J, 选项K, 选项L, 选项M, 选项N, 选项O, 选项P, 选项Q, 选项R, 选项S, 选项T, 选项U, 选项V, 选项W, 选项X, 选项Y, 选项Z };

}