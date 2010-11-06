using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Mvc.Html;

namespace urlme.Site.Helpers
{
    public static class HtmlHelpers
    {
        public static UrlHelper UrlHelper(this HtmlHelper helper)
        {
            return new UrlHelper(helper.ViewContext.RequestContext);
        }
        
        public static string ResolveClientUrl(this HtmlHelper helper, string url)
        {
            return HtmlHelpers.UrlHelper(helper).Content(url);
        }

        public static string BuildUserBox(this HtmlHelper helper)
        {
            StringBuilder html = new StringBuilder();
            if (Model.User.Current.IsAuthenticated)
            {
                html.AppendFormat("Welcome, <a href=\"{0}\">{1}</a>&nbsp;<span class=\"separator\">|</a>&nbsp;<a href=\"{2}\">Sign out</a>", string.Empty, Model.User.Current.Email, ResolveClientUrl(helper, "~/account/signout/"));
            }
            else
            {
                html.Append("hi there user box");    
            }
            return html.ToString();
        }

        public static string MakeSnippet(this HtmlHelper helper, string input, int threshold)
        {
            if (input.Length > threshold)
                return input.Substring(0, threshold) + "...";
            else
                return input;
        }
    }
}