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
        public JsonResult Index(string zip)
        {
            var theaters = seeitornot.model.Theater.Get(zip);
            return this.Json(theaters, JsonRequestBehavior.AllowGet);
        }
    }
}
