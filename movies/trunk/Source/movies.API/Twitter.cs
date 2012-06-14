using Twitterizer;

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

        public static void UpdateStatus(string text)
        {
            OAuthTokens oauthTokens = new OAuthTokens();
            oauthTokens.AccessToken = "608444231-1QmSod4dQmemO87veR5Wqixkfz4yLZMp4d0OOPdI";
            oauthTokens.AccessTokenSecret = "cfLx0xGqthc7ibdXVPooMPhH1W5F94IeYZfJVxIsw";
            oauthTokens.ConsumerKey = "GtqE8V0qYdlq9qDKDBWMxA";
            oauthTokens.ConsumerSecret = "B2y9DhMyzFzGP246q6LnQhue07FtDNfoHfqAughA";

            TwitterStatus.Update(oauthTokens, text);
        }
    }
}
