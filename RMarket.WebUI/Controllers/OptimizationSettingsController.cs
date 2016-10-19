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
    public class OptimizationSettingsController : Controller
    {
        private IOptimizationSettingService settingService;
        private IEntityInfoRepository entityInfoRepository;
        private readonly IResolver resolver;

        public OptimizationSettingsController(IOptimizationSettingService settingService, IEntityInfoRepository entityInfoRepository, IResolver resolver)
        {
            this.settingService = settingService;
            this.entityInfoRepository = entityInfoRepository;
            this.resolver = resolver;
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

            OptimizationSettingModel model = null;

            if (Id != 0)
            {
                model = settingService.GetById(Id, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр настройки \"{0}\"  не найден!", Id);
                    return RedirectToAction("Index");
                }
                RepairParams(model);
            }
            else //Запрос на создание
                model = new OptimizationSettingModel();

            OptimizationSettingModelUI modelUI = MyMapper.Current
                .Map<OptimizationSettingModel, OptimizationSettingModelUI>(model);

            return View("Edit", modelUI);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(OptimizationSettingModelUI modelUI, IEnumerable<ParamEntityUI> entityParams)
        {
            modelUI.EntityParams = entityParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                OptimizationSettingModel model = MyMapper.Current
                    .Map<OptimizationSettingModelUI, OptimizationSettingModel>(modelUI);

                settingService.Save(model);

                TempData["message"] = String.Format("Сохранены изменения в экземпляре: {0}", modelUI.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(model: modelUI);
            }
        }

        public ActionResult Copy(int Id)
        {
            OptimizationSettingModel model = settingService.GetById(Id, true);
            if (model == null)
            {
                TempData["error"] = String.Format("Экземпляр настройки \"{0}\"  не найден!", Id);
                return RedirectToAction("Index");
            }
            RepairParams(model);

            model.Id = 0;

            OptimizationSettingModelUI modelUI = MyMapper.Current
                .Map<OptimizationSettingModel, OptimizationSettingModelUI>(model);

            return _Edit(model: modelUI);
        }

        #region////////////////////////////Private metods

        private ActionResult _Edit(OptimizationSettingModelUI model)
        {
            InitializeLists();
            LoadNavigationProperties(model);

            return View("Edit", model);
        }

        private void InitializeLists()
        {
            ViewBag.EntityInfoList = ModelHelper.GetOptimizationInfoList(entityInfoRepository);
        }

        private void LoadNavigationProperties(OptimizationSettingModelUI model)
        {
            if (model.EntityInfo == null && model.EntityInfoId != 0)
                model.EntityInfo = entityInfoRepository.GetById(model.EntityInfoId);

        }

        /// <summary>
        /// добавить несохраненные параметры
        /// </summary>
        /// <param name="model"></param>
        private void RepairParams(OptimizationSettingModel model)
        {
            var entity = resolver.Resolve<IOptimization>(Type.GetType(model.EntityInfo.TypeName));
            new SettingHelper().RepairValues(entity, model.EntityParams);
        }


        #endregion


    }
}