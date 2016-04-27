namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changebooleansvalue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Options", "IsCorrect", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Options", "IsCorrect", c => c.Boolean());
        }
    }
}
