namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahforce : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SealFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        File = c.Binary(),
                        FileUrl = c.String(),
                        SealId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SealDocuments", t => t.SealId, cascadeDelete: true)
                .Index(t => t.SealId);
            
            AlterColumn("dbo.SealDocuments", "Message", c => c.String(nullable: false));
            DropColumn("dbo.SealDocuments", "File");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SealDocuments", "File", c => c.Binary());
            DropForeignKey("dbo.SealFiles", "SealId", "dbo.SealDocuments");
            DropIndex("dbo.SealFiles", new[] { "SealId" });
            AlterColumn("dbo.SealDocuments", "Message", c => c.String());
            DropTable("dbo.SealFiles");
        }
    }
}
