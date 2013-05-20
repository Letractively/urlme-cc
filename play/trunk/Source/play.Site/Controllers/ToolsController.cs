using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [HttpGet]
        public ActionResult Mail(string template = null)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(template))
            {
                return Content("Error. Please provide template code, e.g., ?template=FinalAmount");
            }

            switch (template.ToLower())
            {
                case "finalamount":
                    sb.Append("Dear {NAME},<br/>");
                    sb.Append("<br/>");
                    sb.Append("We have the answer to The Question! <i>So, how much was raised from this weekend?</i><br/>");
                    sb.Append("<br/>");
                    sb.Append("<span style='font-weight:bold; color: blue;'>$3,643.41</span> net proceeds going directly into the Abigail Grace Adoption Fund!<br/>");
                    sb.Append("<br/>");
                    sb.Append("Thanks to you, Heather and Benson are that much closer to meeting their future daughter. Check out <a href='http://abigailsadoption.blogspot.com/'>their Blog</a> for updates on their adoption, and for news on future events. Please contact them if you have any ideas. Thanks!");
                    break;
                default:
                    break;
            }

            if (string.IsNullOrWhiteSpace(sb.ToString()))
            {
                return Content("Error. Please provide a valid template code, e.g., ?template=FinalAmount");
            }

            var vm = new ViewModels.ToolsMail
            {
                Body = sb.ToString()
                , Template = template
                , AdHocs = Models.AdHoc.Get("tools-mail-" + template)
            };

            return View(vm);

            //var orders = play.Site.Models.PlayOrder.Get();

            //foreach (var order in orders)
            //{
            //    string body = sb.ToString().Replace("{NAME}", order.Name);
            //    bool success = Utils.Mail.Send(order.Email.Trim(), order.Name.Trim(), "Net proceeds from the play", body, false, false, false, false, false);
            //}
            //return Content("Sent, done and done-sies.");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Mail(ViewModels.ToolsMail vm)
        {
            if (!vm.ToAllTicketHolders)
            {
                // send to one specific person
                bool notAlreadySent = Models.AdHoc.Save(new Models.AdHoc { Tag = "tools-mail-" + vm.Template, Value = string.Format("{0} - {1}", vm.ToName, vm.ToEmail) });
                if (notAlreadySent)
                {
                    vm.Body = vm.Body.Replace("{NAME}", vm.ToName);
                    bool success = Utils.Mail.Send(vm.ToEmail, vm.ToName, "Net proceeds from the play", vm.Body, false, false, false, false, true);
                    return Redirect("~/tools/mail?template=" + vm.Template);
                }
            }            

            return Redirect("~/tools/mail?template=" + vm.Template);
        }

        public class Alert
        {
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
