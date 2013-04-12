using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace play.Site.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Send()
        {
            Mail.Send("ihdavis@gmail.com", "Ian Davis", "Test Subject w/ includeIan", "Test body w/ includeIan", true);
            Mail.Send("ihdavis@gmail.com", "Ian Davis", "Test Subject w/ !includeIan", "Test body w/ !includeIan", false);
            return Content("Sent!");
        }
    }
}
