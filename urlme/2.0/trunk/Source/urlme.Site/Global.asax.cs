using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using urlme.Site.Helpers;
using urlme.Model.Enums;

namespace urlme.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                "Home-sortbypath",
                "",
                new { controller = "Home", action = "Index", sort = SortOptions.path.ToString() },
                new { Sort = new SortByPathConstraint() }
            );

            routes.MapRouteLowercase(
                "Home-sortbyhits",
                "",
                new { controller = "Home", action = "Index", sort = SortOptions.hits.ToString() },
                new { Sort = new SortByHitsConstraint() }
            );

            routes.MapRouteLowercase(
                "Home-sortbylatest",
                "",
                new { controller = "Home", action = "Index", sort = SortOptions.latest.ToString() }
            );

            routes.MapRouteLowercase(
                "Stats-sortbypath",
                "Stats/",
                new { controller = "Stats", action = "Index", sort = SortOptions.path.ToString() },
                new { Sort = new SortByPathConstraint() }
            );

            routes.MapRouteLowercase(
                "Stats-sortbylatest",
                "Stats/",
                new { controller = "Stats", action = "Index", sort = SortOptions.latest.ToString() },
                new { Sort = new SortByLatestConstraint() }
            );

            routes.MapRouteLowercase(
                "Mail-test",
                "Mail/",
                new { controller = "Mail", action = "Test" }
            );

            routes.MapRouteLowercase(
                "Stats-sortbyhits",
                "Stats/",
                new { controller = "Stats", action = "Index", sort = SortOptions.hits.ToString() }
            );
            
            routes.MapRouteLowercase(
                "OpenIdLogin",
                "Account/OpenIdLogin/",
                new { controller = "Account", action = "OpenIdLogOn" }
            );

            routes.MapRouteLowercase(
                "SignOut",
                "Account/SignOut/",
                new { controller = "Account", action = "SignOut" }
            );

            routes.MapRouteLowercase(
                "Help",
                "Help/",
                new { controller = "Help", action = "Index" }
            );

            routes.MapRouteLowercase(
                "DeleteLink",
                "Delete/Link/{id}/",
                new { controller = "Link", action = "Delete" }
            );

            routes.MapRouteLowercase(
                "AddLink",
                "Add/",
                new { controller = "Add", action = "Index" }
            );

            routes.MapRouteLowercase(
                "ServiceAdd",
                "Service/Add/",
                new { controller = "Service", action = "Add" }
            );

            routes.MapRouteLowercase(
                "PathRedirect",
                "{*path}",
                new { controller = "Link", action = "RedirectToDestinationUrl" }
            );

            //routes.MapRouteLowercase(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //);

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }

        public class SortByPathConstraint : IRouteConstraint
        {
            public bool Match
                (
                    HttpContextBase httpContext,
                    Route route,
                    string parameterName,
                    RouteValueDictionary values,
                    RouteDirection routeDirection
                )
            {
                return httpContext.Request.Url.OriginalString.Contains("?path");
            }
        }

        public class SortByHitsConstraint : IRouteConstraint
        {
            public bool Match
                (
                    HttpContextBase httpContext,
                    Route route,
                    string parameterName,
                    RouteValueDictionary values,
                    RouteDirection routeDirection
                )
            {
                return httpContext.Request.Url.OriginalString.Contains("?hits");
            }
        }

        public class SortByLatestConstraint : IRouteConstraint
        {
            public bool Match
                (
                    HttpContextBase httpContext,
                    Route route,
                    string parameterName,
                    RouteValueDictionary values,
                    RouteDirection routeDirection
                )
            {
                return httpContext.Request.Url.OriginalString.Contains("?latest");
            }
        }
    }
}