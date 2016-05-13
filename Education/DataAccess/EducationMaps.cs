using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Education.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.DataAccess
{
    public class PaperMap : EntityTypeConfiguration<Paper>
    {
        public PaperMap()
        {
            Property(p => p.EditOn).IsOptional();
            HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(p => p.Teacher).WithMany(t => t.Papers).HasForeignKey(p => p.TeacherId);
            HasMany(p => p.Questions).WithRequired(q => q.Paper).HasForeignKey(q => q.PaperId);
            //HasOptional(p => p.Exam).WithRequired(e => e.Paper);
        }
    }
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {

        }
    }
}