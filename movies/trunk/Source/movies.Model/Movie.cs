using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using movies.Core.Extensions;
using movies.Core.Web.Caching;

namespace movies.Model
{
    #region Helper Classes
    public class MovieCollection
    {
        public int total { get; set; }
        public List<Movie> movies { get; set; }
    }
    public class ReleaseDates
    {
        public DateTime theater { get; set; }
    }
    public class Ratings
    {
        public string critics_rating { get; set; }
        public int critics_score { get; set; }
        public string audience_rating { get; set; }
        public int audience_score { get; set; }
    }
    public class Posters
    {
        public string thumbnail { get; set; }
        public string profile { get; set; }
        public string detailed { get; set; }
        public string original { get; set; }
    }
    public class AbridgedCast
    {
        public string name { get; set; }
    }
    public class AlternateIds
    {
        public string imdb { get; set; }
    }
    public class Links
    {
        public string self { get; set; }
        public string alternate { get; set; }
        public string cast { get; set; }
        public string clips { get; set; }
        public string reviews { get; set; }
        public string similar { get; set; }
    }
    public class IMDbMovie
    {
        public string rating { get; set; }
        public string votes { get; set; }
    }
    #endregion

    public class Movie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string mpaa_rating { get; set; }
        public string runtime { get; set; }
        public ReleaseDates release_dates { get; set; }
        public Ratings ratings { get; set; }
        public string synopsis { get; set; }
        public Posters posters { get; set; }
        public AbridgedCast[] abridged_cast { get; set; }
        public AlternateIds alternate_ids { get; set; }
        public Links links { get; set; }

        // view helpers (items NOT inherently provided by RT api)
        public string ShowtimesHtml { get; set; }
        public string IMDbRating { get; set; } // need? cuz each movie has imdbmovie obj. hmmmm
        public string IMDbVotes { get; set; } // need?
        public bool IMDbLoaded { get; set; }
        public string IMDbClass { get { return this.IMDbLoaded ? "" : "imdbNotSet"; } }
        public bool IsReleased { get { return System.DateTime.Now >= this.release_dates.theater; } }
        public string ReleaseDate { get { return this.release_dates.theater.ToString("MMM d, yyyy"); } }
        public string ParentalGuideUrl { get { return this.alternate_ids != null ? API.IMDb.GetParentalGuideUrl(this.alternate_ids.imdb) : null; } }
        public string IMDbMovieUrl { get { return this.alternate_ids != null ? API.IMDb.GetMovieUrl(this.alternate_ids.imdb) : null; } }
        public string IMDbQ
        {
            get {
                return this.alternate_ids == null ? null : this.alternate_ids.imdb;
                // return string.Format("{0}&year={1}", this.title.Replace(" ", "+"), release_dates.theater.ToString("yyyy")); 
            }
        }
        public string MovieSlug { get { return this.title.Slugify() + "/" + this.id; } }
        public string Duration
        {
            get
            {
                if (!string.IsNullOrEmpty(this.runtime))
                {
                    int runTime = int.Parse(this.runtime);
                    var hrs = Math.Floor((double)(runTime / 60));
                    var mins = runTime % 60;

                    return string.Format("{0} hr. {1} min.", hrs, mins);
                }
                return string.Empty;
            }
        }
        public string DurationCompact
        {
            get
            {
                return "0" + this.Duration.Replace(" ", "").Replace("hr.", ":").Replace("min.", "");
            }
        }
        //Snippet: function (text, len) {
        //    return text.snippet(len);
        //},

        public static IMDbMovie GetIMDbMovie(string imdbMovieId)
        {
            return Cache.GetValue<IMDbMovie>(
                string.Format("codejkjk.movies.Model.Movie.GetIMDbMovie-{0}", imdbMovieId),
                () =>
                {
                    string imdbJson = API.IMDb.GetMovieJson(imdbMovieId);
                    return imdbJson.FromJson<IMDbMovie>();
                });
        }

        public static Movie GetRottenTomatoesMovie(string rtMovieId)
        {
            Movie movie = Cache.GetValue<Movie>(
                string.Format("codejkjk.movies.Model.Movie.GetRottenTomatoesMovie-{0}", rtMovieId),
                () =>
                {
                    Movie ret = null;

                    // check if this movie has been loaded yet
                    if (Cache.KeyExists("codejkjk.movies.Model.Movie.GetBoxOffice"))
                    {
                        var boxOfficeMovies = GetBoxOffice();
                        if (boxOfficeMovies.ContainsKey(rtMovieId))
                        {
                            ret = boxOfficeMovies[rtMovieId];
                        }
                    }
                    else if (Cache.KeyExists("codejkjk.movies.Model.Movie.GetUpcoming"))
                    {
                        var upcomingMovies = GetUpcoming();
                        if (upcomingMovies.ContainsKey(rtMovieId))
                        {
                            ret = upcomingMovies[rtMovieId];
                        }
                    }

                    // didn't exist in above 2 cached lists
                    if (ret == null)
                    {
                        string rtJson = API.RottenTomatoes.GetMovieJson(rtMovieId);
                        ret = rtJson.FromJson<Movie>();
                    }

                    return ret;
                });

            // can we load imdb?
            TryLoadIMDb(ref movie);

            return movie;
        }

