using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Movie = movies.Model.Movie;

namespace movies.Site.ViewModels.Home
{
    public class Index
    {
        public List<Movie> BoxOfficeMovies { get; set; }
        public List<Movie> UpcomingMovies { get; set; }
    }
}