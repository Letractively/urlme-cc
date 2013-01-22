using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;

namespace futonFinder.Site
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "oubhYrc5iKo7G3EVyqKRng",
                consumerSecret: "hBkifIbyM5Tb599obm1jDkVoaNgZE4K29ESZDZd2o");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "314488141986761",
                appSecret: "998f3cd53ee4af3420734d8ee386ce3c");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
