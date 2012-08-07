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
                "mail-test", // Route name
                "mail/test", // URL with parameters
                new { controller = "Mail", action = "Test" } // Parameter defaults
            );

            routes.MapRoute(
                "about-index", // Route name
                "about", // URL with parameters
                new { controller = "About", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "reviews-index", // Route name
                "reviews", // URL with parameters
                new { controller = "Reviews", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "reviews-approve", // Route name
                "reviews/approve/{movieId}", // URL with parameters
                new { controller = "MovieReview", action = "Approve" } // Parameter defaults
            );

            routes.MapRoute(
                "reviews-disapprove", // Route name
                "reviews/disapprove/{movieId}", // URL with parameters
                new { controller = "MovieReview", action = "Disapprove" } // Parameter defaults
            );

            routes.MapRoute(
                "MovieReview-Save", // Route name
                "moviereview/save", // URL with parameters
                new { controller = "MovieReview", action = "Save" } // Parameter defaults
            );

            routes.MapRoute(
                "Movie-Save", // Route name
                "movie/save", // URL with parameters
                new { controller = "Movie", action = "Save" } // Parameter defaults
            );

            routes.MapRoute(
                "MovieReview-Delete", // Route name
                "moviereview/delete", // URL with parameters
                new { controller = "MovieReview", action = "Delete" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Movie_Review", // Route name
                "api/get_movie_review.json/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetMovieReviewForReviewer" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Movie_For_Admin", // Route name
                "api/get_movie_for_admin.json/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetMovieForAdmin" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Movie", // Route name
                "api/get_rt_movie.json/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetRottenTomatoesMovie" } // Parameter defaults
            );

            routes.MapRoute(
                "Redirect", // Route name
                "redirect", // URL with parameters
                new { controller = "Home", action = "Redirect" } // Parameter defaults
            );

            routes.MapRoute(
                "Reports", // Route name
                "reports", // URL with parameters
                new { controller = "Report", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Home-AuthReviewer", // Route name
                "auth-reviewer", // URL with parameters
                new { controller = "Home", action = "AuthReviewer" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Auth_User", // Route name
                "api/auth_user.js/{facebookUserId}", // URL with parameters
                new { controller = "Api", action = "GetAuthUserJs" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_RedboxMovieAvail", // Route name
                "api/get_rb_movie_avail.html/{productId}/{latitude},{longitude}", // URL with parameters
                new { controller = "Api", action = "GetRedboxMovieAvailHtml" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Movie_Html", // Route name
                "api/get_rt_movie.html/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetMovieHtml" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Movie_Simple_Html", // Route name
                "api/get_rt_movie_simple.html/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetMovieSimpleHtml" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_RedboxMovie_Html", // Route name
                "api/get_rb_movie.html/{rbSlug}", // URL with parameters
                new { controller = "Api", action = "GetRedboxMovieHtml" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_Movie_Mobile_Html", // Route name
                "api/get_rt_movie_mobile.html/{rtMovieId}", // URL with parameters
                new { controller = "Api", action = "GetMovieMobileHtml" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Search_Movies", // Route name
                "api/search_movies.html", // URL with parameters
                new { controller = "Api", action = "SearchMovies" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Search_Movies_json", // Route name
                "api/search_movies.json", // URL with parameters
                new { controller = "Api", action = "SearchMoviesJson" } // Parameter defaults
            );

            routes.MapRoute(
                "Api-Get_TheaterMovies", // Route name
                "api/get_theater_movies.json/{date}/{zip}/{theaterId}", // URL with parameters
                new { controller = "Api", action = "GetTheaterMovies" } // Parameter defaults
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
                "Home-Theater", // Route name
                "theater/{zipCode}/{theaterId}", // URL with parameters
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
                "MovieDetailsRedbox", // Route name
                "redbox/{titleSlug}", // URL with parameters
                new { controller = "Home", action = "Index", isRedbox = true } // Parameter defaults
            );            
            
            routes.MapRoute(
                "MovieDetails", // Route name
                "{titleSlug}/{rtMovieId}", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
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
            // mobileJsBundle.AddFile("~/content/jquery.mobile-1.1.0-rc.1/jquery.mobile-1.1.0-rc.1.min.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.siteActions.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Defaults.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Api.js");
            mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.Geo.js");
            mobileJsBundle.AddFile("~/scripts/plugins/mask/jquery.mask.min.js");
            mobileJsBundle.AddFile("~/scripts/plugins/jquery-ui-1.8.21/js/jquery-ui-1.8.21.custom.min.js");
            mobileJsBundle.AddFile("~/scripts/plugins/date.js");
            mobileJsBundle.AddFile("~/scripts/plugins/jsrender.js");
            mobileJsBundle.AddFile("~/externals/local-cache/local-cache.js");
            // mobileJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Mobile.js");
            mobileJsBundle.AddFile("~/scripts/plugins/jquery.history.js");
            // mobileJsBundle.AddFile("~/scripts/plugins/jquery.overscroll.min.js");
            BundleTable.Bundles.Add(mobileJsBundle);

            var mobileCssBundle = new Bundle("~/mobile-css-bundle", new CssMinify());
            mobileCssBundle.AddFile("~/content/common.css");
            mobileCssBundle.AddFile("~/content/mobile.css");
            mobileCssBundle.AddFile("~/scripts/plugins/jquery-ui-1.8.21/css/ui-lightness/jquery-ui-1.8.21.custom.css");
            BundleTable.Bundles.Add(mobileCssBundle);

            var desktopJsBundle = new Bundle("~/desktop-js-bundle", new JsMinify());
            desktopJsBundle.AddFile("~/scripts/jquery-1.7.1.min.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.siteActions.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Defaults.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.movies.Api.js");
            desktopJsBundle.AddFile("~/scripts/codejkjk/codejkjk.Geo.js");
            desktopJsBundle.AddFile("~/scripts/plugins/date.js");
            desktopJsBundle.AddFile("~/scripts/plugins/jsrender.js");
            desktopJsBundle.AddFile("~/externals/local-cache/local-cache.js");
            desktopJsBundle.AddFile("~/scripts/plugins/zeroclipboard/zeroclipboard.js");
            // desktopJsBundle.AddFile("~/scripts/plugins/qTip2/jquery.qtip.min.js");
            desktopJsBundle.AddFile("~/scripts/plugins/mask/jquery.mask.min.js");
            desktopJsBundle.AddFile("~/scripts/plugins/jquery.history.js");
            desktopJsBundle.AddFile("~/scripts/plugins/blockui/jquery.blockUI.js");
            desktopJsBundle.AddFile("~/scripts/plugins/jquery.hotkeys.js");
            desktopJsBundle.AddFile("~/scripts/plugins/jquery-ui-1.8.21/js/jquery-ui-1.8.21.custom.min.js");
            desktopJsBundle.AddFile("~/scripts/plugins/swfobject.js");
            BundleTable.Bundles.Add(desktopJsBundle);

            var desktopCssBundle = new Bundle("~/desktop-css-bundle", new CssMinify());
            desktopCssBundle.AddFile("~/content/common.css");
            desktopCssBundle.AddFile("~/content/site.css");
            desktopCssBundle.AddFile("~/scripts/plugins/blockui/style.css");
            desktopCssBundle.AddFile("~/scripts/plugins/qTip2/jquery.qtip.min.css");
            desktopCssBundle.AddFile("~/Scripts/Plugins/iPhoneStyleCheckboxes/style.css");
            desktopCssBundle.AddFile("~/scripts/plugins/jquery-ui-1.8.21/css/ui-lightness/jquery-ui-1.8.21.custom.css");

            BundleTable.Bundles.Add(desktopCssBundle);
        }
    }
}