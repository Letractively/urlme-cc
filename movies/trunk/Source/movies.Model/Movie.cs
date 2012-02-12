using System.Collections.Generic;
using movies.Core.Web.Caching;

namespace movies.Model
{
    public class Movie
    {
        public string RtMovieId { get; set; }
        public string ImdbMovieId { get; set; }
        public string Title { get; set; }
        public string MpaaRating { get; set; }
        public string Duration { get; set; }
        public int RtCriticsRating { get; set; }
        public int RtAudienceRating { get; set; }

        public static List<Movie> GetMovie(string rtMovieId)
        {
            {
                return Cache.GetValue<List<Movie>>(
                    string.Format("codejkjk.movies.Model.GetMovie-{0}", rtMovieId),
                    () =>
                    {
                        return null;
                    });
            }
        }
    }
}
