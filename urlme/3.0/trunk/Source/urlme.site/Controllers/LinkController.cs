using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urlme.data.Models;

namespace urlme.site.Controllers
{
    [RoutePrefix("link")]
    public class LinkController : Controller
    {
        //
        // GET: /Link/
        [Route("")]
        public JsonResult Get()
        {
            var links = Link.Get(User.Identity.Name.UserId());
            
            // tweak data for front-end
            foreach (var link in links)
            {
                link.DestinationUrl = link.DestinationUrl.Snippet();
            }

            return this.Json(links, JsonRequestBehavior.AllowGet);
        }
	}
}