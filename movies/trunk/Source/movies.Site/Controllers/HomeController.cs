using System.Web.Mvc;
using movies.Model;
using movies.Core.Web.Caching;
using System.Collections.Generic;
using System.Linq;

namespace movies.Site.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var inTheatersMovies = Movie.GetMovies(Enumerations.MovieLists.InTheaters);
            
            // copy of intheaters movies, b/c we'll be removing some keys (don't want to remove from the referenced object in cache)
            Dictionary<string, Movie> inTheatersMoviesDisplay = new Dictionary<string, Movie>();
            inTheatersMovies.ToList().ForEach(x => inTheatersMoviesDisplay.Add(x.Key, x.Value));

            var vm = new ViewModels.Home.Index
            {
                OpeningMovies = Movie.GetMovies(Enumerations.MovieLists.Opening),
                BoxOfficeMovies = Movie.GetMovies(Enumerations.MovieLists.BoxOffice),
                InTheatersMovies = inTheatersMoviesDisplay,
                
                OverlayMovie = null,
                UseAjaxForLinks = true,
                PrefetchLinks = false
            };

            // load upcoming and redbox only if for desktop view
            if (!Request.Browser.IsMobileDevice)
            {
                vm.UpcomingMovies = Movie.GetMovies(Enumerations.MovieLists.Upcoming);
                if (!Request.Url.ToString().Contains("localhost"))
                {
                    vm.RedboxMovies = Redbox.GetMovies();
                }
            }

            // remove any movies in InTheatersMovies that are already in Box Office
            foreach (var movie in vm.BoxOfficeMovies)
            {
                if (vm.InTheatersMovies.ContainsKey(movie.Key))
                {
                    vm.InTheatersMovies.Remove(movie.Key);
                }
            }

            // remove any movies in InTheatersMovies that are in Opening
            foreach (var movie in vm.OpeningMovies)
            {
                if (vm.InTheatersMovies.ContainsKey(movie.Key))
                {
                    vm.InTheatersMovies.Remove(movie.Key);
                }
            }

            if (Request.Browser.IsMobileDevice)
            {
                return View("Index.Mobile", vm);
            }
            else
            {
                return View("Index", vm);
            }
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

            var redboxMovies = Model.Redbox.GetMovies();

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

            //var redBoxMovies = Model.Movie.GetMovies(Enumerations.MovieLists.Redbox);
            //foreach (var movie in redBoxMovies.Values)
            //{
            //    if (movie.IMDbQ != null)
            //    {
            //        Model.Movie.GetIMDbMovie(movie.IMDbQ);
            //    }
            //}

            return Content("Done! - " + System.DateTime.Now);
        }

        /* mobile - showtimes */
        public ActionResult Showtimes()
        {
            if (Request.Browser.IsMobileDevice)
            {
                // mobile, only set what we need to
                var vm = new ViewModels.Home.Index
                {
                    OverlayMovie = null,
                    UseAjaxForLinks = true,
                    PrefetchLinks = false
                };

                return View("Showtimes.Mobile", vm);
            }
            else
            {
                // desktop, return whatever Index does, b/c there's js to look at the path to determine which section to show
                return Index();
            }
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
                RedboxMovies = Redbox.GetMovies(),

                OverlayMovie = null,
                UseAjaxForLinks = true,
                PrefetchLinks = false
            };
            return View(vm);
        }
    }
}
