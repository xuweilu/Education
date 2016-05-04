namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeguidtokey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Options", "ChoiceQuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Exams", "Id", "dbo.Papers");
            DropForeignKey("dbo.Questions", "PaperId", "dbo.Papers");
            DropForeignKey("dbo.Answers", "SheetId", "dbo.Sheets");
            DropPrimaryKey("dbo.Answers");
            DropPrimaryKey("dbo.Questions");
            DropPrimaryKey("dbo.Papers");
            DropPrimaryKey("dbo.Sheets");
            DropPrimaryKey("dbo.Options");
            AlterColumn("dbo.Answers", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Questions", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Papers", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Sheets", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Options", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Answers", "Id");
            AddPrimaryKey("dbo.Questions", "Id");
            AddPrimaryKey("dbo.Papers", "Id");
            AddPrimaryKey("dbo.Sheets", "Id");
            AddPrimaryKey("dbo.Options", "Id");
            AddForeignKey("dbo.Options", "ChoiceQuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Exams", "Id", "dbo.Papers", "Id");
            AddForeignKey("dbo.Questions", "PaperId", "dbo.Papers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Answers", "SheetId", "dbo.Sheets", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "SheetId", "dbo.Sheets");
            DropForeignKey("dbo.Questions", "PaperId", "dbo.Papers");
            DropForeignKey("dbo.Exams", "Id", "dbo.Papers");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Options", "ChoiceQuestionId", "dbo.Questions");
            DropPrimaryKey("dbo.Options");
            DropPrimaryKey("dbo.Sheets");
            DropPrimaryKey("dbo.Papers");
            DropPrimaryKey("dbo.Questions");
            DropPrimaryKey("dbo.Answers");
            AlterColumn("dbo.Options", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Sheets", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Papers", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Questions", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Answers", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Options", "Id");
            AddPrimaryKey("dbo.Sheets", "Id");
            AddPrimaryKey("dbo.Papers", "Id");
            AddPrimaryKey("dbo.Questions", "Id");
            AddPrimaryKey("dbo.Answers", "Id");
            AddForeignKey("dbo.Answers", "SheetId", "dbo.Sheets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questions", "PaperId", "dbo.Papers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Exams", "Id", "dbo.Papers", "Id");
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Options", "ChoiceQuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
    }
}
