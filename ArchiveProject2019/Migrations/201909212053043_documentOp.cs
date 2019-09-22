namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documentOp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentDepartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: false)
                .Index(t => t.DocumentId)
                .Index(t => t.DepartmentId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.DocumentGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.DocumentId)
                .Index(t => t.GroupId)
                .Index(t => t.CreatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.DocumentGroups", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentGroups", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentDepartments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentDepartments", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.DocumentDepartments", "CreatedById", "dbo.AspNetUsers");
            DropIndex("dbo.DocumentGroups", new[] { "CreatedById" });
            DropIndex("dbo.DocumentGroups", new[] { "GroupId" });
            DropIndex("dbo.DocumentGroups", new[] { "DocumentId" });
            DropIndex("dbo.DocumentDepartments", new[] { "CreatedById" });
            DropIndex("dbo.DocumentDepartments", new[] { "DepartmentId" });
            DropIndex("dbo.DocumentDepartments", new[] { "DocumentId" });
            DropTable("dbo.DocumentGroups");
            DropTable("dbo.DocumentDepartments");
        }
    }
}
