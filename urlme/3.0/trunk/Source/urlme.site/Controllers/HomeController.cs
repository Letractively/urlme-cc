using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace urlme.site.Controllers
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            var links = new List<urlme.data.Models.Link>();
            if (Request.IsAuthenticated)
            {
                links = data.Models.Link.GetByUserId(User.Identity.Name.UserId());
            }
            return View(links);
        }
    }
}