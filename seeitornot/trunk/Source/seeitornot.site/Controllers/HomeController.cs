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

        [Route("")]
        public ActionResult Index(string rtMovieId)
        {
            // HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            var vm = new ViewModels.Home.Index(rtMovieId);
            return View(vm);
        }

        [Route("showtimes/{zip}/{theaterId}")]
        public ActionResult Showtimes(string zip, string theaterId)
        {
            var vm = new ViewModels.Home.Index("");
            return View("Index", vm);
        }

        [Route("{movieSlug}/{movieId}")]
        public ActionResult Movie(string movieSlug, string movieId)
        {
            var vm = new ViewModels.Home.Index("");
            return View("Index", vm);
        }
    }
}
