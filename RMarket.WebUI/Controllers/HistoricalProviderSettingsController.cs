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
    public class HistoricalProviderSettingsController : Controller
    {
        private IHistoricalProviderSettingService settingService;
        private IEntityInfoRepository entityInfoRepository;

        public HistoricalProviderSettingsController(IHistoricalProviderSettingService settingService, IEntityInfoRepository entityInfoRepository)
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
        public ActionResult Edit( int id = 0)
        {
            InitializeLists();

            HistoricalProviderSettingModel model = null;

            if (id != 0)
            {
                model = settingService.GetById(id, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр настройки \"{0}\"  не найден!", id);
                    return RedirectToAction("Index");
                }

            }
            else //Запрос на создание
                model = new HistoricalProviderSettingModel();

            HistoricalProviderSettingModelUI modelUI = MyMapper.Current
                .Map<HistoricalProviderSettingModel, HistoricalProviderSettingModelUI>(model);

            return View("Edit", modelUI);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HistoricalProviderSettingModelUI modelUI, IEnumerable<ParamEntityUI> entityParams)
        {
            modelUI.EntityParams = entityParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                HistoricalProviderSettingModel model = MyMapper.Current
                    .Map<HistoricalProviderSettingModelUI, HistoricalProviderSettingModel>(modelUI);

                settingService.Save(model);

                TempData["message"] = String.Format("Сохранены изменения в экземпляре: {0}", modelUI.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(model: modelUI);
            }
        }

        public ActionResult Copy(int id)
        {
            HistoricalProviderSettingModel setting = settingService.GetById(id, true);
            if (setting == null)
            {
                TempData["error"] = String.Format("Экземпляр настройки \"{0}\"  не найден!", id);
                return RedirectToAction("Index");
            }

            setting.Id = 0;

            HistoricalProviderSettingModelUI modelUI = MyMapper.Current
                .Map<HistoricalProviderSettingModel, HistoricalProviderSettingModelUI>(setting);

            return _Edit(model: modelUI);
        }

        #region////////////////////////////Private metods

        private ActionResult _Edit(HistoricalProviderSettingModelUI model)
        {
            InitializeLists();
            LoadNavigationProperties(model);

            return View("Edit", model);
        }

        private void InitializeLists()
        {
            ViewBag.EntityInfoList = ModelHelper.GetHistoricalProviderInfoList(entityInfoRepository);
        }

        private void LoadNavigationProperties(HistoricalProviderSettingModelUI model)
        {
            if (model.EntityInfo == null && model.EntityInfoId != 0)
                model.EntityInfo = entityInfoRepository.GetById(model.EntityInfoId);

        }

        #endregion


    }
}