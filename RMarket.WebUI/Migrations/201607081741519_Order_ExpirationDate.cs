namespace RMarket.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order_ExpirationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ExpirationDate", c => c.DateTime(precision: 0, storeType: "datetime2"));
            DropColumn("dbo.Orders", "Expiration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Expiration", c => c.DateTime(nullable: false));
            DropColumn("dbo.Orders", "ExpirationDate");
        }
    }
}
