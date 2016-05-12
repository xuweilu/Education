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
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public DateTime? EditOn { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual List<Question> Questions { get; set; }
        public Paper()
        {
            Id = Guid.NewGuid();
            //Exam = new Exam();
            Questions = new List<Question>();
        }

    }
    public abstract class Question : IEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public QuestionType Type { get; set; }
        public string Content { get; set; }

        [ForeignKey("Paper")]
        public Guid PaperId { get; set; }
        public virtual Paper Paper { get; set; }
        
        public virtual List<Answer> Answers { get; set; }
        public Question()
        {
            Id = Guid.NewGuid();
            Answers = new List<Answer>();
        }
    }
    public class TrueOrFalseQuestion : Question
    {
        [Column("TruseOrFalseCorrectAnswer")]
        public bool IsCorrect { get; set; }
        public TrueOrFalseQuestion() : base()
        {
            Type = QuestionType.判断题;
        }

    }
    public class ChoiceQuestion : Question
    {
        public virtual List<Option> Options { get; set; }
        public ChoiceQuestion() : base()
        {
            Options = new List<Option>();
        }
    }
    public class Option : IEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("ChoiceQuestion")]
        public Guid ChoiceQuestionId { get; set; }
        public virtual ChoiceQuestion ChoiceQuestion { get; set; }

        public OptionType OptionId { get; set; }

        public string OptionProperty { get; set; }

        public bool IsCorrect { get; set; }
        public Option()
        {
            Id = Guid.NewGuid();
        }
    }
    public enum QuestionType { 判断题 = 1, 单选题, 多选题 };
    public enum OptionType { 选项A = 1, 选项B, 选项C, 选项D, 选项E, 选项F, 选项G, 选项H, 选项I, 选项J, 选项K, 选项L, 选项M, 选项N, 选项O, 选项P, 选项Q, 选项R, 选项S, 选项T, 选项U, 选项V, 选项W, 选项X, 选项Y, 选项Z };

}