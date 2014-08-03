using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace seeitornot.site.Controllers
{
    [RoutePrefix("api")]
    public class ApiController : Controller
    {
        [Route("movie/{movieId}")]
        public ActionResult Movie(string movieId)
        {
            var movie = model.Movie.Get(movieId);
            return PartialView(movie);
        }
        
        [Route("theaters")]
        public JsonResult Theaters(string zip)
        {
            var theaters = seeitornot.model.Theater.Get(zip);
            return this.Json(theaters, JsonRequestBehavior.AllowGet);
        }

        [Route("theaters-with-movies")]
        public JsonResult TheatersWithMovies(string zip, string theaterId, DateTime date)
        {
            var theaters = seeitornot.model.Theater.GetWithMovies(zip, theaterId, date);
            return this.Json(theaters, JsonRequestBehavior.AllowGet);
        }
    }
}
