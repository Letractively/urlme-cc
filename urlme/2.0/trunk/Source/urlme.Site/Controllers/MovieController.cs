using System.Web.Mvc;

namespace urlme.Site.Controllers
{
    public class MovieController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Redirect(string titleSlug, string rtMovieId)
        {
            return Redirect(string.Format("http://bakersdozen13.lfchosting.com/movies/www01/movie/{0}/{1}", titleSlug, rtMovieId));
        }
    }
}
