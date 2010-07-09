using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrlMe.cc.Model.Enums;

namespace UrlMe.cc.Model
{
    [Serializable]
    public class SiteUpdate
    {
        #region Properties
        private DateTime updateDate = System.DateTime.Now;
        public DateTime UpdateDate { get { return this.updateDate; } }
        private string text = string.Empty;
        public string Text { get { return this.text; } }
        #endregion

        #region Constructors
        private SiteUpdate(Data.SiteUpdate siteUpdate)
        {
            this.updateDate = siteUpdate.UpdateDate;
            this.text = siteUpdate.Text;
        }
        #endregion

        #region Public Static Methods
        public static List<SiteUpdate> GetSiteUpdatesBySite(string siteCd)
        {
            List<SiteUpdate> ret = new List<SiteUpdate>();
            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                var siteUpdates = db.SiteUpdates.Where(x => x.SiteCD == siteCd).ToList();
                foreach (Data.SiteUpdate siteUpdate in siteUpdates)
                {
                    ret.Add(new SiteUpdate(siteUpdate));
                }
            }
            return ret;
        }
        #endregion
    }
}
