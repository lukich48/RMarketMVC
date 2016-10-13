using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using RMarket.Concrete.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarker.Concrete.Optimization.Infrastructure
{
    public class OptimizationsContextInicializer: IContextInitializer<OptimizationSetting>
    {
        public IEnumerable<OptimizationSetting> Get()
        {
            var res = new List<OptimizationSetting>();

            res.Add(GetGaSimple());

            return res;
        }

        private OptimizationSetting GetGaSimple()
        {
            List<ParamEntity> entityParams = new List<ParamEntity>
            {
                new ParamEntity {FieldName="GenerationPower",FieldValue="1" },
            };

            return new OptimizationSetting
            {
                Name = "GaSimple default",
                CreateDate = DateTime.Now,
                Description = "",
                StrParams = Serializer.Serialize(entityParams),
                EntityInfo = new EntityInfo
                {
                    Name = "GaSimple",
                    TypeName = typeof(GaSimple).AssemblyQualifiedName,
                    EntityType = EntityType.OptimizationInfo
                }
            };
        }

    }
}
