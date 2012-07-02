using System.Collections.Generic;
using System.Web.Mvc;
using movies.Data.DomainModels;
using movies.Site.ViewModels;

namespace movies.Site.Controllers
{
    public class ReviewsController : BaseController
    {
        public ActionResult Index()
        {
            var reviews = MovieReview.Get();
            return View(new ViewModelItem<List<MovieReview>> { Item = reviews });
        }
    }
}
