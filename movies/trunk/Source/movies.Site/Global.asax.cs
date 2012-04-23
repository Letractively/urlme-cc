using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace movies
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Api-Get_IMDb_Movie", // Route name
                "api/get_imdb_movie.json/{imdbMovieId}", // URL with parameters
                new { controller = "Api", action = "GetIMDbMovie" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Movie", // Route name
                "api/get_rt_movie.json/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetRottenTomatoesMovie" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Movie_Html", // Route name
                "api/get_rt_movie.html/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetMovieHtml" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Search_Movies", // Route name
                "api/search_movies.html", // URL with parameters
                new { controller = "Api", action = "SearchMovies" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Showtimes", // Route name
                "api/get_showtimes.json/{date}/{zip}", // URL with parameters
                new { controller = "Api", action = "GetPostalCode" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Showtimes_For_Movie", // Route name
                "api/get_showtimes_for_movie.html/{date}/{zip}/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetPostalCodeForMovie" } // Parameter defaults
            );

            // get_rbs.html/{1},{2}
            routes.MapRoute(
                "Api-Get_Redboxes", // Route name
                "api/get_rbs.html/{latitude},{longitude}", // URL with parameters
                new { controller = "Api", action = "GetRedboxesHtml" } // Parameter defaults
            );

            routes.MapRoute(
                "Home-Showtimes", // Route name
                "showtimes", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Home-RedBox", // Route name
                "redbox", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Home-ComingSoon", // Route name
                "comingsoon", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
            );            
            
            // TODO: re-enable mobile routes
            //routes.MapRoute(
            //    "Mobile-Showtimes", // Route name
            //    "showtimes", // URL with parameters
            //    new { controller = "Home", action = "Showtimes" } // Parameter defaults
            //);

            //routes.MapRoute(
            //    "Mobile-ComingSoon", // Route name
            //    "comingsoon", // URL with parameters
            //    new { controller = "Home", action = "ComingSoon" } // Parameter defaults
            //);

            routes.MapRoute(
                "Task-CacheImdbData", // Route name
                "cacheimdbdata", // URL with parameters
                new { controller = "Home", action = "CacheImdbData" } // Parameter defaults
            );

            routes.MapRoute(
                "Seo-Movie", // Route name
                "seo/{titleSlug}/{rtMovieId}", // URL with parameters
                new { controller = "Seo", action = "Movie" } // Parameter defaults
            );   

            routes.MapRoute(
                "MovieDetails", // Route name
                "{titleSlug}/{rtMovieId}", // URL with parameters
                new { controller = "Movie", action = "Index" } // Parameter defaults
            );   

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.EnableDefaultBundles();
            // BundleTable.Bundles.RegisterTemplateBundles();

            var mobileJsBundle = new Bundle("~/mobile-js-bundle", new JsMinify());
            mobileJsBundle.AddFile("~/scripts/jquery-1.7.1.min.js");
            mobileJsBundle.AddFile("~/content/jquery.mobile-1.1.0-rc.1/jquery.mobile-1.1.0-rc.1.min.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Defaults.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Api.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.Geo.js");
            mobileJsBundle.AddFile("~/scripts/plugins/date.js");
            mobileJsBundle.AddFile("~/scripts/plugins/jsrender.js");
            mobileJsBundle.AddFile("~/externals/local-cache/local-cache.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Mobile.js");
            BundleTable.Bundles.Add(mobileJsBundle);

            var mobileCssBundle = new Bundle("~/mobile-css-bundle", new CssMinify());
            mobileCssBundle.AddFile("~/content/jquery.mobile-1.1.0-rc.1/jquery.mobile-1.1.0-rc.1.min.css");
            mobileCssBundle.AddFile("~/content/common.css");
            mobileCssBundle.AddFile("~/content/mobile.css");
            BundleTable.Bundles.Add(mobileCssBundle);

            var desktopJsBundle = new Bundle("~/desktop-js-bundle", new JsMinify());
            desktopJsBundle.AddFile("~/scripts/jquery-1.7.1.min.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Defaults.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Api.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.Geo.js");
            desktopJsBundle.AddFile("~/scripts/plugins/date.js");
            desktopJsBundle.AddFile("~/scripts/plugins/jsrender.js");
            desktopJsBundle.AddFile("~/externals/local-cache/local-cache.js");
            desktopJsBundle.AddFile("~/scripts/plugins/zeroclipboard/zeroclipboard.js");
            desktopJsBundle.AddFile("~/scripts/plugins/jquery.mask.min.js");
            desktopJsBundle.AddFile("~/scripts/plugins/jquery.history.js");
            BundleTable.Bundles.Add(desktopJsBundle);

            var desktopCssBundle = new Bundle("~/desktop-css-bundle", new CssMinify());
            desktopCssBundle.AddFile("~/content/common.css");
            desktopCssBundle.AddFile("~/content/site.css");
            BundleTable.Bundles.Add(desktopCssBundle);
        }
    }
}