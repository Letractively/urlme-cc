using System.Web.Mvc;
using movies.Model;

namespace movies.Site.Controllers
{
    public class MovieController : BaseController
    {
        //
        // GET: /Movie/
        public ActionResult Index(string rtMovieId, string titleSlug, bool isRedbox)
        {
            // first, get the movie
            Model.Movie movie = null;
            
            if (isRedbox)
            {
                movie = Model.Redbox.GetRottenTomatoesMovie(titleSlug);
                if (movie == null)
                {
                    return Content("No corresponding RottenTomatoes movie found :/");
                }
                movie.MovieType = Enumerations.MovieType.AtRedboxes;
            }
            else
            {
                movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);
                movie.MovieType = Model.Movie.GetMovieType(rtMovieId);
            }

            // set open graph props
            this.OpenGraphImage = movie.posters.detailed;
            this.OpenGraphTitle = movie.title;

            // mobile?
            if (Request.Browser.IsMobileDevice)
            {
                var vm = new ViewModels.Movie.Index {
                    UseAjaxForLinks = false,
                    PrefetchLinks = false,
                    Movie = movie
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
                    //RedboxMovies = Redbox.GetMovies(),

                    OverlayMovie = movie
                };

                // remove any movies in InTheatersMovies that are already in Box Office
                foreach (var m in vm.BoxOfficeMovies)
                {
                    if (vm.InTheatersMovies.ContainsKey(m.Key))
                    {
                        vm.InTheatersMovies.Remove(m.Key);
                    }
                }

                // remove any movies in InTheatersMovies that are in Opening
                foreach (var m in vm.BoxOfficeMovies)
                {
                    if (vm.OpeningMovies.ContainsKey(m.Key))
                    {
                        vm.InTheatersMovies.Remove(m.Key);
                    }
                }

                return View("~/views/home/index.cshtml", vm);
            }
        }
    }
}
