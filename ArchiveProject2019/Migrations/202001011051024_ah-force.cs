namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahforce : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "IsGeneralize", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "IsGeneralize");
        }
    }
}
