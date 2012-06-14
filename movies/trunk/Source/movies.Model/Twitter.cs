using System.Collections.Generic;
using System.Linq;
using movies.Core.Extensions;
using movies.Core.Web.Caching;

namespace movies.Model
{
    public class Twitter
    {
        public class SearchResult
        {
            public List<Tweet> results { get; set; }
        }
        public class Tweet
        {
            public string text { get; set; }
        }

        public class Review
        {
            public string ClassName { get; set; }
            public string Text { get; set; }
            public string Url { get; set; }
        }

        public static Review GetMovieReview(string rtMovieId)
        {
            //return Cache.GetValue<string>(
            //    string.Format("codejkjk.movies.Model.Twitter.GetMovieReview-{0}", rtMovieId),
            //    () =>
            //    {
                    string tweetsJson = API.Twitter.GetMovieReviewJson(rtMovieId);
                    var searchResult = tweetsJson.FromJson<SearchResult>();
                    if (searchResult.results.Any())
                    {
                        string text = searchResult.results.ElementAt(0).text;
                        int startIndex = 0;
                        string className = null;
                        if (text.ToLower().Contains("#seeit"))
                        {
                            className = "seeIt";
                            startIndex = text.ToLower().IndexOf("#seeit") + 7;
                        }
                        else
                        {
                            className = "orNot";
                            startIndex = text.ToLower().IndexOf("#ornot") + 7;
                        }
                        return new Review { ClassName = className, Text = text.Substring(startIndex), Url = "//johnhanlonreviews.com" };
                    }
                    return null;
                //});            
        }
    }
}
