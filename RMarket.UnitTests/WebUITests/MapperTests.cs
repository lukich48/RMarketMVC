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

namespace RMarket.UnitTests.WebUITests
{
    [TestFixture]
    public class MapperTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var inicializer = new CompositionRoot.Inicializer();
            inicializer.SetMapperConfiguration(new List<Profile> { new AutoMapperUIProfile() });

            var kernel = new Ninject.StandardKernel();
            inicializer.SetIoC(kernel);

        }

        [Test]
        public void MapperTest()
        {
            var repository = new HistoricalProviderRepository();
            HistoricalProviderSetting entity = repository.Get().Where(m => m.Id == 1).FirstOrDefault();

            HistoricalProviderSettingModel model = MyMapper.Current
                .Map<HistoricalProviderSetting, HistoricalProviderSettingModel>(entity);

            HistoricalProviderSettingModelUI modelUI = MyMapper.Current
                .Map<HistoricalProviderSettingModel, HistoricalProviderSettingModelUI>(model);

            HistoricalProviderSettingModel modelRes = MyMapper.Current
                .Map<HistoricalProviderSettingModelUI, HistoricalProviderSettingModel>(modelUI);

            HistoricalProviderSetting entityRes = MyMapper.Current
                .Map<HistoricalProviderSettingModel, HistoricalProviderSetting>(modelRes);

            Assert.AreEqual(modelUI.Id, entity.Id);
            Assert.AreEqual(2, model.EntityParams.Count);
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
    }
}
