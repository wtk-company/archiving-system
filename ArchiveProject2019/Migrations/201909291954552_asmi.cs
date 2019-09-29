namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asmi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TypeOfMails", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "TypeOfMailId", "dbo.TypeOfMails");
            DropForeignKey("dbo.TypeOfMails", "UpdatedById", "dbo.AspNetUsers");
            DropIndex("dbo.Documents", new[] { "TypeOfMailId" });
            DropIndex("dbo.TypeOfMails", new[] { "CreatedById" });
            DropIndex("dbo.TypeOfMails", new[] { "UpdatedById" });
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
            CreateIndex("dbo.TypeOfMails", "UpdatedById");
            CreateIndex("dbo.TypeOfMails", "CreatedById");
            CreateIndex("dbo.Documents", "TypeOfMailId");
            AddForeignKey("dbo.TypeOfMails", "UpdatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Documents", "TypeOfMailId", "dbo.TypeOfMails", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TypeOfMails", "CreatedById", "dbo.AspNetUsers", "Id");
        }
    }
}
