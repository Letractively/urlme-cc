using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ianhd.core.Extensions;

namespace ianhd.admin.Controllers
{
    public class TaglineController : ianhd.core.Mvc.Controllers.BaseController
    {
        private static string siteCd = "IANHD";

        //
        // GET: /Home/

        public ActionResult Index()
        {
            var vm = new ViewModels.TaglineIndex
            {
                NewTagline = new data.Tagline(siteCd),
                Taglines = ianhd.data.Tagline.Get(siteCd)
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ViewModels.TaglineIndex vm)
        {
            var dbTagline = new data.Tagline {
                Archive = vm.NewTagline.Archive,
                LastDate = vm.NewTagline.LastDate,
                SiteCd = vm.NewTagline.SiteCd,
                StartDate = vm.NewTagline.StartDate,
                Text = vm.NewTagline.Text.TrimToNull()
            };

            bool success = data.Tagline.CreateUpdate(dbTagline);
            
            return Redirect("~/tagline");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            bool success = data.Tagline.Delete(id);

            return Redirect("~/tagline");
        }
    }
}
