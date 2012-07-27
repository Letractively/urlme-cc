using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using movies.Core.Web.Caching;
using HtmlAgilityPack;
using System.Data;
using System.IO;
using movies.Core.Extensions;

namespace movies.Model
{
    public class TrailerAddict
    {
        public class Trailer
        {
            public string title { get; set; }
            public string link { get; set; }
            public DateTime pubDate { get; set; }
            public string trailer_id { get; set; }
            public string embed { get; set; }
        }

        public static List<Trailer> GetFeatured(int count = 5, int width = 450)
        {
            return Cache.GetValue<List<Trailer>>(
                string.Format("codejkjk.movies.Model.TrailerAddict.GetFeatured-{0}-{1}", count, width),
                () =>
                {
                    string xml = API.TrailerAddict.GetFeaturedXml(count, width);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new StringReader(xml));
                    return null;

                    //if (ds.Tables.Contains("Publishedid") && ds.Tables["Publishedid"].Rows.Count == 1 && ds.Tables["Publishedid"].Columns.Contains("Publishedid_Text"))
                    //{
                    //    return ds.Tables["Publishedid"].Rows[0]["Publishedid_Text"].ToString();
                    //}
                    //return string.Empty;
                }, 120);
        }
    }
}
