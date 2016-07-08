namespace RMarket.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateClose_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DateClose", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.Orders", "DateCloseUTC", c => c.DateTime(precision: 3, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DateCloseUTC", c => c.DateTime(nullable: false, precision: 3, storeType: "datetime2"));
            AlterColumn("dbo.Orders", "DateClose", c => c.DateTime(nullable: false, precision: 0, storeType: "datetime2"));
        }
    }
}
