﻿using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers;
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

        public ActionResult Edit( int settingId = 0)
        {
            return _Edit(settingId: settingId);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HistoricalProviderSettingModel model, IEnumerable<ParamEntity> entityParams)
        {
            model.EntityParams = entityParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                settingService.Save(model);

                TempData["message"] = String.Format("Сохранены изменения в экземпляре: {0}", model.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(model: model);
            }
        }

        public PartialViewResult EditParams(IEnumerable<ParamEntity> entityParams, int settingId = 0)
        {

            if (entityParams == null)
                entityParams = new List<ParamEntity>();

            if (entityParams.Count() == 0)
            {
                if (settingId != 0)
                {
                    //Сохраненный вариант
                    HistoricalProviderSettingModel setting = settingService.GetById(settingId, true);
                    entityParams = setting.EntityParams;
                }
            }
            return PartialView(entityParams);
        }

        //Новый экземпляр
        public PartialViewResult EditParamsNew(int entityInfoId)
        {
            EntityInfo entityInfo = entityInfoRepository.GetById(entityInfoId);

            IEnumerable<ParamEntity> entityParams = new SettingHelper().GetEntityParams<ParamEntity>(entityInfo);

            return PartialView("EditParams",entityParams);
        }


        public ActionResult Copy(int settingId)
        {
            HistoricalProviderSettingModel setting = settingService.GetById(settingId);
            if (setting == null)
            {
                TempData["error"] = String.Format("Экземпляр настройки \"{0}\"  не найден!", settingId);
                return RedirectToAction("Index");
            }

            setting.Id = 0;

            return _Edit(model: setting);
        }

        #region////////////////////////////Private metods

        private ActionResult _Edit(HistoricalProviderSettingModel model = null, int settingId = 0)
        {

            if (model != null) //повторно пришло
            {
                LoadNavigationProperties(model);
                model.EntityParams = new SettingHelper().GetEntityParams(model.EntityInfo, model.EntityParams).ToList();
            }
            else if (settingId != 0)
            {
                model = settingService.GetById(settingId);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр настройки \"{0}\"  не найден!", settingId);
                    return RedirectToAction("Index");
                }

            }
            else //Запрос без параметров
                model = new HistoricalProviderSettingModel();

            return View("Edit", model);
        }

        private void InitializeLists()
        {
            ViewBag.EntityInfoList = ModelHelper.GetHistoricalProviderInfoList(entityInfoRepository);
        }

        private void LoadNavigationProperties(HistoricalProviderSettingModel model)
        {
            if (model.EntityInfo == null && model.EntityInfoId != 0)
                model.EntityInfo = entityInfoRepository.GetById(model.EntityInfoId);

        }

        #endregion


    }
}