using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

namespace Library.Configuration
{
    public class Site
    {
        public static string Env
        {
            get {
                // enhancement opp: use caching
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnvironmentOverride"]))
                {
                    string httpHostToLower = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower();
                    foreach (string environmentHttpHostToLowerContains in ConfigurationManager.AppSettings["EnvironmentHttpHostToLowerContains"].Split(new char[] { '|' }))
                    {
                        // enhancement opp: allow for comma-separated http host contains for 1 environment, e.g., dev^dev02,dev03
                        string env = environmentHttpHostToLowerContains.Split(new char[] { '^' })[0];
                        string httpHostToLowerContains = environmentHttpHostToLowerContains.Split(new char[] { '^' })[1];
                        if (httpHostToLower.Contains(httpHostToLowerContains))
                            return env;
                    } // next environmentHttpHostToLowerContains pairing
                    return null;
                }
                else
                {
                    return ConfigurationManager.AppSettings["EnvironmentOverride"];
                }
            }
        }

        public static string Logo
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteLogo"];
            }
        }
        public static string UrlNoEndingSlash
        {
            get
            {
                return ConfigurationManager.AppSettings[string.Format("{0}_{1}",Env,"UrlNoEndingSlash")];
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
