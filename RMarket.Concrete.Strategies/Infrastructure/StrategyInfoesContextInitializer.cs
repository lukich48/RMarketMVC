using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Concrete.Strategies.Infrastructure
{
    public class StrategyInfoesContextInitializer: IContextInitializer<EntityInfo>
    {
        public IEnumerable<EntityInfo> Get()
        {
            List<EntityInfo> listStrategyInfo = new List<EntityInfo>
            {
                new EntityInfo {Name="StrategyDonch1", TypeName = typeof(StrategyDonch1).AssemblyQualifiedName, EntityType = EntityType.StrategyInfo },
                new EntityInfo {Name="StrategyMock", TypeName = typeof(StrategyMock).AssemblyQualifiedName, EntityType = EntityType.StrategyInfo },
            };

            return listStrategyInfo;
        }

    }
}
