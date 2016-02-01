using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
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
        private ISelectionRepository selectionRepository;
        private ITickerRepository tickerRepository;
        private ITimeFrameRepository timeFrameRepository;
        private IStrategyInfoRepository strategyInfoRepository;

        public SelectionController(ISelectionRepository selectionRepository, ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository, IStrategyInfoRepository strategyInfoRepository)
        {
            this.selectionRepository = selectionRepository;
            this.tickerRepository = tickerRepository;
            this.timeFrameRepository = timeFrameRepository;
            this.strategyInfoRepository = strategyInfoRepository;
        }

        // GET: Selection
        public ViewResult Index(int strategyInfoId = 0)
        {
            IQueryable<Selection> res = selectionRepository.Selections
                .Where(i => i.StrategyInfoId == strategyInfoId || strategyInfoId == 0)
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
        /// <param name="selectionId">Если 0 - создание нвого варианта</param>
        /// <returns></returns>
        private ActionResult _Edit(SelectionModel model = null, int selectionId = 0)
        {
            ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
            ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);
            ViewBag.StrategyInfoList = ModelHelper.GetStrategyInfoList(strategyInfoRepository);

            if (model != null && model.StrategyInfo != null) //повторно пришло
            {
                model.SelectionParams = EntityHelper.GetEntityParams(model.StrategyInfo, model.SelectionParams);
            }
            else if (selectionId != 0)
            {
                model = selectionRepository.FindModel(selectionId);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", selectionId);
                    return RedirectToAction("Index");
                }
            }
            else //Запрос без параметров
            {
                model = new SelectionModel();
                model.DateFrom = DateTime.Now.Date.AddMonths(-1);
                model.DateTo = DateTime.Now.Date;
            }

            return View("Edit", model);

        }

        [HttpGet]
        public ActionResult Edit(int instanceId = 0)
        {
            return _Edit(selectionId: instanceId);
        }

        [HttpPost]
        public ActionResult Edit(SelectionModel selection, IEnumerable<ParamSelection> selectionParams)
        {
            selection.SelectionParams = selectionParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                selectionRepository.Save(selection);

                TempData["message"] = String.Format("Сохранены изменения в селекции: {0}", selection.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return _Edit(model: selection);
            }

        }

        public PartialViewResult EditParams(IEnumerable<ParamSelection> selectionParams, int strategyInfoId = 0, int instanceId = 0)
        {

            if(selectionParams==null)
                selectionParams = new List<ParamSelection>();

            if (selectionParams.Count() == 0)
            {
                if (instanceId != 0)
                {
                    //Сохраненный вариант
                    Selection selection = selectionRepository.Find(instanceId);
                    selectionParams = StrategyHelper.GetStrategyParams(selection);
                }
                else if (strategyInfoId != 0)
                {
                    //Новый вариант
                    StrategyInfo strategyInfo = strategyInfoRepository.Find(strategyInfoId);
                    selectionParams = EntityHelper.GetEntityParams<ParamSelection>(strategyInfo);
                }
            }      

            return PartialView(selectionParams);
        }

        public ActionResult Copy(int instanceId)
        {
            SelectionModel selection = selectionRepository.FindModel(instanceId);
            if (selection == null)
            {
                TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", instanceId);
                return RedirectToAction("Index");
            }

            selection.Id = 0;

            return _Edit(model: selection);
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
            ViewBag.CurStrategyInfoId = strategyInfoId;

            return PartialView(selectionRepository.Selections);
        }

        public ActionResult Details(int instanceId)
        {
            SelectionModel model = new SelectionModel();
            if (instanceId != 0)
            {
                model = selectionRepository.FindModel(instanceId);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", instanceId);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                selectionRepository.Dispose();
                strategyInfoRepository.Dispose();
                tickerRepository.Dispose();
                timeFrameRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        #region AJAX
        public PartialViewResult InstanceRecCollection(int instanceId)
        {
            Selection selection = selectionRepository.Find(instanceId);
            IEnumerable<Selection> oldInstances = selectionRepository.Selections.Where(i => i.GroupID == selection.GroupID && i.Id != instanceId).OrderByDescending(i => i.Id);

            return PartialView(oldInstances);
        }

        public PartialViewResult InstanceRecTooltip(int instanceId)
        {
            SelectionModel seleection = selectionRepository.FindModel(instanceId);

            return PartialView(seleection);
        }


        #endregion
    }
}