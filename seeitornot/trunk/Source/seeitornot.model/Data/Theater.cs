using ianhd.core.Web.Caching;
using System.Collections.Generic;
using HtmlAgilityPack;
using System;
using ianhd.core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace seeitornot.model
{
    public partial class Theater
    {
        public static List<Theater> GetWithMovies(string zip, string theaterId, DateTime date)
        {
            var dateStr = date.ToString("yyyyMMdd");

            var rtn = Cache.GetValue<List<Theater>>(
                string.Format("seeitornot.model.Theater.GetWithMovies.{0}.{1}.{2}", zip, theaterId, dateStr), 
                () =>
                {
                    var theaters = new List<Theater>();

                    var url = api.Flixster.GetShowtimesUrl(zip, date);

                    // html agility pack magic
                    var htmlWeb = new HtmlWeb();
                    var doc = htmlWeb.Load(url);
                    var root = doc.DocumentNode;

                    foreach (HtmlNode theaterDiv in root.SelectNodes("//div[@class='theater clearfix']"))
                    {
                        // does theater have showtimes?
                        if (theaterDiv.SelectNodes("div/div") == null)
                        {
                            continue; // to next theater
                        }

                        var theater = new Theater(theaterDiv);
                        theater.movies = new List<Movie>();

                        var addTheater = theaterId == "all" || theater.id == theaterId;
                        if (!addTheater) continue; // do not process or add this theater

                        // showtimes info
                        var movies = new List<Movie>();
                        foreach (HtmlNode showtimeDiv in theaterDiv.SelectNodes("div/div"))
                        {
                            try // to parse movie and add it to the list of movies for this theater
                            {
                                // parse out rt movie id
                                var movieHrefNode = showtimeDiv.SelectSingleNode("h3/a");
                                string movieTitle = movieHrefNode.InnerHtml.Trim();
                                string rtMovieId = movieHrefNode.Attributes["href"].Value.Substring(movieHrefNode.Attributes["href"].Value.LastIndexOf("/") + 1);
                                var movie = (Movie)Movie.Get(rtMovieId).Clone();
                                
                                // parse out showtimes html for this movie
                                var h3ToRemove = showtimeDiv.SelectSingleNode("h3");
                                h3ToRemove.ParentNode.RemoveChild(h3ToRemove);
                                string showtimes = showtimeDiv.InnerHtml.Trim().Replace("\t", "").Replace("\n", "").Replace("&nbsp;", "^").TrimEnd('^');

                                if (string.IsNullOrEmpty(showtimes)) continue; // next movie

                                movie.showtimes = new List<string>(showtimes.StripHtml().Split('^'));
                                movie.is3d = movieTitle.ToLower().Contains("3d");

                                // add to movie list, which we'll add to theater later
                                movies.Add(movie);
                            }
                            catch
                            {
                                continue; // skip this movie
                            }
                        } // next movie

                        theater.movies.AddRange(movies.OrderByDescending(x => x.releaseDate));

                        theaters.Add(theater);
                    }

                    return theaters;
                });

            return rtn;
        }
        
        public static List<Theater> Get(string zip)
        {
            var rtn = Cache.GetValue<List<Theater>>(
                string.Format("seeitornot.model.Theater.Get.{0}", zip),
                () =>
                {
                    var theaters = new List<Theater>();

                    var url = api.Flixster.GetShowtimesUrl(zip);
                    
                    // html agility pack magic
                    var htmlWeb = new HtmlWeb();
                    var doc = htmlWeb.Load(url);
                    var root = doc.DocumentNode;

                    foreach (HtmlNode theaterDiv in root.SelectNodes("//div[@class='theater clearfix']"))
                    {
                        // does theater have showtimes?
                        if (theaterDiv.SelectNodes("div/div") == null)
                        {
                            continue; // to next theater
                        }

                        var theater = new Theater(theaterDiv);
                        theaters.Add(theater);
                    }

                    return theaters;
                });

            return rtn;
        }
    }
}