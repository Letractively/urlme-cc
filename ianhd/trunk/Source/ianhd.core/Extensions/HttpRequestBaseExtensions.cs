using System.Web;

namespace ianhd.core.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        public static string GetPlatform(this HttpRequestBase request, string platFormOverrideCookieName)
        {
            // figure out mobile vs. desktop
            var rtn = request.UserAgent.ToString().ToLower().Contains("mobi") ? "m" : "dt";
            string platformOverride = request.Cookies[platFormOverrideCookieName] == null ? null : request.Cookies[platFormOverrideCookieName].Value;
            if (platformOverride != null)
            {
                rtn = platformOverride;
            }

            return rtn;
        }
    }
}
