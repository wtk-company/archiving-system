namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePartyIdAndDocKindId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "ConcernedParty_Id", "dbo.ConcernedParties");
            DropForeignKey("dbo.Documents", "DocumentKind_Id", "dbo.DocumentKinds");
            DropIndex("dbo.Documents", new[] { "ConcernedParty_Id" });
            DropIndex("dbo.Documents", new[] { "DocumentKind_Id" });
            DropColumn("dbo.Documents", "ConcernedParty_Id");
            DropColumn("dbo.Documents", "DocumentKind_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "DocumentKind_Id", c => c.Int());
            AddColumn("dbo.Documents", "ConcernedParty_Id", c => c.Int());
            CreateIndex("dbo.Documents", "DocumentKind_Id");
            CreateIndex("dbo.Documents", "ConcernedParty_Id");
            AddForeignKey("dbo.Documents", "DocumentKind_Id", "dbo.DocumentKinds", "Id");
            AddForeignKey("dbo.Documents", "ConcernedParty_Id", "dbo.ConcernedParties", "Id");
        }
    }
}
