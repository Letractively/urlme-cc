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
                OpeningMovies = Movie.GetMovies(Enumerations.MovieLists.Opening),
                BoxOfficeMovies = Movie.GetMovies(Enumerations.MovieLists.BoxOffice),
                InTheatersMovies = Movie.GetMovies(Enumerations.MovieLists.InTheaters),
                UpcomingMovies = Movie.GetMovies(Enumerations.MovieLists.Upcoming),
                // RedBoxTop20Movies = Movie.GetMovies(Enumerations.MovieLists.RedboxTop20),
                RedboxMovies = Movie.GetMovies(Enumerations.MovieLists.Redbox),
                
                OverlayMovie = null,
                UseAjaxForLinks = true,
                PrefetchLinks = false
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

            return View(vm);
        }

        public ActionResult CacheImdbData()
        {
            var boxOfficeMovies = Model.Movie.GetMovies(Enumerations.MovieLists.BoxOffice);
            foreach (var movie in boxOfficeMovies.Values)
            {
                if (movie.IMDbQ != null)
                {
                    Model.Movie.GetIMDbMovie(movie.IMDbQ);
                }
            }

            var inTheatersMovies = Model.Movie.GetMovies(Enumerations.MovieLists.InTheaters);
            foreach (var movie in inTheatersMovies.Values)
            {
                if (movie.IMDbQ != null)
                {
                    Model.Movie.GetIMDbMovie(movie.IMDbQ);
                }
            }

            var openingMovies = Model.Movie.GetMovies(Enumerations.MovieLists.Opening);
            foreach (var movie in openingMovies.Values)
            {
                if (movie.IMDbQ != null)
                {
                    Model.Movie.GetIMDbMovie(movie.IMDbQ);
                }
            }

            //var redBoxTop20Movies = Model.Movie.GetMovies(Enumerations.MovieLists.RedBoxTop20);
            //foreach (var movie in redBoxTop20Movies.Values)
            //{
            //    if (movie.IMDbQ != null)
            //    {
            //        Model.Movie.GetIMDbMovie(movie.IMDbQ);
            //    }
            //}

            //var redBoxComingSoonMovies = Model.Movie.GetMovies(Enumerations.MovieLists.RedBoxComingSoon);
            //foreach (var movie in redBoxComingSoonMovies.Values)
            //{
            //    if (movie.IMDbQ != null)
            //    {
            //        Model.Movie.GetIMDbMovie(movie.IMDbQ);
            //    }
            //}

            var redBoxMovies = Model.Movie.GetMovies(Enumerations.MovieLists.Redbox);
            foreach (var movie in redBoxMovies.Values)
            {
                if (movie.IMDbQ != null)
                {
                    Model.Movie.GetIMDbMovie(movie.IMDbQ);
                }
            }

            return Content("Done! - " + System.DateTime.Now);
        }

        /* mobile - showtimes */
        public ActionResult Showtimes()
        {
            var vm = new ViewModels.Home.Index
            {
                OpeningMovies = Movie.GetMovies(Enumerations.MovieLists.Opening),
                BoxOfficeMovies = Movie.GetMovies(Enumerations.MovieLists.BoxOffice),
                InTheatersMovies = Movie.GetMovies(Enumerations.MovieLists.InTheaters),
                UpcomingMovies = Movie.GetMovies(Enumerations.MovieLists.Upcoming),
                RedBoxTop20Movies = Movie.GetMovies(Enumerations.MovieLists.RedboxTop20),

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
                OpeningMovies = Movie.GetMovies(Enumerations.MovieLists.Opening),
                BoxOfficeMovies = Movie.GetMovies(Enumerations.MovieLists.BoxOffice),
                InTheatersMovies = Movie.GetMovies(Enumerations.MovieLists.InTheaters),
                UpcomingMovies = Movie.GetMovies(Enumerations.MovieLists.Upcoming),
                RedBoxTop20Movies = Movie.GetMovies(Enumerations.MovieLists.RedboxTop20),

                OverlayMovie = null,
                UseAjaxForLinks = true,
                PrefetchLinks = false
            };
            return View(vm);
        }
    }
}
