namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onlyoneoption : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Options", name: "QuestionId", newName: "Id");
            RenameIndex(table: "dbo.Options", name: "IX_QuestionId", newName: "IX_Id");
            DropColumn("dbo.Questions", "SingleCorrectAnswer");
            DropColumn("dbo.Options", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Options", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Questions", "SingleCorrectAnswer", c => c.Int());
            RenameIndex(table: "dbo.Options", name: "IX_Id", newName: "IX_QuestionId");
            RenameColumn(table: "dbo.Options", name: "Id", newName: "QuestionId");
        }
    }
}
