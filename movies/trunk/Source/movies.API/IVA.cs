namespace movies.API
{
    public class IVA
    {
        private const string BaseUrl = "http://api.internetvideoarchive.com/1.0/DataService/";
        private const string ApiKey = "e340d670-9c4e-4726-b7b8-8a84a68d3c53";

        public static string GetItemXml(string rtMovieId)
        {
            string url = string.Format("{0}GetEntertainmentProgramByPinpointId?ID='{1}'&IdType=32&developerid={2}", BaseUrl, rtMovieId, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }
    }
}
