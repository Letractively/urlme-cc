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
            return PartialView("MovieList", Movie.SearchMovies(q));
        }

        [HttpGet]
        public JsonResult GetRottenTomatoesMovie(string rtMovieId)
        {
            return this.Json(Movie.GetRottenTomatoesMovie(rtMovieId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPostalCode(string date, string zip)
        {
            return this.Json(PostalCode.Get(date, zip), JsonRequestBehavior.AllowGet);
        }
    }
}
