namespace movies.API
{
    public class RedBox
    {
        private const string BaseUrl = "https://api.redbox.com/";
        private const string ApiKey = "7e6905a9f6b8edcd13092f51e1412b37";
        private const string RefCode = "cid=api:tps:idavis:041712";

        /// <summary>
        /// Get list of top 20 movies that have been rented given period of time (# days ago)
        /// </summary>
        // https://api.redbox.com/v3/products/movies/top20?apiKey=7e6905a9f6b8edcd13092f51e1412b37&period=30
        public static string GetTop20Xml(int? withinDaysAgo = 30)
        {
            string url = string.Format("{0}v3/products/movies/top20?apiKey={1}&period={2}", BaseUrl, ApiKey, withinDaysAgo);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        // this doesn't really work
        public static string GetComingSoonXml()
        {
            string url = string.Format("{0}v3/products/movies/comingsoon?apiKey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        // way too much data, but, should probably use this for landing page / search auto-suggest
        // https://api.redbox.com/v3/products/movies/default?apiKey=7e6905a9f6b8edcd13092f51e1412b37&includingComingSoon=false
        public static string GetXml()
        {
            // string url = string.Format("{0}v3/products/movies?apiKey={1}", BaseUrl, ApiKey);
            string url = string.Format("{0}v3/products/movies/default?apiKey={1}&includeComingSoon=false", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        /// <summary>
        /// Get list of redbox locations given latitude and longitude
        /// </summary>
        /// // https://api.redbox.com/stores/latlong/37.573557,-77.521813?apiKey=7e6905a9f6b8edcd13092f51e1412b37
        public static string GetRedboxesXml(string latitude, string longitude)
        {
            string url = string.Format("{0}stores/latlong/{1},{2}?apiKey={3}", BaseUrl, latitude, longitude, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        /// <summary>
        /// Get redbox inventory
        /// </summary>
        // https://api.redbox.com/v3/inventory/stores/AF2A04F9-BD59-4497-9939-A536B1FB95A3?apiKey=7e6905a9f6b8edcd13092f51e1412b37
        public static string GetRedboxInventoryXml(string storeId)
        {
            string url = string.Format("{0}v3/inventory/stores/{1}?apiKey={2}", BaseUrl, storeId, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}
