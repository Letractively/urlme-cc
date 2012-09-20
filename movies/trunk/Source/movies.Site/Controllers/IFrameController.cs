using System.Web.Mvc;

namespace movies.Site.Controllers
{
    public class IFrameController : BaseController
    {
        public ActionResult IMDbRating(string id)
        {
            if (!id.StartsWith("tt"))
            {
                id = "tt" + id;
            }
            var vm = new movies.Site.ViewModels.ViewModelItem<string> { Item = id };
            return View(vm);
        }
    }
}
