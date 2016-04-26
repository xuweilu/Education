namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mergethemultipleandsingleintoonechoicequestion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Options", "Id", "dbo.Questions");
            DropForeignKey("dbo.Options", "MultipleQuestion_Id", "dbo.Questions");
            DropForeignKey("dbo.Options", "SingleQuestion_Id", "dbo.Questions");
            DropIndex("dbo.Options", new[] { "Id" });
            DropIndex("dbo.Options", new[] { "MultipleQuestion_Id" });
            DropIndex("dbo.Options", new[] { "SingleQuestion_Id" });
            RenameColumn(table: "dbo.Options", name: "MultipleQuestion_Id", newName: "ChoiceQuestionId");
            RenameColumn(table: "dbo.Options", name: "SingleQuestion_Id", newName: "ChoiceQuestionId");
            AlterColumn("dbo.Options", "ChoiceQuestionId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Options", "ChoiceQuestionId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Options", "ChoiceQuestionId");
            AddForeignKey("dbo.Options", "ChoiceQuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Options", "ChoiceQuestionId", "dbo.Questions");
            DropIndex("dbo.Options", new[] { "ChoiceQuestionId" });
            AlterColumn("dbo.Options", "ChoiceQuestionId", c => c.Guid());
            AlterColumn("dbo.Options", "ChoiceQuestionId", c => c.Guid());
            RenameColumn(table: "dbo.Options", name: "ChoiceQuestionId", newName: "SingleQuestion_Id");
            RenameColumn(table: "dbo.Options", name: "ChoiceQuestionId", newName: "MultipleQuestion_Id");
            CreateIndex("dbo.Options", "SingleQuestion_Id");
            CreateIndex("dbo.Options", "MultipleQuestion_Id");
            CreateIndex("dbo.Options", "Id");
            AddForeignKey("dbo.Options", "SingleQuestion_Id", "dbo.Questions", "Id");
            AddForeignKey("dbo.Options", "MultipleQuestion_Id", "dbo.Questions", "Id");
            AddForeignKey("dbo.Options", "Id", "dbo.Questions", "Id");
        }
    }
}
