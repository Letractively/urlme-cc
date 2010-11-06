using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urlme.Model.Enums;

namespace urlme.Site.Controllers
{
    public class AddController : Controller
    {
        //
        // GET: /Link/

        public ActionResult Index(string url)
        {
            return View(Model.User.Current);
        }
    }
}
