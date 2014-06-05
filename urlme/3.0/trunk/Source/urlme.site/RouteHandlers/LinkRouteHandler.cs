using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace urlme.site.RouteHandlers
{
    public class LinkRouteHandler : IRouteHandler
    {
        protected string Path { get; set; }

        public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var path = requestContext.RouteData.Values["path"] as string;

            return new urlme.site.HttpHandlers.LinkRouteHandler(path, requestContext);
        }
    }
}