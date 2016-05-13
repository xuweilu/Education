using Education.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Education.Models
{
    //考试，一场考试和一张卷子一一对应，同时对应很多学生，对应很多答卷
    public class Exam : IEntity
    {
        [Key, ForeignKey("Paper")]
        public Guid Id { get; set; }

        [DisplayName("考试时间")]
        public DateTime? ExamOn { get; set; }

        [DisplayName("考试地点")]
        public string Address { get; set; }

        [DisplayName("考试名")]
        public string ExamName { get; set; }

        public virtual Paper Paper { get; set; }

        public virtual List<Student> Students { get; set; }
        public virtual List<Sheet> Sheets { get; set; }

        public Exam()
        {
            //Id = Guid.NewGuid();
            Students = new List<Student>();
            Sheets = new List<Sheet>();
        }
    }

    //答卷，一张答卷对应一场考试，由一个学生完成，有一个分数，同时有很多回答
    public class Sheet : IEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public double Score { get; set; }
        public DateTime? AnswerOn { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Exam")]
        public Guid ExamId { get; set; }
        public virtual Exam Exam { get; set; }

        public virtual List<Answer> Answers { get; set; }
        public Sheet()
        {
            Id = Guid.NewGuid();
            Answers = new List<Answer>();
        }
    }

    //一个回答属于一张答卷，由一个学生完成，对应一个题目
    public abstract class Answer : IEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Sheet")]
        public Guid SheetId { get; set; }
        public virtual Sheet Sheet { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public Answer()
        {
            Id = Guid.NewGuid();
        }
    }
    public class TrueOrFalseAnswer : Answer
    {
        [Column("TrueOrFalseAnswer")]
        public bool? Answer { get; set; }
    }
    public class SingleAnswer : Answer
    {
        [Column("SingleAnswer")]
        public OptionType? Answer { get; set; }
    }
    public class MultipleAnswer : Answer
    {
        [Column("MultipleAnswer")]
        public string Answer { get; set; }
    }
}