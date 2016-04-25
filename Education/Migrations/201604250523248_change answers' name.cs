namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeanswersname : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Answers", name: "Answer", newName: "MultipleAnswer");
            RenameColumn(table: "dbo.Answers", name: "Answer1", newName: "SingleAnswer");
            RenameColumn(table: "dbo.Answers", name: "Answer2", newName: "TrueOrFalseAnswer");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Answers", name: "TrueOrFalseAnswer", newName: "Answer2");
            RenameColumn(table: "dbo.Answers", name: "SingleAnswer", newName: "Answer1");
            RenameColumn(table: "dbo.Answers", name: "MultipleAnswer", newName: "Answer");
        }
    }
}
