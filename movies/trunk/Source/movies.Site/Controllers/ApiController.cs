using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace movies.Site.Controllers
{
    public class ApiController : Controller
    {
        [HttpGet]
        public ActionResult GetIMDbMovie(string imdbMovieId)
        {
            return null;
            // return Content(API.IMDb.GetMovieJson(imdbMovieId));
        }
    }
}
