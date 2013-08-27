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
            vm.movieLists = new List<model.MovieList>();
            vm.movieLists.Add(
                new model.MovieList { title = "This Weekend", movies = new Dictionary<string,model.Movie>() }
            );
            vm.movieLists.Add(
                new model.MovieList { title = "In Theaters", movies = new Dictionary<string,model.Movie>() }
            );
            vm.movieLists.Add(
                new model.MovieList { title = "Coming Soon", movies = new Dictionary<string,model.Movie>() }
            );

            return View(vm);
        }

    }
}
