using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Infrastructure;
using RMarket.ClassLib.Helpers;
using RMarket.WebUI.Models;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Infrastructure.AmbientContext;

namespace RMarket.WebUI.Controllers
{
    public class InstanceController : Controller
    {
        private IInstanceService instanceService;
        private ITickerRepository tickerRepository;
        private ITimeFrameRepository timeFrameRepository;
        private IEntityInfoRepository entityInfoRepository;
        private readonly IResolver resolver;


        public InstanceController(IInstanceService instanceService, ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository, IEntityInfoRepository entityInfoRepository, IResolver resolver)
        {
            this.instanceService = instanceService;
            this.tickerRepository = tickerRepository;
            this.timeFrameRepository = timeFrameRepository;
            this.entityInfoRepository = entityInfoRepository;
            this.resolver = resolver;
        }

        /// <summary>
        /// Список вариантов стратегий
        /// </summary>
        /// <returns></returns>
        public ViewResult Index(int entityInfoId = 0)
        {
            IEnumerable<InstanceModel> res = instanceService.Get(T => T
                .Where(i => i.EntityInfoId == entityInfoId || entityInfoId == 0)
                .GroupBy(s => s.GroupID)
                .SelectMany(g => g.Select(s => s)
                    .OrderByDescending(s => s.Id)
                    .Take(1)
                ).Include(m => m.EntityInfo)
            );

            return View(res);

        }

        /// <summary>
        /// Редактирование стратегии
        /// </summary>
        /// <param name="id">Если 0 - создание нвого варианта</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            InitializeLists();
            InstanceModel model = null;

            if (id != 0)
            {
                model = instanceService.GetById(id, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", id);
                    return RedirectToAction("Index");
                }
                RepairParams(model);
            }
            else 
                model = new InstanceModel();

            InstanceModelUI modelUI = MyMapper.Current
                .Map<InstanceModel, InstanceModelUI>(model);

            return View("Edit", modelUI);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(InstanceModelUI modelUI, IEnumerable<ParamEntityUI> strategyParams)
        {
            modelUI.EntityParams = strategyParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                InstanceModel model = MyMapper.Current
                    .Map<InstanceModelUI, InstanceModel>(modelUI);

                instanceService.Save(model);

                TempData["message"] = String.Format("Сохранены изменения в экземпляре: {0}", model.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(modelUI);
            }

        }

        [HttpGet]
        public ActionResult Copy(int id)
        {
            InstanceModel model = instanceService.GetById(id, true);
            if (model == null)
            {
                TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", id);
                return RedirectToAction("Index");
            }
            RepairParams(model);

            model.Id = 0;

            InstanceModelUI modelUI = MyMapper.Current
                .Map<InstanceModel, InstanceModelUI>(model);

            return _Edit(modelUI);
        }

        public ActionResult Details(int id)
        {
            InstanceModel model = new InstanceModel();
            if (id != 0)
            {
                model = instanceService.GetById(id, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", id);
                    return RedirectToAction("Index");
                }
                RepairParams(model);
            }

            InstanceModelUI modelUI = MyMapper.Current
                .Map<InstanceModel, InstanceModelUI>(model);

            return View(modelUI);
        }

        public PartialViewResult MenuNav(int entityInfoId = 0)
        {

            IEnumerable<MenuNavModel> models = instanceService.Get(t => t
            .GroupBy(i => i.EntityInfo).OrderBy(g => g.Key.Name).Select(g => new MenuNavModel{ EntityInfo = g.Key, Count = g.Select(i => i.GroupID).Distinct().Count() })
            );

            return PartialView(models);
        }

        #region ////////////////////////////AJAX
        public PartialViewResult InstanceRecCollection(int instanceId)
        {
            InstanceModel instance = instanceService.GetById(instanceId);
            IEnumerable<InstanceModel> oldInstances = instanceService.Get(T=>T
                .Where(i => i.GroupID == instance.GroupID && i.Id != instanceId)
                .OrderByDescending(i => i.Id)
            );

            return PartialView(oldInstances);
        }

        public PartialViewResult InstanceRecTooltip(int instanceId)
        {
            InstanceModel instance = instanceService.GetById(instanceId);

            return PartialView(instance);
        }
        #endregion


        #region////////////////////////////Private metods
        /// <summary>
        /// Редактирование стратегии
        /// </summary>
        /// <param name="instanceId">Если 0 - создание нвого варианта</param>
        /// <returns></returns>
        private ViewResult _Edit(InstanceModelUI model)
        {
            InitializeLists();
            LoadNavigationProperties(model);

            return View("Edit", model);

        }

        private void InitializeLists()
        {
            ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
            ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);
            ViewBag.StrategyInfoList = ModelHelper.GetStrategyInfoList(entityInfoRepository);

        }

        private void LoadNavigationProperties(InstanceModelUI model)
        {
            if (model.EntityInfo == null && model.EntityInfoId != 0)
                model.EntityInfo = entityInfoRepository.GetById(model.EntityInfoId);

            if (model.Ticker == null && model.TickerId != 0)
                model.Ticker = tickerRepository.GetById(model.TickerId);

            if (model.TimeFrame == null && model.TimeFrameId != 0)
                model.TimeFrame = timeFrameRepository.GetById(model.TimeFrameId);

        }

        /// <summary>
        /// добавить несохраненные параметры
        /// </summary>
        /// <param name="model"></param>
        private void RepairParams(InstanceModel model)
        {
            IStrategy strategy = resolver.Resolve<IStrategy>(Type.GetType(model.EntityInfo.TypeName));
            new SettingHelper().RepairValues(strategy, model.EntityParams);
        }

        #endregion

    }
}