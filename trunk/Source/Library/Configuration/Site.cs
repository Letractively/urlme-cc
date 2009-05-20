using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Library.Configuration
{
    public class Site
    {
        public static string Logo
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteLogo"];
            }
        }
        public static string ApplicationUrlRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplicationUrlRoot"];
            }
        }
        public static int RedirectDelaySeconds
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["RedirectDelaySeconds"]);
            }
        }
        public static int SiteUpdateNewNoticeDaySpan
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["SiteUpdateNewNoticeDaySpan"]);
            }
        }
    }
}
