using System.Collections.Generic;
using System.Linq;
using ianhd.core.Web.Caching;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using seeitornot.data;

namespace seeitornot.model
{
    public partial class Movie
    {
        public static Movie Get(string rtMovieId)
        {
            var movie = Cache.GetValue<Movie>(
                string.Format("codejkjk.movies.Model.GetMovie.{0}", rtMovieId),
                () =>
                {
                    string json = api.RottenTomatoes.GetMovieJson(rtMovieId);
                    var jObj = (JObject)JsonConvert.DeserializeObject(json);

                    return new Movie(jObj);
                });

            return movie;
        }

        public static Dictionary<string, Movie> Get(Enumerations.MovieLists movielist)
        {
            // get list of rt movies from cache
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                string.Format("codejkjk.movies.Model.GetMovies.{0}", movielist.ToString()),
                () =>
                {
                    string json = string.Empty;
                    switch (movielist)
                    {
                        case Enumerations.MovieLists.BoxOffice:
                            //rtJson = api.RottenTomatoes.GetBoxOfficeJson();
                            break;
                        case Enumerations.MovieLists.InTheaters:
                            json = api.RottenTomatoes.GetInTheatersJson();
                            break;
                        case Enumerations.MovieLists.Opening:
                            json = api.RottenTomatoes.GetOpeningJson();
                            break;
                        case Enumerations.MovieLists.Upcoming:
                            //rtJson = api.RottenTomatoes.GetUpcomingJson();
                            break;
                    }
                    var jObj = (JObject)JsonConvert.DeserializeObject(json);
                    var ret = jObj["movies"].Select(item => new Movie(item));

                    return ret.Where(x => !x.posterDetailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            // set reviews for each movie
            // TODO: movies.Values.ToList().ForEach(x => x.Review = Data.DomainModels.MovieReview.Get(int.Parse(x.id)));

            return movies;
        }
    }
}