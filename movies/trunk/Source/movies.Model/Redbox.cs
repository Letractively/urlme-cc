using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using movies.Core.Web.Caching;
using HtmlAgilityPack;
using System.Data;
using System.IO;

namespace movies.Model
{
    public class Redbox
    {

        #region Properties
        public string StoreId { get; set; }
        public string StoreType { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Retailer { get; set; }
        public string IndoorOutdoor { get; set; }
        public string Address { get; set; }
        public double Distance { get; set; }
        public List<Redbox.Movie> Products { get; set; }
        #endregion

        #region Helper Classes
        public class Movie
        {
            public string Title { get; set; }
            public bool Available { get; set; }
        }
        #endregion

        //public List<Redbox> GetStoresHavingProductId(string latitude, string longitude, string redboxProductId)
        //{
        //    return Cache.GetValue<List<Redbox>>(
        //        string.Format("codejkjk.movies.Model.Redbox.GetStoresHavingProductId-{0}-{1}-{2}", latitude, longitude, redboxProductId),
        //        () =>
        //        {
        //            List<Redbox> redboxes = GetStores(latitude, latitude);
        //            foreach (var redbox in redboxes)
        //            {
        //                List<Redbox.Movie> products = GetStoreProducts();
                        
        //            }
        //            return null;
        //        });
        //}
        
        public static List<Redbox> GetStores(string latitude, string longitude)
        {
            return Cache.GetValue<List<Redbox>>(
                string.Format("codejkjk.movies.Model.Redbox.GetStores-{0}-{1}", latitude, longitude),
                () =>
                {
                    string xml = API.RedBox.GetRedboxesXml(latitude, longitude);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new StringReader(xml));

                    return null;
                    // return API.Flixster.GetTheatersHtml(date, zip);
                });
        }
    }
}
