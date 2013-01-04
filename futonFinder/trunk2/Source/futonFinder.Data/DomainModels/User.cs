using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using futonFinder.Data;

namespace futonFinder.Data.DomainModels
{
    public class User
    {
        private const string AuthenticationCookieName = "User";

        #region Properties
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsAuthenticated { get { return (this.UserId != null); } }
        #endregion

        #region Constructors
        private User(int userId, string email, string userName)
        {
            this.UserId = userId;
            this.Email = email;
            this.UserName = userName;
        }
        private User()
        {
            this.UserId = null;
        }
        #endregion

        #region Public Static Methods
        public static void IssueAuthTicket(string email, bool rememberMe)
        {
            int userId = -1;
            string userName = null;
            futonFinder.Data.User user = new futonFinder.Data.User();
            using (bd13DataContext db = new futonFinder.Data.bd13DataContext())
            {
                user = db.Users.Where(x => x.Email == email).SingleOrDefault();
                if (user == null)
                {
                    user = new futonFinder.Data.User();
                    user.Email = email;
                    user.CreateDate = System.DateTime.Now;
                    db.Users.InsertOnSubmit(user);
                    db.SubmitChanges();
                }
            }

            SetAuthenticationCookie(user, rememberMe);
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
            User.RemoveAuthenticationCookie();

            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session.Abandon();
        }

        public static User Current
        {
            get
            {
                string contextKey = "User";
                User ret = HttpContext.Current.Items[contextKey] as User;

                if (ret == null)
                {
                    // check to see if it's in cookie
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[AuthenticationCookieName];
                    if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                        cookie = HttpContext.Current.Response.Cookies[AuthenticationCookieName];

                    if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                    {
                        // parse user properties from cookie
                        int? userId = null;
                        string email = null;
                        string userName = null;
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
                            foreach (string nameValue in ticket.UserData.Split('&'))
                            {
                                string name = nameValue.Split('=')[0];
                                string value = nameValue.Split('=')[1];
                                switch (name)
                                {
                                    case "UserId":
                                        userId = int.Parse(value);
                                        break;
                                    case "Email":
                                        email = value;
                                        break;
                                    case "UserName":
                                        userName = value;
                                        break;
                                }
                            }
                            ret = new User((int)userId, email, userName);
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
        #endregion

        private static void SetAuthenticationCookie(futonFinder.Data.User user, bool rememberMe)
        {
            string cookieValue = string.Format("UserId={0}&Email={1}&UserName={2}", user.UserId, user.Email, user.UserName);
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
    }
}
