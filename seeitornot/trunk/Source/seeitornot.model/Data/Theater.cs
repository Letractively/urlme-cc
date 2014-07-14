using ianhd.core.Web.Caching;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace seeitornot.model
{
    public partial class Theater
    {
        public static List<Theater> Get(string zip)
        {
            var rtn = Cache.GetValue<List<Theater>>(
                string.Format("seeitornot.model.Theater.Get.{0}", zip),
                () =>
                {
                    var theaters = new List<Theater>();

                    string url = api.Flixster.GetShowtimesUrl(zip);
                    
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

                        var theaterLinkNode = theaterDiv.SelectSingleNode("h2/a");

                        // theater info: title, theaterId, address, mapUrl
                        string theaterHref = theaterLinkNode.Attributes["href"].Value;
                        int theaterId = int.Parse(theaterHref.Substring(theaterHref.LastIndexOf("/") + 1));
                        string theaterTitle = theaterLinkNode.InnerHtml;
                        string mapUrl = theaterDiv.SelectSingleNode("h2/span/a").Attributes["href"].Value;
                        var spanToRemove = theaterDiv.SelectSingleNode("h2/span/a");
                        spanToRemove.ParentNode.RemoveChild(spanToRemove);
                        string theaterAddress = theaterDiv.SelectSingleNode("h2/span").InnerHtml.Split('-')[1].Trim();

                        var theater = new Theater
                        {
                            id = theaterId,
                            name = theaterTitle,
                            address = theaterAddress,
                            mapUrl = mapUrl
                        };

                        theaters.Add(theater);
                    }

                    return theaters;
                });

            return rtn;
        }
    }
}