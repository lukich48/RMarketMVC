namespace RMarket.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Tickers", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tickers", new[] { "Code" });
        }
    }
}
