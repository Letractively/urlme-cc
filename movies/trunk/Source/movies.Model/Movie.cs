using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using movies.Core.Extensions;
using movies.Core.Web.Caching;
using System.Data;
using System.IO;
using movies.Data.Repository;
using HtmlAgilityPack;

namespace movies.Model
{
    #region Helper Classes
    public class AutocompleteItem
    {
        public string title { get; set; }
        public string year { get; set; }

        // helpers
        public AlternateIds alternate_ids { get; set; }
        public string mpaa_rating { get; set; }
        public AbridgedCast[] abridged_cast { get; set; }
        public Posters posters { get; set; }
        public string id { get; set; }
        public string imgUrl { get { return this.posters.thumbnail; } }
        public string url { get { return "/" + this.title.Slugify() + "/" + this.id; } }
        public string cast
        {
            get
            {
                string ret = "";
                if (this.abridged_cast != null && this.abridged_cast.Count() >= 2)
                {
                    ret = string.Format("{0}, {1}", this.abridged_cast[0].name, this.abridged_cast[1].name);
                }
                return ret;
            }
        }
    }
    public class AutocompleteItemCollection
    {
        public List<AutocompleteItem> movies { get; set; }
    }

    public class ClipCollection
    {
        public Clip[] clips { get; set; }
    }
    public class Clip
    {
        public string thumbnail { get; set; }
    }

    public class MovieCollection
    {
        public int total { get; set; }
        public List<Movie> movies { get; set; }
    }
    public class ReleaseDates
    {
        public DateTime theater { get; set; }
    }
    public class Ratings
    {
        public string critics_rating { get; set; }
        public int critics_score { get; set; }
        public string audience_rating { get; set; }
        public int audience_score { get; set; }
    }
    public class Posters
    {
        public string thumbnail { get; set; }
        public string profile { get; set; }
        public string detailed { get; set; }
        public string original { get; set; }
    }
    public class AbridgedCast
    {
        public string name { get; set; }
    }
    public class AlternateIds
    {
        public string imdb { get; set; }
    }
    public class Links
    {
        public string self { get; set; }
        public string alternate { get; set; }
        public string cast { get; set; }
        public string clips { get; set; }
        public string reviews { get; set; }
        public string similar { get; set; }
    }
    #endregion

    public class Movie
    {
        private static readonly DirectRepository repo = new DirectRepository();

        public string id { get; set; }
        public string year { get; set; }
        public string title { get; set; }
        public string mpaa_rating { get; set; }
        public string runtime { get; set; }
        public ReleaseDates release_dates { get; set; }
        public Ratings ratings { get; set; }
        public string synopsis { get; set; }
        public Posters posters { get; set; }
        public AbridgedCast[] abridged_cast { get; set; }
        public AlternateIds alternate_ids { get; set; }
        public Links links { get; set; }

        // view helpers (items NOT inherently provided by RT api)
        public string IVAPublishedId { get; set; }
        public Data.DomainModels.MovieReview Review { get; set; }
        public Enumerations.MovieType MovieType { get; set; }
        public string ShowtimesHtml { get; set; }
        public bool IsReleased { get { return System.DateTime.Now >= this.release_dates.theater; } }
        public string ReleaseDate { get { return this.release_dates.theater.ToString("MMM d, yyyy"); } }
        public string ParentalGuideUrl { get { return this.alternate_ids != null ? API.IMDb.GetParentalGuideUrl(this.alternate_ids.imdb) : null; } }
        public string ParentalGuideMobileUrl { get { return this.alternate_ids != null ? API.IMDb.GetParentalGuideMobileUrl(this.alternate_ids.imdb) : null; } }
        public string IMDbMovieUrl { get { return this.alternate_ids != null ? API.IMDb.GetMovieUrl(this.alternate_ids.imdb) : null; } }
        public string IMDbPluginHtml { get { return this.alternate_ids != null ? API.IMDb.GetPluginHtmlSmall(this.alternate_ids.imdb, this.title) : null; } }
        public string IMDbId { get { return this.alternate_ids != null ? this.alternate_ids.imdb : null; } }
        public string AbridgedCast
        {
            get
            {
                string ret = "";
                if (this.abridged_cast != null && this.abridged_cast.Count() >= 2)
                {
                    ret = string.Format("{0}, {1}", this.abridged_cast[0].name, this.abridged_cast[1].name);
                }
                return ret;
            }
        }
        public string MovieSlug { get { return this.title.Slugify() + "/" + this.id; } }
        public string FullMovieUrl { get { return string.Format("http://seeitornot.co/{0}", this.MovieSlug); } }
        public string Duration
        {
            get
            {
                if (!string.IsNullOrEmpty(this.runtime))
                {
                    int runTime = int.Parse(this.runtime);
                    var hrs = Math.Floor((double)(runTime / 60));
                    var mins = runTime % 60;

                    return string.Format("{0} hr. {1} min.", hrs, mins);
                }
                return string.Empty;
            }
        }
        public string DurationCompact
        {
            get
            {
                return "0" + this.Duration.Replace(" ", "").Replace("hr.", ":").Replace("min.", "");
            }
        }

