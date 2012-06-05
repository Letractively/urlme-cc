using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using movies.Core.Web.Caching;
using HtmlAgilityPack;
using System.Data;
using System.IO;
using movies.Core.Extensions;

namespace movies.Model
{
    public class Redbox
    {

        #region Properties
        public string StoreId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Retailer { get; set; }
        public string IndoorOutdoor { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Distance { get; set; }
        public List<Redbox.Movie> Products { get; set; }
        #endregion

        #region Helper Classes
        public class Movie
        {
            public string Title { get; set; }
            public bool Available { get; set; }
            public string ThumbnailUrl { get; set; }
            public bool IsNewRelease { get; set; }
            public string MovieSlug { get { return "redbox/" + this.Slug; } }
            public string Slug { get; set; }
        }
        #endregion

        public static Model.Movie GetMovie(string rbSlug)
        {
            return Cache.GetValue<Model.Movie>(
                string.Format("codejkjk.movies.Model.Redbox.GetMovie-{0}", rbSlug),
                () =>
                {
                    var allMovies = GetMovies();
                    var rbMovie = allMovies[rbSlug];

                    var rtMovies = Model.Movie.SearchMovies(rbMovie.Title);
                    // iterate thru search results; return one that matches title and release year
                    foreach (var rtMovie in rtMovies.Values)
                    {
                        if (rtMovie.title == rbMovie.Title)
                        {
                            return rtMovie;
                        }
                    }
                    return null;
                });
        }

        public static Dictionary<string, Redbox.Movie> GetMovies()
        {
            return Cache.GetValue<Dictionary<string, Redbox.Movie>>(
                "codejkjk.movies.Model.Redbox.GetMovies",
                () =>
                {
                    var ret = new List<Redbox.Movie>();
                    string xml = API.RedBox.GetXml();
                    DataSet ds = new DataSet();
                    ds.ReadXml(new StringReader(xml));
                    DataTable rbMovies2 = ds.Tables["Movie"];
                    foreach (DataRow movie in rbMovies2.Select("Title <> '' and ReleaseYear <> ''"))
                    {
                        string thumbnailUrl = movie.GetChildRows("Movie_BoxArtImages")[0].GetChildRows("BoxArtImages_link").FirstOrDefault(x => x["rel"].ToString().ToLower() == "http://api.redbox.com/Links/BoxArt/Thumb150".ToLower())["href"].ToString();
                        var newRelease = movie.GetChildRows("Movie_Flags")[0].GetChildRows("Flags_Flag").FirstOrDefault(x => x["type"].ToString().ToLower() == "newrelease");
                        string newReleaseStart = newRelease["beginDate"].ToString();
                        string newReleaseEnd = newRelease["endDate"].ToString();
                        DateTime now = System.DateTime.Now;
                        bool isNewRelease = !string.IsNullOrEmpty(newReleaseStart) && !string.IsNullOrEmpty(newReleaseEnd) && now >= DateTime.Parse(newReleaseStart) && now < DateTime.Parse(newReleaseEnd).AddDays(1);
                        string websiteUrl = movie["websiteUrl"].ToString();

                        var rbMovie = new Redbox.Movie
                        {
                            Title = movie["Title"].ToString(),
                            ThumbnailUrl = thumbnailUrl,
                            IsNewRelease = isNewRelease,
                            Slug = websiteUrl.Substring(websiteUrl.LastIndexOf("/") + 1)
                        };
                        ret.Add(rbMovie);
                    }
                    return ret.ToDictionary(key => key.Slug, value => value);
                });
        }

        //public List<Redbox> GetStoresHavingProductId(string latitude, string longitude, string redboxProductId)
        //{
        //    return Cache.GetValue<List<Redbox>>(
        //        string.Format("codejkjk.movies.Model.Redbox.GetStoresHavingProductId-{0}-{1}-{2}", latitude, longitude, redboxProductId),
        //        () =>
        //        {
        //            List<Redbox> redboxes = GetStores(latitude, latitude);
        //            foreach (var redbox in redboxes)
        //            {
        //                List<Redbox.Movie> products = GetStoreProducts();

        //            }
        //            return null;
        //        });
        //}

        public static List<Redbox> GetStores(string latitude, string longitude)
        {
            return Cache.GetValue<List<Redbox>>(
                string.Format("codejkjk.movies.Model.Redbox.GetStores-{0}-{1}", latitude, longitude),
                () =>
                {
                    var ret = new List<Redbox>();
                    string xml = API.RedBox.GetRedboxesXml(latitude, longitude);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new StringReader(xml));
                    foreach (DataRow store in ds.Tables["Store"].Select("commStatus = 'Online'"))
                    {
                        DataRow location = store.GetChildRows("Store_Location").FirstOrDefault();
                        var redbox = new Redbox
                        {
                            StoreId = store["storeId"].ToString(),
                            Retailer = store["Retailer"].ToString(),
                            IndoorOutdoor = store["storeType"].ToString(),
                            Distance = store["DistanceFromSearchLocation"].ToString(),
                            Latitude = location["lat"].ToString(),
                            Longitude = location["long"].ToString(),
                            Address = location["Address"].ToString(),
                            City = location["City"].ToString(),
                            State = location["State"].ToString(),
                            Zip = location["Zipcode"].ToString()
                        };
                        ret.Add(redbox);
                    }

                    return ret;
                });
        }
    }
}
