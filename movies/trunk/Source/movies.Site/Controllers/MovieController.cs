using System.Web.Mvc;
using movies.Model;

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
                    PrefetchLinks = false,
                    Movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId)
                };
                return View("Index", vm);
            }
            else
            {
                var vm = new ViewModels.Home.Index
                {
                    OpeningMovies = Movie.GetMovies(Enumerations.MovieLists.Opening),
                    BoxOfficeMovies = Movie.GetMovies(Enumerations.MovieLists.BoxOffice),
                    InTheatersMovies = Movie.GetMovies(Enumerations.MovieLists.InTheaters),
                    UpcomingMovies = Movie.GetMovies(Enumerations.MovieLists.Upcoming),

                    OverlayMovie = Model.Movie.GetRottenTomatoesMovie(rtMovieId),
                    UseAjaxForLinks = true,
                    PrefetchLinks = false
                };
                return View("~/views/home/index.cshtml", vm);
            }
        }
    }
}
