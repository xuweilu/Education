namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createmodelsofexam : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MultipleOptions", newName: "Options");
            DropIndex("dbo.Options", new[] { "MultipleQuestionId" });
            DropIndex("dbo.SingleOptions", new[] { "SingleQuestionId" });
            RenameColumn(table: "dbo.Questions", name: "SingleAnswer", newName: "SingleCorrectAnswer");
            RenameColumn(table: "dbo.Questions", name: "TruseOrFalseAnswer", newName: "TruseOrFalseCorrectAnswer");
            DropPrimaryKey("dbo.Options");
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SheetId = c.Guid(nullable: false),
                        StudentId = c.String(maxLength: 128),
                        QuestionId = c.Guid(nullable: false),
                        Answer = c.String(),
                        Answer1 = c.Int(),
                        Answer2 = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Sheets", t => t.SheetId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId)
                .Index(t => t.SheetId)
                .Index(t => t.StudentId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ExamOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Papers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Sheets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Score = c.Double(nullable: false),
                        AnswerOn = c.DateTime(),
                        StudentId = c.String(maxLength: 128),
                        ExamId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exams", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.ExamId);
            
            CreateTable(
                "dbo.StudentExams",
                c => new
                    {
                        Student_Id = c.String(nullable: false, maxLength: 128),
                        Exam_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_Id, t.Exam_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.Exams", t => t.Exam_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Exam_Id);
            
            AddColumn("dbo.Papers", "EditOn", c => c.DateTime());
            AddColumn("dbo.Options", "SingleQuestionId", c => c.Guid());
            AddColumn("dbo.Options", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Options", "MultipleQuestionId", c => c.Guid());
            AddPrimaryKey("dbo.Options", "OptionId");
            CreateIndex("dbo.Options", "MultipleQuestionId");
            CreateIndex("dbo.Options", "SingleQuestionId");
            DropColumn("dbo.Options", "Id");
            DropTable("dbo.SingleOptions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SingleOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SingleQuestionId = c.Guid(nullable: false),
                        OptionId = c.Int(nullable: false),
                        OptionProperty = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Options", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Answers", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "SheetId", "dbo.Sheets");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Sheets", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentExams", "Exam_Id", "dbo.Exams");
            DropForeignKey("dbo.StudentExams", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Sheets", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.Exams", "Id", "dbo.Papers");
            DropIndex("dbo.StudentExams", new[] { "Exam_Id" });
            DropIndex("dbo.StudentExams", new[] { "Student_Id" });
            DropIndex("dbo.Sheets", new[] { "ExamId" });
            DropIndex("dbo.Sheets", new[] { "StudentId" });
            DropIndex("dbo.Exams", new[] { "Id" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Answers", new[] { "StudentId" });
            DropIndex("dbo.Answers", new[] { "SheetId" });
            DropIndex("dbo.Options", new[] { "SingleQuestionId" });
            DropIndex("dbo.Options", new[] { "MultipleQuestionId" });
            DropPrimaryKey("dbo.Options");
            AlterColumn("dbo.Options", "MultipleQuestionId", c => c.Guid(nullable: false));
            DropColumn("dbo.Options", "Discriminator");
            DropColumn("dbo.Options", "SingleQuestionId");
            DropColumn("dbo.Papers", "EditOn");
            DropTable("dbo.StudentExams");
            DropTable("dbo.Sheets");
            DropTable("dbo.Exams");
            DropTable("dbo.Answers");
            AddPrimaryKey("dbo.Options", "Id");
            RenameColumn(table: "dbo.Questions", name: "TruseOrFalseCorrectAnswer", newName: "TruseOrFalseAnswer");
            RenameColumn(table: "dbo.Questions", name: "SingleCorrectAnswer", newName: "SingleAnswer");
            CreateIndex("dbo.SingleOptions", "SingleQuestionId");
            CreateIndex("dbo.Options", "MultipleQuestionId");
            RenameTable(name: "dbo.Options", newName: "MultipleOptions");
        }
    }
}
