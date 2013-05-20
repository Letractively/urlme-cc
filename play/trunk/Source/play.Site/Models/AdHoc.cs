using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace play.Site.Models
{
    public partial class AdHoc
    {
        public static bool Save(Models.AdHoc adHoc)
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    adHoc.CreateDate = System.DateTime.Now;
                    ctx.AdHocs.InsertOnSubmit(adHoc);
                    ctx.SubmitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static List<Models.AdHoc> Get(string tag)
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = false })
                {
                    return ctx.AdHocs.Where(x => x.Tag == tag).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public static Models.AdHoc Get(string tag, string value)
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = false })
                {
                    return ctx.AdHocs.FirstOrDefault(x => x.Tag == tag && x.Value == value);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}