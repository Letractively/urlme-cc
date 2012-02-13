using System.Web.Mvc;
using movies.Model;

namespace movies.Site.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var vm = new ViewModels.Home.Index
            {
                BoxOfficeMovies = Movie.GetBoxOffice(),
                UpcomingMovies = Movie.GetUpcoming()
            };
            return View(vm);
        }
    }
}
