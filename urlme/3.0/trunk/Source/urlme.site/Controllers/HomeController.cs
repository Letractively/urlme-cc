using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace urlme.site.Controllers
{
    [RoutePrefix("")]
    public class HomeController : BaseController
    {
        [Route("add")]
        public ActionResult Add(string url)
        {
            return View();
        }

        [Route("add-sign-in")]
        public ActionResult AddSignIn()
        {
            return View();
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}