using RMarket.ClassLib.Abstract;
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
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Controllers
{
    public class SelectionController : Controller
    {
        private readonly ISelectionService selectionService;
        private readonly ITickerRepository tickerRepository;
        private readonly ITimeFrameRepository timeFrameRepository;
        private readonly IStrategyInfoRepository strategyInfoRepository;

        public SelectionController(ISelectionService selectionService, ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository, IStrategyInfoRepository strategyInfoRepository)
        {
            this.selectionService = selectionService;
            this.tickerRepository = tickerRepository;
            this.timeFrameRepository = timeFrameRepository;
            this.strategyInfoRepository = strategyInfoRepository;
        }

        // GET: Selection
        public ViewResult Index(int strategyInfoId = 0)
        {
            IEnumerable<SelectionModel> res = selectionService.Get(T => T
                .Where(i => i.StrategyInfoId == strategyInfoId || strategyInfoId == 0)
                .GroupBy(s => s.GroupID)
                .SelectMany(g => g.Select(s => s)
                    .OrderByDescending(s => s.Id)
                    .Take(1)
                ).Include(m => m.StrategyInfo)
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
            SelectionModel model = null;

            if (instanceId != 0)
            {
                model = selectionService.GetById(instanceId, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", instanceId);
                    return RedirectToAction("Index");
                }
            }
            else
                model = new SelectionModel();

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(SelectionModel selection, IEnumerable<ParamSelection> strategyParams)
        {
            selection.SelectionParams = strategyParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                selectionService.Save(selection);

                TempData["message"] = String.Format("Сохранены изменения в селекции: {0}", selection.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(selection);
            }

        }

        public PartialViewResult EditParams(IEnumerable<ParamSelection> strategyParams, int strategyInfoId = 0, int instanceId = 0)
        {

            if(strategyParams==null)
                strategyParams = new List<ParamSelection>();

            if (strategyParams.Count() == 0)
            {
                if (instanceId != 0)
                {
                    //Сохраненный вариант
                    SelectionModel selection = selectionService.GetById(instanceId, s=>s.StrategyInfo);
                    strategyParams = selection.SelectionParams;
                }
                else if (strategyInfoId != 0)
                {
                    //Новый вариант
                    StrategyInfo strategyInfo = strategyInfoRepository.GetById(strategyInfoId);
                    strategyParams = StrategyHelper.GetEntityParams<ParamSelection>(strategyInfo);
                }
            }      

            return PartialView(strategyParams);
        }

        public ActionResult Copy(int instanceId)
        {
            SelectionModel selection = selectionService.GetById(instanceId, true);
            if (selection == null)
            {
                TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", instanceId);
                return RedirectToAction("Index");
            }

            selection.Id = 0;

            return _Edit(selection);
        }

        //[HttpGet]
        //public ActionResult Start()
        //{
        //    ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
        //    ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);
        //    ViewBag.StrategyInfoList = ModelHelper.GetStrategyInfoList(strategyInfoRepository);

        //    return View();
        //}

        public PartialViewResult MenuNav(int strategyInfoId = 0)
        {
            IEnumerable<MenuNavModel> models = selectionService.Get(t => t
            .GroupBy(i => i.StrategyInfo).OrderBy(g => g.Key.Name).Select(g => new MenuNavModel { StrategyInfo = g.Key, Count = g.Select(i => i.GroupID).Distinct().Count() })
            );

            return PartialView(models);
        }

        public ActionResult Details(int instanceId)
        {
            SelectionModel model = new SelectionModel();
            if (instanceId != 0)
            {
                model = selectionService.GetById(instanceId, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", instanceId);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        #region AJAX
        public PartialViewResult InstanceRecCollection(int instanceId)
        {
            SelectionModel instance = selectionService.GetById(instanceId);
            IEnumerable<SelectionModel> oldInstances = selectionService.Get(T => T
                .Where(i => i.GroupID == instance.GroupID && i.Id != instanceId)
                .OrderByDescending(i => i.Id)
            );
            return PartialView(oldInstances);
        }

        public PartialViewResult InstanceRecTooltip(int instanceId)
        {
            SelectionModel seleection = selectionService.GetById(instanceId);

            return PartialView(seleection);
        }


        #endregion

        #region////////////////////////////Private metods
        /// <summary>
        /// Редактирование стратегии
        /// </summary>
        /// <param name="instanceId">Если 0 - создание нвого варианта</param>
        /// <returns></returns>
        private ActionResult _Edit(SelectionModel model)
        {
            InitializeLists();

            LoadNavigationProperties(model);
            model.SelectionParams = StrategyHelper.GetEntityParams(model.StrategyInfo, model.SelectionParams).ToList();

            return View("Edit", model);

        }

        private void InitializeLists()
        {
            ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
            ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);
            ViewBag.StrategyInfoList = ModelHelper.GetStrategyInfoList(strategyInfoRepository);

        }

        public void LoadNavigationProperties(SelectionModel model)
        {
            if (model.StrategyInfo == null && model.StrategyInfoId != 0)
                model.StrategyInfo = strategyInfoRepository.GetById(model.StrategyInfoId);

            if (model.Ticker == null && model.TickerId != 0)
                model.Ticker = tickerRepository.GetById(model.TickerId);

            if (model.TimeFrame == null && model.TimeFrameId != 0)
                model.TimeFrame = timeFrameRepository.GetById(model.TimeFrameId);
        }

        #endregion

    }
}