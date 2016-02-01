using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RMarket.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DownloadHistory",
                url: "DownloadHistory/{providerName}",
                defaults: new { controller = "DownloadHistory", action = "Download" },
                constraints: new { httphttpMethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Instance", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
