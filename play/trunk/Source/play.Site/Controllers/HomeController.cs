using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using play.Site.ViewModels;

namespace play.Site.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var vm = new HomeIndex();
            vm.AboutPlay = "There's nothing like the smell of coffee & homicide in the morning.  Coco, the over-caffeinated owner of Coco's Coffee Shop & Cafe, stumbles upon an unlucky citizen of Pleasant Valley who has met his demise in her very own coffee shop. A casual glance at the scene reveals this was no accident. On top of how devastating this could be for business, she is considered a suspect, along with the rest of the town. The Inspector from the bordering county is determined to find the killer, but he could undoubtedly use some help. Join us for a delicious 3-course dinner along with a night of fun, entertainment, and scandal when YOU become the jury to help figure out 'whodunit'!";

            return View(vm);
        }

    }
}
