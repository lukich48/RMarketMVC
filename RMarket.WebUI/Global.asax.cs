using AutoMapper;
using LightInject;
using RMarket.WebUI.Infrastructure.MapperProfiles;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RMarket.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Инициализация статических зависимостей
            var inicializer = new CompositionRoot.Inicializer();
            inicializer.InitializeDbContext();
            inicializer.SetMapperConfiguration();

            inicializer.InitIoC((container) =>
            {
                container.EnableMvc();
                container.RegisterControllers();
            });

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Привязчики
            //ModelBinders.Binders.Add(typeof(Ticker), new TickerBinder(new EFTickerRepository())); 
            //ModelBinders.Binders.Add(typeof(TimeFrame), new TimeFrameBinder(new EFTimeFrameRepository())); 

            //Активируем миграции
            //new RMarket.ClassLib.Bootstrapper().SetMigrations<Migrations.Configuration>();
        }
    }
}
