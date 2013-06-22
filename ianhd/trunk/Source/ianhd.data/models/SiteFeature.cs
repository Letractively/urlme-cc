using System;
using System.Collections.Generic;
using System.Data.Linq;
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
            this.SiteFeatureCategory = SiteFeatureCategoryGet(siteFeatureCategoryId);

            // figure out default start date
            var siteFeatures = Get(siteFeatureCategoryId);
            var tryDate = System.DateTime.Now;
            bool found = false;
            while (!found)
            {
                found = siteFeatures.FirstOrDefault(x => x.StartDate == tryDate) == null;
                if (!found)
                    tryDate = tryDate.AddDays(1);
            }

            this.StartDate = tryDate;
        }
        public static SiteFeatureCategory SiteFeatureCategoryGet(int siteFeatureCategoryId)
        {
            using (var ctx = new bd13DataContext { ObjectTrackingEnabled = false })
            {
                var dlo = new DataLoadOptions();
                dlo.LoadWith<SiteFeatureCategory>(x => x.FeatureCategory);
                ctx.LoadOptions = dlo;

                return ctx.SiteFeatureCategories.FirstOrDefault(x => x.SiteFeatureCategoryId == siteFeatureCategoryId);
            }
        }
        public static SiteFeature GetById(int siteFeatureId)
        {
            using (var ctx = new bd13DataContext { ObjectTrackingEnabled = false })
            {
                return ctx.SiteFeatures.FirstOrDefault(x => x.SiteFeatureId == siteFeatureId);
            }
        }
        public static List<SiteFeature> Get(int siteFeatureCategoryId)
        {
            using (var ctx = new bd13DataContext { ObjectTrackingEnabled = false })
            {
                var now = System.DateTime.Today;
                return ctx.SiteFeatures.Where(x => x.SiteFeatureCategoryId == siteFeatureCategoryId && !x.Archive && (!x.StartDate.HasValue || x.StartDate <= now) && (!x.LastDate.HasValue || x.LastDate >= now)).ToList();
            }
        }
        public static bool Delete(int id)
        {
            try
            {
                using (var db = new bd13DataContext { ObjectTrackingEnabled = true })
                {
                    var record = db.SiteFeatures.FirstOrDefault(x => x.SiteFeatureId == id);
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
