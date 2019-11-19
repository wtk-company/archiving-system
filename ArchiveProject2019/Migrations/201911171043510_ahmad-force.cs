namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahmadforce : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        EnableEdit = c.Boolean(nullable: false),
                        UpdatedAt = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.DocumentId)
                .Index(t => t.UserId)
                .Index(t => t.CreatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentUsers", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentUsers", "CreatedById", "dbo.AspNetUsers");
            DropIndex("dbo.DocumentUsers", new[] { "CreatedById" });
            DropIndex("dbo.DocumentUsers", new[] { "UserId" });
            DropIndex("dbo.DocumentUsers", new[] { "DocumentId" });
            DropTable("dbo.DocumentUsers");
        }
    }
}
