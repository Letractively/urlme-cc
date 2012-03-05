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
        //[HttpGet]
        //public JsonResult GetIMDbMovie(string imdbMovieId)
        //{
        //    return this.Json(Movie.GetIMDbMovie(imdbMovieId), JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetIMDbMovie2(string q)
        {
            return this.Json(Movie.GetIMDbMovie2(q), JsonRequestBehavior.AllowGet);
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
        public ActionResult GetShowtimes(string date, string zip)
        {
            return Content(PostalCode.GetShowtimes(date, zip));
        }
    }
}
