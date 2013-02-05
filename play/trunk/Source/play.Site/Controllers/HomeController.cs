using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using play.Site.ViewModels;

namespace play.Site.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContactUsSuccess()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(play.Site.ViewModels.ContactUs contactUs)
        {
            Mail.Send(contactUs.Name, contactUs.Email, contactUs.Message);
            return Redirect("~/contactussuccess");
        }

    }
}
