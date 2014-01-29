﻿using Newtonsoft.Json.Linq;
using ianhd.core.Extensions;

namespace seeitornot.model
{
    public partial class Movie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string mpaa_rating { get; set; }
        public string posterDetailed { get; set; }
        public string movieSlug { get; set; }

        public Movie(JToken item)
        {
            this.id = (string)item["id"];
            this.title = (string)item["title"];
            this.posterDetailed = (string)item["posters"]["detailed"];
            this.mpaa_rating = (string)item["mpaa_rating"];
            this.movieSlug = string.Format("{0}/{1}", this.title.Slugify(), this.id);
        }
    }
}
