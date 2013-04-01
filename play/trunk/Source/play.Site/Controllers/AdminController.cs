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

            int seatCount = 0, satSeatCount = 0, sunSeatCount = 0;
            int orderTotal = 0, satOrderTotal = 0, sunOrderTotal = 0;
            foreach (var order in vm.Orders)
            {
                seatCount += (order.CoupleTicketCount * 2) + order.IndividualTicketCount;
                orderTotal += (order.CoupleTicketCount * 60) + (order.IndividualTicketCount * 35);
                if (order.PlayDate.Day == 17)
                {
                    satSeatCount += (order.CoupleTicketCount * 2) + order.IndividualTicketCount;
                    satOrderTotal += (order.CoupleTicketCount * 60) + (order.IndividualTicketCount * 35);
                }
                else
                {
                    sunSeatCount += (order.CoupleTicketCount * 2) + order.IndividualTicketCount;
                    sunOrderTotal += (order.CoupleTicketCount * 60) + (order.IndividualTicketCount * 35);
                }
            }

            vm.SeatCount = seatCount;
            vm.OrderTotal = orderTotal;
            vm.SatOrderTotal = satOrderTotal;
            vm.SatSeatCount = satSeatCount;
            vm.SunOrderTotal = sunOrderTotal;
            vm.SunSeatCount = sunSeatCount;

            return View(vm);
        }

    }
}
