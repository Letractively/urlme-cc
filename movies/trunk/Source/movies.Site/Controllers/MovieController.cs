using System.Web.Mvc;
using movies.Model;

namespace movies.Site.Controllers
{
    public class MovieController : BaseController
    {
        [HttpPost]
        public JsonResult Save(int facebookUserId, string movieId, bool onSeeItBlackList, bool onSeeItWhiteList)
        {
            if (!Data.DomainModels.User.IsReviewer(facebookUserId))
            {
                return this.Json(new { WasSuccessful = false }, JsonRequestBehavior.AllowGet);
            }

            var movie = Model.Movie.GetRottenTomatoesMovie(movieId);
            bool success = Data.DomainModels.MovieReview.UpdateSeeItBlackList(int.Parse(movieId), movie.title, onSeeItBlackList) && Data.DomainModels.MovieReview.UpdateSeeItWhiteList(int.Parse(movieId), movie.title, onSeeItWhiteList);

            return this.Json(new { WasSuccessful = success }, JsonRequestBehavior.AllowGet);
        }        
    }
}
