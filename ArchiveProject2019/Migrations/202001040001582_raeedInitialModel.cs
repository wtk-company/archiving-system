namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class raeedInitialModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Documents", "IsGeneralize");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "IsGeneralize", c => c.Boolean(nullable: false));
        }
    }
}
