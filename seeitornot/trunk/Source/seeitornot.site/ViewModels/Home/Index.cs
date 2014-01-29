using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using seeitornot.model;
using seeitornot.data;

namespace seeitornot.site.ViewModels.Home
{
    public class Index : ViewModelBase
    {
        public List<model.MovieList> movieLists { get; set; }
        public model.Movie OverlayMovie { get; set; }

        public Index(string rtMovieId)
        {
            // build movie lists
            this.movieLists = new List<model.MovieList>();
            this.movieLists.Add(
                new model.MovieList { title = "In Theaters", movies = Movie.GetMovies(Enumerations.MovieLists.InTheaters) }
            );
            this.movieLists.Add(
                new model.MovieList { title = "Opening", movies = Movie.GetMovies(Enumerations.MovieLists.Opening) }
            );

            // build overlay movie, if passed in
            if (rtMovieId != null)
            {
                this.OverlayMovie = Movie.GetMovie(rtMovieId);
            }
        }
    }
}