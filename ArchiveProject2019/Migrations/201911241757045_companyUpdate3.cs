namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyUpdate3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.WTKCompanies", newName: "CompanyCompanies");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CompanyCompanies", newName: "WTKCompanies");
        }
    }
}
