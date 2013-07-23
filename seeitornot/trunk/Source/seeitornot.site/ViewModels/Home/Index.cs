using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace seeitornot.site.ViewModels.Home
{
    public class Index : ViewModelBase
    {
        public List<model.MovieList> movieLists { get; set; }

        public Dictionary<string, model.Movie> ThisWeekendMovies { get; set; }
        public Dictionary<string, model.Movie> UpcomingMovies { get; set; } // be sure to include OpeningMovies
        public Dictionary<string, model.Movie> InTheatersMovies { get; set; }

        //public List<Model.TrailerAddict.Trailer> FeatureTrailers { get; set; }
        //public Dictionary<string, Model.Redbox.Movie> RedboxMovies { get; set; }
        //public List<Data.DomainModels.MovieReview> RecentReviews { get; set; }

        public model.Movie OverlayMovie { get; set; }
        //public Model.TrailerAddict.Trailer OverlayTrailer { get; set; }
    }
}