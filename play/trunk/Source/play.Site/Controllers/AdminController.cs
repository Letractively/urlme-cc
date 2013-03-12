using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace play.Site.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            var vm = new play.Site.ViewModels.AdminIndex
            {
                Order = new Models.PlayOrder(),
                Orders = Models.PlayOrder.Get()
            };

            int seatCount = 0;
            int orderTotal = 0;
            foreach (var order in vm.Orders)
            {
                seatCount += (order.CoupleTicketCount * 2) + order.IndividualTicketCount;
                orderTotal += (order.CoupleTicketCount * 60) + (order.IndividualTicketCount * 35);
            }

            vm.SeatCount = seatCount;
            vm.OrderTotal = orderTotal;

            return View(vm);
        }

    }
}
