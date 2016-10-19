using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.WebUI.Infrastructure;
using RMarket.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Controllers
{
    public class DownloadHistoryController : Controller
    {
        private readonly ITickerRepository tickerRepository;
        private readonly ITimeFrameRepository timeFrameRepository;
        private readonly IHistoricalProviderSettingService historicalProviderSettingService;
        private readonly IResolver resolver;

        public DownloadHistoryController(IHistoricalProviderSettingService historicalProviderSettingService, ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository, IResolver resolver)
        {
            this.historicalProviderSettingService = historicalProviderSettingService;
            this.tickerRepository = tickerRepository;
            this.timeFrameRepository = timeFrameRepository;
            this.resolver = resolver;
        }

        // GET
        public ViewResult Index()
        {
            InitializeLists();

            DownloadHistoryModel model = new DownloadHistoryModel();
            model.DateFrom = DateTime.Now.Date.AddDays(-1);
            model.DateTo = DateTime.Now.Date;

            return View(model);
        }

        [HttpPost]
        public ViewResult Index(DownloadHistoryModel model)
        {
            InitializeLists();

            if (ModelState.IsValid)
            {
                LoadNavigationProperties(model);

                //получаем объект провайдера
                IHistoricalProvider provider = new SettingHelper().CreateEntityObject<IHistoricalProvider>(model.Setting, resolver);

                int countDownload = provider.DownloadAndSave(model.DateFrom, model.DateFrom, model.Ticker, model.TimeFrame);

                TempData["message"] = String.Format("Успешно загружено: {0} свечей", countDownload);
            }

            return View(model);

        }

        #region////////////////////////////Private metods

        private void InitializeLists()
        {
            ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
            ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);
            ViewBag.SettingList = ModelHelper.GetHistoricalProviderSettingList(historicalProviderSettingService);
        }

        private void LoadNavigationProperties(DownloadHistoryModel model)
        {
            if (model.Ticker == null && model.TickerId != 0)
                model.Ticker = tickerRepository.GetById(model.TickerId);

            if (model.TimeFrame == null && model.TimeFrameId != 0)
                model.TimeFrame = timeFrameRepository.GetById(model.TimeFrameId);

            if (model.Setting == null && model.SettingId != 0)
                model.Setting = historicalProviderSettingService.GetById(model.SettingId, true);

        }

        #endregion

    }
}