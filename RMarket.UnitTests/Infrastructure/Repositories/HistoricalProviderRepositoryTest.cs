using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using RMarket.Concrete.HistoricalProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure.Repositories
{
    public class HistoricalProviderRepositoryTest
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
                    Id = 1,
                    Name = "Finam default",
                    CreateDate = DateTime.Now,
                    Description = "Загрузка с сайта Финам",
                    StrParams = Serializer.Serialize(entityParams),
                    EntityInfoId = 1,
                    EntityInfo = new EntityInfo
                    {
                        Id = 1,
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
