using System;
namespace seeitornot.api
{
    public class Flixster
    {
        private const string BaseUrl = "http://opensocial.flixster.com/igoogle/showtimes";

        public static string GetShowtimesUrl(string zip, DateTime date)
        {
            return string.Format("{0}?date={1}&postal={2}", BaseUrl, date.ToString("yyyyMMdd"), zip);
        }

        public static string GetShowtimesUrl(string zip)
        {
            return GetShowtimesUrl(zip, System.DateTime.Now);
        }
    }
}
