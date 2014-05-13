using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace urlme.site.Controllers
{
    public class RedirectController : Controller
    {
        //
        // GET: /Redirect/
        public ActionResult Index(string path)
        {
            var link = urlme.data.Models.Link.Get(path);
            if (link != null)
            {
                return Redirect(link.DestinationUrl);
            }

            return Content(string.Format("http://urlme.cc/{0} does not exist", path));
        }
	}
}