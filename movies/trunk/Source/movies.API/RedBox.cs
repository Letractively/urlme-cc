﻿namespace movies.API
{
    public class RedBox
    {
        private const string BaseUrl = "https://api.redbox.com/";
        private const string ApiKey = "7e6905a9f6b8edcd13092f51e1412b37";
        private const string RefCode = "cid=api:tps:idavis:041712";

        public static string GetTop20Xml(int? withinDaysAgo = 30)
        {
            string url = string.Format("{0}v3/products/movies/top20?apiKey={1}&period={2}", BaseUrl, ApiKey, withinDaysAgo);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}