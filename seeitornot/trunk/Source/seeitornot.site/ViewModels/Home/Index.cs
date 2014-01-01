using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace seeitornot.site.ViewModels.Home
{
    public class Index : ViewModelBase
    {
        public List<model.MovieList> movieLists { get; set; }
        public model.Movie OverlayMovie { get; set; }

        public Index()
        {
            this.movieLists = new List<model.MovieList>();
            this.movieLists.Add(
                new model.MovieList { title = "This Weekend", movies = new Dictionary<string, model.Movie>() }
            );
            this.movieLists.Add(
                new model.MovieList { title = "In Theaters", movies = new Dictionary<string, model.Movie>() }
            );
            this.movieLists.Add(
                new model.MovieList { title = "Coming Soon", movies = new Dictionary<string, model.Movie>() }
            );
        }
    }
}