using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Movie = movies.Model.Movie;

namespace movies.Site.ViewModels.Home
{
    public class Index
    {
        public Dictionary<string, Movie> BoxOfficeMovies { get; set; }
        public Dictionary<string, Movie> UpcomingMovies { get; set; }
        public string OverlayRtMovieId { get; set; }
    }
}