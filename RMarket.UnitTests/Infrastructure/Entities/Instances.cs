using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.UnitTests.Infrastructure.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure.Entities
{
    public static class Instances
    {
        public static InstanceModel GetInstance1()
        {
            EntityInfo entityInfo = new EntityInfo
            {
                Id = 1,
                Name = "Strategy Mosk",
                EntityType = EntityType.StrategyInfo,
                TypeName = typeof(StrategyMock1).AssemblyQualifiedName
            };

            Ticker ticker = new Ticker { Id = 1, Name = "SBER", Code = "SBER", QtyInLot = 10 };
            TimeFrame timeFrame = new TimeFrame { Id = 4, Name = "10", ToMinute = 10 };

            InstanceModel instance = new InstanceModel
            {
                Id = 1,
                Name = "test instance1",
                EntityInfoId = entityInfo.Id,
                EntityInfo = entityInfo,
                TickerId = ticker.Id,
                Ticker = ticker,
                TimeFrameId = timeFrame.Id,
                TimeFrame = timeFrame,
                Balance = 1000,
                Slippage = 0,
                Rent = 0,
                GroupID = Guid.NewGuid(),
                CreateDate = new DateTime(2016, 01, 01),
            };

            return instance;
        }
    }
}
