using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urlme.Site.ViewModels;
using urlme.Model.Enums;
using urlme.Model;
using urlme.Core.Extensions;

namespace urlme.Site.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(string sort)
        {
            SortOptions sortBy = (SortOptions)System.Enum.Parse(typeof(SortOptions), sort.ToLower());
            return View(new HomeViewModel(Model.User.Current, sortBy));
        }

        [HttpPost]
        public ActionResult Index(Link link, string sort)
        {
            if (this.ModelState.IsValid)
            {
                CrudLinkResults result = Model.Link.CreateLink(link.Path, link.DestinationUrl);
                string feedback = EnumHelper.GetStringValue(result);
                TempData.Add("Feedback", feedback);

                if (result == CrudLinkResults.Success)
                    return RedirectToAction("Index", new { controller = "Home", sort = sort });
                else {
                    SortOptions sortBy = (SortOptions)System.Enum.Parse(typeof(SortOptions), sort.ToLower());
                    return View(new HomeViewModel(Model.User.Current, sortBy, link));
                }
            }
            else
            {
                SortOptions sortBy = (SortOptions)System.Enum.Parse(typeof(SortOptions), sort.ToLower());
                return View(new HomeViewModel(Model.User.Current, sortBy, link));
            }
        }
    }
}
