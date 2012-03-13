using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace movies.Site.Controllers
{
    public class MovieController : Controller
    {
        //
        // GET: /Movie/

        public ActionResult Index(string titleSlug, string rtMovieId)
        {
            if (Request.Browser.IsMobileDevice)
            {
                var vm = new ViewModels.Movie.Index {
                    UseAjaxForLinks = true,
                    PrefetchLinks = true,
                    Movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId)
                };
                return View("Index", vm);
            }
            else
            {
                var vm = new ViewModels.Home.Index
                {
                    BoxOfficeMovies = Model.Movie.GetBoxOffice(),
                    UpcomingMovies = Model.Movie.GetUpcoming(),
                    OverlayRtMovieId = rtMovieId,
                    UseAjaxForLinks = true,
                    PrefetchLinks = true
                };
                return View("~/views/home/index", vm);
            }
        }
    }
}
