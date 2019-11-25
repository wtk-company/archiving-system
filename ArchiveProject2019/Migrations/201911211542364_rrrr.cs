namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rrrr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentTargetDepartments", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentTargetDepartments", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.DocumentTargetDepartments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentTargetGroups", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentTargetGroups", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentTargetGroups", "GroupId", "dbo.Groups");
            DropIndex("dbo.DocumentTargetDepartments", new[] { "DocumentId" });
            DropIndex("dbo.DocumentTargetDepartments", new[] { "DepartmentId" });
            DropIndex("dbo.DocumentTargetDepartments", new[] { "CreatedById" });
            DropIndex("dbo.DocumentTargetGroups", new[] { "DocumentId" });
            DropIndex("dbo.DocumentTargetGroups", new[] { "GroupId" });
            DropIndex("dbo.DocumentTargetGroups", new[] { "CreatedById" });
            DropTable("dbo.DocumentTargetDepartments");
            DropTable("dbo.DocumentTargetGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DocumentTargetGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentTargetDepartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.DocumentTargetGroups", "CreatedById");
            CreateIndex("dbo.DocumentTargetGroups", "GroupId");
            CreateIndex("dbo.DocumentTargetGroups", "DocumentId");
            CreateIndex("dbo.DocumentTargetDepartments", "CreatedById");
            CreateIndex("dbo.DocumentTargetDepartments", "DepartmentId");
            CreateIndex("dbo.DocumentTargetDepartments", "DocumentId");
            AddForeignKey("dbo.DocumentTargetGroups", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DocumentTargetGroups", "DocumentId", "dbo.Documents", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DocumentTargetGroups", "CreatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DocumentTargetDepartments", "DocumentId", "dbo.Documents", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DocumentTargetDepartments", "DepartmentId", "dbo.Departments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DocumentTargetDepartments", "CreatedById", "dbo.AspNetUsers", "Id");
        }
    }
}
