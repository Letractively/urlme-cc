using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace urlme.api
{
    public class Google
    {
        public static string ShortenUrl(string longUrl)
        {
            string url = "https://www.googleapis.com/urlshortener/v1/url";
            using (var client = new WebClient())
            {
                var data = new NameValueCollection();
                data["longUrl"] = longUrl;

                var response = client.UploadValues(url, "POST", data);
                return response.ToString();
            }
        }
    }
}
