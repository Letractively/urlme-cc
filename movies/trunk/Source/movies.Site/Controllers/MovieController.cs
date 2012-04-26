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
            { // desktop
                var vm = new ViewModels.Home.Index
                {
                    OpeningMovies = Movie.GetMovies(Enumerations.MovieLists.Opening),
                    BoxOfficeMovies = Movie.GetMovies(Enumerations.MovieLists.BoxOffice),
                    InTheatersMovies = Movie.GetMovies(Enumerations.MovieLists.InTheaters),
                    UpcomingMovies = Movie.GetMovies(Enumerations.MovieLists.Upcoming),
                    RedboxMovies = Redbox.GetMovies(),

                    OverlayMovie = Model.Movie.GetRottenTomatoesMovie(rtMovieId)
                };

                // remove any movies in InTheatersMovies that are already in Box Office
                foreach (var movie in vm.BoxOfficeMovies)
                {
                    if (vm.InTheatersMovies.ContainsKey(movie.Key))
                    {
                        vm.InTheatersMovies.Remove(movie.Key);
                    }
                }

                // remove any movies in InTheatersMovies that are in Opening
                foreach (var movie in vm.BoxOfficeMovies)
                {
                    if (vm.OpeningMovies.ContainsKey(movie.Key))
                    {
                        vm.InTheatersMovies.Remove(movie.Key);
                    }
                }

                return View("~/views/home/index.cshtml", vm);
            }
        }
    }
}
