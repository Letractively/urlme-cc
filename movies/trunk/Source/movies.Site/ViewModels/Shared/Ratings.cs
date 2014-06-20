using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using movies.Model;

namespace movies.Site.ViewModels.Shared
{
    public class Ratings
    {
        public Enumerations.RatingSizes RatingSize { get; set; }
        public Model.Movie Movie { get; set; }
        public bool ShowSion { get; set; }
        public bool ShowIMDb { get; set; }
        public bool IFrameIMDbRating { get; set; }
    }
}