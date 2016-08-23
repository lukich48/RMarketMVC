namespace RMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingsRename : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Settings", newName: "DataProviders");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DataProvider", newName: "Settings");
        }
    }
}
