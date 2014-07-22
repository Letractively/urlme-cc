using Newtonsoft.Json.Linq;
using ianhd.core.Extensions;
using System.Collections.Generic;
using System;

namespace seeitornot.model
{
    public partial class Movie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string mpaaRating { get; set; }
        public string posterDetailed { get; set; }
        public string posterThumbnail { get; set; }
        public string parentalGuideUrl { get; set; }
        public int audienceScore { get; set; }
        public string audienceScoreTag { get; set; }
        public string runtime { get; set; }
        public DateTime releaseDate { get; set; }
        public string slug { get; set; }

        // props that do NOT come from RT API
        public List<string> showtimes { get; set; }

        public Movie(JToken item)
        {
            try
            {
                this.id = (string)item["id"];
                this.title = (string)item["title"];
                this.slug = this.title.Slugify();
                this.posterThumbnail = ((string)item["posters"]["detailed"]).Replace("_tmb", "_mob");
                this.posterDetailed = ((string)item["posters"]["detailed"]).Replace("_tmb", "_det");
                this.mpaaRating = (string)item["mpaa_rating"];
                this.runtime = (int)item["runtime"] + " min";
                // this.movieSlug = string.Format("{0}/{1}", this.title.Slugify(), this.id);
                this.audienceScore = (int)item["ratings"]["audience_score"];
                this.audienceScoreTag = (string)item["ratings"]["audience_rating"];
                this.parentalGuideUrl = string.Format("http://www.imdb.com/title/tt{0}/parentalguide", (string)item["alternate_ids"]["imdb"]);
                this.releaseDate = (DateTime)item["release_dates"]["theater"];
            }
            catch
            {
                // silent, leave all defaults, some comment.
            }
        }
    }
}
