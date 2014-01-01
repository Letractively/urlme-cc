using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace seeitornot.site.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            // HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            var vm = new ViewModels.Home.Index();
            return View(vm);
        }

    }
}
