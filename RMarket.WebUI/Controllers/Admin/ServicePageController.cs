using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Controllers.Admin
{
    public class ServicePageController : Controller
    {
        private ITickRepository tickRepository;

        public ServicePageController(ITickRepository tickRepository)
        {
            this.tickRepository = tickRepository;
        }


        // GET: ServicePage
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult SaveTicks(DateTime dateFrom, DateTime dateTo, string tickerCode)
        {

            IEnumerable<Tick> ticks = tickRepository.Ticks.Where(t => t.TickerCode == tickerCode && t.Date >= dateFrom && t.Date < dateTo);

            string strTicks = Serializer.Serialize(ticks);

            //пишем в файл
            FileInfo f = new FileInfo(ControllerContext.HttpContext.Server.MapPath("~/Mytext.txt"));
            using (StreamWriter w = f.CreateText())
            {
                w.Write(strTicks);
            }

                return View();
        }
    }
}