namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeexamdatetimetonullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Exams", "ExamOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exams", "ExamOn", c => c.DateTime(nullable: false));
        }
    }
}
