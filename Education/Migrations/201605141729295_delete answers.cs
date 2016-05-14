namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteanswers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "SheetId", "dbo.Sheets");
            DropForeignKey("dbo.Answers", "StudentId", "dbo.AspNetUsers");
            DropIndex("dbo.Answers", new[] { "SheetId" });
            DropIndex("dbo.Answers", new[] { "StudentId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            AddColumn("dbo.Sheets", "AnswerSheet_Id", c => c.Guid());
            CreateIndex("dbo.Sheets", "AnswerSheet_Id");
            AddForeignKey("dbo.Sheets", "AnswerSheet_Id", "dbo.Papers", "Id");
            DropTable("dbo.Answers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SheetId = c.Guid(nullable: false),
                        StudentId = c.String(maxLength: 128),
                        QuestionId = c.Guid(nullable: false),
                        MultipleAnswer = c.String(),
                        SingleAnswer = c.Int(),
                        TrueOrFalseAnswer = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Sheets", "AnswerSheet_Id", "dbo.Papers");
            DropIndex("dbo.Sheets", new[] { "AnswerSheet_Id" });
            DropColumn("dbo.Sheets", "AnswerSheet_Id");
            CreateIndex("dbo.Answers", "QuestionId");
            CreateIndex("dbo.Answers", "StudentId");
            CreateIndex("dbo.Answers", "SheetId");
            AddForeignKey("dbo.Answers", "StudentId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Answers", "SheetId", "dbo.Sheets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
    }
}
