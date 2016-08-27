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

namespace RMarket.WebUI.Controllers
{
    public class InstanceController : Controller
    {
        private IInstanceService instanceService;
        private ITickerRepository tickerRepository;
        private ITimeFrameRepository timeFrameRepository;
        private IEntityInfoRepository strategyInfoRepository;

        public InstanceController(IInstanceService instanceService, ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository, IEntityInfoRepository strategyInfoRepository)
        {
            this.instanceService = instanceService;
            this.tickerRepository = tickerRepository;
            this.timeFrameRepository = timeFrameRepository;
            this.strategyInfoRepository = strategyInfoRepository;
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
        /// <param name="instanceId">Если 0 - создание нвого варианта</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int instanceId = 0)
        {
            InitializeLists();
            InstanceModel model = null;

            if (instanceId != 0)
            {
                model = instanceService.GetById(instanceId, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", instanceId);
                    return RedirectToAction("Index");
                }
            }
            else 
                model = new InstanceModel();

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(InstanceModel instance, IEnumerable<ParamEntity> strategyParams)
        {
            instance.StrategyParams = strategyParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                instanceService.Save(instance);

                TempData["message"] = String.Format("Сохранены изменения в экземпляре: {0}", instance.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(instance);
            }

        }

        public PartialViewResult EditParams(IEnumerable<ParamEntity> strategyParams, int instanceId = 0, int entityInfoId = 0)
        {

            if (strategyParams == null)
                strategyParams = new List<ParamEntity>();

            if (strategyParams.Count() == 0)
            {
                if (instanceId != 0)
                {
                    //Сохраненный вариант
                    InstanceModel instance = instanceService.GetById(instanceId, i=>i.EntityInfo);
                    strategyParams = instance.StrategyParams;
                }
                else if (entityInfoId != 0)
                {
                    //Новый вариант
                    EntityInfo entityInfo = strategyInfoRepository.GetById(entityInfoId);
                    strategyParams = StrategyHelper.GetEntityParams<ParamEntity>(entityInfo);
                }
            }

            return PartialView(strategyParams);
        }

        [HttpGet]
        public ActionResult Copy(int instanceId)
        {
            InstanceModel instance = instanceService.GetById(instanceId, true);
            if (instance == null)
            {
                TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", instanceId);
                return RedirectToAction("Index");
            }

            instance.Id = 0;

            return _Edit(instance);
        }

        public PartialViewResult MenuNav(int entityInfoId = 0)
        {

            IEnumerable<MenuNavModel> models = instanceService.Get(t => t
            .GroupBy(i => i.EntityInfo).OrderBy(g => g.Key.Name).Select(g => new MenuNavModel{ EntityInfo = g.Key, Count = g.Select(i => i.GroupID).Distinct().Count() })
            );

            return PartialView(models);
        }

        public ActionResult Details(int instanceId)
        {
            InstanceModel model = new InstanceModel();
            if (instanceId != 0)
            {
                model = instanceService.GetById(instanceId, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", instanceId);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
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
        private ActionResult _Edit(InstanceModel model)
        {
            InitializeLists();

            LoadNavigationProperties(model);
            model.StrategyParams = StrategyHelper.GetEntityParams(model.EntityInfo, model.StrategyParams).ToList();

            return View("Edit", model);

        }

        private void InitializeLists()
        {
            ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
            ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);
            ViewBag.StrategyInfoList = ModelHelper.GetStrategyInfoList(strategyInfoRepository);

        }

        private void LoadNavigationProperties(InstanceModel model)
        {
            if (model.EntityInfo == null && model.EntityInfoId != 0)
                model.EntityInfo = strategyInfoRepository.GetById(model.EntityInfoId);

            if (model.Ticker == null && model.TickerId != 0)
                model.Ticker = tickerRepository.GetById(model.TickerId);

            if (model.TimeFrame == null && model.TimeFrameId != 0)
                model.TimeFrame = timeFrameRepository.GetById(model.TimeFrameId);

            //if (model.Selection == null && model.SelectionId.HasValue)
            //    model.Selection = selectionService.GetById(model.SelectionId.Value);

        }

        #endregion

    }
}