using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ianhd.core.Web.Caching;
using ianhd.core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using seeitornot.data;

namespace seeitornot.model
{
    public class Movie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string mpaa_rating { get; set; }
        public string posterDetailed { get; set; }

        public Movie(JToken item)
        {
            this.id = (string)item["id"];
            this.title = (string)item["title"];
            this.posterDetailed = (string)item["posters"]["detailed"];
            this.mpaa_rating = (string)item["mpaa_rating"];
        }

        public static Dictionary<string, Movie> GetMovies(Enumerations.MovieLists movielist)
        {
            // get list of rt movies from cache
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                string.Format("codejkjk.movies.Model.Movie.{0}", movielist.ToString()),
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
                            //rtJson = api.RottenTomatoes.GetOpeningJson();
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
