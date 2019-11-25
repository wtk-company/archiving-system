namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyUpdate6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "PhoneNumber1", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "PhoneNumber2", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "MobileNumber1", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "MobileNumber2", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "MobileNumber2", c => c.String());
            AlterColumn("dbo.Companies", "MobileNumber1", c => c.String());
            AlterColumn("dbo.Companies", "PhoneNumber2", c => c.String());
            AlterColumn("dbo.Companies", "PhoneNumber1", c => c.String());
        }
    }
}
