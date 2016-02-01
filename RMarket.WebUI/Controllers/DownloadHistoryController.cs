using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
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
        private ITickerRepository tickerRepository;
        private ITimeFrameRepository timeFrameRepository;

        public DownloadHistoryController(ITickerRepository tickerRepository, ITimeFrameRepository timeFrameRepository)
        {
            this.tickerRepository = tickerRepository;
            this.timeFrameRepository = timeFrameRepository;
        }

        // GET
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Download(string providerName)
        {
            ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
            ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);

            DownloadHistoryModel model = new DownloadHistoryModel();
            model.DateFrom = DateTime.Now.Date.AddDays(-1);
            model.DateTo = DateTime.Now.Date;
            model.ProviderName = providerName;

            return View(model);
        }

        [HttpPost]
        private void Download(DownloadHistoryModel model)
        {
            if (ModelState.IsValid)
            {
                int countDownload = model.DownloadAndSave();

                TempData["message"] = String.Format("Успешно загружено: {0} свечей", countDownload);
            }

            ViewBag.TickerList = ModelHelper.GetTickerList(tickerRepository);
            ViewBag.TimeFrameList = ModelHelper.GetTimeFrameList(timeFrameRepository);

        }

        [HttpPost]
        public ActionResult Finam(DownloadHistoryModel model)
        {
            IHistoricalProvider provider = new ClassLib.HistoricalProviders.Finam();
            model.Provider = provider;
            Download(model);
            return View("Download", model);
        }



    }
}