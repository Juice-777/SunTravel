using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace SunTravel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //    );

            routes.MapRoute(
                name: "Home_Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SunTravel.Controllers" }
                );

            routes.MapRoute(
                name: "Hotel_Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Hotel", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SunTravel.Controllers" }
                );

            routes.MapRoute(
                name: "Order_Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SunTravel.Controllers" }
                );
        }
    }
}
