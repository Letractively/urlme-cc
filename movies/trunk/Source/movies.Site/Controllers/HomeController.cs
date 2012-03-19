using System.Web.Mvc;
using movies.Model;
using movies.Core.Web.Caching;
using System.Collections.Generic;

namespace movies.Site.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var vm = new ViewModels.Home.Index
            {
                BoxOfficeMovies = Movie.GetBoxOffice(),
                UpcomingMovies = Movie.GetUpcoming(),
                OverlayMovie = null,
                UseAjaxForLinks = true,
                PrefetchLinks = false
            };
            return View(vm);
        }

        public ActionResult CacheImdbData()
        {
            var boxOfficeMovies = Model.Movie.GetBoxOffice();
            foreach (var movie in boxOfficeMovies.Values)
            {
                if (movie.IMDbQ != null)
                {
                    Model.Movie.GetIMDbMovie(movie.IMDbQ);
                }
            }

            var inTheatersMovies = Model.Movie.GetInTheaters();
            foreach (var movie in inTheatersMovies.Values)
            {
                if (movie.IMDbQ != null)
                {
                    Model.Movie.GetIMDbMovie(movie.IMDbQ);
                }
            }

            return Content("Done!");
        }

        /* mobile - showtimes */
        public ActionResult Showtimes()
        {
            var vm = new ViewModels.Home.Index
            {
                BoxOfficeMovies = Movie.GetBoxOffice(),
                UpcomingMovies = Movie.GetUpcoming(),
                OverlayMovie = null,
                UseAjaxForLinks = true,
                PrefetchLinks = false
            };
            return View(vm);
        }

        /* mobile - coming soon */
        public ActionResult ComingSoon()
        {
            var vm = new ViewModels.Home.Index
            {
                BoxOfficeMovies = Movie.GetBoxOffice(),
                UpcomingMovies = Movie.GetUpcoming(),
                OverlayMovie = null,
                UseAjaxForLinks = true,
                PrefetchLinks = false
            };
            return View(vm);
        }
    }
}
