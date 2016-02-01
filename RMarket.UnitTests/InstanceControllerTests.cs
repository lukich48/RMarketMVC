using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Abstract;
using RMarket.WebUI.Controllers;
using RMarket.WebUI.Models;
using RMarket.ClassLib.Models;

namespace RMarket.UnitTests
{
    [TestClass]
    public class InstanceControllerTests
    {
        #region Тестирование метода Index
        [TestMethod]
        public void Index_result()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IInstanceRepository> mock = new Mock<IInstanceRepository>();
            mock.Setup(m => m.Instances).Returns(new List<Instance>
            {
                new Instance {Id=1, GroupID=new Guid("a76dc9b4-8816-438e-aed3-c123374c2e3e")},
                new Instance {Id=2, GroupID=new Guid("a76dc9b4-8816-438e-aed3-c123374c2e3e")},
                new Instance {Id=3, GroupID=new Guid("a8a15362-1c4b-4f5a-97a3-50e7484d9bac")},
            }.AsQueryable());

            // Организация - создание контроллера
            InstanceController controller = CreateInstanceController(mock.Object, null, null, null);

            //Действие
            List<Instance> model = ((IEnumerable<Instance>)controller.Index().ViewData.Model).ToList();

            // Утверждение - получаем верный список
            Assert.AreEqual(model.Count(), 3);
            Assert.AreEqual(1, model[0].Id);
            Assert.AreEqual(2, model[1].Id);
            Assert.AreEqual(3, model[2].Id);
        }

        #endregion

        #region Тестирование метода Edit

        [TestMethod]
        public void Edit_New_Instance()
        {
            // Организация - создание контроллера
            InstanceController controller = CreateInstanceController(null,null,null,null);

            //Действие
            Instance model1 = ((ViewResult)controller.Edit()).ViewData.Model as Instance;

            // Утверждение - Вариант стратегии пустой
            Assert.AreEqual(0, model1.Id);

        }

        /// <summary>
        /// Тестируем Get метод Edit
        /// </summary>
        [TestMethod]
        public void Can_Edit_Instance()
        {
            // Организация - создание имитированного хранилища данных            
            StrategyInfo strategyInfo = new StrategyInfo { Id = 1, Name = "Mock", TypeName = "RMarket.Examples.Strategies.StrategyMock, RMarket.Examples" };
            Instance instance1 = new Instance { Id = 1, GroupID = new Guid("a76dc9b4-8816-438e-aed3-c123374c2e3e"), StrategyInfoId = 1, StrategyInfo = strategyInfo };
            Instance instance2 = new Instance { Id = 2, GroupID = new Guid("a76dc9b4-8816-438e-aed3-c123374c2e3e"), StrategyInfoId = 1, StrategyInfo = strategyInfo };
            Instance instance3 = new Instance { Id = 3, GroupID = new Guid("a8a15362-1c4b-4f5a-97a3-50e7484d9bac"), StrategyInfoId = 1, StrategyInfo = strategyInfo };

            Mock<IInstanceRepository> instanceRepoMock = new Mock<IInstanceRepository>();
            instanceRepoMock.Setup(m => m.Instances).Returns(new List<Instance>
            {
                instance1,
                instance2,
                instance3,
            }.AsQueryable());
            instanceRepoMock.Setup(m => m.Find(1)).Returns(instance1);
            instanceRepoMock.Setup(m => m.Find(2)).Returns(instance2);
            instanceRepoMock.Setup(m => m.Find(3)).Returns(instance3);
            instanceRepoMock.Setup(m => m.Find(6)).Returns<object>(null);


            // Организация - создание контроллера
            InstanceController controller = CreateInstanceController(instanceRepoMock.Object, null, null, null);

            //Действие
            InstanceModel model1 = ((ViewResult)controller.Edit( 1)).ViewData.Model as InstanceModel;
            InstanceModel model2 = ((ViewResult)controller.Edit( 2)).ViewData.Model as InstanceModel;
            InstanceModel model3 = ((ViewResult)controller.Edit( 3)).ViewData.Model as InstanceModel;
            ActionResult result6 = controller.Edit( 6);

            // Утверждение - Редактируем верные объекты из списка
            Assert.AreEqual(1, model1.Id);
            Assert.AreEqual(2, model2.Id);
            Assert.AreEqual(3, model3.Id);

            //Неверный объект 
            Assert.IsInstanceOfType(result6, typeof(ActionResult));

        }

        /// <summary>
        /// Тестируем POST метод Edit. Проверяем валидность модели
        /// </summary>
        [TestMethod]
        public void Can_Save_Valid_Instance()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IInstanceRepository> mock = new Mock<IInstanceRepository>();

            // Организация - создание контроллера
            InstanceController controller = CreateInstanceController(mock.Object, null, null, null);

            //Организация - создание объекта Варианта
            InstanceModel instance = new InstanceModel();

            // Действие - попытка сохранения Варианта
            ActionResult result = controller.Edit(instance,null);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.Save(instance));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));

        }

        /// <summary>
        /// Тестируем POST метод Edit. Проверяем валидность модели
        /// </summary>
        [TestMethod]
        public void CanNot_Save_InValid_Instance()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IInstanceRepository> mock = new Mock<IInstanceRepository>();

            // Организация - создание контроллера
            InstanceController controller = CreateInstanceController(mock.Object, null, null, null);

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения Варианта
            ActionResult result = controller.Edit(null,null);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.Save(It.IsAny<Instance>(),null), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        #endregion

        #region Вспомогательные методы

        public InstanceController CreateInstanceController(IInstanceRepository instanceRepository, ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository, IStrategyInfoRepository strategyInfoRepository)
        {
            // Организация - создание имитированного хранилища данных
            Mock<IInstanceRepository> mockInstance = new Mock<IInstanceRepository>();
            Mock<ITickerRepository> mockTicker = new Mock<ITickerRepository>();
            Mock<ITimeFrameRepository> mockTimeFrame = new Mock<ITimeFrameRepository>();
            Mock<IStrategyInfoRepository> mockStrategyInfo = new Mock<IStrategyInfoRepository>();

            if (instanceRepository == null)
                instanceRepository = mockInstance.Object;
            if (tickerRepository == null)
                tickerRepository = mockTicker.Object;
            if (timeFrameRepository == null)
                timeFrameRepository = mockTimeFrame.Object;
            if (strategyInfoRepository == null)
                strategyInfoRepository = mockStrategyInfo.Object;

            // Организация - создание контроллера
            InstanceController controller = new InstanceController(instanceRepository, tickerRepository, timeFrameRepository, strategyInfoRepository);

            return controller;
        }

        #endregion



    }
}
