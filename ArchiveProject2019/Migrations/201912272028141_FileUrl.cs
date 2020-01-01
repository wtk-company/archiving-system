namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SealFiles", "FileUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SealFiles", "FileUrl");
        }
    }
}
