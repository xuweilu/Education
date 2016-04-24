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

    }
    public class Administrator : ApplicationUser
    {

    }
}