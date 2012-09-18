using System.IO;
using System.Net;

namespace movies.Core.Net
{
    public class HttpWebRequest
    {
        public static string GetResponse(string url) {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd().Trim(); 
        }
    }
}
