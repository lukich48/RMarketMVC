using System;
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
using NUnit.Framework;

namespace RMarket.UnitTests.Emul
{
    [TestFixture]
    public class EmulManagerTests
    {
        [Test]
        public void StartEmul()
        {
            //создаем инстанс
            InstanceModel instance = StrategyMock1.GetModel();

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
