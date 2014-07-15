using Newtonsoft.Json.Linq;
using ianhd.core.Extensions;
using HtmlAgilityPack;

namespace seeitornot.model
{
    public partial class Theater
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string mapUrl { get; set; }

        public Theater() { }

        public Theater(HtmlNode theaterDiv)
        {
            var theaterLinkNode = theaterDiv.SelectSingleNode("h2/a");

            // theater info: title, theaterId, address, mapUrl
            string theaterHref = theaterLinkNode.Attributes["href"].Value;
            int theaterId = int.Parse(theaterHref.Substring(theaterHref.LastIndexOf("/") + 1));
            string theaterTitle = theaterLinkNode.InnerHtml;
            string mapUrl = theaterDiv.SelectSingleNode("h2/span/a").Attributes["href"].Value;
            var spanToRemove = theaterDiv.SelectSingleNode("h2/span/a");
            spanToRemove.ParentNode.RemoveChild(spanToRemove);
            string theaterAddress = theaterDiv.SelectSingleNode("h2/span").InnerHtml.Split('-')[1].Trim();

            this.id = theaterId;
            this.name = theaterTitle;
            this.address = theaterAddress;
            this.mapUrl = mapUrl;
        }
    }
}
