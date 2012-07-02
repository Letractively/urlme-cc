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
    public class IVA
    {
        public static string GetPublishedId(string rtMovieId)
        {
            return Cache.GetValue<string>(
                string.Format("codejkjk.movies.Model.IVA.GetPublishedId-{0}", rtMovieId),
                () =>
                {
                    string xml = API.IVA.GetItemXml(rtMovieId);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new StringReader(xml));
                    if (ds.Tables.Contains("Publishedid") && ds.Tables["Publishedid"].Rows.Count == 1 && ds.Tables["Publishedid"].Columns.Contains("Publishedid_Text"))
                    {
                        return ds.Tables["Publishedid"].Rows[0]["Publishedid_Text"].ToString();
                    }
                    return string.Empty;
                });
        }
    }
}
