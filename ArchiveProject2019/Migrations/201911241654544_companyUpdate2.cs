namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyUpdate2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompanyCompanies", "Logo", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyCompanies", "Logo", c => c.Int(nullable: false));
        }
    }
}
