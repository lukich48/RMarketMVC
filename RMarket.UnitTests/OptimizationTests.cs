using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers.Extentions;

namespace RMarket.UnitTests
{
    /// <summary>
    /// Тестирование методов оптимизации
    /// </summary>
    [TestClass]
    public class OptimizationTests
    {
        
        [TestMethod]
        public void CreateFirstGeneration()
        {
            //!!! Долго

            Ticker ticker = new Ticker
            {
                Id = 1,
                Name = "sber",
                Code = "sber",
            };
            TimeFrame timeFrame = new TimeFrame
            {
                Id = 10,
                Name = "10",
                ToMinute = 10
            };
            StrategyInfo strategyInfo = new StrategyInfo
            {
                Id = 20,
                Name = "Mock",
                TypeName = "RMarket.Examples.Strategies.StrategyMock, RMarket.Examples"
            };

            SelectionModel selection = new SelectionModel
            {
                Id = 30,
                Name = "TestSelection",
                TickerId = ticker.Id,
                Ticker = ticker,
                TimeFrameId = timeFrame.Id,
                TimeFrame=timeFrame,
                StrategyInfoId = strategyInfo.Id,
                StrategyInfo=strategyInfo,
                AmountResults = 10,
                Balance = 1000,
                CreateDate = new DateTime(2016, 1, 1),
                DateFrom = new DateTime(2016, 1, 1),
                DateTo = new DateTime(2016, 1, 10),
                Description = "for test",
                GroupID = new Guid("7ada9499-cb8e-44ad-862a-a93920ff4760"),
                Slippage = 0.01M,
                Rent = 0.1M,
                SelectionParams = new List<ParamSelection>
                {
                    new ParamSelection
                    {
                        FieldName="Param1",
                        ValueMin = 1,
                        ValueMax=5
                    },
                    new ParamSelection
                    {
                        FieldName="Param2",
                        ValueMin = (byte)1,
                        ValueMax=(byte)5
                    },
                    new ParamSelection
                    {
                        FieldName="OtherParam",
                        ValueMin = true,
                        ValueMax=true
                    }
                }
            };

            List<InstanceModel> instances1 = OptimizationHelper.CreateFirstGeneration(selection);

            Assert.AreEqual(10, instances1.Count);
            Assert.AreEqual(1, instances1[0].TickerId);
            Assert.AreEqual(10, instances1[0].TimeFrameId);
            Assert.AreEqual(20, instances1[0].StrategyInfoId);
            Assert.AreEqual(30, instances1[0].SelectionId);

            Assert.AreEqual(true, instances1[0].StrategyParams.Any(p=>p.FieldName== "OtherParam" && (bool)p.FieldValue==true));
            Assert.AreEqual(true, instances1[0].StrategyParams.Any(p=>p.FieldName== "Param1" && (int)p.FieldValue>=1 && (int)p.FieldValue <= 5));

        }
    }
}
