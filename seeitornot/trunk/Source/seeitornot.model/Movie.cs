using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ianhd.core.Web.Caching;
using seeitornot.data;
using ianhd.core.Extensions;

namespace seeitornot.model
{
    public class MovieCollection
    {
        public int total { get; set; }
        public List<Movie> movies { get; set; }
    }    
    public class Movie
    {
        public string id { get; set; }

        public static Dictionary<string, Movie> GetMovies(Enumerations.MovieLists movielist)
        {
            // get list of rt movies from cache
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                string.Format("codejkjk.movies.Model.Movie.{0}", movielist.ToString()),
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = string.Empty;
                    switch (movielist)
                    {
                        case Enumerations.MovieLists.BoxOffice:
                            //rtJson = api.RottenTomatoes.GetBoxOfficeJson();
                            break;
                        case Enumerations.MovieLists.InTheaters:
                            rtJson = api.RottenTomatoes.GetInTheatersJson();
                            break;
                        case Enumerations.MovieLists.Opening:
                            //rtJson = api.RottenTomatoes.GetOpeningJson();
                            break;
                        case Enumerations.MovieLists.Upcoming:
                            //rtJson = api.RottenTomatoes.GetUpcomingJson();
                            break;
                    }
                    var movieCollection = rtJson.FromJson<MovieCollection>();
                    movieCollection.movies.ForEach(x => ret.Add(x));

                    return ret.ToDictionary(key => key.id, value => value);
                    // TODO: return ret.Where(x => !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            // set reviews for each movie
            // TODO: movies.Values.ToList().ForEach(x => x.Review = Data.DomainModels.MovieReview.Get(int.Parse(x.id)));

            return movies;
        }
    }
}
