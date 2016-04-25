namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetheinheritancerelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Options", "MultipleQuestionId", "dbo.Questions");
            DropForeignKey("dbo.Options", "SingleQuestionId", "dbo.Questions");
            RenameColumn(table: "dbo.Options", name: "MultipleQuestionId", newName: "MultipleQuestion_Id");
            RenameColumn(table: "dbo.Options", name: "SingleQuestionId", newName: "SingleQuestion_Id");
            RenameIndex(table: "dbo.Options", name: "IX_MultipleQuestionId", newName: "IX_MultipleQuestion_Id");
            RenameIndex(table: "dbo.Options", name: "IX_SingleQuestionId", newName: "IX_SingleQuestion_Id");
            DropPrimaryKey("dbo.Options");
            AddColumn("dbo.Options", "QuestionId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Options", "QuestionId");
            CreateIndex("dbo.Options", "QuestionId");
            AddForeignKey("dbo.Options", "QuestionId", "dbo.Questions", "Id");
            AddForeignKey("dbo.Options", "MultipleQuestion_Id", "dbo.Questions", "Id");
            AddForeignKey("dbo.Options", "SingleQuestion_Id", "dbo.Questions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Options", "SingleQuestion_Id", "dbo.Questions");
            DropForeignKey("dbo.Options", "MultipleQuestion_Id", "dbo.Questions");
            DropForeignKey("dbo.Options", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Options", new[] { "QuestionId" });
            DropPrimaryKey("dbo.Options");
            DropColumn("dbo.Options", "QuestionId");
            AddPrimaryKey("dbo.Options", "OptionId");
            RenameIndex(table: "dbo.Options", name: "IX_SingleQuestion_Id", newName: "IX_SingleQuestionId");
            RenameIndex(table: "dbo.Options", name: "IX_MultipleQuestion_Id", newName: "IX_MultipleQuestionId");
            RenameColumn(table: "dbo.Options", name: "SingleQuestion_Id", newName: "SingleQuestionId");
            RenameColumn(table: "dbo.Options", name: "MultipleQuestion_Id", newName: "MultipleQuestionId");
            AddForeignKey("dbo.Options", "SingleQuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Options", "MultipleQuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
    }
}
