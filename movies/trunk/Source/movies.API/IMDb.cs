namespace movies.API
{
    public class IMDb
    {
        private const string BaseUrl = "http://www.imdbapi.com/?i=tt";

        public static string GetMovieJson(string imdbMovieId)
        {
            string url = string.Format("{0}{1}", BaseUrl, imdbMovieId);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetParentalGuideUrl(string imdbMovieId)
        {
            if (imdbMovieId.IndexOf("tt") != 0)
                imdbMovieId = "tt" + imdbMovieId;
            return string.Format("http://www.imdb.com/title/{0}/parentalguide", imdbMovieId);
        }

        public static string GetMovieUrl(string imdbMovieId)
        {
            return string.Format("http://www.imdb.com/title/tt{0}/", imdbMovieId);
        }
    }
}
