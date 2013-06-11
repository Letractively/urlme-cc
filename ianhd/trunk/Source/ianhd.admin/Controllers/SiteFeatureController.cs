using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ianhd.core.Extensions;

namespace ianhd.admin.Controllers
{
    public class SiteFeatureController : ianhd.core.Mvc.Controllers.BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index(int siteFeatureCategoryId)
        {
            var vm = new ViewModels.SiteFeatureIndex
            {
                NewSiteFeature = new data.SiteFeature(siteFeatureCategoryId),
                SiteFeatures = ianhd.data.SiteFeature.Get(siteFeatureCategoryId)
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ViewModels.SiteFeatureIndex vm, int siteFeatureCategoryId)
        {
            var dbSiteFeature = new data.SiteFeature {
                Archive = vm.NewSiteFeature.Archive,
                LastDate = vm.NewSiteFeature.LastDate,
                SiteFeatureCategoryId = siteFeatureCategoryId,
                StartDate = vm.NewSiteFeature.StartDate,
                Value = vm.NewSiteFeature.Value.TrimToNull()
            };

            bool success = data.SiteFeature.CreateUpdate(dbSiteFeature);
            
            return Redirect("~/siteFeature");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            bool success = data.SiteFeature.Delete(id);

            return Redirect("~/siteFeature");
        }
    }
}
