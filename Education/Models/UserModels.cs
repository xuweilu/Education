using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Education.Models
{
    public class Teacher : ApplicationUser
    {
        public virtual List<Paper> Papers { get; set; }
        public Teacher()
        {
            Papers = new List<Paper>();
        }
    }
    public class Student : ApplicationUser
    {
        public virtual List<Exam> Exams { get; set; }
        public virtual List<Sheet> Sheets { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public Student()
        {
            Exams = new List<Exam>();
            Sheets = new List<Sheet>();
            Answers = new List<Answer>();
        }
    }
    public class Administrator : ApplicationUser
    {

    }
}