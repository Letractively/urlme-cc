using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace play.Site.Views.Tools
{
    public class ToolsController : Controller
    {
        //
        // GET: /Tools/

        public ActionResult Director()
        {
            return View();
        }

        public class Alert {
            public string category { get; set; }
            public string onOff { get; set; }
            public string to { get; set; }
            public string message { get; set; }
        }

        // ?soon=false&category=house&onOff=off&to=555
        public JsonResult Txt(bool soon, string category, string onOff, string to)
        {
            to = to.Replace("-", "");
            to = to.Trim();

            category = category.ToLower().Trim();
            if (category == "house")
            {
                category = "House";
            }
            else if (category == "spot")
            {
                category = "Spot";
            }
            else
            {
                category = "Stage";
            }

            string soonStr = soon ? "Soon... " : "";
            onOff = onOff.Trim().ToUpper();
            onOff = onOff == "ON" ? "UP" : "DOWN";
            string postfix = !soon ? " - Now please" : "";

            string msg = string.Format("{0}{1} {2}{3}", soonStr, category, onOff, postfix);

            var alert = new Alert()
            {
                category = category,
                message = msg,
                onOff = onOff,
                to = to
            };

            Utils.Caching.Cache.Store("alert", alert);

            // bool success = Utils.Sms.Send(to, msg);
            bool success = alert != null;

            return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
