namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypeOfMail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "TypeOfMail", c => c.String());
            AddColumn("dbo.Documents", "MailingNumber", c => c.String());
            AddColumn("dbo.Documents", "MailingDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "MailingDate");
            DropColumn("dbo.Documents", "MailingNumber");
            DropColumn("dbo.Documents", "TypeOfMail");
        }
    }
}
