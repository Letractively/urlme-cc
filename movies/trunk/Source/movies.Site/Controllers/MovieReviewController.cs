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

            bool success = Data.DomainModels.MovieReview.Save(movieReview, movie.mpaa_rating);

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
