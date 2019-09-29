namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class afdsfdf : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TypeOfMails", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "TypeOfMailId", "dbo.TypeOfMails");
            DropForeignKey("dbo.TypeOfMails", "UpdatedById", "dbo.AspNetUsers");
            DropIndex("dbo.Documents", new[] { "TypeOfMailId" });
            DropIndex("dbo.TypeOfMails", new[] { "CreatedById" });
            DropIndex("dbo.TypeOfMails", new[] { "UpdatedById" });
            AddColumn("dbo.ConcernedParties", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "UpdatedById", c => c.String());
            AddColumn("dbo.Departments", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.JobTitles", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.Forms", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.Fields", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.Groups", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.DocumentKinds", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.FormDepartments", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.FormGroups", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.PermissionRoles", "UpdatedById", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetRoles", "UpdatedById", c => c.String(maxLength: 128));
            AlterColumn("dbo.ConcernedParties", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.DocumentKinds", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.ConcernedParties", "UpdatedById");
            CreateIndex("dbo.Departments", "UpdatedById");
            CreateIndex("dbo.JobTitles", "UpdatedById");
            CreateIndex("dbo.Forms", "UpdatedById");
            CreateIndex("dbo.Fields", "UpdatedById");
            CreateIndex("dbo.Groups", "UpdatedById");
            CreateIndex("dbo.DocumentKinds", "UpdatedById");
            CreateIndex("dbo.FormDepartments", "UpdatedById");
            CreateIndex("dbo.FormGroups", "UpdatedById");
            CreateIndex("dbo.PermissionRoles", "UpdatedById");
            CreateIndex("dbo.AspNetRoles", "UpdatedById");
            AddForeignKey("dbo.Departments", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.JobTitles", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ConcernedParties", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Fields", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Forms", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Groups", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DocumentKinds", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FormDepartments", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FormGroups", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetRoles", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PermissionRoles", "UpdatedById", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Documents", "TypeOfMailId");
            DropTable("dbo.TypeOfMails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TypeOfMails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        Type = c.Int(nullable: false),
                        UpdatedAt = c.String(),
                        UpdatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Documents", "TypeOfMailId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PermissionRoles", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetRoles", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.FormGroups", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.FormDepartments", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentKinds", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Forms", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Fields", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConcernedParties", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobTitles", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "UpdatedById", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", new[] { "UpdatedById" });
            DropIndex("dbo.PermissionRoles", new[] { "UpdatedById" });
            DropIndex("dbo.FormGroups", new[] { "UpdatedById" });
            DropIndex("dbo.FormDepartments", new[] { "UpdatedById" });
            DropIndex("dbo.DocumentKinds", new[] { "UpdatedById" });
            DropIndex("dbo.Groups", new[] { "UpdatedById" });
            DropIndex("dbo.Fields", new[] { "UpdatedById" });
            DropIndex("dbo.Forms", new[] { "UpdatedById" });
            DropIndex("dbo.JobTitles", new[] { "UpdatedById" });
            DropIndex("dbo.Departments", new[] { "UpdatedById" });
            DropIndex("dbo.ConcernedParties", new[] { "UpdatedById" });
            AlterColumn("dbo.DocumentKinds", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ConcernedParties", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.AspNetRoles", "UpdatedById");
            DropColumn("dbo.PermissionRoles", "UpdatedById");
            DropColumn("dbo.FormGroups", "UpdatedById");
            DropColumn("dbo.FormDepartments", "UpdatedById");
            DropColumn("dbo.DocumentKinds", "UpdatedById");
            DropColumn("dbo.Groups", "UpdatedById");
            DropColumn("dbo.Fields", "UpdatedById");
            DropColumn("dbo.Forms", "UpdatedById");
            DropColumn("dbo.JobTitles", "UpdatedById");
            DropColumn("dbo.Departments", "UpdatedById");
            DropColumn("dbo.AspNetUsers", "UpdatedById");
            DropColumn("dbo.ConcernedParties", "UpdatedById");
            CreateIndex("dbo.TypeOfMails", "UpdatedById");
            CreateIndex("dbo.TypeOfMails", "CreatedById");
            CreateIndex("dbo.Documents", "TypeOfMailId");
            AddForeignKey("dbo.TypeOfMails", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Documents", "TypeOfMailId", "dbo.TypeOfMails", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TypeOfMails", "CreatedById", "dbo.AspNetUsers", "Id");
        }
    }
}
