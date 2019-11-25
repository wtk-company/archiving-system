namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class company : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        MobileNumber = c.String(),
                        Mail = c.String(),
                        Logo = c.Int(nullable: false),
                        CreatedAt = c.String(),
                        CreateById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreateById)
                .Index(t => t.CreateById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyCompanies", "CreateById", "dbo.AspNetUsers");
            DropIndex("dbo.CompanyCompanies", new[] { "CreateById" });
            DropTable("dbo.CompanyCompanies");
        }
    }
}
