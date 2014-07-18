using Newtonsoft.Json.Linq;
using ianhd.core.Extensions;
using System.Collections.Generic;

namespace seeitornot.model
{
    public partial class Movie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string mpaaRating { get; set; }
        public string posterDetailed { get; set; }
        // public string movieSlug { get; set; }
        public string parentalGuideUrl { get; set; }
        public int audienceScore { get; set; }
        public string audienceScoreTag { get; set; }
        public string runtime { get; set; }

        // props that do NOT come from RT API
        public List<string> showtimes { get; set; }

        public Movie(JToken item)
        {
            try
            {
                this.id = (string)item["id"];
                this.title = (string)item["title"];
                this.posterDetailed = ((string)item["posters"]["detailed"]).Replace("_tmb", "_mob");
                this.mpaaRating = (string)item["mpaa_rating"];
                this.runtime = (int)item["runtime"] + " min";
                // this.movieSlug = string.Format("{0}/{1}", this.title.Slugify(), this.id);
                this.audienceScore = (int)item["ratings"]["audience_score"];
                this.audienceScoreTag = (string)item["ratings"]["audience_rating"];
                this.parentalGuideUrl = string.Format("http://www.imdb.com/title/tt{0}/parentalguide", (string)item["alternate_ids"]["imdb"]);
            }
            catch
            {
                // silent, leave all defaults
            }
        }
    }
}
