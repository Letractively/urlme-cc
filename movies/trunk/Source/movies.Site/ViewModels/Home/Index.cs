using System.Collections.Generic;

namespace movies.Site.ViewModels.Home
{
    public class Index
    {
        public Dictionary<string, Model.Movie> BoxOfficeMovies { get; set; }
        public Dictionary<string, Model.Movie> UpcomingMovies { get; set; }
        public Model.Movie OverlayMovie { get; set; }
        public bool UseAjaxForLinks { get; set; }
        public bool PrefetchLinks { get; set; }
    }
}