using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Entities;
using RMarket.UnitTests.Infrastructure.Strategies;
using RMarket.ClassLib.Abstract;
using RMarket.Concrete.DataProviders;
using RMarket.ClassLib.Abstract.IRepository;
using Moq;
using System.Linq;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;

namespace RMarket.UnitTests.EmulTests
{
    [TestClass]
    public class EmulManagerTest
    {
        [TestMethod]
        public void StartEmul()
        {
            //создаем инстанс

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

            //создаем датапровайдер
            var mockTickerRepo = new Mock<ITickerRepository>();
            mockTickerRepo.Setup(m => m.Get()).Returns(Enumerable.Repeat(ticker,1)); //создать тестовыё репозиторий
            ITickerRepository tickerRepository = mockTickerRepo.Object;

            IDataProvider dataProvider = new CsvFileProvider(tickerRepository);
            //IDataProvider dataProvider = new SettingHelper().CreateDataProvider(setting);


            //получаем стратегию 
            IStrategy strategy = StrategyHelper.CreateStrategy(instance);

            //устанавливаем остальные свойства
            Instrument instr = new Instrument(instance.Ticker, instance.TimeFrame);

            Portfolio portf = new Portfolio
            {
                Balance = instance.Balance,
                Rent = instance.Rent,
                Slippage = instance.Slippage
            };




            //создаем менеджер


            //стартуем


            //


        }
    }
}
