using System.Collections.Generic;
using System.Linq;
using movies.Core.Web.Caching;
using System.Web.Script.Serialization;
using System;

namespace movies.Model
{
    #region Helper Classes
    public class MovieCollection
    {
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
        public string Imdb { get; set; }
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
    public class ImdbMovie
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
        public int Runtime { get; set; }
        public ReleaseDates Release_Dates { get; set; }
        public Ratings Ratings { get; set; }
        public string Synopsis { get; set; }
        public Posters Posters { get; set; }
        public AbridgedCast[] Abridged_Cast { get; set; }
        public AlternateIds Alternate_Ids { get; set; }
        public Links Links { get; set; }
        public string ImdbRating { get; set; }
        public string ImdbVotes { get; set; }

        // view helpers
        public bool IsReleased { get { return System.DateTime.Now >= this.Release_Dates.Theater; } }
        public string CriticsClass { get { return this.Ratings.Critics_Rating.Contains("Fresh") ? "criticsFresh" : "criticsRotten"; } }
        public string AudienceClass { get { return this.Ratings.Critics_Rating.Contains("Upright") ? "audienceUpright" : "audienceSpilled"; } }
        public string ReleaseDate { get { return this.Release_Dates.Theater.ToString("MMM d,yyyy"); } }
        public string ParentalGuideUrl { get { return API.IMDb.GetParentalGuideUrl(this.Alternate_Ids.Imdb); } }
        public string IMDbMovieUrl { get { return API.IMDb.GetMovieUrl(this.Alternate_Ids.Imdb); } }
        public string Duration
        {
            get
            {
                var hrs = Math.Floor((double)(this.Runtime / 60));
                var mins = this.Runtime % 60;

                return string.Format("{0} hr. {1} min.", hrs, mins);
            }
        }
        //Snippet: function (text, len) {
        //    return text.snippet(len);
        //},
        
        public static Movie GetMovie(string rtMovieId)
        {
            return Cache.GetValue<Movie>(
                string.Format("codejkjk.movies.Model.GetMovie-{0}", rtMovieId),
                () =>
                {
                    return null;
                });
        }

        public static List<Movie> GetBoxOffice()
        {
            return Cache.GetValue<List<Movie>>(
                "codejkjk.movies.Model.GetBoxOffice",
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.GetBoxOfficeJson();
                    // Array arr;

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var movieCollection = jss.Deserialize<MovieCollection>(rtJson);
                    
                    movieCollection.Movies.ForEach(x => ret.Add(x));

                    // for each movie, get imdb info
                    foreach (var movie in ret.Where(x => x.Alternate_Ids != null))
                    {
                        string imdbJson = API.IMDb.GetMovieJson(movie.Alternate_Ids.Imdb);
                        var imdbMovie = jss.Deserialize<ImdbMovie>(imdbJson);
                        if (imdbMovie.Rating != "N/A")
                        {
                            movie.ImdbRating = imdbMovie.Rating;
                            movie.ImdbVotes = imdbMovie.Votes;
                        }
                    }

                    return ret;
                });
        }
    }
}
