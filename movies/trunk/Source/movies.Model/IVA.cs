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
    public class IVA
    {
        public static string GetPublishedId(string imdbId)
        {
            return Cache.GetValue<string>(
                string.Format("codejkjk.movies.Model.IVA.GetPublishedId-{0}", imdbId),
                () =>
                {
                    string xml = API.IVA.GetItemXml(imdbId);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new StringReader(xml));
                    if (ds.Tables.Contains("Publishedid") && ds.Tables["Publishedid"].Rows.Count == 1 && ds.Tables["Publishedid"].Columns.Contains("Publishedid_Text"))
                    {
                        return ds.Tables["Publishedid"].Rows[0]["Publishedid_Text"].ToString();
                    }
                    return string.Empty;
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
