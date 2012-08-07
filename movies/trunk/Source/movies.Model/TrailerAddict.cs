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
                    List<Trailer> rtn = new List<Trailer>();
                    string xml = API.TrailerAddict.GetFeaturedXml(count, width);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new StringReader(xml));
                    if (ds.Tables.Contains("trailer") && ds.Tables["trailer"].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables["trailer"].Rows)
                        {
                            rtn.Add(new Trailer
                            {
                                embed = row["embed"].ToString(),
                                link = row["link"].ToString(),
                                pubDate = DateTime.Parse(row["pubDate"].ToString()),
                                title = row["title"].ToString(),
                                trailer_id = row["trailer_id"].ToString()
                            });
                        }
                    }

                    return rtn;
                }, 120);
        }
    }
}
