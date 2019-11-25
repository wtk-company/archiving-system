namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyUpdate5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "Description");
        }
    }
}