        private static Dictionary<string, string> GetRtIds(Enumerations.MovieLists movieList)
        {
            return Cache.GetValue<Dictionary<string, string>>(
                string.Format("codejkjk.movies.Model.Movie.GetRtIds.{0}", movieList.ToString()),
                () =>
                {
                    Dictionary<string, string> ret = new Dictionary<string, string>();
                    Dictionary<string, Movie> movies = new Dictionary<string, Movie>();
                    switch (movieList)
                    {
                        case Enumerations.MovieLists.InTheaters:
                            movies = Model.Movie.GetMovies(Enumerations.MovieLists.InTheaters);
                            break;
                        default:
                            movies = Model.Movie.GetMovies(Enumerations.MovieLists.Opening);
                            break;
                    }
                    movies.Values.ToList().ForEach(x => ret.Add(x.id, x.id));
                    return ret;
                });
        }

        public static Enumerations.MovieType GetMovieType(string rtMovieId)
        {
            var inTheaterRtIds = GetRtIds(Enumerations.MovieLists.InTheaters);
            if (inTheaterRtIds.ContainsKey(rtMovieId))
            {
                return Enumerations.MovieType.InTheaters;
            }

            var openingRtIds = GetRtIds(Enumerations.MovieLists.Opening);
            if (openingRtIds.ContainsKey(rtMovieId))
            {
                return Enumerations.MovieType.InTheaters;
            }

            var topBoxOfficeRtIds = GetRtIds(Enumerations.MovieLists.BoxOffice);
            if (topBoxOfficeRtIds.ContainsKey(rtMovieId))
            {
                return Enumerations.MovieType.InTheaters;
            }

            // for redbox movie type, that's set manually in the Redbox actions
            return Enumerations.MovieType.Neither;
        }

        public static string GetPluginHtmlSmall(string imdbMovieId, string movieTitle)
        {
            return API.IMDb.GetPluginHtmlSmall(imdbMovieId, movieTitle);
        }

        public static string GetPluginHtmlWide(string imdbMovieId, string movieTitle)
        {
            return API.IMDb.GetPluginHtmlWide(imdbMovieId, movieTitle);
        }

        public static string GetRottenTomatoesClipPosterUrl(string rtMovieId)
        {
            return Cache.GetValue<string>(
                string.Format("codejkjk.movies.Model.Movie.GetRottenTomatoesClipPosterUrl-{0}", rtMovieId),
                () =>
                {
                    string rtClipsJson = API.RottenTomatoes.GetMovieClipsJson(rtMovieId);
                    ClipCollection clipColl = rtClipsJson.FromJson<ClipCollection>();
                    if (clipColl.clips != null && clipColl.clips.Any() && !string.IsNullOrWhiteSpace(clipColl.clips.ElementAt(0).thumbnail))
                    {
                        return clipColl.clips.ElementAt(0).thumbnail;
                    }
                    return "/content/images/no-trailer.png";
                }, 60);
        }

