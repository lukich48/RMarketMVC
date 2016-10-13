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
        private readonly IEntityInfoRepository entityInfoRepository;
        private readonly Resolver resolver;

        public SelectionController(ISelectionService selectionService, ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository, IEntityInfoRepository entityInfoRepository, Resolver resolver)
        {
            this.selectionService = selectionService;
            this.tickerRepository = tickerRepository;
            this.timeFrameRepository = timeFrameRepository;
            this.entityInfoRepository = entityInfoRepository;
            this.resolver = resolver;
        }

        // GET: Selection
        public ViewResult Index(int entityInfoId = 0)
        {
            IEnumerable<SelectionModel> res = selectionService.Get(T => T
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
            SelectionModel model = null;

            if (id != 0)
            {
                model = selectionService.GetById(id, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", id);
                    return RedirectToAction("Index");
                }
            }
            else
                model = new SelectionModel();

            SelectionModelUI modelUI = MyMapper.Current
                .Map<SelectionModel, SelectionModelUI>(model);

            return View("Edit", modelUI);
        }

        [HttpPost]
        public ActionResult Edit(SelectionModelUI modelUI, IEnumerable<ParamSelectionUI> selectionParams)
        {
            modelUI.SelectionParams = selectionParams.ToList();

            if (ModelState.IsValid)
            {
                //Сохранение
                SelectionModel model = MyMapper.Current
                    .Map<SelectionModelUI, SelectionModel>(modelUI);

                selectionService.Save(model);

                TempData["message"] = String.Format("Сохранены изменения в селекции: {0}", modelUI.Name);
                return RedirectToAction("Index");
            }

            else
            {
                return _Edit(modelUI);
            }

        }

        public ActionResult Copy(int id)
        {
            SelectionModel model = selectionService.GetById(id, true);
            if (model == null)
            {
                TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", id);
                return RedirectToAction("Index");
            }

            model.Id = 0;

            SelectionModelUI modelUI = MyMapper.Current
                .Map<SelectionModel, SelectionModelUI>(model);

            return _Edit(modelUI);
        }

        public PartialViewResult MenuNav(int entityInfoId = 0)
        {
            IEnumerable<MenuNavModel> models = selectionService.Get(t => t
            .GroupBy(i => i.EntityInfo).OrderBy(g => g.Key.Name).Select(g => new MenuNavModel { EntityInfo = g.Key, Count = g.Select(i => i.GroupID).Distinct().Count() })
            );

            return PartialView(models);
        }

        public ActionResult Details(int id)
        {
            SelectionModel model = new SelectionModel();
            if (id != 0)
            {
                model = selectionService.GetById(id, true);
                if (model == null)
                {
                    TempData["error"] = String.Format("Экземпляр селекции \"{0}\"  не найден!", id);
                    return RedirectToAction("Index");
                }
            }

            SelectionModelUI modelUI = MyMapper.Current
                .Map<SelectionModel, SelectionModelUI>(model);

            return View(modelUI);
        }

        //Новый экземпляр
        public PartialViewResult EditParamsNew(int entityInfoId)
        {
            EntityInfo entityInfo = entityInfoRepository.GetById(entityInfoId);
            object entity = resolver.Resolve(Type.GetType(entityInfo.TypeName));

            IEnumerable<ParamSelection> entityParams = new SettingHelper().GetEntityParams<ParamSelection>(entity);

            //Конвертим параметры в UI модель
            IEnumerable<ParamSelectionUI> entityParamsUI = MyMapper.Current
                .Map<IEnumerable<ParamSelection>, IEnumerable<ParamSelectionUI>>(entityParams);

            return PartialView("EditParams", entityParamsUI);
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
        private ActionResult _Edit(SelectionModelUI model)
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

        public void LoadNavigationProperties(SelectionModelUI model)
        {
            if (model.EntityInfo == null && model.EntityInfoId != 0)
                model.EntityInfo = entityInfoRepository.GetById(model.EntityInfoId);

            if (model.Ticker == null && model.TickerId != 0)
                model.Ticker = tickerRepository.GetById(model.TickerId);

            if (model.TimeFrame == null && model.TimeFrameId != 0)
                model.TimeFrame = timeFrameRepository.GetById(model.TimeFrameId);
        }

        #endregion

    }
}