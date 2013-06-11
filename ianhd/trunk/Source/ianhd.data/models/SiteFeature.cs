using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ianhd.data
{
    public partial class SiteFeature
    {
        public SiteFeature(int siteFeatureCategoryId)
        {
            this.SiteFeatureCategoryId = siteFeatureCategoryId;
        }
        public static FeatureCategory CategoryGet(string featureCategoryCd)
        {
            using (var ctx = new bd13DataContext { ObjectTrackingEnabled = false })
            {
                return ctx.FeatureCategories.FirstOrDefault(x => x.FeatureCategoryCd == featureCategoryCd);
            }
        }
        public static List<SiteFeature> Get(int siteFeatureCategoryId)
        {
            using (var ctx = new bd13DataContext { ObjectTrackingEnabled = false })
            {
                var now = System.DateTime.Now;
                return ctx.SiteFeatures.Where(x => x.SiteFeatureCategoryId == siteFeatureCategoryId && !x.Archive && (!x.StartDate.HasValue || x.StartDate <= now) && (!x.LastDate.HasValue || x.LastDate >= now)).ToList();
            }
        }
        public static bool Delete(int id)
        {
            try
            {
                using (var db = new bd13DataContext { ObjectTrackingEnabled = true })
                {
                    var record = db.SiteFeatures.FirstOrDefault(x => x.SiteFeatureCategoryId == id);
                    if (record != null)
                    {
                        // delete
                        db.SiteFeatures.DeleteOnSubmit(record);
                        db.SubmitChanges();
                    }
                }

                return true;
            }
            catch
            {
                // silent
            }
            return false;
        }
        public static bool CreateUpdate(SiteFeature siteFeature) {
            try
            {
                using (var db = new bd13DataContext { ObjectTrackingEnabled = true })
                {
                    var record = db.SiteFeatures.FirstOrDefault(x => x.SiteFeatureId == siteFeature.SiteFeatureId);
                    if (record == null) {
                        // create
                        record = new SiteFeature { CreateDate = System.DateTime.Now };
                        db.SiteFeatures.InsertOnSubmit(record);
                    } else {
                        // update
                        record.ModifyDate = System.DateTime.Now;
                    }

                    // update props
                    record.SiteFeatureCategoryId = siteFeature.SiteFeatureCategoryId;
                    record.Value = siteFeature.Value;
                    record.StartDate = siteFeature.StartDate;
                    record.LastDate = siteFeature.LastDate;
                    record.Archive = siteFeature.Archive;
                    
                    db.SubmitChanges();
                }
                
                return true;
            }
            catch
            {
                // silent
            }
            return false;
        }
    }
}
