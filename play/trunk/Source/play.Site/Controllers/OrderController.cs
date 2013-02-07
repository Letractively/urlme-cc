using System;
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

        [HttpPost]
        public ActionResult Paypal_Ipn()
        {
            int orderId = int.Parse(Request.Form["custom"]);
            int result = Models.PlayOrder.MarkAsPaid(orderId);
            if (result == -1)
                Models.Log.Save("Error in marking as paid; orderId = " + orderId + ".");

            var order = Models.PlayOrder.Get(orderId);
            string body = string.Format("{0} {1} has paid for his/her order of {2} couple ticket(s), {3} indiv. ticket(s) for {4} !<br/><br/><a href='http://cocoscoffeeshop.com/admin'>Go to admin</a>.", order.Name, order.Email, order.CoupleTicketCount, order.IndividualTicketCount, order.PlayDate.ToString("MMM dd"));
            Mail.SendToShari(string.Format("Order for {0} !", order.PlayDate.ToString("MMM dd")), body, true); 

            return null;
        }

        [HttpPost]
        public JsonResult ToggleSeated(int playOrderId)
        {
            bool success = Models.PlayOrder.ToggleSeated(playOrderId);
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

        [HttpGet]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
