using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMarket.UnitTests.Infrastructure.Repositories;
using RMarket.ClassLib.Entities;
using System.Linq;
using RMarket.WebUI.Models;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.EntityModels;
using System.Collections.Generic;
using RMarket.WebUI.Infrastructure.MapperProfiles;
using AutoMapper;

namespace RMarket.UnitTests.WebUITests
{
    [TestClass]
    public class MapperTests
    {
        //!!!Проинициализировать контекст маппера
        public MapperTests()
        {
            var inicializer = new CompositionRoot.Inicializer();
            inicializer.SetMapperConfiguration(new List<Profile> { new AutoMapperUIProfile() });

            var kernel = new Ninject.StandardKernel();
            inicializer.SetIoC(kernel);

        }

        [TestMethod]
        public void HistoricalProviderSettingModelMappingTest()
        {
            var repository = new HistoricalProviderRepositoryTest();
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
            Assert.AreEqual(modelUI.EntityParams.Count, model.EntityParams.Count);
            Assert.AreEqual(modelUI.EntityParams.Count, model.EntityParams.Count);

            Assert.AreEqual(model.Id, modelRes.Id);

            //параметры сконвертировались правильно
            Assert.AreEqual(((Dictionary<string, string>)model.EntityParams.FirstOrDefault(m => m.FieldName == "CodeFinams").FieldValue)["SBER"]
                , ((Dictionary<string, string>)modelRes.EntityParams.FirstOrDefault(m => m.FieldName == "CodeFinams").FieldValue)["SBER"]);
            Assert.AreEqual(entityRes.StrParams, entity.StrParams);

        }
    }
}
