namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revise : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Paper_Id", "dbo.Papers");
            DropForeignKey("dbo.Questions", "Paper_Id1", "dbo.Papers");
            DropIndex("dbo.Questions", new[] { "Paper_Id" });
            DropIndex("dbo.Questions", new[] { "Paper_Id1" });
            DropColumn("dbo.Questions", "Paper_Id");
            DropColumn("dbo.Questions", "Paper_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Paper_Id1", c => c.Guid());
            AddColumn("dbo.Questions", "Paper_Id", c => c.Guid());
            CreateIndex("dbo.Questions", "Paper_Id1");
            CreateIndex("dbo.Questions", "Paper_Id");
            AddForeignKey("dbo.Questions", "Paper_Id1", "dbo.Papers", "Id");
            AddForeignKey("dbo.Questions", "Paper_Id", "dbo.Papers", "Id");
        }
    }
}
