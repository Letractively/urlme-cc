using System.Web.Mvc;
using movies.Model;

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
                OverlayRtMovieId = null,
                UseAjaxForLinks = false,
                PrefetchLinks = true
            };
            return View(vm);
        }

        /* mobile - showtimes */
        public ActionResult Showtimes()
        {
            var vm = new ViewModels.Home.Index
            {
                BoxOfficeMovies = Movie.GetBoxOffice(),
                UpcomingMovies = Movie.GetUpcoming(),
                OverlayRtMovieId = null,
                UseAjaxForLinks = false,
                PrefetchLinks = true
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
                OverlayRtMovieId = null,
                UseAjaxForLinks = false,
                PrefetchLinks = true
            };
            return View(vm);
        }

        public ActionResult IndexWithMovieOverlay(string titleSlug, string rtMovieId)
        {
            var vm = new ViewModels.Home.Index
            {
                BoxOfficeMovies = Movie.GetBoxOffice(),
                UpcomingMovies = Movie.GetUpcoming(),
                OverlayRtMovieId = rtMovieId,
                UseAjaxForLinks = false,
                PrefetchLinks = false
            };
            return View("Index", vm);
        }
    }
}
