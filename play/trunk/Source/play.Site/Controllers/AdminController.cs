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

            int seatCount = 0, friSeatCount = 0, satSeatCount = 0;
            int orderTotal = 0, friOrderTotal = 0, satOrderTotal = 0;
            foreach (var order in vm.Orders)
            {
                seatCount += (order.CoupleTicketCount * 2) + order.IndividualTicketCount;
                orderTotal += (order.CoupleTicketCount * 60) + (order.IndividualTicketCount * 35);
                if (order.PlayDate.Day == 17)
                {
                    friSeatCount += (order.CoupleTicketCount * 2) + order.IndividualTicketCount;
                    friOrderTotal += (order.CoupleTicketCount * 60) + (order.IndividualTicketCount * 35);
                }
                else
                {
                    satSeatCount += (order.CoupleTicketCount * 2) + order.IndividualTicketCount;
                    satOrderTotal += (order.CoupleTicketCount * 60) + (order.IndividualTicketCount * 35);
                }
            }

            vm.SeatCount = seatCount;
            vm.OrderTotal = orderTotal;
            vm.FriOrderTotal = friOrderTotal;
            vm.FriSeatCount = friSeatCount;
            vm.SatOrderTotal = satOrderTotal;
            vm.SatSeatCount = satSeatCount;

            return View(vm);
        }

    }
}
