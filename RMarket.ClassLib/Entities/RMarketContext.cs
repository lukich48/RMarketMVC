using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Helpers;
using Ninject;
using Ninject.Web.Common;

namespace RMarket.ClassLib.Entities
{
    public partial class RMarketContext : DbContext
    {
        public RMarketContext()
            : base("name=RMarket")
        { }

        public virtual DbSet<Instance> Instances { get; set; }
        public virtual DbSet<StrategyInfo> StrategyInfoes { get; set; }
        public virtual DbSet<Ticker> Tickers { get; set; }
        public virtual DbSet<TimeFrame> TimeFrames { get; set; }
        public virtual DbSet<Candle> Candles { get; set; }
        public virtual DbSet<Selection> Selections { get; set; }
        public virtual DbSet<ConnectorInfo> ConnectorInfoes { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Tick> Ticks { get; set; }
        //public virtual DbSet<Tick.TickKeyValue> TickKeyValues { get; set; }
        public virtual DbSet<AliveStrategy> AliveStrategies { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instance>()
                .Property(e => e.Balance)
                .HasPrecision(19, 2);

            modelBuilder.Entity<Instance>()
                .Property(e => e.Slippage)
                .HasPrecision(19, 7);

            modelBuilder.Entity<Instance>()
                .Property(e => e.Rent)
                .HasPrecision(6, 4);

            modelBuilder.Entity<Candle>()
                .Property(e => e.DateOpen).HasColumnType("datetime2")
                .HasPrecision(0);

            modelBuilder.Entity<Candle>()
                .Property(e => e.OpenPrice)
                .HasPrecision(19, 7);

            modelBuilder.Entity<Candle>()
                .Property(e => e.HighPrice)
                .HasPrecision(19, 7);

            modelBuilder.Entity<Candle>()
                .Property(e => e.LowPrice)
                .HasPrecision(19, 7);

            modelBuilder.Entity<Candle>()
                .Property(e => e.ClosePrice)
                .HasPrecision(19, 7);

            modelBuilder.Entity<Candle>()
                .Property(e => e.CreateDate).HasColumnType("datetime2")
                .HasPrecision(3);

            modelBuilder.Entity<Selection>()
                .Property(e => e.Balance)
                .HasPrecision(19, 2);

            modelBuilder.Entity<Selection>()
                .Property(e => e.Slippage)
                .HasPrecision(19, 7);

            modelBuilder.Entity<Selection>()
                .Property(e => e.Rent)
                .HasPrecision(6, 4);

            modelBuilder.Entity<Setting>()
                .Property(e => e.CreateDate).HasColumnType("datetime2")
                .HasPrecision(3);

            modelBuilder.Entity<Tick.KeyValue>()
                .ToTable("TickKeyValue");

            modelBuilder.Entity<AliveStrategy>()
                .Property(e => e.CreateDate).HasColumnType("datetime2")
                .HasPrecision(3);

            modelBuilder.Entity<Order>()
                .Property(e => e.DateOpen).HasColumnType("datetime2")
                .HasPrecision(0);

            modelBuilder.Entity<Order>()
                .Property(e => e.DateClose).HasColumnType("datetime2")
                .HasPrecision(0);

            modelBuilder.Entity<Order>()
                .Property(e => e.DateOpenUTC).HasColumnType("datetime2")
                .HasPrecision(3);

            modelBuilder.Entity<Order>()
                .Property(e => e.DateCloseUTC).HasColumnType("datetime2")
                .HasPrecision(3);

            modelBuilder.Entity<Order>()
                .Property(e => e.TakeProfit)
                .HasPrecision(19, 7);
            
            modelBuilder.Entity<Order>()
                .Property(e => e.StopLoss)
                .HasPrecision(19, 7);

            modelBuilder.Entity<Order>()
                .Property(e => e.PriceOpen)
                .HasPrecision(19, 7);

            modelBuilder.Entity<Order>()
                 .Property(e => e.PriceClose)
                 .HasPrecision(19, 7);

            modelBuilder.Entity<Order>()
                 .Property(e => e.Profit)
                 .HasPrecision(19, 2);


        }
    }

    public class RMarketInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<RMarketContext>
    {
        protected override void Seed(RMarketContext context)
        {
            List<Ticker> listTicker = new List<Ticker>
            {
                new Ticker {Id=1, Name="SBER", Code="SBER", CodeFinam="3"},
                new Ticker {Id=2,  Name="GAZP", Code="GAZP", CodeFinam="16842"},
                new Ticker {Id=3, Name="AVAZ", Code="AVAZ", CodeFinam="39"},
            };
            context.Tickers.AddRange(listTicker);
            context.SaveChanges();

            List<TimeFrame> listTimeFrame = new List<TimeFrame>
            {
                new TimeFrame {Id=1,Name="tick",CodeFinam="1",ToMinute=0 },
                new TimeFrame {Id=2,Name="1",CodeFinam="2",ToMinute=1 },
                new TimeFrame {Id=3,Name="2",CodeFinam="3",ToMinute=2 },
                new TimeFrame {Id=4,Name="10",CodeFinam="4",ToMinute=10 },
                new TimeFrame {Id=5,Name="15",CodeFinam="5",ToMinute=15 },
                new TimeFrame {Id=6,Name="30",CodeFinam="6",ToMinute=30 },
                new TimeFrame {Id=7,Name="60",CodeFinam="7",ToMinute=60 },
                new TimeFrame {Id=8,Name="day",CodeFinam="8",ToMinute=1440 },
            };
            context.TimeFrames.AddRange(listTimeFrame);
            context.SaveChanges();

            List<StrategyInfo> listStrategyInfo = new List<StrategyInfo>
            {
                new StrategyInfo {TypeName="RMarket.Examples.Strategies.StrategyDonch1, RMarket.Examples", Name="StrategyDonch1" },
                new StrategyInfo {TypeName="RMarket.Examples.Strategies.StrategyMock, RMarket.Examples", Name="Mock" },
            };
            context.StrategyInfoes.AddRange(listStrategyInfo);
            context.SaveChanges();

            List<ConnectorInfo> listConnectorInfo = new List<ConnectorInfo>
            {
                new ConnectorInfo {TypeName="RMarket.ClassLib.Connectors.QuikConnector, RMarket.ClassLib", Name="QuikConnector" },
            };
            context.ConnectorInfoes.AddRange(listConnectorInfo);
            context.SaveChanges();

            //***connector
            List<ParamEntity> entityParams = new List<ParamEntity>
            {
                new ParamEntity {FieldName="serverName",FieldValue="RMarket" },
                new ParamEntity {FieldName="col_Date",FieldValue="Дата" },
                new ParamEntity {FieldName="col_Time",FieldValue="Время" },
                new ParamEntity {FieldName="col_TickerCode",FieldValue="Код бумаги" },
                new ParamEntity {FieldName="col_Price",FieldValue="Цена" },
                new ParamEntity {FieldName="col_Qty",FieldValue="Кол-во" },
                new ParamEntity {FieldName="col_Period",FieldValue="Период" }
            };
            List<Setting> settings = new List<Setting>
            {
                new Setting {Name="Quik default", StrategyInfoId=null, TypeSetting = SettingType.ConnectorInfo,
                    EntityInfoId = 1, CreateDate= DateTime.Now, Description="русскоязычные настройки квика",
                StrParams = Serializer.Serialize(entityParams)}
            };
            context.Settings.AddRange(settings);
            context.SaveChanges();
            //**



        }
    }
}
