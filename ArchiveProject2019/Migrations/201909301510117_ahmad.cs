namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahmad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "KindId", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "KindId");
            AddForeignKey("dbo.Documents", "KindId", "dbo.DocumentKinds", "Id", cascadeDelete: true);
            DropColumn("dbo.Documents", "Kind");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "Kind", c => c.String());
            DropForeignKey("dbo.Documents", "KindId", "dbo.DocumentKinds");
            DropIndex("dbo.Documents", new[] { "KindId" });
            DropColumn("dbo.Documents", "KindId");
        }
    }
}
