using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ianhd.data;
using System.Text;

namespace ianhd.admin.ViewModels
{
    public class SiteFeatureIndex : ianhd.core.Mvc.ViewModels.ViewModelBase
    {
        public class siteFeature
        {
            public int siteFeatureId { get; set; }
            public int siteFeatureCategoryId { get; set; }
            public string value { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? lastDate { get; set; }
            public string categoryTitle { get; set; }
            public string hintText { get; set; }
            public int maxValueLength { get; set; }
            public string valueType { get; set; }
            public DateTime? createDate { get; set; }
            public string when
            {
                get
                {
                    if (!this.startDate.HasValue) // no start date value, which means no end date value
                    {
                        return "Whenever";
                    }
                    else if (this.startDate.HasValue && this.lastDate.HasValue && this.startDate.Value == this.lastDate.Value)
                    {
                        // same day for both start and last date
                        return this.startDate.Value.ToString("MM-dd-yyyy");
                    }

                    // custom
                    return string.Format("{0} - {1}", this.startDate.Value.ToString("MM-dd-yyyy"), this.lastDate.Value.ToString("MM-dd-yyyy"));
                }
            }

            public siteFeature() { }
            public siteFeature(data.SiteFeature dbSiteFeature)
            {
                this.siteFeatureId = dbSiteFeature.SiteFeatureId;
                this.value = dbSiteFeature.Value;
                this.startDate = dbSiteFeature.StartDate;
                this.lastDate = dbSiteFeature.LastDate;
                this.createDate = dbSiteFeature.CreateDate;

                if (dbSiteFeature.SiteFeatureCategory == null || dbSiteFeature.SiteFeatureCategory.FeatureCategory == null)
                    return;

                var featureCategory = dbSiteFeature.SiteFeatureCategory.FeatureCategory;
                this.categoryTitle = featureCategory.Title;
                this.hintText = featureCategory.HintText;
                this.maxValueLength = featureCategory.MaxValueLength;
                this.siteFeatureCategoryId = dbSiteFeature.SiteFeatureCategory.SiteFeatureCategoryId;
                this.valueType = featureCategory.ValueType;
            }

            public static List<siteFeature> siteFeatures(List<data.SiteFeature> dbSiteFeatures)
            {
                var rtn = new List<siteFeature>();
                dbSiteFeatures.ForEach(x => rtn.Add(new siteFeature(x)));
                return rtn;
            }
        }


        public siteFeature newSiteFeature { get; set; }
        public List<siteFeature> siteFeatures { get; set; }
    }
}