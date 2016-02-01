using RMarket.ClassLib.EFRepository;
using RMarket.ClassLib.Entities;
using RMarket.WebUI.Infrastructure.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RMarket.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Привязчики
            ModelBinders.Binders.Add(typeof(Ticker), new TickerBinder(new EFTickerRepository())); //Связанность!!!
            ModelBinders.Binders.Add(typeof(TimeFrame), new TimeFrameBinder(new EFTimeFrameRepository())); //Связанность!!!
        }
    }
}
