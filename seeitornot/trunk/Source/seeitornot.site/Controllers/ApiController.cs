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
        [Route("theaters")]
        public JsonResult Theaters(string zip)
        {
            var theaters = seeitornot.model.Theater.Get(zip);
            return this.Json(theaters, JsonRequestBehavior.AllowGet);
        }

        [Route("theaters-with-movies")]
        public JsonResult TheatersWithMovies(string zip, string theaterId)
        {
            var theaters = seeitornot.model.Theater.Get(zip, theaterId, System.DateTime.Now);
            return this.Json(theaters, JsonRequestBehavior.AllowGet);
        }
    }
}
