using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace seeitornot.site.ViewHelpers
{
    using System.Configuration;
    using System.Web.Mvc;

    public static class SiteHelper
    {
        public static string GetVersionedUrl(this UrlHelper urlhelper, string relativeurl)
        {
            string rtn = urlhelper.Content(relativeurl);
            string version = data.Constants.AssemblyVersion;

            if (!string.IsNullOrWhiteSpace(relativeurl) && !string.IsNullOrWhiteSpace(version))
            {
                rtn += string.Format("?v={0}", version);
            }

            return rtn;
        }
    }
}