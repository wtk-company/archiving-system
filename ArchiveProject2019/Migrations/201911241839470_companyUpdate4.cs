namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyUpdate4 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CompanyCompanies", newName: "Companies");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Companies", newName: "CompanyCompanies");
        }
    }
}
