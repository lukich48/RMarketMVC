using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System.Collections.Generic;
using System.Linq;
using RMarket.Examples.Strategies;
using RMarket.ClassLib.Helpers;

namespace RMarket.UnitTests
{
    [TestClass]
    public class CommonClassTests
    {

        /// <summary>
        /// Параметры стратегии
        /// </summary>
        [TestMethod]
        public void Get_StrategyParam()
        {
            //Организация - создание стратегии
            IStrategy strategyMock = new Examples.Strategies.StrategyMock();
            StrategyInfo strategyInfo = new StrategyInfo { Id = 1, Name = "Mock", TypeName = "RMarket.Examples.Strategies.StrategyMock, RMarket.Examples" };
            Instance instance = new Instance { Id = 1, StrategyInfoId = strategyInfo.Id, StrategyInfo = strategyInfo, GroupID = new Guid("a76dc9b4-8816-438e-aed3-c123374c2e3e") };

            //Действие - Получение параметров
            IEnumerable<ParamEntity> strategyParams = StrategyHelper.GetStrategyParams(instance);

            ParamEntity param1 = strategyParams.FirstOrDefault(t => t.FieldName == "decimalParam");
            ParamEntity param2 = strategyParams.FirstOrDefault(t => t.FieldName == "period3");

            //Утверждение - Количество параметров
            Assert.AreEqual(6, strategyParams.Count());

            Assert.AreEqual(param1.DisplayName, "decimal param");
            Assert.AreEqual(param1.FieldValue, 30.25m);
            Assert.IsInstanceOfType(param1.FieldValue, typeof(Decimal));
            Assert.AreEqual(param1.Description, "Какой-то параметр типа Decimal");

            Assert.AreEqual(param2.DisplayName, "period3");
            Assert.AreEqual(param2.FieldValue, 30);
            Assert.AreEqual(param2.Description, null);

        }

        /// <summary>
        /// Параметры стратегии
        /// </summary>
        [TestMethod]
        public void Get_StrategyParams_With_Save_Param()
        {
            //Организация - создание стратегии
            StrategyInfo strategyInfo = new StrategyInfo { Id = 1, Name = "Mock", TypeName = "RMarket.Examples.Strategies.StrategyMock, RMarket.Examples" };
            Instance instance = new Instance { Id = 1, StrategyInfoId = strategyInfo.Id, StrategyInfo = strategyInfo, GroupID = new Guid("a76dc9b4-8816-438e-aed3-c123374c2e3e"), StrParams = "[{\"FieldName\":\"decimalParam\",\"FieldValue\":30.26}]" };

            //Действие - Получение параметров
            IEnumerable<ParamEntity> strategyParams = StrategyHelper.GetStrategyParams(instance);

            ParamEntity param1 = strategyParams.FirstOrDefault(t => t.FieldName == "decimalParam");
            ParamEntity param2 = strategyParams.FirstOrDefault(t => t.FieldName == "period3");

            //Утверждение - Количество параметров
            Assert.AreEqual(6, strategyParams.Count());

            Assert.AreEqual(param1.DisplayName, "decimal param");
            Assert.AreEqual(param1.FieldValue, 30.26m);
            Assert.IsInstanceOfType(param1.FieldValue, typeof(Decimal));
            Assert.AreEqual(param1.Description, "Какой-то параметр типа Decimal");

        }

        /// <summary>
        /// Стратегия с сохраненными параметрами
        /// </summary>
        [TestMethod]
        public void Get_Strategy_With_Save_Param()
        {
            //Организация - создание стратегии
            StrategyInfo strategyInfo = new StrategyInfo { Id = 1, Name = "Mock", TypeName = "RMarket.Examples.Strategies.StrategyMock, RMarket.Examples" };
            Instance instance = new Instance { Id = 1, StrategyInfoId = strategyInfo.Id, StrategyInfo = strategyInfo, GroupID = new Guid("a76dc9b4-8816-438e-aed3-c123374c2e3e"), StrParams = "[{\"FieldName\":\"decimalParam\",\"FieldValue\":30.26}]" };

            //Действие - Получение параметров
            IStrategy strategy = StrategyHelper.CreateStrategy(instance);

            //Утверждение - значения параметров стратегии
            var strategyMock = (Examples.Strategies.StrategyMock)strategy;

            Assert.AreEqual(strategyMock.period1, 5);
            Assert.AreEqual(strategyMock.decimalParam, 30.26m);
            Assert.IsInstanceOfType(strategyMock.decimalParam, typeof(Decimal));
        }

        ///Извлечь индикаторы из стратегии
        [TestMethod]
        public void GetIndicatorList()
        {
            IStrategy strategy = new StrategyMock();
            Ticker ticker = new Ticker { Id = 1, Code = "SBER" };
            TimeFrame timeFrame = new TimeFrame { Id = 11, Name = "1 day" };
            strategy.Instr = new Instrument(ticker, timeFrame);

            strategy.Initialize();

            //Действие - Получение Коллекции индикаторов
            Dictionary<string, IIndicator> indicatorsDict = StrategyHelper.ExtractIndicatorsInStrategy(strategy);

            //Утверждение 
            //1. количество индикаторов
            Assert.AreEqual(2, indicatorsDict.Count);

            //2. Имена индикаторов
            Assert.IsTrue(indicatorsDict.ContainsKey("Индикатор1"));
            Assert.IsTrue(indicatorsDict.ContainsKey("ind2"));

        }

    }
}
