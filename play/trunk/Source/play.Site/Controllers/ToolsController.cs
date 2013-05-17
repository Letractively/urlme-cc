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

        public JsonResult Txt(bool soon, string category, string onOff, string to)
        {
            to = to.Replace("-", "");
            to = to.Trim();

            category = category.ToLower().Trim() == "house" ? "House" : "Stage";

            string txt = string.Format("{0}{1} {2}{3}", soon ? "Soon... " : "", category, onOff.Trim().ToUpper(), !soon ? " - Now please" : "");
            bool success = Sms.Send(to, txt);

            return this.Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }
    }
}
