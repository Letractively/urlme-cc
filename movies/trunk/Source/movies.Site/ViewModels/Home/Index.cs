using System.Collections.Generic;

namespace movies.Site.ViewModels.Home
{
    public class Index : ViewModelBase
    {
        public Dictionary<string, Model.Movie> ThisWeekendMovies { get; set; }
        public Dictionary<string, Model.Movie> UpcomingMovies { get; set; }
        public Dictionary<string, Model.Movie> OpeningMovies { get; set; }
        public Dictionary<string, Model.Movie> InTheatersMovies { get; set; }
        public List<Model.TrailerAddict.Trailer> FeatureTrailers { get; set; }
        public Dictionary<string, Model.Redbox.Movie> RedboxMovies { get; set; }
        public List<Data.DomainModels.MovieReview> RecentReviews { get; set; }

        public Model.Movie OverlayMovie { get; set; }
        public Model.TrailerAddict.Trailer OverlayTrailer { get; set; }
    }
}