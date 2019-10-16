namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _as : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConcernedParties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Gender = c.String(),
                        DepartmentId = c.Int(),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        RoleName = c.String(),
                        JobTitleId = c.Int(),
                        UpdatedAt = c.String(),
                        IsDefaultMaster = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Department_Id = c.Int(),
                        JobTitle_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.JobTitles", t => t.JobTitle_Id)
                .ForeignKey("dbo.JobTitles", t => t.JobTitleId)
                .Index(t => t.DepartmentId)
                .Index(t => t.CreatedById)
                .Index(t => t.JobTitleId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Department_Id)
                .Index(t => t.JobTitle_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.ParentId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.JobTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Symbol = c.String(maxLength: 50),
                        MaximumMember = c.Int(nullable: false),
                        TypeOfDisplayForm = c.Int(nullable: false),
                        TypeOfDisplayDocument = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .Index(t => t.DocumentId)
                .Index(t => t.DepartmentId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileUrl = c.String(),
                        Subject = c.String(nullable: false, maxLength: 50),
                        KindId = c.Int(nullable: false),
                        MailingNumber = c.String(),
                        MailingDate = c.String(),
                        Party = c.String(),
                        Description = c.String(maxLength: 1000),
                        DocumentDate = c.String(),
                        CreatedAt = c.String(),
                        DocumentNumber = c.String(),
                        Address = c.String(),
                        Notes = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        DepartmentId = c.Int(nullable: false),
                        FormId = c.Int(nullable: false),
                        TypeMailId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: false)
                .ForeignKey("dbo.DocumentKinds", t => t.KindId, cascadeDelete: true)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.TypMails", t => t.TypeMailId, cascadeDelete: true)
                .Index(t => t.KindId)
                .Index(t => t.CreatedById)
                .Index(t => t.DepartmentId)
                .Index(t => t.FormId)
                .Index(t => t.TypeMailId);
            
            CreateTable(
                "dbo.DocumentKinds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        Type = c.String(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                        FormId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .Index(t => t.CreatedById)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.Values",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldValue = c.String(),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        FieldId = c.Int(nullable: false),
                        Document_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Documents", t => t.Document_id, cascadeDelete: true)
                .ForeignKey("dbo.Fields", t => t.FieldId, cascadeDelete: false)
                .Index(t => t.CreatedById)
                .Index(t => t.FieldId)
                .Index(t => t.Document_id);
            
            CreateTable(
                "dbo.RelatedDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RelatedDocId = c.Int(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        Document_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Documents", t => t.Document_id, cascadeDelete: true)
                .Index(t => t.CreatedById)
                .Index(t => t.Document_id);
            
            CreateTable(
                "dbo.TypMails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
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
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 50),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                        UpdatedById = c.String(maxLength: 128),
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedById)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FormDepartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Is_Active = c.Boolean(nullable: false),
                        FormId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        Updatedat = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .Index(t => t.FormId)
                .Index(t => t.DepartmentId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.FormGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Is_Active = c.Boolean(nullable: false),
                        FormId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        Updatedat = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.FormId)
                .Index(t => t.GroupId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.PermissionRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Is_Active = c.Boolean(nullable: false),
                        PermissionId = c.Int(nullable: false),
                        RoleId = c.String(maxLength: 128),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        Updatedat = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Permissions", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.PermissionId)
                .Index(t => t.RoleId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        UpdatedAt = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex")
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.PermissionsUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Is_Active = c.Boolean(nullable: false),
                        PermissionId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CreatedAt = c.String(),
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Permissions", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PermissionId)
                .Index(t => t.UserId)
                .Index(t => t.CreatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PermissionsUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PermissionsUsers", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.PermissionsUsers", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.PermissionRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetRoles", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.PermissionRoles", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.PermissionRoles", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.FormGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.FormGroups", "FormId", "dbo.Forms");
            DropForeignKey("dbo.FormGroups", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.FormDepartments", "FormId", "dbo.Forms");
            DropForeignKey("dbo.FormDepartments", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.FormDepartments", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGroups", "UpdatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentGroups", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentGroups", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentDepartments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "TypeMailId", "dbo.TypMails");
            DropForeignKey("dbo.TypMails", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.RelatedDocuments", "Document_id", "dbo.Documents");
            DropForeignKey("dbo.RelatedDocuments", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Values", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.Values", "Document_id", "dbo.Documents");
            DropForeignKey("dbo.Values", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Fields", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Fields", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Forms", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "KindId", "dbo.DocumentKinds");
            DropForeignKey("dbo.DocumentKinds", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Documents", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentDepartments", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.DocumentDepartments", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConcernedParties", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "JobTitleId", "dbo.JobTitles");
            DropForeignKey("dbo.AspNetUsers", "JobTitle_Id", "dbo.JobTitles");
            DropForeignKey("dbo.JobTitles", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.AspNetUsers", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "ParentId", "dbo.Departments");
            DropForeignKey("dbo.AspNetUsers", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PermissionsUsers", new[] { "CreatedById" });
            DropIndex("dbo.PermissionsUsers", new[] { "UserId" });
            DropIndex("dbo.PermissionsUsers", new[] { "PermissionId" });
            DropIndex("dbo.AspNetRoles", new[] { "CreatedById" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PermissionRoles", new[] { "CreatedById" });
            DropIndex("dbo.PermissionRoles", new[] { "RoleId" });
            DropIndex("dbo.PermissionRoles", new[] { "PermissionId" });
            DropIndex("dbo.FormGroups", new[] { "CreatedById" });
            DropIndex("dbo.FormGroups", new[] { "GroupId" });
            DropIndex("dbo.FormGroups", new[] { "FormId" });
            DropIndex("dbo.FormDepartments", new[] { "CreatedById" });
            DropIndex("dbo.FormDepartments", new[] { "DepartmentId" });
            DropIndex("dbo.FormDepartments", new[] { "FormId" });
            DropIndex("dbo.UserGroups", new[] { "UserId" });
            DropIndex("dbo.UserGroups", new[] { "GroupId" });
            DropIndex("dbo.UserGroups", new[] { "UpdatedById" });
            DropIndex("dbo.UserGroups", new[] { "CreatedById" });
            DropIndex("dbo.Groups", new[] { "CreatedById" });
            DropIndex("dbo.DocumentGroups", new[] { "CreatedById" });
            DropIndex("dbo.DocumentGroups", new[] { "GroupId" });
            DropIndex("dbo.DocumentGroups", new[] { "DocumentId" });
            DropIndex("dbo.TypMails", new[] { "CreatedById" });
            DropIndex("dbo.RelatedDocuments", new[] { "Document_id" });
            DropIndex("dbo.RelatedDocuments", new[] { "CreatedById" });
            DropIndex("dbo.Values", new[] { "Document_id" });
            DropIndex("dbo.Values", new[] { "FieldId" });
            DropIndex("dbo.Values", new[] { "CreatedById" });
            DropIndex("dbo.Fields", new[] { "FormId" });
            DropIndex("dbo.Fields", new[] { "CreatedById" });
            DropIndex("dbo.Forms", new[] { "CreatedById" });
            DropIndex("dbo.DocumentKinds", new[] { "CreatedById" });
            DropIndex("dbo.Documents", new[] { "TypeMailId" });
            DropIndex("dbo.Documents", new[] { "FormId" });
            DropIndex("dbo.Documents", new[] { "DepartmentId" });
            DropIndex("dbo.Documents", new[] { "CreatedById" });
            DropIndex("dbo.Documents", new[] { "KindId" });
            DropIndex("dbo.DocumentDepartments", new[] { "CreatedById" });
            DropIndex("dbo.DocumentDepartments", new[] { "DepartmentId" });
            DropIndex("dbo.DocumentDepartments", new[] { "DocumentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.JobTitles", new[] { "CreatedById" });
            DropIndex("dbo.Departments", new[] { "ParentId" });
            DropIndex("dbo.Departments", new[] { "CreatedById" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "JobTitle_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Department_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "JobTitleId" });
            DropIndex("dbo.AspNetUsers", new[] { "CreatedById" });
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentId" });
            DropIndex("dbo.ConcernedParties", new[] { "CreatedById" });
            DropTable("dbo.PermissionsUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Permissions");
            DropTable("dbo.PermissionRoles");
            DropTable("dbo.FormGroups");
            DropTable("dbo.FormDepartments");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Groups");
            DropTable("dbo.DocumentGroups");
            DropTable("dbo.TypMails");
            DropTable("dbo.RelatedDocuments");
            DropTable("dbo.Values");
            DropTable("dbo.Fields");
            DropTable("dbo.Forms");
            DropTable("dbo.DocumentKinds");
            DropTable("dbo.Documents");
            DropTable("dbo.DocumentDepartments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.JobTitles");
            DropTable("dbo.Departments");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ConcernedParties");
        }
    }
}
