namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahmadforce : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "FamelyState", c => c.Int());
            AlterColumn("dbo.Companies", "PhoneNumber1", c => c.String());
            AlterColumn("dbo.Companies", "PhoneNumber2", c => c.String());
            AlterColumn("dbo.Companies", "MobileNumber1", c => c.String());
            AlterColumn("dbo.Companies", "MobileNumber2", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "MobileNumber2", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "MobileNumber1", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "PhoneNumber2", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "PhoneNumber1", c => c.Int(nullable: false));
            DropColumn("dbo.Documents", "FamelyState");
        }
    }
}
