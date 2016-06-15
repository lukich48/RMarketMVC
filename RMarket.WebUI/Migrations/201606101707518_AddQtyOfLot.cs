namespace RMarket.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQtyOfLot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickers", "QtyInLot", c => c.Int());
            AddColumn("dbo.Orders", "DateOpen", c => c.DateTime(nullable: false, precision: 0, storeType: "datetime2"));
            AddColumn("dbo.Orders", "DateOpenUTC", c => c.DateTime(nullable: false, precision: 3, storeType: "datetime2"));
            AddColumn("dbo.Orders", "DateClose", c => c.DateTime(nullable: false, precision: 0, storeType: "datetime2"));
            AddColumn("dbo.Orders", "DateCloseUTC", c => c.DateTime(nullable: false, precision: 3, storeType: "datetime2"));
            DropColumn("dbo.Orders", "DateOpenCandle");
            DropColumn("dbo.Orders", "DateOpenUTS");
            DropColumn("dbo.Orders", "DateCloseCandle");
            DropColumn("dbo.Orders", "DateCloseUTS");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "DateCloseUTS", c => c.DateTime(nullable: false, precision: 3, storeType: "datetime2"));
            AddColumn("dbo.Orders", "DateCloseCandle", c => c.DateTime(nullable: false, precision: 0, storeType: "datetime2"));
            AddColumn("dbo.Orders", "DateOpenUTS", c => c.DateTime(nullable: false, precision: 3, storeType: "datetime2"));
            AddColumn("dbo.Orders", "DateOpenCandle", c => c.DateTime(nullable: false, precision: 0, storeType: "datetime2"));
            DropColumn("dbo.Orders", "DateCloseUTC");
            DropColumn("dbo.Orders", "DateClose");
            DropColumn("dbo.Orders", "DateOpenUTC");
            DropColumn("dbo.Orders", "DateOpen");
            DropColumn("dbo.Tickers", "QtyInLot");
        }
    }
}
