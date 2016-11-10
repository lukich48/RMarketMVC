using System;
using RMarket.UnitTests.Infrastructure.Repositories;
using RMarket.ClassLib.Entities;
using System.Linq;
using RMarket.WebUI.Models;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.EntityModels;
using System.Collections.Generic;
using RMarket.WebUI.Infrastructure.MapperProfiles;
using AutoMapper;
using NUnit.Framework;
using RMarket.UnitTests.Infrastructure.HistoricalProviders;
using RMarket.UnitTests.Infrastructure.Strategies;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Helpers;
using LightInject;

namespace RMarket.UnitTests.WebUITests
{
    [TestFixture]
    public class MapperTests
    {
        private Scope scope;

        public MyMapper Mapper { get; set; }

        [OneTimeSetUp]
        public void Init()
        {
            //загружаем сборки в домен
            var a = typeof(HistoricalProviderSettingModelUI);

            var inicializer = new CompositionRoot.Inicializer();
            //inicializer.SetMapperConfiguration();
            inicializer.InitIoC((c) => {

                c.InjectProperties(this);
                scope = c.BeginScope();
            });
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            scope.Dispose();
        }


        [Test]
        public void MapperTest()
        {
            HistoricalProviderSetting entity = Finam.GetInstance();

            HistoricalProviderSettingModel model = Mapper
                .Map<HistoricalProviderSetting, HistoricalProviderSettingModel>(entity);

            HistoricalProviderSettingModelUI modelUI = Mapper
                .Map<HistoricalProviderSettingModel, HistoricalProviderSettingModelUI>(model);

            HistoricalProviderSettingModel modelRes = Mapper
                .Map<HistoricalProviderSettingModelUI, HistoricalProviderSettingModel>(modelUI);

            HistoricalProviderSetting entityRes = Mapper
                .Map<HistoricalProviderSettingModel, HistoricalProviderSetting>(modelRes);

            Assert.AreEqual(modelUI.Id, entity.Id);
            //все парметры из объекта
            Assert.AreEqual(2, model.EntityParams.Count);
            //один параметр не был сохранен, он должен быть null
            Assert.IsNull(model.EntityParams.SingleOrDefault(p => p.FieldName == "TimeFrameCodeFinams").FieldValue);

            Assert.AreEqual(modelUI.EntityParams.Count, model.EntityParams.Count);
            Assert.AreEqual(modelUI.EntityParams.Count, model.EntityParams.Count);

            Assert.AreEqual(modelRes.Id, model.Id);

            //параметры сконвертировались правильно
            foreach(var param in modelRes.EntityParams)
            {
                var foundParam = model.EntityParams.SingleOrDefault(p => p.FieldName == param.FieldName);
                Assert.AreEqual(param.FieldValue?.ToString(), foundParam.FieldValue?.ToString());
            }

        }

        [Test]
        public void EntityParams_RepairValues()
        {
            //case: в стратегию добавился новый параметр после того, как был сохранен инстанс

            //получаем сохраненный инстанс
            Instance instance = StrategyMockWithoutBody.GetInstance();

            InstanceModel model = Mapper
                .Map<Instance, InstanceModel>(instance);

            var missingParam = model.EntityParams.Single(p => p.FieldName == nameof(StrategyMockWithoutBody.Period2));
            Assert.IsNull(missingParam.FieldValue);

            //создаем объект стратегии
            IStrategy strategy = new StrategyMockWithoutBody();

            //вызыввем метод восстановления параметров
            new SettingHelper().RepairValues(strategy, model.EntityParams);

            Assert.AreEqual(10, missingParam.FieldValue);
        }
    }
}
