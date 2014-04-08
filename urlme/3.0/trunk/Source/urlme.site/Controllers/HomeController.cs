using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace urlme.site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var links = new List<urlme.data.Models.Link>();
            if (Request.IsAuthenticated)
            {
                links = data.Models.Link.Get(User.Identity.Name.UserId());
            }
            return View(links);
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}