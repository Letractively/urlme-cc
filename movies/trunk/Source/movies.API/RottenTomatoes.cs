using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using movies.Core;

namespace movies.API
{
    public class RottenTomatoes
    {
        private const string BaseUrl = "http://api.rottentomatoes.com/api/public/v1.0/";
        private const string ApiKey = "pfrwfgnr53tpaydw8pnhrymy";

        public static string GetBoxOfficeJson()
        {
            string url = string.Format("{0}lists/movies/box_office.json?page_limit=20&page=1&country=us&apikey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetUpcomingJson()
        {
            string url = string.Format("{0}lists/movies/upcoming.json?page_limit=20&page=1&country=us&apikey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}
