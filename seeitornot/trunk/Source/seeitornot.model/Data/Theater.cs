using ianhd.core.Web.Caching;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace seeitornot.model
{
    public partial class Theater
    {
        public static Dictionary<int, Theater> Get(string zip)
        {
            Dictionary<int, Theater> rtn = Cache.GetValue<Dictionary<int, Theater>>(
                string.Format("codejkjk.movies.Theater.Get.{0}", zip),
                () =>
                {
                    string url = api.Flixster.GetShowtimesUrl(zip);
                    
                    // html agility pack magic
                    var htmlWeb = new HtmlWeb();
                    var doc = htmlWeb.Load(url);
                    var root = doc.DocumentNode;

                    return new Dictionary<int, Theater>();

                    // return ret.Where(x => !x.posterDetailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            return rtn;
        }
    }
}