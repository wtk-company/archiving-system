namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyUpdate8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CompanyDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "CompanyDate");
        }
    }
}
