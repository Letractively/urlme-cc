using ianhd.core.Web.Caching;
using System.Collections.Generic;
using HtmlAgilityPack;
using System;

namespace seeitornot.model
{
    public partial class Theater
    {
        public static List<TheaterWithShowtimes> Get(string zip, string theaterId, DateTime date)
        {
            var dateStr = date.ToString("yyyyMMdd");

            var rtn = Cache.GetValue<List<TheaterWithShowtimes>>(
                string.Format("seeitornot.model.Theater.Get.{0}.{1}.{2}", zip, theaterId, dateStr), 
                () =>
                {
                    var url = api.Flixster.GetShowtimesUrl(zip, date);

                    // html agility pack magic
                    var htmlWeb = new HtmlWeb();
                    var doc = htmlWeb.Load(url);
                    var root = doc.DocumentNode;

                    foreach (HtmlNode theaterDiv in root.SelectNodes("//div[@class='theater clearfix']"))
                    {
                        // does theater have showtimes?
                        if (theaterDiv.SelectNodes("div/div") == null)
                        {
                            continue; // to next theater
                        }

                        var theater = new Theater(theaterDiv);

                        //theaters.Add(theater);
                    }

                    return null;
                });

            return null;
        }
        
        public static List<Theater> Get(string zip)
        {
            var rtn = Cache.GetValue<List<Theater>>(
                string.Format("seeitornot.model.Theater.Get.{0}", zip),
                () =>
                {
                    var theaters = new List<Theater>();

                    var url = api.Flixster.GetShowtimesUrl(zip);
                    
                    // html agility pack magic
                    var htmlWeb = new HtmlWeb();
                    var doc = htmlWeb.Load(url);
                    var root = doc.DocumentNode;

                    foreach (HtmlNode theaterDiv in root.SelectNodes("//div[@class='theater clearfix']"))
                    {
                        // does theater have showtimes?
                        if (theaterDiv.SelectNodes("div/div") == null)
                        {
                            continue; // to next theater
                        }

                        var theater = new Theater(theaterDiv);
                        theaters.Add(theater);
                    }

                    return theaters;
                });

            return rtn;
        }
    }
}