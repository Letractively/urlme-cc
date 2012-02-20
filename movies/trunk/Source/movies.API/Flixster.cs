namespace movies.API
{
    public class Flixster
    {
        private const string BaseUrl = "http://opensocial.flixster.com/igoogle/showtimes";

        public static string GetTheaters(string date, string zip)
        {
            string url = string.Format("{0}?date={1}&postal={2}", BaseUrl, date, zip);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}