        public static Dictionary<string, Movie> SearchMovies(string q)
        {
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                string.Format("codejkjk.movies.Model.Movie.SearchMovies-{0}", q),
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.SearchMoviesJson(q);
                    var movieCollection = rtJson.FromJson<MovieCollection>();
                    movieCollection.movies.ForEach(x => x.IMDbLoaded = false); // init all imdbloaded to false
                    movieCollection.movies.ForEach(x => ret.Add(x));
                    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            // for each movie, get imdb info if it exists in cache
            foreach (var movie in movies.Values.Where(x => x.alternate_ids != null))
            {
                if (Cache.KeyExists(string.Format("codejkjk.movies.Model.Movie.GetIMDbMovie-{0}", movie.alternate_ids.imdb)))
                {
                    var imdbMovie = GetIMDbMovie(movie.alternate_ids.imdb);
                    movie.IMDbRating = imdbMovie.rating;
                    movie.IMDbVotes = imdbMovie.votes;
                    movie.IMDbLoaded = true;
                }
                else
                {
                    movie.IMDbLoaded = false;
                }
            }

            return movies;
        }

        public static Dictionary<string, Movie> GetBoxOffice()
        {
            // get list of rt movies from cache
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                "codejkjk.movies.Model.Movie.GetBoxOffice",
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.GetBoxOfficeJson();
                    var movieCollection = rtJson.FromJson<MovieCollection>();
                    movieCollection.movies.ForEach(x => x.IMDbLoaded = false); // init all imdbloaded to false
                    movieCollection.movies.ForEach(x => ret.Add(x));
                    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            // for each movie, get imdb info if it exists in cache
            foreach (var movie in movies.Values.Where(x => x.alternate_ids != null))
            {
                if (Cache.KeyExists(string.Format("codejkjk.movies.Model.Movie.GetIMDbMovie-{0}", movie.alternate_ids.imdb)))
                {
                    var imdbMovie = GetIMDbMovie(movie.IMDbQ);
                    movie.IMDbRating = imdbMovie.rating;
                    movie.IMDbVotes = imdbMovie.votes;
                    movie.IMDbLoaded = true;
                }
                else
                {
                    movie.IMDbLoaded = false;
                }
            }

            return movies;
        }

        public static Dictionary<string, Movie> GetOpening()
        {
            // get list of rt movies from cache
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                "codejkjk.movies.Model.Movie.GetOpening",
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.GetOpeningJson();
                    var movieCollection = rtJson.FromJson<MovieCollection>();
                    movieCollection.movies.ForEach(x => x.IMDbLoaded = false); // init all imdbloaded to false
                    movieCollection.movies.ForEach(x => ret.Add(x));
                    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            // for each movie, get imdb info if it exists in cache
            foreach (var movie in movies.Values.Where(x => x.alternate_ids != null))
            {
                if (Cache.KeyExists(string.Format("codejkjk.movies.Model.Movie.GetIMDbMovie-{0}", movie.alternate_ids.imdb)))
                {
                    var imdbMovie = GetIMDbMovie(movie.IMDbQ);
                    movie.IMDbRating = imdbMovie.rating;
                    movie.IMDbVotes = imdbMovie.votes;
                    movie.IMDbLoaded = true;
                }
                else
                {
                    movie.IMDbLoaded = false;
                }
            }

            return movies;
        }

        public static Dictionary<string, Movie> GetInTheaters()
        {
            // get list of rt movies from cache
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                "codejkjk.movies.Model.Movie.GetInTheaters",
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.GetInTheatersJson();
                    var movieCollection = rtJson.FromJson<MovieCollection>();
                    movieCollection.movies.ForEach(x => x.IMDbLoaded = false); // init all imdbloaded to false
                    movieCollection.movies.ForEach(x => ret.Add(x));
                    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            // for each movie, get imdb info if it exists in cache
            foreach (var movie in movies.Values.Where(x => x.alternate_ids != null))
            {
                if (Cache.KeyExists(string.Format("codejkjk.movies.Model.Movie.GetIMDbMovie-{0}", movie.alternate_ids.imdb)))
                {
                    var imdbMovie = GetIMDbMovie(movie.IMDbQ);
                    movie.IMDbRating = imdbMovie.rating;
                    movie.IMDbVotes = imdbMovie.votes;
                    movie.IMDbLoaded = true;
                }
                else
                {
                    movie.IMDbLoaded = false;
                }
            }

            return movies;
        }

        private static void TryLoadIMDb(ref Movie movie)
        {
            // can we load imdb?
            if (movie.alternate_ids != null && !movie.IMDbLoaded)
            {
                if (Cache.KeyExists(string.Format("codejkjk.movies.Model.Movie.GetIMDbMovie-{0}", movie.alternate_ids.imdb)))
                {
                    var imdbMovie = GetIMDbMovie(movie.IMDbQ);
                    movie.IMDbRating = imdbMovie.rating;
                    movie.IMDbVotes = imdbMovie.votes;
                    movie.IMDbLoaded = true;
                }
                else
                {
                    movie.IMDbLoaded = false;
                }
            }
        }

        public static Dictionary<string, Movie> GetUpcoming()
        {
            return Cache.GetValue<Dictionary<string, Movie>>(
                "codejkjk.movies.Model.Movie.GetUpcoming",
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.GetUpcomingJson();

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var movieCollection = jss.Deserialize<MovieCollection>(rtJson);

                    movieCollection.movies.ForEach(x => ret.Add(x));

                    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });
        }
    }
}
