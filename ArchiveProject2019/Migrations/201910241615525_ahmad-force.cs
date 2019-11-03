namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahmadforce : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "NotificationOwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notifications", "NotificationOwnerId");
            AddForeignKey("dbo.Notifications", "NotificationOwnerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "NotificationOwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "NotificationOwnerId" });
            DropColumn("dbo.Notifications", "NotificationOwnerId");
        }
    }
}
