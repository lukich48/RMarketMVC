using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Entities_old;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public class RMarketInitializerHelper
    {

        public void SeedTickers(RMarketContext context)
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

        public void SeedTimeFrames(RMarketContext context)
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

        public void SeedStrategyInfoes(RMarketContext context)
        {
            List<StrategyInfo> listStrategyInfo = new List<StrategyInfo>
            {
                new StrategyInfo {TypeName="RMarket.Examples.Strategies.StrategyDonch1, RMarket.Examples", Name="StrategyDonch1" },
                new StrategyInfo {TypeName="RMarket.Examples.Strategies.StrategyMock, RMarket.Examples", Name="Mock" },
            };
            context.StrategyInfoes.AddRange(listStrategyInfo);
            context.SaveChanges();
        }

        public void SeedConnectorInfoes(RMarketContext context)
        {
            List<ConnectorInfo> listConnectorInfo = new List<ConnectorInfo>
            {
                new ConnectorInfo {TypeName="RMarket.ClassLib.Connectors.QuikConnector, RMarket.ClassLib", Name="QuikConnector" },
                new ConnectorInfo {TypeName="RMarket.ClassLib.Connectors.CsvFileConnector, RMarket.ClassLib", Name="CsvFileConnector" },

            };
            context.ConnectorInfoes.AddRange(listConnectorInfo);
            context.SaveChanges();
        }

        public void SeedConnectorQuik(RMarketContext context)
        {
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
                new Setting {Name="Quik default", StrategyInfoId=null, SettingType = SettingType.ConnectorInfo,
                    EntityInfoId = 1, CreateDate= DateTime.Now, Description="русскоязычные настройки квика",
                StrParams = Serializer.Serialize(entityParams)}
            };
            context.Settings.AddRange(settings);
            context.SaveChanges();
        }

        public void SeedConnectorCsvFile(RMarketContext context)
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
            List<Setting> settings = new List<Setting>
            {
                new Setting {Name="Quik default", StrategyInfoId=null, SettingType = SettingType.ConnectorInfo,
                    EntityInfoId = 1, CreateDate= DateTime.Now, Description="русскоязычные настройки квика",
                StrParams = Serializer.Serialize(entityParams)}
            };
            context.Settings.AddRange(settings);
            context.SaveChanges();
        }


    }
}
