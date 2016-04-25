namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changemodelsrelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Options", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Options", "MultipleQuestion_Id", "dbo.Questions");
            DropForeignKey("dbo.Options", "SingleQuestion_Id", "dbo.Questions");
            DropIndex("dbo.Options", new[] { "QuestionId" });
            RenameColumn(table: "dbo.Options", name: "MultipleQuestion_Id", newName: "MultipleQuestionId");
            RenameColumn(table: "dbo.Options", name: "SingleQuestion_Id", newName: "SingleQuestionId");
            RenameIndex(table: "dbo.Options", name: "IX_MultipleQuestion_Id", newName: "IX_MultipleQuestionId");
            RenameIndex(table: "dbo.Options", name: "IX_SingleQuestion_Id", newName: "IX_SingleQuestionId");
            DropPrimaryKey("dbo.Options");
            AddPrimaryKey("dbo.Options", "OptionId");
            AddForeignKey("dbo.Options", "MultipleQuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Options", "SingleQuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            DropColumn("dbo.Options", "QuestionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Options", "QuestionId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Options", "SingleQuestionId", "dbo.Questions");
            DropForeignKey("dbo.Options", "MultipleQuestionId", "dbo.Questions");
            DropPrimaryKey("dbo.Options");
            AddPrimaryKey("dbo.Options", "QuestionId");
            RenameIndex(table: "dbo.Options", name: "IX_SingleQuestionId", newName: "IX_SingleQuestion_Id");
            RenameIndex(table: "dbo.Options", name: "IX_MultipleQuestionId", newName: "IX_MultipleQuestion_Id");
            RenameColumn(table: "dbo.Options", name: "SingleQuestionId", newName: "SingleQuestion_Id");
            RenameColumn(table: "dbo.Options", name: "MultipleQuestionId", newName: "MultipleQuestion_Id");
            CreateIndex("dbo.Options", "QuestionId");
            AddForeignKey("dbo.Options", "SingleQuestion_Id", "dbo.Questions", "Id");
            AddForeignKey("dbo.Options", "MultipleQuestion_Id", "dbo.Questions", "Id");
            AddForeignKey("dbo.Options", "QuestionId", "dbo.Questions", "Id");
        }
    }
}
