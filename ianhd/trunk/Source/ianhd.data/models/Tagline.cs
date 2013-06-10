using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ianhd.data
{
    public partial class Tagline
    {
        public Tagline(string siteCd)
        {
            this.Archive = false;
            this.LastDate = null;
            this.SiteCd = siteCd;
            this.StartDate = System.DateTime.Now;
        }

        public static List<Tagline> Get(string siteCd)
        {
            using (var ctx = new bd13DataContext { ObjectTrackingEnabled = false })
            {
                var now = System.DateTime.Now;
                return ctx.Taglines.Where(x => x.SiteCd == siteCd && !x.Archive && x.StartDate <= now && (!x.LastDate.HasValue || x.LastDate >= now)).ToList();
            }
        }
        public static bool Delete(int id)
        {
            try
            {
                using (var db = new bd13DataContext { ObjectTrackingEnabled = true })
                {
                    var record = db.Taglines.FirstOrDefault(x => x.TaglineId == id);
                    if (record != null)
                    {
                        // delete
                        db.Taglines.DeleteOnSubmit(record);
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
        public static bool CreateUpdate(Tagline tagline) {
            try
            {
                using (var db = new bd13DataContext { ObjectTrackingEnabled = true })
                {
                    var record = db.Taglines.FirstOrDefault(x => x.TaglineId == tagline.TaglineId);
                    if (record == null) {
                        // create
                        record = new Tagline { CreateDate = System.DateTime.Now };
                        db.Taglines.InsertOnSubmit(record);
                    } else {
                        // update
                        record.ModifyDate = System.DateTime.Now;
                    }

                    // update props
                    record.SiteCd = tagline.SiteCd;
                    record.Text = tagline.Text;
                    record.StartDate = tagline.StartDate;
                    record.LastDate = tagline.LastDate;
                    record.Archive = tagline.Archive;
                    
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
