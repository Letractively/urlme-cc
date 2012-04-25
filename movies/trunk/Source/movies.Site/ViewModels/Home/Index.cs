﻿using System.Collections.Generic;

namespace movies.Site.ViewModels.Home
{
    public class Index
    {
        public Dictionary<string, Model.Movie> OpeningMovies { get; set; }
        public Dictionary<string, Model.Movie> BoxOfficeMovies { get; set; }
        public Dictionary<string, Model.Movie> UpcomingMovies { get; set; }
        public Dictionary<string, Model.Movie> InTheatersMovies { get; set; }
        public Dictionary<string, Model.Movie> RedBoxTop20Movies { get; set; }
        public Dictionary<string, Model.Movie> RedBoxComingSoonMovies { get; set; }
        public Dictionary<string, Model.Movie> RedboxMovies { get; set; }

        public Model.Movie OverlayMovie { get; set; }
        public bool UseAjaxForLinks { get; set; }
        public bool PrefetchLinks { get; set; }
    }
}