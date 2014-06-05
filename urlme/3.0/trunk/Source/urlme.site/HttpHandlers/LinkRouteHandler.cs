using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace urlme.site.HttpHandlers
{
    public class LinkRouteHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }
        protected string Path { get; set; }
        protected RequestContext RequestContext { get; set; }

        public LinkRouteHandler(string path, RequestContext requestContext)
        {
            this.Path = path;
            this.RequestContext = requestContext;
        }

        public void ProcessRequest(HttpContext context)
        {
            var link = urlme.data.Models.Link.GetDestinationUrl(this.Path);
            if (link != null)
            {
                context.Response.Redirect(link.DestinationUrl);
            }
            else
            {
                context.Response.ContentType = "text/html";
                context.Response.Write(string.Format("http://urlme.cc/{0} does not exist", this.Path));
            }
        }
    }
}