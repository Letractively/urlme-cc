using System;
namespace seeitornot.api
{
    public class Flixster
    {
        private const string BaseUrl = "http://opensocial.flixster.com/igoogle/showtimes";

        public static string GetShowtimesUrl(DateTime date, string zip)
        {
            return string.Format("{0}?date={1}&postal={2}", BaseUrl, date.ToString("yyyyMMdd"), zip);
        }

        public static string GetShowtimesUrl(string zip)
        {
            return GetShowtimesUrl(System.DateTime.Now, zip);
        }
    }
}
