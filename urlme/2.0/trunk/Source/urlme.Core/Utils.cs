using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Data;
using System.Net;

namespace urlme.Core
{
    public static class Utils
    {
        public static string ResolveUrl(string path)
        {
            if (String.IsNullOrEmpty(path) || path.ToLower().StartsWith("http"))
                return path;

            if (path.StartsWith("~") && HttpContext.Current != null)
            {
                string applicationPath = "";
                if (HttpContext.Current.Request.ApplicationPath.Length > 1)
                    applicationPath = HttpContext.Current.Request.ApplicationPath;
                path = string.Format("http{0}://{1}{2}{3}", (HttpContext.Current.Request.IsSecureConnection) ? "s" : "", HttpContext.Current.Request.Url.Host.ToLower(), applicationPath.ToLower(), path.Remove(0, 1));
            }
            return path;
        }

        public static string UrlWithOutQueryString()
        {
            string path = string.Empty;
            if (HttpContext.Current != null)
            {
                path = HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"];
                path = path.Split('?')[0];
            }

            return path;
        }

        public static string RequestUrl(HttpRequest request)
        {
            string appPath = request.ApplicationPath.ToLower();
            string requestUrl = request.ServerVariables["HTTP_X_REWRITE_URL"] ?? request.Url.PathAndQuery;
            return HttpUtility.UrlDecode(requestUrl);
        }

        public static Control FindControl(Control root, string id)
        {
            if (root.ID == id)
                return root;
            foreach (Control ctl in root.Controls)
            {
                Control t = FindControl(ctl, id);
                if ((t != null))
                    return t;
            }
            return null;
        }

        public static void RedirectPermanent(string url)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = 301;
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", url);
                HttpContext.Current.Response.End();
            }
        }

        public static void RedirectPageNotFound(string url)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = 404;
                HttpContext.Current.Response.Status = "404 Not Found";
                HttpContext.Current.Response.Redirect(url);
            }
        }
    }
}