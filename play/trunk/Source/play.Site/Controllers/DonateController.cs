using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace play.Site.Controllers
{
    public class DonateController : Controller
    {
        //
        // GET: /Donate/

        public ActionResult ThankYou()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Kickstarter()
        {
            return View();
        }
    }
}
