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

namespace RMarket.WebUI.Controllers
{
    public class InstanceController : Controller
    {
        private IInstanceRepository instanceRepository;
        private ITickerRepository tickerRepository;
        private ITimeFrameRepository timeFrameRepository;
        private IStrategyInfoRepository strategyInfoRepository;

        public InstanceController(IInstanceRepository instanceRepository, ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository, IStrategyInfoRepository strategyInfoRepository)
        {
            this.instanceRepository = instanceRepository;
            this.tickerRepository = tickerRepository;
            this.timeFrameRepository = timeFrameRepository;
            this.strategyInfoRepository = strategyInfoRepository;
        }

        /// <summary>
        /// Список вариантов стратегий
        /// </summary>
        /// <returns></returns>
        public ViewResult Index(int strategyInfoId = 0)
        {
            IQueryable<Instance> res = instanceRepository.Instances
                .Where(i=>i.StrategyInfoId==strategyInfoId || strategyInfoId==0)
                .GroupBy(s => s.GroupID)
                .SelectMany(g => g.Select(s => s)
                    .OrderByDescending(s => s.Id)
                    .Take(1)
                ).Include(m => m.StrategyInfo);

            return View(res);

        }

        /// <summary>
        /// Редактирование стратегии
        /// </summary>
        /// <param name="instanceId">Если 0 - создание нвого варианта</param>
        /// <returns></returns>
        private ActionResult _Edit(InstanceModel model = null, int instanceId = 0)
        {
            ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
            ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);
            ViewBag.StrategyInfoList = ModelHelper.GetStrategyInfoList(strategyInfoRepository);

            if (model != null && model.StrategyInfo != null) //повторно пришло
            {
                model.StrategyParams = EntityHelper.GetEntityParams(model.StrategyInfo, model.StrategyParams);
            }
            else if (instanceId != 0)
            {
                model = instanceRepository.FindModel(instanceId);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", instanceId);
                    return RedirectToAction("Index");
                }
            }
            else //Запрос без параметров
                model = new InstanceModel();

            return View("Edit", model);

        }

        /// <summary>
        /// Редактирование стратегии
        /// </summary>
        /// <param name="instanceId">Если 0 - создание нвого варианта</param>
        /// <returns></returns>
        public ActionResult Edit(int instanceId = 0)
        {
            return _Edit(instanceId: instanceId);
        }

        [HttpPost]
        public ActionResult Edit(InstanceModel instance, IEnumerable<ParamEntity> strategyParems)
        {
            instance.StrategyParams = strategyParems.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                instanceRepository.Save(instance);

                TempData["message"] = String.Format("Сохранены изменения в экземпляре: {0}", instance.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(model: instance);
            }

        }

        public PartialViewResult EditParams(IEnumerable<ParamEntity> strategyParams, int instanceId = 0, int strategyInfoId = 0)
        {

            if (strategyParams == null)
                strategyParams = new List<ParamEntity>();

            if (strategyParams.Count() == 0)
            {
                if (instanceId != 0)
                {
                    //Сохраненный вариант
                    InstanceModel instance = instanceRepository.FindModel(instanceId);
                    strategyParams = instance.StrategyParams;
                }
                else if (strategyInfoId != 0)
                {
                    //Новый вариант
                    StrategyInfo strategyInfo = strategyInfoRepository.Find(strategyInfoId);
                    strategyParams = EntityHelper.GetEntityParams<ParamEntity>(strategyInfo);
                }
            }

            return PartialView(strategyParams);
        }

        public ActionResult Copy(int instanceId)
        {
            InstanceModel instance = instanceRepository.FindModel(instanceId);
            if (instance == null)
            {
                TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", instanceId);
                return RedirectToAction("Index");
            }

            instance.Id = 0;

            return _Edit(model: instance);
        }

        public PartialViewResult MenuNav(int strategyInfoId = 0)
        {
            ViewBag.CurStrategyInfoId = strategyInfoId;

            return PartialView(instanceRepository.Instances);
        }

        public ActionResult Details(int instanceId)
        {
            InstanceModel model = new InstanceModel();
            if (instanceId != 0)
            {
                model = instanceRepository.FindModel(instanceId);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр стратегии \"{0}\"  не найден!", instanceId);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                instanceRepository.Dispose();
                strategyInfoRepository.Dispose();
                tickerRepository.Dispose();
                timeFrameRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        #region AJAX
        public PartialViewResult InstanceRecCollection(int instanceId)
        {
            Instance instance = instanceRepository.Find(instanceId);
            IEnumerable<Instance> oldInstances = instanceRepository.Instances.Where(i => i.GroupID == instance.GroupID && i.Id != instanceId).OrderByDescending(i => i.Id);

            return PartialView(oldInstances);
        }

        public PartialViewResult InstanceRecTooltip(int instanceId)
        {
            InstanceModel instance = instanceRepository.FindModel(instanceId);

            return PartialView(instance);
        }


        #endregion

    }
}