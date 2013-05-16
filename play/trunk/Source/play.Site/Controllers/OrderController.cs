﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace play.Site.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCount()
        {
            return this.Json(new { count = Models.PlayOrder.Get().Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            return this.Json(Models.PlayOrder.Get(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Paypal_Ipn()
        {
            string custom = Request.Form["custom"];
            if (custom == "donation")
            {
                Mail.SendToShari(string.Format("Someone clicked 'Just Donate' button, amount of {0} !", Request.Form["amount"]), "Booyahhh", true);
                return Content("");
            }

            int orderId = int.Parse(Request.Form["custom"]);
            int result = Models.PlayOrder.MarkAsPaid(orderId);
            if (result == -1)
            {
                Models.Log.Save("Error in marking as paid; orderId = " + orderId + ".");
                Mail.SendToShari("Error in marking as paid; orderId = " + orderId + ".", "Error :/  Ian is CC'ed on this, so he'll take a looksies.", true);
                return Content("");
            }

            var order = Models.PlayOrder.Get(orderId);

            string ticketDisplay = GetTicketDisplay(order);

            // send good news to Shari
            string body = string.Format("{0} ({1}) ordered {2} for {3}!<br/><br/><a href='http://cocoscoffeeshop.com/admin'>Go to admin</a>.", order.Name, order.Email, ticketDisplay, order.PlayDate.ToString("MMM dd"));
            Mail.SendToShari(string.Format("Order for {0} !", order.PlayDate.ToString("MMM dd")), body, false, true);

            // send order conf to payer
            string orderConf = string.Format("Dear {0},<br/><br/>Thank you for your order of {1}! You can pick up your ticket(s) at will call on <b>{2}</b>. Please try and arrive at <b>Commonwealth Chapel</b> (1836 Park Ave, Richmond VA) close to <b>6pm</b> so you can be seated.<br/><br/>Thanks!<br/>- Shari Davis", order.Name, ticketDisplay, order.PlayDate.ToString("MMM dd, yyyy"));
            Mail.Send(order.Email, order.Name, "Crazy Capers Dinner Theater Order Confirmation", orderConf, false);

            // SEND A TXT !!1
            string txt = string.Format("{0} ordered {1} for {2}! http://urlme.cc/admin", order.Name, ticketDisplay, order.PlayDate.ToString("MMM dd"));
            Sms.SendToTeamDavis(txt);

            return Content("");
        }

        private string GetTicketDisplay(Models.PlayOrder order) {
            string rtn = "";
            int cpl = order.CoupleTicketCount;
            int ind = order.IndividualTicketCount;
            if (cpl > 0)
            {
                rtn += string.Format("{0} couple ticket{1}", cpl, cpl > 1 ? "s" : "");
            }
            if (ind > 0)
            {
                rtn += string.Format("{0}{1} individual ticket{2}", cpl > 0 ? " and " : "", ind, ind > 1 ? "s" : "");
            }
            return rtn;
        }

        [HttpPost]
        public JsonResult Delete(string secret, int playOrderId)
        {
            bool success = Models.PlayOrder.Delete(secret, playOrderId);
            return this.Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BlastReminder(int day)
        {
            
            return Content("Sent, done and done-sies.");
        }

        [HttpPost]
        public JsonResult SendConfirmation(int playOrderId)
        {
            var order = Models.PlayOrder.Get(playOrderId);
            var ticketDisplay = GetTicketDisplay(order);
            bool success = Models.PlayOrder.SendConfirmation(order, ticketDisplay);
            return this.Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Submit(Models.PlayOrder order)
        {
            if (order.Name.ToLower() == "test")
            {
                order.Email = System.DateTime.Now.Ticks + "@test.com";
                order.Name = "test"; // lower-case it incase it's not already
            }

            int orderId = Models.PlayOrder.Save(order);
            return this.Json(new { orderId = orderId }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
