using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using movies.Model;
using movies.Core.Extensions;

namespace movies.Site.Controllers
{
    public class MovieReviewController : Controller
    {
        public ActionResult Approve(int movieId)
        {
            bool success = Data.DomainModels.MovieReview.UpdateStatus(movieId, Data.Enumerations.MovieReviewStatus.Approved);
            if (success)
            {
                return Content("Approved! Go to <a href='//seeitornot.co'>seeitornot.co</a>.");
            }
            else
            {
                return Content("Failure :/ Go to <a href='//seeitornot.co'>seeitornot.co</a>.");
            }
        }

        public ActionResult Disapprove(int movieId)
        {
            bool success = Data.DomainModels.MovieReview.UpdateStatus(movieId, Data.Enumerations.MovieReviewStatus.Disapproved);
            if (success)
            {
                return Content("Disapproved! Go to <a href='//seeitornot.co'>seeitornot.co</a>.");
            }
            else
            {
                return Content("Failure :/ Go to <a href='//seeitornot.co'>seeitornot.co</a>.");
            }
        }
        
        [HttpPost]
        public JsonResult Save(int facebookUserId, Data.DomainModels.MovieReview movieReview)
        {
            if (!Data.DomainModels.User.IsReviewer(facebookUserId))
            {
                return this.Json(new { WasSuccessful = false }, JsonRequestBehavior.AllowGet);
            }

            var movie = Model.Movie.GetRottenTomatoesMovie(movieReview.MovieId.ToString());
            // set remaining moviereview properties that we need before we attempt to save this mofo
            movieReview.Title = movie.title;
            movieReview.Year = int.Parse(movie.year);
            movieReview.ThumbnailPosterUrl = movie.posters.thumbnail;
            movieReview.DetailedPosterUrl = movie.posters.detailed;
            movieReview.ProfilePosterUrl = movie.posters.profile;

            string parentalGuideHtml = movie.alternate_ids != null ? Model.Movie.GetParentalGuideHtml(movie.alternate_ids.imdb) : "No parental guide b/c this movie does not have an IMDb id.";
            bool success = Data.DomainModels.MovieReview.Save(movieReview, movie.mpaa_rating, parentalGuideHtml);

            return this.Json(new { WasSuccessful = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int facebookUserId, int movieId)
        {
            if (!Data.DomainModels.User.IsReviewer(facebookUserId))
            {
                return this.Json(new { WasSuccessful = false }, JsonRequestBehavior.AllowGet);
            }

            bool success = Data.DomainModels.MovieReview.Delete(movieId);

            return this.Json(new { WasSuccessful = success }, JsonRequestBehavior.AllowGet);
        }
    }
}
