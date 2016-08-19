using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RMarket.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Привязчики
            //ModelBinders.Binders.Add(typeof(Ticker), new TickerBinder(new EFTickerRepository())); //Связанность!!!
            //ModelBinders.Binders.Add(typeof(TimeFrame), new TimeFrameBinder(new EFTimeFrameRepository())); //Связанность!!!

            //Активируем миграции
            //new RMarket.ClassLib.Bootstrapper().SetMigrations<Migrations.Configuration>();
        }
    }
}
