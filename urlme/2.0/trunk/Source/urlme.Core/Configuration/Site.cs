using System;
using System.Data;
using System.Xml;
using System.Web;
using System.Web.Caching;
using System.Text.RegularExpressions;

namespace urlme.Core.Configuration
{
    public class Site
    {

        public static string ApplicationPath
        {
            get
            {
                string s = ConfigurationManager.Instance.AppSettings["ApplicationPath"];
                if (s.EndsWith("/"))
                    s = s.Substring(0, s.Length - 1);
                return s.ToLower();
            }
        }

        public static bool AdsVisible
        {
            get
            {
                bool ret = true;
                bool.TryParse(ConfigurationManager.Instance.AppSettings["AdsVisible"] ?? "true", out ret);
                return ret;
            }
        }

        public static string TakeOverConnectionString
        {
            get
            {
                if (Core.Configuration.ConfigurationManager.Instance.ConnectionStrings["TakeOverConnectionString"] == null)
                    return "";
                return Core.Configuration.ConfigurationManager.Instance.ConnectionStrings["TakeOverConnectionString"].ConnectionString;
            }
        }

        public static bool CaptureGoogleAnalytics
        {
            get
            {
                bool ret = true;
                bool.TryParse(ConfigurationManager.Instance.AppSettings["CaptureGoogleAnalytics"] ?? "true", out ret);
                return ret;
            }
        }

        public static string HostName
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["HostName"];
            }
        }

        public static bool EnableTakeover
        {
            get
            {
                bool ret = true;
                bool.TryParse(ConfigurationManager.Instance.AppSettings["EnableTakeover"] ?? "true", out ret);
                return ret;
            }
        }

        public static bool ShowTwitterWidget
        {
            get
            {
                bool ret = true;
                bool.TryParse(ConfigurationManager.Instance.AppSettings["ShowTwitterWidget"] ?? "true", out ret);
                return ret;
            }
        }

        public static bool UseCombinedJavascript
        {
            get
            {
                bool ret = true;
                bool.TryParse(ConfigurationManager.Instance.AppSettings["UseCombinedJavascript"] ?? "true", out ret);
                return ret;
            }
        }
    }
}
