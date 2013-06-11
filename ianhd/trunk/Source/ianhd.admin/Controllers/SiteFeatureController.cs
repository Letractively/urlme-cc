using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ianhd.core.Extensions;
using ianhd.data;

namespace ianhd.admin.Controllers
{
    public class SiteFeatureController : ianhd.core.Mvc.Controllers.BaseController
    {
        //
        // GET: /Home/

        [HttpGet]
        public ActionResult Index(int id)
        {
            var vm = new ViewModels.SiteFeatureIndex
            {
                NewSiteFeature = new SiteFeature(id),
                SiteFeatures = SiteFeature.Get(id),
                SiteFeatureCategory = SiteFeature.CategoryGet(id)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(ViewModels.SiteFeatureIndex vm, int id)
        {
            var dbSiteFeature = new data.SiteFeature {
                Archive = vm.NewSiteFeature.Archive,
                LastDate = vm.NewSiteFeature.LastDate,
                SiteFeatureCategoryId = id,
                StartDate = vm.NewSiteFeature.StartDate,
                Value = vm.NewSiteFeature.Value.TrimToNull()
            };

            bool success = data.SiteFeature.CreateUpdate(dbSiteFeature);
            
            return Redirect("~/siteFeature/index/" + id);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            bool success = data.SiteFeature.Delete(id);

            // HACK - need to make this NOT hard-coded
            return Redirect("~/siteFeature/index/1");
        }
    }
}
