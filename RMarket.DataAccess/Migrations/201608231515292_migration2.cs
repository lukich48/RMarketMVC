namespace RMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DataProvider1", newName: "DataProviders");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DataProviders", newName: "DataProvider1");
        }
    }
}
