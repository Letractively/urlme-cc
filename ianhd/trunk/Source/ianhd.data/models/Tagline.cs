using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ianhd.data
{
    public partial class Tagline
    {
        public static List<Tagline> Get(string siteCd)
        {
            using (var ctx = new bd13DataContext { ObjectTrackingEnabled = false })
            {
                var now = System.DateTime.Now;
                return ctx.Taglines.Where(x => x.SiteCd == siteCd && !x.Archive && x.StartDate <= now && (!x.LastDate.HasValue || x.LastDate >= now)).ToList();
            }
        }
        public static bool CreateUpdate(Tagline tagline) {
            try
            {
                using (var db = new bd13DataContext { ObjectTrackingEnabled = false })
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
