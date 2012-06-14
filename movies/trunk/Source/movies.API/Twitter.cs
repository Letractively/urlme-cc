namespace movies.API
{
    public class Twitter
    {
        private const string GetMovieReviewBaseUrl = "http://search.twitter.com/search.json?q=from%3Acodejkjk%20%23";

        public static string GetMovieReviewJson(string movieId)
        {
            string url = string.Format("{0}{1}", GetMovieReviewBaseUrl, movieId);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}
