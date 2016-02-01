using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMarket.WebUI.Controllers;
using RMarket.ClassLib.Abstract;
using RMarket.WebUI.Models;
using Moq;
using System.Web.Mvc;
using RMarket.WebUI.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using System.Linq;

namespace RMarket.UnitTests
{
    [TestClass]
    public class TesterControllerTests
    {
        [TestMethod]
        public void Index1()
        {
            //В сессии нет сохраненных результатов теста
            SessionHelper.session = new SessionStateMock();

            TesterController controller = CreateTestManagerController(null, null);

            //Действие
            ActionResult result = controller.Index();

            // Утверждение
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

        }

        [TestMethod]
        public void Index2()
        {
            //В сессии есть сохраненные результаты теста
            SessionStateMock session = new SessionStateMock();
            List<TestResult> testCollection = new List<TestResult>();
            testCollection.Add(new TestResult { Id = 1 });
            session["TestResultCollection"] = testCollection;

            SessionHelper.session = session;

            TesterController controller = CreateTestManagerController(null, null);

            //Действие
            ActionResult result = controller.Index();

            // Утверждение
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            List<TestResult> model = ((ViewResult)result).ViewData.Model as List<TestResult>;
            Assert.AreEqual(testCollection, model);

        }

        [TestMethod]
        public void BeginTest_HttpPost()
        {
            SessionHelper.session = new SessionStateMock();
            StrategyInfo strategyInfo = new StrategyInfo { Id = 1, Name = "Mock", TypeName = "RMarket.Examples.Strategies.StrategyMock, RMarket.Examples" };
            Ticker ticker = new Ticker { Id = 1, Code = "SBER" };
            TimeFrame timeFrame = new TimeFrame { Id = 11, Name = "1 day" };
            Instance instance = new Instance
            {
                Id = 1,
                GroupID = new Guid("a76dc9b4-8816-438e-aed3-c123374c2e3e"),
                StrParams = "[{\"FieldName\":\"withSleep\",\"FieldValue\":true}]",
                StrategyInfoId = strategyInfo.Id,
                StrategyInfo = strategyInfo,
                TickerId=ticker.Id,
                Ticker = ticker,
                TimeFrameId=timeFrame.Id,
                TimeFrame=timeFrame
            };
            Mock<IInstanceRepository> instanceRepoMock = new Mock<IInstanceRepository>();
            instanceRepoMock.Setup(m => m.Instances).Returns(new List<Instance>
            {
                instance
            }.AsQueryable());
            instanceRepoMock.Setup(m => m.Find(1)).Returns(instance);

            Mock<ICandleRepository> candleRepoMock = new Mock<ICandleRepository>();
            candleRepoMock.Setup(m => m.Candles).Returns(new List<Candle>
            {
                new Candle(ticker.Id,timeFrame.Id,new DateTime(2015,1,1),10m,15m,8m,11m,0),
                new Candle(ticker.Id,timeFrame.Id,new DateTime(2015,1,2),11m,15m,8m,11m,0),
                new Candle(ticker.Id,timeFrame.Id,new DateTime(2015,1,3),11m,16m,10m,15m,0),
            }.AsQueryable());

            TesterModel model = new TesterModel
            {
                InstanceId = 1,
                DateFrom=new DateTime(2015,1,1),
                DateTo=new DateTime(2015,2,1)
            };

            TesterController controller = CreateTestManagerController(instanceRepoMock.Object, candleRepoMock.Object);
           
            //Действие
            ActionResult result = controller.BeginTest(model);

            // Утверждение
            //1. В сесии есть объект результата
            List<TestResult> testResultCollection = (List<TestResult>)SessionHelper.session["TestResultCollection"];
            Assert.AreNotEqual(null, testResultCollection);

            //2. Проверка реквизитов сформированной стратегии
            IStrategy strategy = testResultCollection.FirstOrDefault(t => t.Id == 1).Strategy;
            Assert.AreEqual(ticker, strategy.Instr.Ticker);
            Assert.AreEqual(timeFrame, strategy.Instr.TimeFrame);

            //3. Стратегия стартована
            Assert.AreEqual(true, strategy.Manager.IsStarted);

        }

        #region Вспомогательные методы

        public TesterController CreateTestManagerController(IInstanceRepository instanceRepository, ICandleRepository candleRepository)
        {
            // Организация - создание имитированного хранилища данных
            Mock<IInstanceRepository> mockInstance = new Mock<IInstanceRepository>();
            Mock<ICandleRepository> mockCandle = new Mock<ICandleRepository>();

            if (instanceRepository == null)
                instanceRepository = mockInstance.Object;
            if (candleRepository == null)
                candleRepository = mockCandle.Object;

            // Организация - создание контроллера
            TesterController controller = new TesterController(instanceRepository);

            return controller;
        }

        #endregion
    }
}
