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
        public ActionResult Index(int siteFeatureCategoryId)
        {
            var dbNewSiteFeature = new SiteFeature(siteFeatureCategoryId);
            var dbSiteFeatures = SiteFeature.Get(siteFeatureCategoryId);

            var vm = new ViewModels.SiteFeatureIndex
            {
                newSiteFeature = new ViewModels.SiteFeatureIndex.siteFeature(dbNewSiteFeature),
                siteFeatures = ViewModels.SiteFeatureIndex.siteFeature.siteFeatures(dbSiteFeatures)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(ViewModels.SiteFeatureIndex.siteFeature siteFeature)
        {
            var dbSiteFeature = new data.SiteFeature {
                Archive = false,
                LastDate = siteFeature.lastDate,
                SiteFeatureCategoryId = siteFeature.siteFeatureCategoryId,
                StartDate = siteFeature.startDate,
                Value = siteFeature.value.TrimToNull()
            };

            bool success = data.SiteFeature.CreateUpdate(dbSiteFeature);
            
            return Redirect("~/siteFeatures/" + siteFeature.siteFeatureCategoryId);
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
