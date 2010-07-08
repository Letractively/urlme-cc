using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UrlMe.cc
{
    public partial class Router : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = Request.QueryString["path"];
            if (path.Substring(path.Length-1, 1) == "/")
                path = path.Substring(0, path.Length - 1); // get rid of trailing "/"

            if (path.ToLower() == "urlme") {
                Response.Write("You're an asshole.");
            } else {
                string destinationUrl = Library.Data.LinkData.LookupPath(path);
                if (!String.IsNullOrEmpty(destinationUrl)){
                    // Response.Write(String.Format("Redirecting to <a href=\"{0}\">{0}</a> ...<br/><br/>Create your own urlme.cc redirect link at <a href=\"http://urlme.cc\">http://urlme.cc</a>.",destinationUrl));
                    // Response.AddHeader("REFRESH",string.Format("{0};URL={1}", Library.Configuration.Site.RedirectDelaySeconds, destinationUrl));
                    Response.Redirect(destinationUrl);
                } else {
                    Response.Write("This path does not exist in the lookup table.");
                }
            }
        }
    }
}
