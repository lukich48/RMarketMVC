using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure.Strategies
{
    public class StrategyMockWithoutBody: IStrategy
    {
        public Instrument Instr { get; set; }
        public IManager Manager { get; set; }
        public List<Order> Orders { get; set; }

        [Parameter(Name = "period1", Description = "Период наименьшего канала")]
        public int Period1 { get; set; } = 5;

        [Parameter(Description = "Период среднего канала")]
        public int Period2 { get; set; } = 10;


        public void Initialize()
        {
        }

        public void Begin()
        {
        }

        public void OnTickPoked(object sender, TickEventArgs e)
        {

        }

        public static Instance GetInstance()
        {
            EntityInfo entityInfo = new EntityInfo
            {
                Id = 1,
                Name = "Strategy MockWithoutBody",
                EntityType = EntityType.StrategyInfo,
                TypeName = typeof(StrategyMockWithoutBody).AssemblyQualifiedName                
            };

            Ticker ticker = new Ticker { Id = 1, Name = "SBER", Code = "SBER", QtyInLot = 10 };
            TimeFrame timeFrame = new TimeFrame { Id = 4, Name = "10", ToMinute = 10 };

            List<ParamEntity> entityParams = new List<ParamEntity>()
            {
                new ParamEntity
                {
                    FieldName = nameof(StrategyMockWithoutBody.Period1),
                    FieldValue = 6,
                }
            };

            Instance instance = new Instance
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
                StrParams = Serializer.Serialize(entityParams)
            };

            return instance;

        }


    }
}
