using System.Web.Mvc;

namespace movies.Site.Controllers
{
    public class ReviewsController : BaseController
    {
        public ActionResult Index()
        {
            return View(new movies.Site.ViewModels.ViewModelBase());
        }
    }
}
