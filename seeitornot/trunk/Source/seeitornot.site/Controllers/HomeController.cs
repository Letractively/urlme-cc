using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace seeitornot.site.Controllers
{
    [RoutePrefix("")]
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        [Route("")]
        public ActionResult Index(string rtMovieId)
        {
            // HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            var vm = new ViewModels.Home.Index(rtMovieId);
            return View(vm);
        }

    }
}
