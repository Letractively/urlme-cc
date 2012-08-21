using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using movies.SiteMvc4.Models;

namespace movies.SiteMvc4
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            // google "Live Connect" to know how to manage apps with Microsoft, or https://manage.dev.live.com/Applications/Index
            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "000000004C0C893E",
            //    clientSecret: "BduRzUtHKSIURBJvmFg6yG9D7Dnl9OnT");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "0UdIWMBBydWKjNHruzxufg",
                consumerSecret: "KLRyDDXlVL4iEGjUoJji87Gtcpx7yEBQcpZnCzFYClo");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "118119081645720",
                appSecret: "010d3720c542904558b8463339ec71e0");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
