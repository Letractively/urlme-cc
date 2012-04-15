using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace movies.Site.Controllers
{
    public class SeoController : Controller
    {
        //
        // GET: /Seo/

        public ActionResult Movie(string rtMovieId)
        {
            var m = Model.Movie.GetRottenTomatoesMovie(rtMovieId);
            return View(m);
        }

    }
}
