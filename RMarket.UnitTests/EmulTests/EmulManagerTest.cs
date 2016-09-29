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
using RMarket.UnitTests.Infrastructure.Repositories;
using RMarket.ClassLib.Managers;
using System.Threading;

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

            //добавляем живую стратегию
            AliveStrategy aliveStrategy = new AliveStrategy
            {
                GroupID = instance.GroupID,
                IsActive = true
            };

            //создаем датапровайдер
            ITickerRepository tickerRepository = new TickerRepository();

            CsvFileProvider dataProvider = new CsvFileProvider(tickerRepository)
            {
                FilePath = @"C:\Projects\RMarketMVCgit\RMarketMVC\RMarket.UnitTests\Infrastructure\files\SBER_160601_160601.csv",
                Separator = ';',
                Col_Date = "<DATE>",
                FormatDate = "yyyyMMdd",
                Col_Time = "<TIME>",
                FormatTime = "HHmmss",
                Col_TickerCode = "<TICKER>",
                Col_Price = "<LAST>",
                Col_Volume = "<VOL>",
                Val_SessionStart = new TimeSpan(10, 0, 0),
                Val_SessionFinish = new TimeSpan(19, 0, 0),
            };

            //получаем стратегию 
            StrategyMock1 strategy = new StrategyMock1();

            //устанавливаем остальные свойства
            Instrument instr = new Instrument(instance.Ticker, instance.TimeFrame);

            Portfolio portf = new Portfolio
            {
                Balance = instance.Balance,
                Rent = instance.Rent,
                Slippage = instance.Slippage
            };

            IOrderRepository orderRepository = new OrderRepository();

            //создаем менеджер
            IManager manager = new EmulManager(orderRepository, strategy, instr, portf, dataProvider, aliveStrategy);

            //стартуем
            manager.StartStrategy();

            Thread.Sleep(2000);

            //Проверить на количество ордеров
            Assert.AreEqual(strategy.Orders.Count, 2);

        }
    }
}
