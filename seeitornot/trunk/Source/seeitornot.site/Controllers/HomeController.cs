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
        public ActionResult Index()
        {
            var vm = new ViewModels.Home.Index("");
            vm.view = "home";

            return View(vm);
        }

        [Route("showtimes/{zip}/{theaterId}")]
        public ActionResult Showtimes(string zip, string theaterId)
        {
            var vm = new ViewModels.Home.Index("");
            vm.view = "showtimes";
            vm.zip = zip;
            vm.theaterId = theaterId;
            vm.theaterName = seeitornot.model.Theater.Get(zip, theaterId).name;

            return View("Index", vm);
        }

        [Route("{movieSlug}/{movieId}")]
        public ActionResult Movie(string movieSlug, string movieId)
        {
            var vm = new ViewModels.Home.Index("");
            vm.view = "movie";
            vm.movieId = movieId;

            return View("Index", vm);
        }
    }
}
