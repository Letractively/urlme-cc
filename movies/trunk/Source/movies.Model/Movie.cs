using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using movies.Core.Web.Caching;
using movies.Core.Extensions;

namespace movies.Model
{
    #region Helper Classes
    public class MovieCollection
    {
        public int Total { get; set; }
        public List<Movie> Movies { get; set; }
    }
    public class ReleaseDates
    {
        public DateTime Theater { get; set; }
    }
    public class Ratings
    {
        public string Critics_Rating { get; set; }
        public int Critics_Score { get; set; }
        public string Audience_Rating { get; set; }
        public int Audience_Score { get; set; }
    }
    public class Posters
    {
        public string Thumbnail { get; set; }
        public string Profile { get; set; }
        public string Detailed { get; set; }
        public string Original { get; set; }
    }
    public class AbridgedCast
    {
        public string Name { get; set; }
    }
    public class AlternateIds
    {
        public string IMDb { get; set; }
    }
    public class Links
    {
        public string Self { get; set; }
        public string Alternate { get; set; }
        public string Cast { get; set; }
        public string Clips { get; set; }
        public string Reviews { get; set; }
        public string Similar { get; set; }
    }
    public class IMDbMovie
    {
        public string Rating { get; set; }
        public string Votes { get; set; }
    }
    #endregion

    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Mpaa_Rating { get; set; }
        public string Runtime { get; set; }
        public ReleaseDates Release_Dates { get; set; }
        public Ratings Ratings { get; set; }
        public string Synopsis { get; set; }
        public Posters Posters { get; set; }
        public AbridgedCast[] Abridged_Cast { get; set; }
        public AlternateIds Alternate_Ids { get; set; }
        public Links Links { get; set; }
        public string IMDbRating { get; set; }
        public string IMDbVotes { get; set; }
        public bool IMDbLoaded { get; set; }

        // view helpers
        public bool IsReleased { get { return System.DateTime.Now >= this.Release_Dates.Theater; } }
        public string CriticsClass { get { return this.Ratings.Critics_Rating.Contains("Fresh") ? "criticsFresh" : "criticsRotten"; } }
        public string AudienceClass { get { return this.Ratings.Critics_Rating.Contains("Upright") ? "audienceUpright" : "audienceSpilled"; } }
        public string ReleaseDate { get { return this.Release_Dates.Theater.ToString("MMM d,yyyy"); } }
        public string ParentalGuideUrl { get { return API.IMDb.GetParentalGuideUrl(this.Alternate_Ids.IMDb); } }
        public string IMDbMovieUrl { get { return API.IMDb.GetMovieUrl(this.Alternate_Ids.IMDb); } }
        public string Duration
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Runtime))
                {
                    int runTime = int.Parse(this.Runtime);
                    var hrs = Math.Floor((double)(runTime / 60));
                    var mins = runTime % 60;

                    return string.Format("{0} hr. {1} min.", hrs, mins);
                }
                return string.Empty;
            }
        }
        //Snippet: function (text, len) {
        //    return text.snippet(len);
        //},

        public static IMDbMovie GetIMDbMovie(string imdbMovieId)
        {
            return Cache.GetValue<IMDbMovie>(
                string.Format("codejkjk.movies.Model.GetIMDbMovie-{0}", imdbMovieId),
                () =>
                {
                    string imdbJson = API.IMDb.GetMovieJson(imdbMovieId);
                    return imdbJson.FromJson<IMDbMovie>();
                });
        }

        public static Movie GetRottenTomatoesMovie(string rtMovieId)
        {
            Movie movie = Cache.GetValue<Movie>(
                string.Format("codejkjk.movies.Model.GetRottenTomatoesMovie-{0}", rtMovieId),
                () =>
                {
                    Movie ret = null;

                    // check if this movie has been loaded yet
                    if (Cache.KeyExists("codejkjk.movies.Model.GetBoxOffice"))
                    {
                        var boxOfficeMovies = GetBoxOffice();
                        if (boxOfficeMovies.ContainsKey(rtMovieId))
                        {
                            ret = boxOfficeMovies[rtMovieId];
                        }
                    }
                    else if (Cache.KeyExists("codejkjk.movies.Model.GetUpcoming"))
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

        public static Dictionary<string, Movie> GetBoxOffice()
        {
            // get list of rt movies from cache
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                "codejkjk.movies.Model.GetBoxOffice",
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.GetBoxOfficeJson();
                    var movieCollection = rtJson.FromJson<MovieCollection>();
                    movieCollection.Movies.ForEach(x => x.IMDbLoaded = false); // init all imdbloaded to false
                    movieCollection.Movies.ForEach(x => ret.Add(x));
                    return ret.ToDictionary(key => key.Id, value => value);
                });

            // for each movie, get imdb info if it exists in cache
            foreach (var movie in movies.Values.Where(x => x.Alternate_Ids != null))
            {
                if (Cache.KeyExists(string.Format("codejkjk.movies.Model.GetIMDbMovie-{0}", movie.Alternate_Ids.IMDb)))
                {
                    var imdbMovie = GetIMDbMovie(movie.Alternate_Ids.IMDb);
                    movie.IMDbRating = imdbMovie.Rating;
                    movie.IMDbVotes = imdbMovie.Votes;
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
            if (movie.Alternate_Ids != null && !movie.IMDbLoaded)
            {
                if (Cache.KeyExists(string.Format("codejkjk.movies.Model.GetIMDbMovie-{0}", movie.Alternate_Ids.IMDb)))
                {
                    var imdbMovie = GetIMDbMovie(movie.Alternate_Ids.IMDb);
                    movie.IMDbRating = imdbMovie.Rating;
                    movie.IMDbVotes = imdbMovie.Votes;
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
                "codejkjk.movies.Model.GetUpcoming",
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.GetUpcomingJson();

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var movieCollection = jss.Deserialize<MovieCollection>(rtJson);

                    movieCollection.Movies.ForEach(x => ret.Add(x));

                    return ret.ToDictionary(key => key.Id, value => value);
                });
        }
    }
}
