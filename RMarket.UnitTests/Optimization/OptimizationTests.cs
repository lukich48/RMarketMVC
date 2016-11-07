using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.EntityModels;
using RMarket.Concrete.Optimization.Helpers;
using NUnit.Framework;
using AutoMapper;
using RMarket.WebUI.Infrastructure.MapperProfiles;

namespace RMarket.UnitTests.Optimization
{
    /// <summary>
    /// Тестирование методов оптимизации
    /// </summary>
    [TestFixture]
    public class OptimizationTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var inicializer = new CompositionRoot.Inicializer();
            inicializer.SetMapperConfiguration();

            //inicializer.InitIoC((c)=> { });

        }

        [Test]
        public void CreateFirstGeneration()
        {
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
            EntityInfo entityInfo = new EntityInfo
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
                EntityInfoId = entityInfo.Id,
                EntityInfo=entityInfo,
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
                        ValueMin = 6,
                        ValueMax=10
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
                    //Добавить вещественный
                }
            };

            var instanceResults = new GaHelper(selection).CreateFirstGeneration(1);
            InstanceModel instance = instanceResults.First().Instance;

            Assert.AreEqual(10, instanceResults.Count());
            Assert.AreEqual(1, instance.TickerId);
            Assert.AreEqual(10, instance.TimeFrameId);
            Assert.AreEqual(20, instance.EntityInfoId);
            Assert.AreEqual(30, instance.SelectionId);

            Assert.AreEqual(true, instance.EntityParams.Any(p=>p.FieldName== "OtherParam" && (bool)p.FieldValue==true));
            Assert.AreEqual(true, instance.EntityParams.Any(p=>p.FieldName== "Param1" && (int)p.FieldValue >= 6 && (int)p.FieldValue <= 10));
            Assert.AreEqual(true, instance.EntityParams.Where(p=>p.FieldName == "Param2").Single().FieldValue.GetType() == typeof(byte));

        }
    }
}
