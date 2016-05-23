using Newtonsoft.Json;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Connectors;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Managers;
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
    public class EmulController : Controller
    {
        private IInstanceRepository instanceRepository;
        private IAliveStrategyRepository aliveStrategyRepository;
        private JsonSerializerSettings jsonSerializerSettings;

        List<TestResult> strategyResultCollection = SessionHelper.Get<List<TestResult>>("EmulResultCollection");

        public EmulController(IInstanceRepository instanceRepository, IAliveStrategyRepository aliveStrategyRepository)
        {
            this.instanceRepository = instanceRepository;
            this.aliveStrategyRepository = aliveStrategyRepository;

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
            ViewBag.InstanceList = ModelHelper.GetInstanceList(instanceRepository);

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
                InstanceModel instance = instanceRepository.GetById(model.InstanceId, true);

                //добавляем живую стратегию
                AliveStrategy aliveStrategy = new AliveStrategy
                {
                    GroupID = instance.GroupID,
                    IsActive = true
                };
                aliveStrategyRepository.Save(aliveStrategy);

                //получаем стратегию 
                IStrategy strategy = StrategyHelper.CreateStrategy(instance);

                //устанавливаем остальные свойства
                Instrument instr = new Instrument(instance.Ticker, instance.TimeFrame);

                Portfolio portf = new Portfolio
                {
                    Balance = instance.Balance,
                    Rent = instance.Rent,
                    Slippage = instance.Slippage
                };

                IDataProvider connector = SettingHelper.CreateDataProvider(instance.StrategyInfo);

                IManager manager = new EmulManager(strategy, instr, portf, connector, aliveStrategy);

                //manager.DateFrom = model.DateFrom;
                //manager.DateTo = model.DateTo;

                //Стартуем стратегию
                manager.StartStrategy();

                //***Положить в сессию
                TestResult testResult = new TestResult
                {
                    Id = aliveStrategy.Id,
                    StartDate = DateTime.Now,
                    Instance = instance,
                    Strategy = strategy,
                    Manager = manager
                };

                //Извлечь индикаторы из  объекта стратегии
                testResult.IndicatorsDict = StrategyHelper.ExtractIndicatorsInStrategy(strategy);

                strategyResultCollection.Add(testResult);
                //**

                TempData["message"] = string.Format("Эмуляция успешно запущена. Id={0}", testResult.Id);

                return RedirectToAction("Index");
            }

            ViewBag.InstanceList = ModelHelper.GetInstanceList(instanceRepository);

            return View(model);
        }

        [HttpGet]
        public ActionResult DisplayResult(int resultId)
        {
            TestResult testResult = strategyResultCollection.FirstOrDefault(t => t.Id == resultId);
            if (testResult == null)
            {
                TempData["error"] = string.Format("Не найдена эмуляция Id:{0}", resultId);
                return RedirectToAction("Index");
            }

            //if (testResult.Strategy.Instr.Candles.Count == 0)
            //{
            //    TempData["error"] = string.Format("Нет сформированных свечей за выбранный период Id:{0}", resultId);
            //    return RedirectToAction("Index");
            //}

            return View(testResult);

        }

        [HttpGet]
        public RedirectToRouteResult TerminateTest(int resultId)
        {
            TestResult testResult = strategyResultCollection.FirstOrDefault(t => t.Id == resultId);
            if (testResult != null && testResult.Manager.IsStarted)
            {
                testResult.Manager.StopStrategy();
                TempData["warning"] = string.Format("Эмуляция Id={0} была прервана!", testResult.Id);
            }

            return RedirectToAction("DisplayResult", new { resultId = testResult.Id });
        }


        #region //////////////////////Навигация по графику
        /// <summary>
        /// Загрузка последних n свечек
        /// </summary>
        /// <param name="resultId"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public ActionResult GetDataJsonInit(int resultId, int maxCount, string way = "right")
        {
            TestResult testResult = strategyResultCollection.FirstOrDefault(t => t.Id == resultId);

            var res = testResult.GetDataJsonInit(maxCount, way);

            return new JsonNetResult(res, JsonRequestBehavior.AllowGet, jsonSerializerSettings);
        }

        /// <summary>
        /// загрузка свечей после определенной даты вперед-назад
        /// </summary>
        /// <param name="resultId"></param>
        /// <param name="lastDateUTC"></param>
        /// <param name="maxCount"></param>
        /// /// <param name="way">right, left</param>
        /// <returns></returns>
        public ActionResult GetDataJsonSlice(int resultId, double lastDateUTC, int maxCount, string way = "right")
        {
            DateTime posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            DateTime lastDate = posixTime.AddMilliseconds(lastDateUTC);

            TestResult testResult = strategyResultCollection.FirstOrDefault(t => t.Id == resultId);
            if (testResult == null)
            {
                TempData["error"] = string.Format("Не найден тест Id={0}", testResult.Id);
                return RedirectToAction("Index");
            }

            var res = testResult.GetDataJsonSlice(lastDate, maxCount, way);

            return new JsonNetResult(res, JsonRequestBehavior.AllowGet, jsonSerializerSettings);
        }

        /// <summary>
        /// подгрузка свечей через интервал
        /// </summary>
        /// <param name="resultId"></param>
        /// <param name="lastDateUTC"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public ActionResult GetDataJsonAdd(int resultId, double lastDateUTC, int maxCount)
        {
            DateTime posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            DateTime lastDate = posixTime.AddMilliseconds(lastDateUTC);

            TestResult testResult = strategyResultCollection.FirstOrDefault(t => t.Id == resultId);
            if (testResult == null)
            {
                TempData["error"] = string.Format("Не найден тест Id={0}", testResult.Id);
                return RedirectToAction("Index");
            }

            var res = testResult.GetDataJsonAdd(lastDate, maxCount);

            return new JsonNetResult(res, JsonRequestBehavior.AllowGet, jsonSerializerSettings);
        }
        #endregion
    }
}