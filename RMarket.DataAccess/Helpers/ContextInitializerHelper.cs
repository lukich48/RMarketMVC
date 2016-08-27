using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using RMarket.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace RMarket.DataAccess.Helpers
{
    public class ContextInitializerHelper
    {
        private readonly RMarketContext context;
        public ContextInitializerHelper(RMarketContext context)
        {
            this.context = context;
        }


        public void SeedTickers()
        {
            List<Ticker> listTicker = new List<Ticker>
            {
                new Ticker {Id=1, Name="SBER", Code="SBER", CodeFinam="3", QtyInLot=10},
                new Ticker {Id=2,  Name="GAZP", Code="GAZP", CodeFinam="16842"},
                new Ticker {Id=3, Name="AVAZ", Code="AVAZ", CodeFinam="39"},
            };
            context.Tickers.AddRange(listTicker);
            context.SaveChanges();

        }

        public void SeedTimeFrames()
        {
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
        }

        public void SeedStrategyInfoes()
        {
            List<EntityInfo> listStrategyInfo = new List<EntityInfo>
            {
                new EntityInfo {TypeName="RMarket.Examples.Strategies.StrategyDonch1, RMarket.Examples", Name="StrategyDonch1", EntityType = EntityType.StrategyInfo },
                new EntityInfo {TypeName="RMarket.Examples.Strategies.StrategyMock, RMarket.Examples", Name="Mock", EntityType = EntityType.StrategyInfo },
            };
            context.EntityInfoes.AddRange(listStrategyInfo);
            context.SaveChanges();
        }

        //public void SeedConnectorInfoes()
        //{
        //    List<EntityInfo> listConnectorInfo = new List<EntityInfo>
        //    {
        //        new EntityInfo {TypeName="RMarket.ClassLib.Connectors.QuikConnector, RMarket.ClassLib", Name="QuikConnector" },
        //        new EntityInfo {TypeName="RMarket.ClassLib.Connectors.CsvFileConnector, RMarket.ClassLib", Name="CsvFileConnector" },

        //    };
        //    context.EntityInfoes.AddRange(listConnectorInfo);
        //    context.SaveChanges();
        //}

        //public void SeedConnectorQuik()
        //{
        //    List<ParamEntity> entityParams = new List<ParamEntity>
        //    {
        //        new ParamEntity {FieldName="serverName",FieldValue="RMarket" },
        //        new ParamEntity {FieldName="col_Date",FieldValue="Дата" },
        //        new ParamEntity {FieldName="col_Time",FieldValue="Время" },
        //        new ParamEntity {FieldName="col_TickerCode",FieldValue="Код бумаги" },
        //        new ParamEntity {FieldName="col_Price",FieldValue="Цена" },
        //        new ParamEntity {FieldName="col_Qty",FieldValue="Кол-во" },
        //        new ParamEntity {FieldName="col_Period",FieldValue="Период" }
        //    };
        //    List<DataProvider> settings = new List<DataProvider>
        //    {
        //        new DataProvider {Name="Quik default", 
        //            EntityInfoId = 1, CreateDate= DateTime.Now, Description="русскоязычные настройки квика",
        //        StrParams = Serializer.Serialize(entityParams)}
        //    };
        //    context.DataProviders.AddRange(settings);
        //    context.SaveChanges();
        //}

        public void SeedConnectorCsvFile()
        {
            //!!!доделать
            List<ParamEntity> entityParams = new List<ParamEntity>
            {
                new ParamEntity {FieldName="filePath",FieldValue=@"C:\Projects\RMarketMVCgit\RMarketMVC\RMarket.Examples\files\SBER_160601_160601.csv" },
                new ParamEntity {FieldName="col_Date",FieldValue="Дата" },
                new ParamEntity {FieldName="col_Time",FieldValue="Время" },
                new ParamEntity {FieldName="col_TickerCode",FieldValue="Код бумаги" },
                new ParamEntity {FieldName="col_Price",FieldValue="Цена" },
                new ParamEntity {FieldName="col_Qty",FieldValue="Кол-во" },
                new ParamEntity {FieldName="col_Period",FieldValue="Период" }
            };
            List<DataProvider> settings = new List<DataProvider>
            {
                new DataProvider {Name="Quik default",
                    EntityInfoId = 1, CreateDate= DateTime.Now, Description="русскоязычные настройки квика",
                StrParams = Serializer.Serialize(entityParams)}
            };
            context.DataProviders.AddRange(settings);
            context.SaveChanges();
        }


    }
}