        public static Movie GetRottenTomatoesMovieByIMDbId(string imdbId)
        {
            Movie movie = Cache.GetValue<Movie>(
                string.Format("codejkjk.movies.Model.Movie.GetRottenTomatoesMovieByIMDbId-{0}", imdbId),
                () =>
                {
                    string rtJson = API.RottenTomatoes.GetMovieByIMDbIdJson(imdbId);
                    return rtJson.FromJson<Movie>();
                });

            // movie may not exist in RT
            if (string.IsNullOrWhiteSpace(movie.id))
            {
                return null;
            }

            movie.Review = Data.DomainModels.MovieReview.Get(int.Parse(movie.id));

            // load trailer id
            if (string.IsNullOrWhiteSpace(movie.IVAPublishedId))
            {
                movie.IVAPublishedId = Model.IVA.GetPublishedId(movie.id);
            }

            return movie;
        }

        public static Movie GetRottenTomatoesMovie(string rtMovieId)
        {
            Movie movie = Cache.GetValue<Movie>(
                string.Format("codejkjk.movies.Model.Movie.GetRottenTomatoesMovie-{0}", rtMovieId),
                () =>
                {
                    Movie ret = null;

                    // check if this movie has been loaded yet
                    if (Cache.KeyExists(string.Format("codejkjk.movies.Model.Movie.{0}", Enumerations.MovieLists.BoxOffice.ToString())))
                    {
                        var boxOfficeMovies = GetMovies(Enumerations.MovieLists.BoxOffice);
                        if (boxOfficeMovies.ContainsKey(rtMovieId))
                        {
                            ret = boxOfficeMovies[rtMovieId];
                        }
                    }
                    else if (Cache.KeyExists(string.Format("codejkjk.movies.Model.Movie.{0}", Enumerations.MovieLists.Upcoming)))
                    {
                        var upcomingMovies = GetMovies(Enumerations.MovieLists.Upcoming);
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

            movie.Review = Data.DomainModels.MovieReview.Get(int.Parse(movie.id));
            
            // load trailer id
            if (string.IsNullOrWhiteSpace(movie.IVAPublishedId))
            {
                movie.IVAPublishedId = Model.IVA.GetPublishedId(movie.id);
            }

            return movie;
        }

        public static List<AutocompleteItem> SearchMoviesForAutoComplete(string q, int pageLimit)
        {
            List<AutocompleteItem> movies = Cache.GetValue<List<AutocompleteItem>>(
                string.Format("codejkjk.movies.Model.Movie.SearchMoviesForAutoComplete-{0}-{1}", q, pageLimit),
                () =>
                {
                    List<AutocompleteItem> ret = new List<AutocompleteItem>();
                    string rtJson = API.RottenTomatoes.SearchMoviesJson(q, pageLimit);
                    var movieCollection = rtJson.FromJson<AutocompleteItemCollection>();
                    movieCollection.movies.ForEach(x => ret.Add(x));
                    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToList();
                });

            return movies;
        }

        public static Dictionary<string, Movie> SearchMovies(string q, int pageLimit)
        {
            Dictionary<string, Movie> movies = Cache.GetValue<Dictionary<string, Movie>>(
                string.Format("codejkjk.movies.Model.Movie.SearchMovies-{0}-{1}", q, pageLimit),
                () =>
                {
                    List<Movie> ret = new List<Movie>();
                    string rtJson = API.RottenTomatoes.SearchMoviesJson(q, pageLimit);
                    var movieCollection = rtJson.FromJson<MovieCollection>();
                    movieCollection.movies.ForEach(x => ret.Add(x));
                    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            return movies;
        }

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
                            rtJson = API.RottenTomatoes.GetBoxOfficeJson();
                            break;
                        case Enumerations.MovieLists.InTheaters:
                            rtJson = API.RottenTomatoes.GetInTheatersJson();
                            break;
                        case Enumerations.MovieLists.Opening:
                            rtJson = API.RottenTomatoes.GetOpeningJson();
                            break;
                        case Enumerations.MovieLists.Upcoming:
                            rtJson = API.RottenTomatoes.GetUpcomingJson();
                            break;
                        //case Enumerations.MovieLists.RedboxTop20:
                        //    string xml = API.RedBox.GetTop20Xml();
                        //    DataSet ds = new DataSet();
                        //    ds.ReadXml(new StringReader(xml));
                        //    DataTable items = ds.Tables["Item"];
                        //    // for each top 20 movie, get rotten tomatoes equivalent
                        //    foreach (DataRow item in items.Rows)
                        //    {
                        //        var searchJson = API.RottenTomatoes.SearchMoviesJson(item["Title"].ToString());
                        //        var resultsCollection = searchJson.FromJson<MovieCollection>();
                        //        var match = resultsCollection.movies.FirstOrDefault(x => x.release_dates.theater.Year == int.Parse(item["ReleaseYear"].ToString()));
                        //        if (match != null)
                        //        {
                        //            if (ret.FirstOrDefault(x => x.id == match.id) == null)
                        //            {
                        //                match.RedboxProductId = item["productId"].ToString();
                        //                ret.Add(match);
                        //            }
                        //        }
                        //    }
                        //    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                        //case Enumerations.MovieLists.RedBoxComingSoon:
                        //    return null;
                        //    string xml2 = API.RedBox.GetComingSoonXml();
                        //    DataSet ds2 = new DataSet();
                        //    ds2.ReadXml(new StringReader(xml2));
                        //    DataTable rbMovies = ds2.Tables["Movie"];
                        //    // for each coming soon movie, get rotten tomatoes equivalent
                        //    foreach (DataRow movie in rbMovies.Rows)
                        //    {
                        //        var searchJson = API.RottenTomatoes.SearchMoviesJson(movie["Title"].ToString());
                        //        var resultsCollection = searchJson.FromJson<MovieCollection>();
                        //        var match = resultsCollection.movies.FirstOrDefault(x => x.release_dates.theater.Year == int.Parse(movie["ReleaseYear"].ToString()));
                        //        if (match != null)
                        //        {
                        //            if (ret.FirstOrDefault(x => x.id == match.id) == null)
                        //            {
                        //                ret.Add(match);
                        //            }
                        //        }
                        //    }
                        //    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                    }
                    var movieCollection = rtJson.FromJson<MovieCollection>();
                    movieCollection.movies.ForEach(x => ret.Add(x));
                    return ret.Where(x => x.alternate_ids != null && !x.posters.detailed.Contains("poster_default.gif") && x.mpaa_rating != "Unrated").ToDictionary(key => key.id, value => value);
                });

            // set reviews for each movie
            movies.Values.ToList().ForEach(x => x.Review = Data.DomainModels.MovieReview.Get(int.Parse(x.id)));

            return movies;
        }

        public static string GetParentalGuideHtml(string imdbMovieId)
        {
            return Cache.GetValue<string>(
                string.Format("codejkjk.movies.Model.Movie.GetParentalGuideHtml-{0}", imdbMovieId),
                () =>
                {
                    string url = API.IMDb.GetParentalGuideMobileUrl(imdbMovieId);

                    try
                    {
                        // do html agility pack magic
                        HtmlWeb htmlWeb = new HtmlWeb();
                        HtmlDocument doc = htmlWeb.Load(url);
                        HtmlNode root = doc.DocumentNode;
                        foreach (HtmlNode sectionDiv in root.SelectNodes("//section/div"))
                        {
                            var h1 = sectionDiv.SelectSingleNode("h1");
                            if (h1.InnerHtml.ToLower().Contains("nudity"))
                            {
                                return string.Format("<b>Sex & Nudity:</b> {0}<br/><br/>More on <a href='{1}'>IMDb's parental guide</a>.", sectionDiv.SelectSingleNode("p").InnerHtml, url);
                            }
                        } // sectionDiv

                        return "No info re: sex & nudity";
                    }
                    catch
                    {
                        return string.Format("<b><font color='red'>Error</font></b> in scraping Sex & Nudity info from <a href='{0}'>IMDb's parental guide</a>.", url);
                    }

                }, 5);
        }
    }
}
