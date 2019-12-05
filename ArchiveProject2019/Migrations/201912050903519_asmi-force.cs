namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asmiforce : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavouriteForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        FormId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.FormId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavouriteForms", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FavouriteForms", "FormId", "dbo.Forms");
            DropIndex("dbo.FavouriteForms", new[] { "FormId" });
            DropIndex("dbo.FavouriteForms", new[] { "UserId" });
            DropTable("dbo.FavouriteForms");
        }
    }
}
