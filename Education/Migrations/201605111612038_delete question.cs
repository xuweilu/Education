namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletequestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Paper_Id", c => c.Guid());
            AddColumn("dbo.Questions", "Paper_Id1", c => c.Guid());
            CreateIndex("dbo.Questions", "Paper_Id");
            CreateIndex("dbo.Questions", "Paper_Id1");
            AddForeignKey("dbo.Questions", "Paper_Id", "dbo.Papers", "Id");
            AddForeignKey("dbo.Questions", "Paper_Id1", "dbo.Papers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Paper_Id1", "dbo.Papers");
            DropForeignKey("dbo.Questions", "Paper_Id", "dbo.Papers");
            DropIndex("dbo.Questions", new[] { "Paper_Id1" });
            DropIndex("dbo.Questions", new[] { "Paper_Id" });
            DropColumn("dbo.Questions", "Paper_Id1");
            DropColumn("dbo.Questions", "Paper_Id");
        }
    }
}
