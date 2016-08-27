using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using RMarket.Concrete.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarker.Concrete.DataProviders.Infrastructure
{
    public class DataProvidersContextInicializer: IContextInitializer<DataProvider>
    {
        public IEnumerable<DataProvider> Get()
        {
            List<DataProvider> dataProviders = new List<DataProvider>();

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

            dataProviders.Add(
                new DataProvider
                {
                    Name ="Quik default",
                    CreateDate= DateTime.Now, Description="русскоязычные настройки квика",
                    StrParams = Serializer.Serialize(entityParams),
                    EntityInfo = new EntityInfo
                    {
                        Name = "QuikProvider",
                        TypeName = typeof(QuikProvider).AssemblyQualifiedName,
                        EntityType = EntityType.DataProviderInfo
                    }
                }
            );

            List<ParamEntity> entityParams2 = new List<ParamEntity>
            {
                new ParamEntity {FieldName="filePath",FieldValue=@"C:\Projects\RMarketMVCgit\RMarketMVC\RMarket.Examples\files\SBER_160601_160601.csv" },
                new ParamEntity {FieldName="col_Date",FieldValue="<DATE>" },
                new ParamEntity {FieldName="FormatDate",FieldValue="yyyyMMdd" },
                new ParamEntity {FieldName="col_Time",FieldValue="<TIME>" },
                new ParamEntity {FieldName="FormatTime",FieldValue="HHmmss" },
                new ParamEntity {FieldName="col_TickerCode",FieldValue="<TICKER>" },
                new ParamEntity {FieldName="col_Price",FieldValue="<LAST>" },
                new ParamEntity {FieldName="col_Qty",FieldValue="Qty" },
                new ParamEntity {FieldName="Col_Volume",FieldValue="<VOL>" },
                new ParamEntity {FieldName="Col_TradePeriod",FieldValue="Period" },
                new ParamEntity {FieldName="Val_PeriodOpening",FieldValue="Opening" },
                new ParamEntity {FieldName="Val_PeriodTrading",FieldValue="Trading" },
                new ParamEntity {FieldName="Val_PeriodClosing",FieldValue="Closing" },
                new ParamEntity {FieldName="Val_SessionStart",FieldValue="10:00:00" },
                new ParamEntity {FieldName="Val_SessionFinish",FieldValue="19:00:00" },
                new ParamEntity {FieldName="Delay",FieldValue="600" }
            };
            dataProviders.Add(
                new DataProvider
                {
                    Name ="csv Finam",
                    EntityInfoId = 2, CreateDate= DateTime.Now, Description="Настройка для .csv от Финама",
                    StrParams = Serializer.Serialize(entityParams),
                    EntityInfo = new EntityInfo
                    {
                        Name = "CsvFileProvider",
                        TypeName = typeof(CsvFileProvider).AssemblyQualifiedName,
                        EntityType = EntityType.DataProviderInfo
                    }
                }
            );

            return dataProviders;
        }
    }
}
