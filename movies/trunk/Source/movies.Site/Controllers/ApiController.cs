using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using movies.Model;
using movies.Core.Extensions;

namespace movies.Site.Controllers
{
    public class ApiController : Controller
    {
        [HttpGet]
        public JsonResult GetIMDbMovie(string imdbMovieId)
        {
            return this.Json(Movie.GetIMDbMovie(imdbMovieId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchMovies(string q)
        {
            return PartialView("MovieList", Movie.SearchMovies(q, 20));
        }

        [HttpGet]
        public ActionResult SearchMoviesJson(string term)
        {
            var results = Movie.SearchMoviesForAutoComplete(term, 5);

            return this.Json(results.Select(x => new { cast = x.cast, url = x.url, title = x.title, imgUrl = x.imgUrl, year = x.year }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRottenTomatoesMovie(string rtMovieId)
        {
            return this.Json(Movie.GetRottenTomatoesMovie(rtMovieId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMovieForAdmin(string rtMovieId)
        {
            var movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);
            var review = Data.DomainModels.MovieReview.Get(int.Parse(rtMovieId), false);
            bool isOnSeeItWhiteList = Data.DomainModels.MovieReview.IsOnSeeItWhiteList(int.Parse(rtMovieId));
            bool isOnSeeItBlackList = Data.DomainModels.MovieReview.IsOnSeeItBlackList(int.Parse(rtMovieId));
            
            return this.Json(
                new
                {
                    id = movie.id,
                    title = movie.title,
                    mpaaRating = movie.mpaa_rating,
                    reviewStatus = review != null ? review.Status : "<i>No review submitted</i>",
                    isOnSeeItBlackList = isOnSeeItBlackList,
                    isOnSeeItWhiteList = isOnSeeItWhiteList
                }
                , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMovieReviewForReviewer(string rtMovieId)
        {
            var movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);
            var review = Data.DomainModels.MovieReview.Get(int.Parse(rtMovieId), false);
            string statusMsg = "";
            if (review != null)
            {
                // existing, so just display what the current status is
                statusMsg = "Approval: " + review.Status;
            }
            else // new review
            {
                bool isOnSeeItWhiteList = Data.DomainModels.MovieReview.IsOnSeeItWhiteList(int.Parse(rtMovieId));
                bool isOnSeeItBlackList = Data.DomainModels.MovieReview.IsOnSeeItBlackList(int.Parse(rtMovieId));
                bool requiresApproval = movie.mpaa_rating.ToLower() == "r" || movie.mpaa_rating.ToLower() == "unrated" || movie.mpaa_rating == "";
                if (isOnSeeItBlackList)
                {
                    statusMsg = "Note: only a review of 'Or Not' is allowed for this movie.";
                }
                else if (isOnSeeItWhiteList)
                {
                    statusMsg = "Note: either 'See It' or 'Or Not' review will be automatically approved for this movie.";
                }
                else if (requiresApproval)
                {
                    statusMsg = "Note: due to the parental guide content of this movie, if marked as 'See It', this review will be submitted for approval.";
                }
                else
                {
                    statusMsg = "Note: either 'See It' or 'Or Not' review will be automatically approved for this movie.";
                }
            }

            return this.Json(
                new
                {
                    id = movie.id,
                    title = movie.title,
                    statusMsg = statusMsg,
                    review = review == null ? null : new { Url = review.Url, Text = review.Text, ClassName = review.ClassName }
                }
                , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAuthUserJs(int facebookUserId)
        {
            if (Data.DomainModels.User.IsReviewer(facebookUserId) || Data.DomainModels.User.IsAdmin(facebookUserId))
            {
                return PartialView("AuthUserJs", new movies.Site.ViewModels.Shared.AuthUserJs { 
                    FacebookUserId = facebookUserId,
                    IsAdmin = Data.DomainModels.User.IsAdmin(facebookUserId),
                    IsReviewer = Data.DomainModels.User.IsReviewer(facebookUserId)
                });
            }
            return Content("console.log('not authorized');");
        }

        public ActionResult GetMovieHtml(string rtMovieId)
        {
            var movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);
            movie.MovieType = Model.Movie.GetMovieType(movie.id);

            return PartialView("MovieDetails", movie);
        }

        public ActionResult GetMovieSimpleHtml(string rtMovieId)
        {
            var movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);
            movie.MovieType = Model.Movie.GetMovieType(movie.id);

            return PartialView("MovieDetailsSimple", movie);
        }

        public ActionResult GetRedboxMovieHtml(string rbSlug)
        {
            var movie = Model.Redbox.GetRottenTomatoesMovie(rbSlug);
            if (movie == null)
            {
                return Content("No corresponding RottenTomatoes movie found :/");
            }
            movie.MovieType = Enumerations.MovieType.AtRedboxes;

            return PartialView("MovieDetails", movie);
        }

        public ActionResult GetMovieMobileHtml(string rtMovieId)
        {
            var movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);

            return PartialView("MovieDetails.Mobile", movie);
        }

        public ActionResult GetRedboxMovieAvailHtml(string productId, string latitude, string longitude)
        {
            // var movies = Model.Redbox.GetStoresWithMovie(productId, latitude, longitude);
            return null;
        }

        public ActionResult GetRedboxesHtml(string latitude, string longitude)
        {
            var redboxes = Model.Redbox.GetStores(latitude, longitude);

            return PartialView("Redboxes", redboxes.Take(6).ToList());
        }

        [HttpGet]
        public JsonResult GetPostalCode(string date, string zip)
        {
            return this.Json(PostalCode.Get(date, zip), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTheaterMovies(string date, string zip, string theaterId)
        {
            var postalCode = PostalCode.Get(date, zip);
            var theater = postalCode.theaters.FirstOrDefault(x => x.id == theaterId);
            return this.Json(theater, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPostalCodeForMovie(string date, string zip, string rtMovieId)
        {
            var postalCode = PostalCode.Get(date, zip);
            var filteredTheaters = new List<PostalCode.Theater>();

            // add to filtered theater list
            foreach (var theater in postalCode.theaters)
            {
                if (theater.movies.Where(x => x.id == rtMovieId).ToList().Any())
                {
                    filteredTheaters.Add(new PostalCode.Theater
                    {
                        address = theater.address,
                        id = theater.id,
                        mapUrl = theater.mapUrl,
                        movies = theater.movies.Where(x => x.id == rtMovieId).ToList(),
                        name = theater.name,
                        theaterUrl = theater.theaterUrl
                    });
                }
            }

            // set return val's theater list to filtered list
            return PartialView("TheatersForMovieList", filteredTheaters);
        }
    }
}
