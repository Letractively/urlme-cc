using System.Collections.Generic;
using movies.Core.Web.Caching;
using System.Web.Script.Serialization;
using System;

namespace movies.Model
{
    public class Movie
    {
        //public string RtMovieId { get; set; }
        //public string ImdbMovieId { get; set; }
        //public string Title { get; set; }
        //public string MpaaRating { get; set; }
        //public string Duration { get; set; }
        //public int RtCriticsRating { get; set; }
        //public int RtAudienceRating { get; set; }
        public string title { get; set; }

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
                    string json = API.RottenTomatoes.GetBoxOfficeJson();
                    // Array arr;

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var arr = jss.Deserialize<Array>(json);

                    List<Movie> movies = jss.Deserialize<List<Movie>>(json); 

                    return null;
                });
        }
    }
}
