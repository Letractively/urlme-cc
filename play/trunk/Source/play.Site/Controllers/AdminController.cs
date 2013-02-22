using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace play.Site.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            var vm = new play.Site.ViewModels.AdminIndex
            {
                Order = new Models.PlayOrder(),
                Orders = Models.PlayOrder.Get()
            };
            return View(vm);
        }

    }
}
