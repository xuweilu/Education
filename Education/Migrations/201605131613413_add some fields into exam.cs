namespace Education.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsomefieldsintoexam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "Address", c => c.String());
            AddColumn("dbo.Exams", "ExamName", c => c.String());
            AlterColumn("dbo.Exams", "ExamOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exams", "ExamOn", c => c.DateTime());
            DropColumn("dbo.Exams", "ExamName");
            DropColumn("dbo.Exams", "Address");
        }
    }
}
