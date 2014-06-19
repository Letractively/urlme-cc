using System.IO;
using System.Net;

namespace movies.Core.Net
{
    public class HttpWebRequest
    {
        public static string GetResponse(string url) {
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return reader.ReadToEnd().Trim(); 
            }
            catch
            {
                return null;
            }
        }
    }
}
