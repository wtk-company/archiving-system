namespace ArchiveProject2019.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahmadforce : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentDepartments", "EnableReplay", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentDepartments", "EnableSeal", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentDepartments", "EnableRelate", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentGroups", "EnableReplay", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentGroups", "EnableSeal", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentGroups", "EnableRelate", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentUsers", "EnableReplay", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentUsers", "EnableSeal", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentUsers", "EnableRelate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentUsers", "EnableRelate");
            DropColumn("dbo.DocumentUsers", "EnableSeal");
            DropColumn("dbo.DocumentUsers", "EnableReplay");
            DropColumn("dbo.DocumentGroups", "EnableRelate");
            DropColumn("dbo.DocumentGroups", "EnableSeal");
            DropColumn("dbo.DocumentGroups", "EnableReplay");
            DropColumn("dbo.DocumentDepartments", "EnableRelate");
            DropColumn("dbo.DocumentDepartments", "EnableSeal");
            DropColumn("dbo.DocumentDepartments", "EnableReplay");
        }
    }
}
