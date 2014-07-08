using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace seeitornot.site.Controllers
{
    public class MovieController : Controller
    {
        //
        // GET: /movie/{rtMovieId}

        public ActionResult Index(string rtMovieId)
        {
            var movie = model.Movie.Get(rtMovieId);
            return PartialView("Movie", movie);
        }

    }
}
