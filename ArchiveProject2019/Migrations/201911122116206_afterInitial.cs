namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class afterInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReplayDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplayDocId = c.Int(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        Document_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Documents", t => t.Document_id, cascadeDelete: true)
                .Index(t => t.CreatedById)
                .Index(t => t.Document_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReplayDocuments", "Document_id", "dbo.Documents");
            DropForeignKey("dbo.ReplayDocuments", "CreatedById", "dbo.AspNetUsers");
            DropIndex("dbo.ReplayDocuments", new[] { "Document_id" });
            DropIndex("dbo.ReplayDocuments", new[] { "CreatedById" });
            DropTable("dbo.ReplayDocuments");
        }
    }
}
