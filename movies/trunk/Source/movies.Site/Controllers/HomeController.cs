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
        //[OutputCache(CacheProfile="OneMinute")]
        public ActionResult Index(string rtMovieId = null, string titleSlug = null, bool isRedbox = false, bool isTrailer = false)
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
                UpcomingMovies = Movie.GetMovies(Enumerations.MovieLists.Upcoming),
                ThisWeekendMovies = new Dictionary<string,Movie>(),

                OverlayMovie = null
            };

            // DESKTOP VIEW? 
            if (!Request.Browser.IsMobileDevice)
            {
                vm.FeatureTrailers = TrailerAddict.GetFeatured(20).Take(8).ToList();
                // vm.RedboxMovies = Redbox.GetMovies();
                vm.RecentReviews = Data.DomainModels.MovieReview.GetLatest(7);
                //if (!Request.Url.ToString().Contains("localhost"))
                //{
                // vm.RedboxMovies = Redbox.GetMovies();
                //}
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

            // WHEN WE DO REMOVE IT FROM OPENING, BE CAREFUL WITH CACHE !!!1
            // add to This Weekend list, pulling (removing) from Opening
            // var openingKeysToRemove = new List<string>();
            foreach (var movie in vm.OpeningMovies)
            {
                if (movie.Value.IsThisWeekend)
                {
                    vm.ThisWeekendMovies.Add(movie.Key, movie.Value);
                    // openingKeysToRemove.Add(movie.Key);
                }
            }
            // openingKeysToRemove.ForEach(x => vm.OpeningMovies.Remove(x));


            // movie view?
            if (!string.IsNullOrWhiteSpace(rtMovieId))
            {
                Model.Movie movie = null;
                if (isRedbox)
                {
                    movie = Model.Redbox.GetRottenTomatoesMovie(titleSlug);
                    if (movie == null)
                    {
                        return Content("No corresponding RottenTomatoes movie found :/");
                    }
                    movie.MovieType = Enumerations.MovieType.AtRedboxes;
                    vm.OverlayMovie = movie;
                }
                else if (isTrailer)
                {
                    vm.OverlayTrailer = vm.FeatureTrailers.FirstOrDefault(x => x.RtMovieId == rtMovieId);
                    movie = vm.OverlayTrailer.RtMovie;
                }
                else
                {
                    movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);
                    movie.MovieType = Model.Movie.GetMovieType(rtMovieId);
                    vm.OverlayMovie = movie;
                }

                // set open graph props
                this.OpenGraphImage = movie.posters.detailed;
                this.OpenGraphTitle = movie.title;
                this.OpenGraphDescription = movie.synopsis;
            }
            else
            {
                // non-movie view. still set open graph stuff
                this.OpenGraphImage = "http://seeitornot.co/content/logo_fbopengraph.png";
                this.OpenGraphTitle = "See it or Not";
                this.OpenGraphDescription = "Movie reviews from JohnHanlonReviews.com and RottenTomatoes.com, showtimes, and quick parental guide access for box office, coming soon, and movies opening this week!";
            }

            return View(vm);
        }

        public ActionResult Redirect()
        {
            return Redirect("http://seeitornot.co");
        }

        public ActionResult AuthReviewer()
        {
            return Redirect("https://graph.facebook.com/oauth/authorize?client_id=118119081645720&redirect_uri=http://seeitornot.co/redirect");
        }

        public ActionResult CacheImdbData()
        {
            List<Dictionary<string, Model.Movie>> movieLists = new List<Dictionary<string, Model.Movie>>();
            var boxOfficeMovies = Model.Movie.GetMovies(Enumerations.MovieLists.BoxOffice);
            movieLists.Add(boxOfficeMovies);
            var inTheatersMovies = Model.Movie.GetMovies(Enumerations.MovieLists.InTheaters);
            movieLists.Add(inTheatersMovies);
            var openingMovies = Model.Movie.GetMovies(Enumerations.MovieLists.Opening);
            movieLists.Add(openingMovies);

            foreach (var movieList in movieLists)
            {
                foreach (var movie in movieList.Values)
                {
                    if (string.IsNullOrWhiteSpace(movie.IVAPublishedId))
                    {
                        movie.IVAPublishedId = Model.IVA.GetPublishedId(movie.id);
                    }
                }
            }

            // var redboxMovies = Model.Redbox.GetMovies();

            var featuredTrailers = TrailerAddict.GetFeatured(20);

            var latestReviews = Data.DomainModels.MovieReview.GetLatest(7);

            // movies.Model.Twitter.UpdateStatus("work please " + System.DateTime.Now);
            return Content("Done! - " + System.DateTime.Now + " - <a href='http://seeitornot.co'>seeitornot.co</a>");
        }
    }
}
