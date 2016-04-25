namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changethenameofsomefields : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Papers", name: "Teacher_Id", newName: "TeacherId");
            RenameIndex(table: "dbo.Papers", name: "IX_Teacher_Id", newName: "IX_TeacherId");
            AddColumn("dbo.Questions", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "Content", c => c.String());
            DropColumn("dbo.Questions", "QuestionType");
            DropColumn("dbo.Questions", "QuestionContent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "QuestionContent", c => c.String());
            AddColumn("dbo.Questions", "QuestionType", c => c.Int(nullable: false));
            DropColumn("dbo.Questions", "Content");
            DropColumn("dbo.Questions", "Type");
            RenameIndex(table: "dbo.Papers", name: "IX_TeacherId", newName: "IX_Teacher_Id");
            RenameColumn(table: "dbo.Papers", name: "TeacherId", newName: "Teacher_Id");
        }
    }
}
