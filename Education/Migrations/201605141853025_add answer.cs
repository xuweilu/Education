namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addanswer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sheets", "AnswerSheet_Id", "dbo.Papers");
            DropIndex("dbo.Sheets", new[] { "AnswerSheet_Id" });
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AnswerType = c.Int(nullable: false),
                        SheetId = c.Guid(nullable: false),
                        QuestionId = c.Guid(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Sheets", t => t.SheetId, cascadeDelete: true)
                .Index(t => t.SheetId)
                .Index(t => t.QuestionId);
            
            DropColumn("dbo.Sheets", "AnswerSheet_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sheets", "AnswerSheet_Id", c => c.Guid());
            DropForeignKey("dbo.Answers", "SheetId", "dbo.Sheets");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Answers", new[] { "SheetId" });
            DropTable("dbo.Answers");
            CreateIndex("dbo.Sheets", "AnswerSheet_Id");
            AddForeignKey("dbo.Sheets", "AnswerSheet_Id", "dbo.Papers", "Id");
        }
    }
}
