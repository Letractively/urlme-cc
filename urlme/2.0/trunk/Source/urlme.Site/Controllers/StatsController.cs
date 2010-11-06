using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urlme.Model.Enums;
using urlme.Site.ViewModels;

namespace urlme.Site.Controllers
{
    public class StatsController : Controller
    {
        //
        // GET: /Stats/

        public ActionResult Index(string sort)
        {
            SortOptions sortBy = (SortOptions)System.Enum.Parse(typeof(SortOptions), sort.ToLower());
            return View(new HomeViewModel(Model.User.Current, sortBy));
        }

    }
}
