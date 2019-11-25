namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyCompanies", "PhoneNumber1", c => c.String());
            AddColumn("dbo.CompanyCompanies", "PhoneNumber2", c => c.String());
            AddColumn("dbo.CompanyCompanies", "MobileNumber1", c => c.String());
            AddColumn("dbo.CompanyCompanies", "MobileNumber2", c => c.String());
            DropColumn("dbo.CompanyCompanies", "PhoneNumber");
            DropColumn("dbo.CompanyCompanies", "MobileNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanyCompanies", "MobileNumber", c => c.String());
            AddColumn("dbo.CompanyCompanies", "PhoneNumber", c => c.String());
            DropColumn("dbo.CompanyCompanies", "MobileNumber2");
            DropColumn("dbo.CompanyCompanies", "MobileNumber1");
            DropColumn("dbo.CompanyCompanies", "PhoneNumber2");
            DropColumn("dbo.CompanyCompanies", "PhoneNumber1");
        }
    }
}
