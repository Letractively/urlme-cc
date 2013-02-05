using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace play.Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ContactUs1",
                url: "contactus",
                defaults: new { controller = "Home", action = "ContactUs" }
            );

            routes.MapRoute(
                name: "ContactUs2",
                url: "contactussuccess",
                defaults: new { controller = "Home", action = "ContactUsSuccess" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}