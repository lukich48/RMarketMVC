using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Managers;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Infrastructure;
using RMarket.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.WebUI.Helpers;
using RMarket.ClassLib.Abstract.IRepository;
using System.Threading.Tasks;
using RMarket.ClassLib.Infrastructure.AmbientContext;

namespace RMarket.WebUI.Controllers
{
    public class TesterController : Controller
    {
        private readonly IInstanceService instanceService;
        private readonly ICandleRepository candleRepository;
        private readonly IResolver resolver;

        private JsonSerializerSettings jsonSerializerSettings;

        List<AliveResult> strategyResultCollection = CurrentUI.SessionHelper.Get<List<AliveResult>>("TestResults"); 

        public TesterController(IInstanceService instanceService, 
            ICandleRepository candleRepository,
            IResolver resolver)
        {
            this.instanceService = instanceService;
            this.candleRepository = candleRepository;
            this.resolver = resolver;

            jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        // GET:
        public ActionResult Index()
        {
            //определить наличия работающего теста и вызвать форму нового, или результата
            if (strategyResultCollection.Count == 0)
            {
                DateTime dateFrom = DateTime.Now.Date.AddMonths(-1);
                DateTime dateTo = DateTime.Now.Date;
                return RedirectToAction("BeginTest", new { instanceId = 0, dateFrom = dateFrom, dateTo = dateTo });
            }
            else
            {
                return View(strategyResultCollection);
            }
        }

        [HttpGet]
        public ViewResult BeginTest()
        {
            ViewBag.InstanceList = ModelHelper.GetInstanceList(instanceService);

            DateTime dateFrom = DateTime.Now.Date.AddMonths(-1);
            DateTime dateTo = DateTime.Now.Date;

            TesterModel model = new TesterModel();
            model.DateFrom = dateFrom;
            model.DateTo = dateTo;

            return View(model);
        }

        [HttpPost]
        public ActionResult BeginTest(TesterModel model) //!!!В модели использовать Instance
        {
            if (ModelState.IsValid)
            {
                InstanceModel instance = instanceService.GetById(model.InstanceId, true);

                //получаем стратегию 
                IStrategy strategy = new SettingHelper().CreateEntityObject<IStrategy>(instance, resolver);

                //устанавливаем остальные свойства
                Instrument instr = new Instrument(instance.Ticker, instance.TimeFrame); 

                Portfolio portf = new Portfolio
                {
                    Balance = instance.Balance,
                    Rent = instance.Rent,
                    Slippage = instance.Slippage
                };

                IManager manager = new TesterManager(candleRepository, strategy, instr, portf);

                manager.DateFrom = model.DateFrom;
                manager.DateTo = model.DateTo;

                //Стартуем стратегию
                new TaskFactory().StartNew(manager.StartStrategy);

                //***Положить в сессию
                AliveResult aliveResult = new AliveResult
                {
                    AliveId = strategyResultCollection.Count() + 1,
                    StartDate = DateTime.Now,
                    Instance = instance,
                    Strategy = strategy,
                    Manager = manager
                };

                //Извлечь индикаторы из  объекта стратегии
                aliveResult.IndicatorsDict = StrategyHelper.ExtractIndicatorsInStrategy(strategy);

                strategyResultCollection.Add(aliveResult);
                //**

                TempData["message"] = string.Format("Тест успешно запущен. Id={0}",aliveResult.AliveId);

                return RedirectToAction("Index");
            }

            ViewBag.InstanceList = ModelHelper.GetInstanceList(instanceService);

            return View(model);
        }

        [HttpGet]
        public ActionResult DisplayResult(int aliveId)
        {
            AliveResult aliveResult = strategyResultCollection.FirstOrDefault(t => t.AliveId == aliveId);
            if (aliveResult == null)
            {
                TempData["error"] = string.Format("Не найден тест Id:{0}", aliveId);
                return RedirectToAction("Index");
            }

            //if (aliveResult.Strategy.Instr.Candles.Count == 0)
            //{
            //    TempData["error"] = string.Format("Нет сформированных свечей за выбранный период Id:{0}", aliveId);
            //    return RedirectToAction("Index");
            //}

            return View(aliveResult);

        }

        [HttpGet]
        public RedirectToRouteResult TerminateTest(int aliveId)
        {
            AliveResult aliveResult = strategyResultCollection.FirstOrDefault(t => t.AliveId == aliveId);
            if (aliveResult != null && aliveResult.Manager.IsStarted)
            {
                aliveResult.Manager.StopStrategy();
                TempData["warning"] = string.Format("Тест Id={0} был прерван!", aliveResult.AliveId);
            }

             return RedirectToAction("DisplayResult", new { aliveId = aliveResult.AliveId });
        }

        #region //////////////////////Навигация по графику
        /// <summary>
        /// Загрузка последних n свечек
        /// </summary>
        /// <param name="aliveId"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public ActionResult GetDataJsonInit(int aliveId, int maxCount, string way = "right")
        {
            AliveResult aliveResult = strategyResultCollection.FirstOrDefault(t => t.AliveId == aliveId);

            AliveResultHelperUI helper = new AliveResultHelperUI(aliveResult);
            var res = helper.GetDataJsonInit(maxCount, way);

            return new JsonNetResult(res, JsonRequestBehavior.AllowGet, jsonSerializerSettings);
        }

        /// <summary>
        /// загрузка свечей после определенной даты вперед-назад
        /// </summary>
        /// <param name="aliveId"></param>
        /// <param name="lastDateUTC"></param>
        /// <param name="maxCount"></param>
        /// /// <param name="way">right, left</param>
        /// <returns></returns>
        public ActionResult GetDataJsonSlice(int aliveId, double lastDateUTC, int maxCount, string way = "right")
        {
            DateTime posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            DateTime lastDate = posixTime.AddMilliseconds(lastDateUTC);

            AliveResult aliveResult = strategyResultCollection.FirstOrDefault(t => t.AliveId == aliveId);
            if (aliveResult == null)
            {
                TempData["error"] = string.Format("Не найден тест Id={0}", aliveResult.AliveId);
                return RedirectToAction("Index");
            }

            AliveResultHelperUI helper = new AliveResultHelperUI(aliveResult);
            var res = helper.GetDataJsonSlice(lastDate, maxCount, way);

            return new JsonNetResult(res, JsonRequestBehavior.AllowGet, jsonSerializerSettings);
        }

        /// <summary>
        /// подгрузка свечей через интервал
        /// </summary>
        /// <param name="aliveId"></param>
        /// <param name="lastDateUTC"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public ActionResult GetDataJsonAdd(int aliveId, double lastDateUTC, int maxCount)
        {
            DateTime posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            DateTime lastDate = posixTime.AddMilliseconds(lastDateUTC);

            AliveResult aliveResult = strategyResultCollection.FirstOrDefault(t => t.AliveId == aliveId);
            if (aliveResult == null)
            {
                TempData["error"] = string.Format("Не найден тест Id={0}", aliveResult.AliveId);
                return RedirectToAction("Index");
            }

            AliveResultHelperUI helper = new AliveResultHelperUI(aliveResult);
            var res = helper.GetDataJsonAdd(lastDate, maxCount);

            return new JsonNetResult(res, JsonRequestBehavior.AllowGet, jsonSerializerSettings);
        }
        #endregion

    }
}