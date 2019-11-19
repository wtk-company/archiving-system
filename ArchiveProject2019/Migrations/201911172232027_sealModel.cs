namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sealModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SealDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        FileName = c.String(),
                        File = c.Binary(),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .Index(t => t.CreatedById)
                .Index(t => t.DocumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SealDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.SealDocuments", "CreatedById", "dbo.AspNetUsers");
            DropIndex("dbo.SealDocuments", new[] { "DocumentId" });
            DropIndex("dbo.SealDocuments", new[] { "CreatedById" });
            DropTable("dbo.SealDocuments");
        }
    }
}
