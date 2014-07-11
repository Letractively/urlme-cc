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
        public ActionResult Index()
        {
            return Content("theaters");
        }
    }
}
