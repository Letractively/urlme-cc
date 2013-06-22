﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ianhd.admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SiteFeature-Index",
                url: "sitefeatures/{siteFeatureCategoryId}",
                defaults: new { controller = "SiteFeature", action = "Index" }
            );

            routes.MapRoute(
                name: "SiteFeature-Delete",
                url: "sitefeatures/delete/{siteFeatureId}",
                defaults: new { controller = "SiteFeature", action = "Delete" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}