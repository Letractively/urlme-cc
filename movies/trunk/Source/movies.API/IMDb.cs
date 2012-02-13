namespace movies.API
{
    public class IMDb
    {
        private const string BaseUrl = "http://api.rottentomatoes.com/api/public/v1.0/";
        private const string ApiKey = "pfrwfgnr53tpaydw8pnhrymy";

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
