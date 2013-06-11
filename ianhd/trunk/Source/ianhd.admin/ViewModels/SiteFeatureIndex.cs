using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ianhd.data;

namespace ianhd.admin.ViewModels
{
    public class SiteFeatureIndex : ianhd.core.Mvc.ViewModels.ViewModelBase
    {
        public List<SiteFeature> SiteFeatures { get; set; }
        public SiteFeature NewSiteFeature { get; set; }
        public FeatureCategory FeatureCategory { get; set; }
    }
}