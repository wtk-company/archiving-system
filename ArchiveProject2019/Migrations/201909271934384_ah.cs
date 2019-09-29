namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ah : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FormDepartments", "UpdatedById", "dbo.AspNetUsers");
            DropIndex("dbo.FormDepartments", new[] { "UpdatedById" });
            DropColumn("dbo.FormDepartments", "UpdatedById");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FormDepartments", "UpdatedById", c => c.String(maxLength: 128));
            CreateIndex("dbo.FormDepartments", "UpdatedById");
            AddForeignKey("dbo.FormDepartments", "UpdatedById", "dbo.AspNetUsers", "Id");
        }
    }
}
