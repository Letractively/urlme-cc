using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using movies.Core.Web.Caching;
using HtmlAgilityPack;

namespace movies.Model
{
    public class PostalCode
    {
        public List<Model.PostalCode.Theater> theaters { get; set; }
        public class Movie
        {
            public string id { get; set; }
            public string imageUrl { get; set; }
            public string title { get; set; }
            public string mpaaRating { get; set; }
            public string duration { get; set; }
            public string showtimesHtml { get; set; }
            public string criticsRating { get; set; }
            public int criticsScore { get; set; }
            public string audienceRating { get; set; }
            public int audienceScore { get; set; }
            public string imdbRating { get; set; }
            public string movieSlug { get; set; }
            public string imdbClass { get; set; }
            public string imdbQ { get; set; }
        }
        public class Theater
        {
            public string id { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string mapUrl { get; set; }
            public string theaterUrl { get; set; }

            public List<Model.PostalCode.Movie> movies { get; set; }
        }

        public static string GetShowtimes(string date, string zip)
        {
            return Cache.GetValue<string>(
                string.Format("codejkjk.movies.Model.PostalCode.GetShowtimes-{0}-{1}", date, zip),
                () =>
                {
                    return API.Flixster.GetTheatersHtml(date, zip);
                });
        }

        public static PostalCode Get(string date, string zip)
        {
            return Cache.GetValue<PostalCode>(
                string.Format("codejkjk.movies.Model.PostalCode.Get-{0}-{1}", date, zip),
                () =>
                {
                    var rtn = new PostalCode { theaters = new List<Theater>() };

                    // do html agility pack magic
                    HtmlWeb htmlWeb = new HtmlWeb();
                    string url = API.Flixster.GetHtmlUrl(date, zip);
                    HtmlDocument doc = htmlWeb.Load(url);
                    HtmlNode root = doc.DocumentNode;
                    foreach (HtmlNode theaterDiv in root.SelectNodes("//div[@class='theater clearfix']"))
                    {
                        // does theater have showtimes?
                        if (theaterDiv.SelectNodes("div/div") == null)
                        {
                            continue; // to next theater
                        }

                        var theaterLinkNode = theaterDiv.SelectSingleNode("h2/a");

                        // theater info: title, theaterId, address, mapUrl
                        string theaterHref = theaterLinkNode.Attributes["href"].Value;
                        string theaterId = theaterHref.Substring(theaterHref.LastIndexOf("/") + 1);
                        string theaterTitle = theaterLinkNode.InnerHtml;
                        string mapUrl = theaterDiv.SelectSingleNode("h2/span/a").Attributes["href"].Value;
                        var spanToRemove = theaterDiv.SelectSingleNode("h2/span/a");
                        spanToRemove.ParentNode.RemoveChild(spanToRemove);
                        string theaterAddress = theaterDiv.SelectSingleNode("h2/span").InnerHtml.Split('-')[1].Trim();

                        var theater = new Theater
                        {
                            id = theaterId,
                            name = theaterTitle,
                            address = theaterAddress,
                            theaterUrl = theaterHref,
                            mapUrl = mapUrl
                        };

                        // showtimes info
                        var movies = new List<Model.PostalCode.Movie>();
                        foreach (HtmlNode showtimeDiv in theaterDiv.SelectNodes("div/div"))
                        {
                            // parse out rt movie id
                            var movieHrefNode = showtimeDiv.SelectSingleNode("h3/a");
                            string rtMovieId = movieHrefNode.Attributes["href"].Value.Substring(movieHrefNode.Attributes["href"].Value.LastIndexOf("/") + 1);
                            var fullMovie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);

                            // parse out showtimes html for this movie
                            var h3ToRemove = showtimeDiv.SelectSingleNode("h3");
                            h3ToRemove.ParentNode.RemoveChild(h3ToRemove);
                            string showtimes = showtimeDiv.InnerHtml.Trim().Replace("\t", "").Replace("\n", "").Replace("&nbsp;","&nbsp;&nbsp;&nbsp;");
                            fullMovie.ShowtimesHtml = showtimes;

                            var theaterMovie = new Model.PostalCode.Movie
                            {
                                audienceRating = fullMovie.ratings.audience_rating,
                                audienceScore = fullMovie.ratings.audience_score,
                                criticsRating = fullMovie.ratings.critics_rating,
                                criticsScore = fullMovie.ratings.audience_score,
                                duration = fullMovie.Duration,
                                id = fullMovie.id,
                                imageUrl = fullMovie.posters.thumbnail,
                                imdbRating = fullMovie.IMDbRating,
                                movieSlug = fullMovie.MovieSlug,
                                mpaaRating = fullMovie.mpaa_rating,
                                showtimesHtml = fullMovie.ShowtimesHtml,
                                title = fullMovie.title,
                                imdbClass = fullMovie.IMDbClass,
                                imdbQ = fullMovie.IMDbQ
                            };

                            // add to movie list, which we'll add to theater later
                            movies.Add(theaterMovie);
                        } // next movie

                        theater.movies = movies;
                        rtn.theaters.Add(theater);
                    } // next theater

                    return rtn; // return postalcode after adding theaters w/ list of movies added to each
                });
        }
    }
}
