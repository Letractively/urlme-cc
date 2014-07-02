using System;
using System.Collections.Generic;
using System.Linq;
using ianhd.core.Data.Extensions;
using ianhd.core.Data;
using System.Web.Security;
using System.Web;

namespace urlme.data.Models
{
    public sealed partial class User
    {
        private const string AuthenticationCookieName = "User";

        public static User Get(string email)
        {
            using (var conn = Db.CreateConnection())
            {
                var query = "select * from [ihdavis].[User] where Email=@email";
                var @params = new { email };

                var user = conn.Query<User>(query, @params).FirstOrDefault();
                if (user == null)
                {
                    conn.Execute("insert [ihdavis].[User] (Email) values (@email)", @params);
                    user = conn.Query<User>(query, @params).FirstOrDefault();
                }
                return user;
            }
        }


        public static void IssueAuthTicket(string email, bool rememberMe)
        {
            var userId = Get(email).UserId;

            SetAuthenticationCookie(userId, email, rememberMe);
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
            User.RemoveAuthenticationCookie();

            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session.Abandon();
        }

        #region Helpers
        private static void RemoveAuthenticationCookie()
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[AuthenticationCookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(AuthenticationCookieName);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            // this cookie expires in the PAST! it was featured on an episode of LOST.
            cookie.Expires = System.DateTime.Now.AddYears(-1);
        }
        private static void SetAuthenticationCookie(int userId, string email, bool rememberMe)
        {
            string cookieValue = string.Format("UserId={0}&Email={1}", userId, email);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, AuthenticationCookieName, System.DateTime.Now, System.DateTime.Now.AddDays(30), rememberMe, cookieValue);
            HttpCookie cookie = new HttpCookie(AuthenticationCookieName);
            cookie.Value = FormsAuthentication.Encrypt(ticket);
            if (rememberMe)
            {
                DateTime cookieExpiry = System.DateTime.Now.AddDays(30);
                if (cookieExpiry <= System.DateTime.Now)
                {
                    // we were about to drop an expired cookie
                    cookieExpiry = System.DateTime.Now.AddDays(7);
                }

                cookie.Expires = cookieExpiry;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion

    }
}
