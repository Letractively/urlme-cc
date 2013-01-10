using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using futonFinder.Core.Extensions;
using System.Web.Mvc;

namespace futonFinder.Site.Models
{
    public class Facebook
    {
        public string Facebook_GraphAPI_Token = "https://graph.facebook.com/oauth/access_token?";
        public string Facebook_GraphAPI_Me = "https://graph.facebook.com/me?";
        public string AppID = "314488141986761";
        public string AppSecret = "998f3cd53ee4af3420734d8ee386ce3c";
        
        public IDictionary<string, string> GetUserData(string accessCode, string redirectURI)
        {
            string token = futonFinder.Core.Net.HttpWebRequest.GetResponse(Facebook_GraphAPI_Token + "client_id=" + AppID + "&redirect_uri=" + HttpUtility.HtmlEncode(redirectURI) + "%3F__provider__%3Dfacebook" + "&client_secret=" + AppSecret + "&code=" + accessCode);
            if (token == null || token == "")
            {
                return null;
            }
            string data = futonFinder.Core.Net.HttpWebRequest.GetResponse(Facebook_GraphAPI_Me + "fields=id,name,email,username,gender,link&access_token=" + token.Substring("access_token=", "&"));

            // this dictionary must contains
            var userData = new Dictionary<string, string>();
            userData.Add("id", data.Substring("\"id\":\"", "\""));
            userData.Add("username", data.Substring("username\":\"", "\""));
            userData.Add("name", data.Substring("name\":\"", "\""));
            userData.Add("link", data.Substring("link\":\"", "\"").Replace("\\/","/"));
            userData.Add("gender", data.Substring("gender\":\"", "\""));
            userData.Add("email", data.Substring("email\":\"", "\"").Replace("\\u0040", "@"));
            userData.Add("accesstoken", token.Substring("access_token=", "&"));
            return userData;
        }
    }
}