namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adfforce : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "Subject", c => c.String(nullable: false));
            AlterColumn("dbo.Documents", "Description", c => c.String());
            AlterColumn("dbo.Documents", "DocumentNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documents", "DocumentNumber", c => c.String());
            AlterColumn("dbo.Documents", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Documents", "Subject", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
