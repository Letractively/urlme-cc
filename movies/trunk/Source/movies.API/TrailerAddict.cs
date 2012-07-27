namespace movies.API
{
    public class TrailerAddict
    {
        private const string BaseUrl = "http://api.traileraddict.com/";

        public static string GetFeaturedXml(int count, int width)
        {
            string url = string.Format("{0}?featured=yes&count={1}&width={2}", BaseUrl, count, width);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetTrailersByImdbXml(string imdbId, int count = 1, int width = 450)
        {
            imdbId = imdbId.Replace("tt", ""); // remove any prefixing "tt" if it's there
            string url = string.Format("{0}?imdb={1}&count={2}&width={3}", BaseUrl, imdbId, count, width);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}
