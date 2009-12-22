using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace UrlMe.cc
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LogOut : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            // System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            HttpContext.Current.Response.Redirect("~/Default.aspx");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
