namespace urlme.Utils.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web;
    using System.Text.RegularExpressions;
    using System.Web.Routing;

    public static class HtmlHelperExtensions
    {
        #region Fields
        /// <summary>
        /// The format to use in constructing a fully qualified url
        /// </summary>
        private const string UrlFormat = "http{0}://{1}{2}";

        /// <summary>
        /// The difference between http and https is the letter s, this allows the variable to only exist once.
        /// </summary>
        private const string HttpsExtraCharacter = "s";

        /// <summary>
        /// The separator indicating the being of a query string
        /// </summary>
        private const char QueryStringSeparator = '?';

        /// <summary>
        /// The trailing slash
        /// </summary>
        private const string TrailingCharacter = "/";

        /// <summary>
        /// The (regular) expression for matching on the trailing slash
        /// </summary>
        private const string TrailingSlashExpression = @"^(?:.*[^/]\.[a-zA-Z0-9]{0,4})$|^(?:.*\/)$";
        #endregion

        /// <summary>
        /// Gets the url helper for this helper context
        /// </summary>
        /// <param name="helper">the helper to extend</param>
        /// <returns>the url helper</returns>
        public static UrlHelper UrlHelper(this HtmlHelper helper)
        {
            return new UrlHelper(helper.ViewContext.RequestContext);
        }

        /// <summary>
        /// Resolves the url for the client
        /// </summary>
        /// <param name="helper">the helper to extend</param>
        /// <param name="url">the url to resolve</param>
        /// <returns>the resolved url</returns>
        public static string ResolveClientUrl(this HtmlHelper helper, string url)
        {
            // for resolving urls with ~
            return string.Format("http{0}://{1}{2}", (helper.UrlHelper().RequestContext.HttpContext.Request.IsSecureConnection) ? "s" : string.Empty, helper.UrlHelper().RequestContext.HttpContext.Request.Url.Host.ToLower(), helper.UrlHelper().Content(url)).ToLower();
        }

        /////// <summary>
        /////// Provides a route with a trailing slash when the route should end in a path
        /////// </summary>
        /////// <param name="helper">The <see cref="HtmlHelper"/> to extend.</param>
        /////// <param name="routeValues">The values to pass to the route.</param>
        /////// <returns>the resolved url</returns>
        ////public static string RouteProperUrl(this HtmlHelper helper, object routeValues)
        ////{
        ////    return helper.UrlHelper().RouteProperUrl(routeValues);
        ////}

        /////// <summary>
        /////// Provides a route with a trailing slash when the route should end in a path
        /////// </summary>
        /////// <param name="helper">The <see cref="UrlHelper"/> to extend.</param>
        /////// <param name="routeValues">The values to pass to the route.</param>
        /////// <returns>the resolved url</returns>
        ////public static string RouteProperUrl(this UrlHelper helper, object routeValues)
        ////{
        ////    return helper.RouteProperUrl(helper.RouteUrl(routeValues));
        ////}


        /// <summary>
        /// Provides a route with a trailing slash when the route should end in a path
        /// </summary>
        /// <param name="helper">The <see cref="HtmlHelper"/> to extend.</param>
        /// <param name="routeValues">The values to pass to the route.</param>
        /// <returns>the resolved url</returns>
        public static string RouteProperUrl(this HtmlHelper helper, RouteValueDictionary routeValues)
        {
            return helper.UrlHelper().RouteProperUrl(routeValues);
        }

        /// <summary>
        /// Provides a route with a trailing slash when the route should end in a path
        /// </summary>
        /// <param name="helper">The <see cref="UrlHelper"/> to extend.</param>
        /// <param name="routeValues">The values to pass to the route.</param>
        /// <returns>the resolved url</returns>
        public static string RouteProperUrl(this UrlHelper helper, RouteValueDictionary routeValues)
        {
            return helper.RouteProperUrl(helper.RouteUrl(routeValues));
        }

        /// <summary>
        /// Single method to handle adding the trailing slash no matter which routeproperurl is used to generate the url
        /// </summary>
        /// <param name="helper">The <see cref="UrlHelper"/> to extend.</param>
        /// <param name="url">The url to ensure we have a valid value for.</param>
        /// <returns>The properly formatted url.</returns>
        public static string RouteProperUrl(this UrlHelper helper, string url)
        {
            url = string.Format(
                        HtmlHelperExtensions.UrlFormat, // "http{0}://{1}{2}", 
                            helper.RequestContext.HttpContext.Request.IsSecureConnection ? HtmlHelperExtensions.HttpsExtraCharacter : string.Empty,
                            helper.RequestContext.HttpContext.Request.Url.Host.ToLower(),
                            url);

            string path = url.Split(HtmlHelperExtensions.QueryStringSeparator)[0];

            if (!Regex.IsMatch(path, HtmlHelperExtensions.TrailingSlashExpression))
            {
                url = Regex.Replace(url, path, path + HtmlHelperExtensions.TrailingCharacter); // two string concatenated is faster than any other solution
            }

            url = Regex.Replace(url, path, path.ToLower());

            return url;
        }

        /// <summary>
        /// 302 redirects the client to a new url
        /// </summary>
        /// <param name="helper">the helper to extend</param>
        /// <param name="url">the url to resolve and direct to</param>
        public static void RedirectToUrl(this HtmlHelper helper, string url)
        {
            helper.ViewContext.Controller.ControllerContext.RequestContext.HttpContext.Response.Redirect(helper.ResolveClientUrl(url));
        }

        public static string GetReturnUrl(this HtmlHelper helper, string ignorePattern)
        {
            return helper.UrlHelper().GetReturnUrl(ignorePattern);
        }

        public static string GetReturnUrl(this UrlHelper helper, string ignorePattern)
        {
            return helper.RequestContext.HttpContext.GetReturnUrl(ignorePattern);
        }

        public static string GetReturnUrl(this HttpContextBase context, string ignorePattern)
        {
            string ret = null;

            HttpRequestBase request = context.Request;

            ret = Regex.Replace(request.Url.OriginalString.ToLower(), ":80/", "/");

            if (!string.IsNullOrEmpty(request["returnurl"]))
            {
                ret = request["returnurl"];
            }

            if (ignorePattern != null && Regex.IsMatch(ret, ignorePattern))
            {
                ret = null;
            }

            if (ret != null)
            {
                ret = ret.ToLower();
            }

            return ret;
        }
    }
}
