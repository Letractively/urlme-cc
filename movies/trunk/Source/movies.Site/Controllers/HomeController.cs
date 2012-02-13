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
            var boxOfficeMovies = Movie.GetBoxOffice();
            return View(boxOfficeMovies);
        }
    }
}
