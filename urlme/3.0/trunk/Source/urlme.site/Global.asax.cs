using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace urlme.site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string ApplicationBuild = string.Format("{0}{1}",
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString(),
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}
