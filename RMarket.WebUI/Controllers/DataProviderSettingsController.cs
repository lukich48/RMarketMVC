using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Infrastructure;
using RMarket.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Controllers
{
    public class DataProviderSettingsController : Controller
    {
        private IDataProviderSettingService settingService;
        private IEntityInfoRepository entityInfoRepository;

        public DataProviderSettingsController(IDataProviderSettingService settingService, IEntityInfoRepository entityInfoRepository)
        {
            this.settingService = settingService;
            this.entityInfoRepository = entityInfoRepository;
        }

        // GET: StrategySettings
        public ActionResult Index()
        {
            var res = settingService.Get().ToList();
            return View(res);
        }

        [HttpGet]
        public ActionResult Edit(int Id = 0)
        {
            InitializeLists();

            DataProviderSettingModel model = null;

            if (Id != 0)
            {
                model = settingService.GetById(Id, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр настройки \"{0}\"  не найден!", Id);
                    return RedirectToAction("Index");
                }

            }
            else //Запрос на создание
                model = new DataProviderSettingModel();

            DataProviderSettingModelUI modelUI = MyMapper.Current
                .Map<DataProviderSettingModel, DataProviderSettingModelUI>(model);

            return View("Edit", modelUI);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DataProviderSettingModelUI modelUI, IEnumerable<ParamEntityUI> entityParams)
        {
            modelUI.EntityParams = entityParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                DataProviderSettingModel model = MyMapper.Current
                    .Map<DataProviderSettingModelUI, DataProviderSettingModel>(modelUI);

                settingService.Save(model);

                TempData["message"] = String.Format("Сохранены изменения в экземпляре: {0}", modelUI.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(model: modelUI);
            }
        }

        //Новый экземпляр
        public PartialViewResult EditParamsNew(int entityInfoId)
        {
            EntityInfo entityInfo = entityInfoRepository.GetById(entityInfoId);

            IEnumerable<ParamEntity> entityParams = new SettingHelper().GetEntityParams<ParamEntity>(entityInfo);

            //Конвертим параметры в UI модель
            IEnumerable<ParamEntityUI> entityParamsUI = MyMapper.Current
                .Map<IEnumerable<ParamEntity>, IEnumerable<ParamEntityUI>>(entityParams);

            return PartialView("EditParams", entityParamsUI);
        }


        public ActionResult Copy(int Id)
        {
            DataProviderSettingModel setting = settingService.GetById(Id, true);
            if (setting == null)
            {
                TempData["error"] = String.Format("Экземпляр настройки \"{0}\"  не найден!", Id);
                return RedirectToAction("Index");
            }

            setting.Id = 0;

            DataProviderSettingModelUI modelUI = MyMapper.Current
                .Map<DataProviderSettingModel, DataProviderSettingModelUI>(setting);

            return _Edit(model: modelUI);
        }

        #region////////////////////////////Private metods

        private ActionResult _Edit(DataProviderSettingModelUI model)
        {
            InitializeLists();
            LoadNavigationProperties(model);

            return View("Edit", model);
        }

        private void InitializeLists()
        {
            ViewBag.EntityInfoList = ModelHelper.GetDataProviderInfoList(entityInfoRepository);
        }

        private void LoadNavigationProperties(DataProviderSettingModelUI model)
        {
            if (model.EntityInfo == null && model.EntityInfoId != 0)
                model.EntityInfo = entityInfoRepository.GetById(model.EntityInfoId);

        }

        #endregion


    }
}