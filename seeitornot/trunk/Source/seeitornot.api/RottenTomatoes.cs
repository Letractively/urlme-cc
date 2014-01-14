namespace seeitornot.api
{
    public class RottenTomatoes
    {
        private const string BaseUrl = "http://api.rottentomatoes.com/api/public/v1.0/";
        private const string ApiKey = "pfrwfgnr53tpaydw8pnhrymy";

        public static string GetInTheatersJson()
        {
            string url = string.Format("{0}lists/movies/in_theaters.json?page_limit=25&page=1&country=us&apikey={1}", BaseUrl, ApiKey);
            return ianhd.core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetBoxOfficeJson()
        {
            string url = string.Format("{0}lists/movies/box_office.json?page_limit=20&page=1&country=us&apikey={1}", BaseUrl, ApiKey);
            return ianhd.core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}
