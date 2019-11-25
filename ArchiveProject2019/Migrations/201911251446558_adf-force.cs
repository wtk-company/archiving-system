namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adfforce : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Forms", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Forms", "Type");
        }
    }
}
