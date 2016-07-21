namespace RMarket.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameSettingType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "SettingType", c => c.Int(nullable: false));
            DropColumn("dbo.Settings", "TypeSetting");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "TypeSetting", c => c.Int(nullable: false));
            DropColumn("dbo.Settings", "SettingType");
        }
    }
}
