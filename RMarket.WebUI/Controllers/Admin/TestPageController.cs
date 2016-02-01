using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Controllers.Admin
{
    public class TestPageController : Controller
    {
        // GET: TestPage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateFirstGeneration()
        {
            return View();
        }
    }
} 