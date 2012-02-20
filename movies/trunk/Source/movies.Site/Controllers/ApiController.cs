﻿using System;
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
        public JsonResult SearchMovies(string q)
        {
            return this.Json(Movie.SearchMovies(q), JsonRequestBehavior.AllowGet);
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
