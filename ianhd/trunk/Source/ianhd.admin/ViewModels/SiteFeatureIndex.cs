using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ianhd.data;

namespace ianhd.admin.ViewModels
{
    public class SiteFeatureIndex : ianhd.core.Mvc.ViewModels.ViewModelBase
    {
        public class siteFeature
        {
            public string value { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? lastDate { get; set; }
            public string categoryTitle { get; set; }
            public string categoryHintText { get; set; }
            public DateTime? createDate { get; set; }

            public siteFeature(data.SiteFeature dbSiteFeature)
            {
                this.value = dbSiteFeature.Value;
                this.startDate = dbSiteFeature.StartDate;
                this.lastDate = dbSiteFeature.LastDate;
                var featureCategory = dbSiteFeature.SiteFeatureCategory.FeatureCategory;
                this.categoryTitle = featureCategory.Title;
                this.categoryHintText = featureCategory.HintText;
                this.createDate = dbSiteFeature.CreateDate;
            }

            public static List<siteFeature> siteFeatures(List<data.SiteFeature> dbSiteFeatures) {
                var rtn = new List<siteFeature>();
                dbSiteFeatures.ForEach(x => rtn.Add(new siteFeature(x)));
                return rtn;
            }
        }


        public siteFeature newSiteFeature { get; set; }
        public List<siteFeature> siteFeatures { get; set; }
    }
}