namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValueDocuemnt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Values", "Docuemnt_Id", "dbo.Documents");
            DropIndex("dbo.Values", new[] { "Docuemnt_Id" });
            DropColumn("dbo.Values", "Document_id");
            RenameColumn(table: "dbo.Values", name: "Docuemnt_Id", newName: "Document_id");
            AlterColumn("dbo.Values", "Document_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Values", "Document_id");
            AddForeignKey("dbo.Values", "Document_id", "dbo.Documents", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Values", "Document_id", "dbo.Documents");
            DropIndex("dbo.Values", new[] { "Document_id" });
            AlterColumn("dbo.Values", "Document_id", c => c.Int());
            RenameColumn(table: "dbo.Values", name: "Document_id", newName: "Docuemnt_Id");
            AddColumn("dbo.Values", "Document_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Values", "Docuemnt_Id");
            AddForeignKey("dbo.Values", "Docuemnt_Id", "dbo.Documents", "Id");
        }
    }
}
