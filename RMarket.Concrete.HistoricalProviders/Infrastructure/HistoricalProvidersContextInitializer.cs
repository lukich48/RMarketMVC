using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Concrete.HistoricalProviders.Infrastructure
{
    public class HistoricalProvidersContextInitializer : IContextInitializer<HistoricalProviderSetting>
    {
        public IEnumerable<HistoricalProviderSetting> Get()
        {
            List<HistoricalProviderSetting> historicalProviders = new List<HistoricalProviderSetting>();

            Dictionary<string, string> codeFinams = new Dictionary<string, string>();
            codeFinams.Add("SBER", "3");
            codeFinams.Add("GAZP", "16842");
            codeFinams.Add("AVAZ", "39");


            List<ParamEntity> entityParams = new List<ParamEntity>
            {
                new ParamEntity {FieldName="CodeFinams",FieldValue=codeFinams },
            };


            historicalProviders.Add(
                new HistoricalProviderSetting
                {
                    Name = "Finam default",
                    CreateDate = DateTime.Now,
                    Description = "Загрузка с сайта Финам",
                    StrParams = Serializer.Serialize(entityParams),
                    EntityInfo = new EntityInfo
                    {
                        Name = "FinamProvider",
                        TypeName = typeof(Finam).AssemblyQualifiedName,
                        EntityType = EntityType.HistoricalProviderInfo
                    }
                }
            );

            return historicalProviders;
        }
    }
}