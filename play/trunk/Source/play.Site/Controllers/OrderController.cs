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
            int orderId = int.Parse(Request.Form["item_id"]);
            int result = Models.PlayOrder.MarkAsPaid(orderId);
            if (result == -1)
                Models.Log.Save("Error in marking as paid; orderId = " + orderId + ".");

            return null;
        }

        [HttpPost]
        public JsonResult Submit(Models.PlayOrder order)
        {
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
