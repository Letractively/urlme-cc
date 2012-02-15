namespace movies.API
{
    public class IMDb
    {
        private const string BaseUrl = "http://www.imdbapi.com/?i=tt";

        public static string GetMovieJson(string imdbMovieId)
        {
            // baseurl already has tt, so, make sure tt is not in passed-in imdbMovieId
            imdbMovieId = imdbMovieId.Replace("tt", "");

            string url = string.Format("{0}{1}", BaseUrl, imdbMovieId);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetParentalGuideUrl(string imdbMovieId)
        {
            // e.g., http://www.imdb.com/title/tt1606389/parentalguide
            // make sure passed-in imbMovieId starts with "tt", per url example above
            if (!imdbMovieId.StartsWith("tt"))
                imdbMovieId = "tt" + imdbMovieId;

            return string.Format("http://www.imdb.com/title/{0}/parentalguide", imdbMovieId);
        }

        public static string GetMovieUrl(string imdbMovieId)
        {
            // e.g., http://www.imdb.com/title/tt1606389/
            // make sure passed-in imbMovieId starts with "tt", per url example above
            if (!imdbMovieId.StartsWith("tt"))
                imdbMovieId = "tt" + imdbMovieId;

            return string.Format("http://www.imdb.com/title/{0}/", imdbMovieId);
        }
    }
}
