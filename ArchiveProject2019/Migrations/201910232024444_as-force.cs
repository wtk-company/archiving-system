namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asforce : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "NotificationDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "NotificationDate");
        }
    }
}
