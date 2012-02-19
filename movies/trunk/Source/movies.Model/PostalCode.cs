using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using movies.Core.Web.Caching;

namespace movies.Model
{
    public class PostalCode
    {
        public string postalCode { get; set; }
        public DateTime date { get; set; }
        public List<Theater> theaters { get; set; }

        public static PostalCode GetShowtimes(string date, string zip)
        {
            // get postal code from cache
            PostalCode postalCode = Cache.GetValue<PostalCode>(
                string.Format("codejkjk.movies.PostalCode.GetShowtimes-{0}-{1}", date, zip),
                () =>
                {
                    return null;
                    //List<Movie> ret = new List<Movie>();
                    //string rtJson = API.RottenTomatoes.GetBoxOfficeJson();
                    //var movieCollection = rtJson.FromJson<MovieCollection>();
                    //movieCollection.movies.ForEach(x => x.IMDbLoaded = false); // init all imdbloaded to false
                    //movieCollection.movies.ForEach(x => ret.Add(x));
                    //return ret.ToDictionary(key => key.id, value => value);
                });            
            
            return null;
        }

        
        public class Theater
        {
            public string id { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public List<Movie> movies { get; set; }
        }
    }
}
