namespace movies.API
{
    public class RedBox
    {
        private const string BaseUrl = "https://api.redbox.com/";
        private const string ApiKey = "7e6905a9f6b8edcd13092f51e1412b37";
        private const string RefCode = "cid=api:tps:idavis:041712";

        // https://api.redbox.com/v3/products/movies/top20?apiKey=7e6905a9f6b8edcd13092f51e1412b37&period=30
        public static string GetTop20Xml(int? withinDaysAgo = 30)
        {
            string url = string.Format("{0}v3/products/movies/top20?apiKey={1}&period={2}", BaseUrl, ApiKey, withinDaysAgo);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetComingSoonXml()
        {
            string url = string.Format("{0}v3/products/movies/comingsoon?apiKey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetXml()
        {
            string url = string.Format("{0}v3/products/movies?apiKey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        // https://api.redbox.com/stores/latlong/37.573557,-77.521813?apiKey=7e6905a9f6b8edcd13092f51e1412b37
        public static string GetRedboxesXml(string latitude, string longitude)
        {
            string url = string.Format("{0}stores/latlong/{1},{2}?apiKey={1}", BaseUrl, ApiKey, latitude, longitude);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}
