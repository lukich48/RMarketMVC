namespace RMarket.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AliveStrategies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupID = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsReal = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false, precision: 3, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Candles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TickerId = c.Int(nullable: false),
                        TimeFrameId = c.Int(nullable: false),
                        DateOpen = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
                        OpenPrice = c.Decimal(nullable: false, precision: 19, scale: 7),
                        HighPrice = c.Decimal(nullable: false, precision: 19, scale: 7),
                        LowPrice = c.Decimal(nullable: false, precision: 19, scale: 7),
                        ClosePrice = c.Decimal(nullable: false, precision: 19, scale: 7),
                        Volume = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false, precision: 3, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickers", t => t.TickerId, cascadeDelete: true)
                .ForeignKey("dbo.TimeFrames", t => t.TimeFrameId, cascadeDelete: true)
                .Index(t => t.TickerId)
                .Index(t => t.TimeFrameId);
            
            CreateTable(
                "dbo.Tickers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Code = c.String(nullable: false, maxLength: 50),
                        CodeFinam = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeFrames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        ToMinute = c.Int(nullable: false),
                        CodeFinam = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConnectorInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        TypeName = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Instances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        StrategyInfoId = c.Int(nullable: false),
                        TickerId = c.Int(nullable: false),
                        TimeFrameId = c.Int(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 19, scale: 2),
                        Slippage = c.Decimal(nullable: false, precision: 19, scale: 7),
                        Rent = c.Decimal(nullable: false, precision: 6, scale: 4),
                        Description = c.String(maxLength: 2000),
                        GroupID = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        StrParams = c.String(),
                        SelectionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Selections", t => t.SelectionId)
                .ForeignKey("dbo.StrategyInfoes", t => t.StrategyInfoId, cascadeDelete: true)
                .ForeignKey("dbo.Tickers", t => t.TickerId, cascadeDelete: true)
                .ForeignKey("dbo.TimeFrames", t => t.TimeFrameId, cascadeDelete: true)
                .Index(t => t.StrategyInfoId)
                .Index(t => t.TickerId)
                .Index(t => t.TimeFrameId)
                .Index(t => t.SelectionId);
            
            CreateTable(
                "dbo.Selections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        StrategyInfoId = c.Int(nullable: false),
                        TickerId = c.Int(nullable: false),
                        TimeFrameId = c.Int(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 19, scale: 2),
                        Slippage = c.Decimal(nullable: false, precision: 19, scale: 7),
                        Rent = c.Decimal(nullable: false, precision: 6, scale: 4),
                        Description = c.String(maxLength: 2000),
                        GroupID = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        AmountResults = c.Int(nullable: false),
                        StrParams = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StrategyInfoes", t => t.StrategyInfoId, cascadeDelete: true)
                .ForeignKey("dbo.Tickers", t => t.TickerId, cascadeDelete: true)
                .ForeignKey("dbo.TimeFrames", t => t.TimeFrameId, cascadeDelete: true)
                .Index(t => t.StrategyInfoId)
                .Index(t => t.TickerId)
                .Index(t => t.TimeFrameId);
            
            CreateTable(
                "dbo.StrategyInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        TypeName = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TickerCode = c.String(),
                        OrderType = c.Int(nullable: false),
                        TakeProfit = c.Decimal(nullable: false, precision: 19, scale: 7),
                        StopLoss = c.Decimal(nullable: false, precision: 19, scale: 7),
                        DateOpenCandle = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
                        DateOpenUTS = c.DateTime(nullable: false, precision: 3, storeType: "datetime2"),
                        Expiration = c.DateTime(nullable: false),
                        DateCloseCandle = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
                        DateCloseUTS = c.DateTime(nullable: false, precision: 3, storeType: "datetime2"),
                        Volume = c.Int(nullable: false),
                        PriceOpen = c.Decimal(nullable: false, precision: 19, scale: 7),
                        PriceClose = c.Decimal(nullable: false, precision: 19, scale: 7),
                        Profit = c.Decimal(nullable: false, precision: 19, scale: 2),
                        Comment = c.String(),
                        AliveStrategyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AliveStrategies", t => t.AliveStrategyId, cascadeDelete: true)
                .Index(t => t.AliveStrategyId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        StrategyInfoId = c.Int(),
                        TypeSetting = c.Int(nullable: false),
                        EntityInfoId = c.Int(nullable: false),
                        Description = c.String(maxLength: 2000),
                        Priority = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false, precision: 3, storeType: "datetime2"),
                        StrParams = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StrategyInfoes", t => t.StrategyInfoId)
                .Index(t => t.StrategyInfoId);
            
            CreateTable(
                "dbo.Ticks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TickerCode = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        TradePeriod = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TickKeyValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TickId = c.Int(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ticks", t => t.TickId, cascadeDelete: true)
                .Index(t => t.TickId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TickKeyValue", "TickId", "dbo.Ticks");
            DropForeignKey("dbo.Settings", "StrategyInfoId", "dbo.StrategyInfoes");
            DropForeignKey("dbo.Orders", "AliveStrategyId", "dbo.AliveStrategies");
            DropForeignKey("dbo.Instances", "TimeFrameId", "dbo.TimeFrames");
            DropForeignKey("dbo.Instances", "TickerId", "dbo.Tickers");
            DropForeignKey("dbo.Instances", "StrategyInfoId", "dbo.StrategyInfoes");
            DropForeignKey("dbo.Instances", "SelectionId", "dbo.Selections");
            DropForeignKey("dbo.Selections", "TimeFrameId", "dbo.TimeFrames");
            DropForeignKey("dbo.Selections", "TickerId", "dbo.Tickers");
            DropForeignKey("dbo.Selections", "StrategyInfoId", "dbo.StrategyInfoes");
            DropForeignKey("dbo.Candles", "TimeFrameId", "dbo.TimeFrames");
            DropForeignKey("dbo.Candles", "TickerId", "dbo.Tickers");
            DropIndex("dbo.TickKeyValue", new[] { "TickId" });
            DropIndex("dbo.Settings", new[] { "StrategyInfoId" });
            DropIndex("dbo.Orders", new[] { "AliveStrategyId" });
            DropIndex("dbo.Selections", new[] { "TimeFrameId" });
            DropIndex("dbo.Selections", new[] { "TickerId" });
            DropIndex("dbo.Selections", new[] { "StrategyInfoId" });
            DropIndex("dbo.Instances", new[] { "SelectionId" });
            DropIndex("dbo.Instances", new[] { "TimeFrameId" });
            DropIndex("dbo.Instances", new[] { "TickerId" });
            DropIndex("dbo.Instances", new[] { "StrategyInfoId" });
            DropIndex("dbo.Candles", new[] { "TimeFrameId" });
            DropIndex("dbo.Candles", new[] { "TickerId" });
            DropTable("dbo.TickKeyValue");
            DropTable("dbo.Ticks");
            DropTable("dbo.Settings");
            DropTable("dbo.Orders");
            DropTable("dbo.StrategyInfoes");
            DropTable("dbo.Selections");
            DropTable("dbo.Instances");
            DropTable("dbo.ConnectorInfoes");
            DropTable("dbo.TimeFrames");
            DropTable("dbo.Tickers");
            DropTable("dbo.Candles");
            DropTable("dbo.AliveStrategies");
        }
    }
}
