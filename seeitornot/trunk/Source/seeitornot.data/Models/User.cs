using System;
using System.Collections.Generic;
using System.Linq;
using ianhd.core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using ianhd.core.Data;
using System.Web.Security;
using System.Web;

namespace seeitornot.data.Models
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

        public static User Current
        {
            get
            {
                string contextKey = AuthenticationCookieName; // use same
                var ret = HttpContext.Current.Items[contextKey] as User;

                if (ret == null)
                {
                    // check to see if it's in cookie
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[AuthenticationCookieName];
                    if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                        cookie = HttpContext.Current.Response.Cookies[AuthenticationCookieName];

                    if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                    {
                        // parse user properties from cookie
                        int userId = 0;
                        string name = null;
                        FormsAuthenticationTicket ticket = null;
                        try
                        {
                            ticket = FormsAuthentication.Decrypt(cookie.Value);
                        }
                        catch
                        {
                            User.RemoveAuthenticationCookie();
                            return new User();
                        }

                        if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                        {
                            var userData = ticket.UserData;
                            var parts = userData.Split('^');
                            userId = int.Parse(parts[0]);
                            name = parts[1];

                            ret = new User { UserId = userId, Name = name };
                            HttpContext.Current.Items[contextKey] = ret;
                        }
                    }
                }

                if (ret == null)
                {
                    // return a nulled-out user, so u.IsAuthenticated returns False instead of object not found exception
                    return new User();
                }

                return ret;
            }
        }

        public static void IssueAuthTicket(string name, bool rememberMe)
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
            string cookieValue = string.Format("{0}^{1}", userId, email);
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
