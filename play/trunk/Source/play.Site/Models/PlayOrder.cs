using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace play.Site.Models
{
    public partial class PlayOrder
    {
        public static int Save(Models.PlayOrder playOrder)
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    playOrder.CreateDate = System.DateTime.Now;
                    playOrder.Paid = false;
                    ctx.PlayOrders.InsertOnSubmit(playOrder);
                    ctx.SubmitChanges();
                    return playOrder.PlayOrderId;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool SendConfirmation(Models.PlayOrder order, string ticketDisplay)
        {
            // send order conf to payer
            string orderConf = string.Format("Dear {0},<br/><br/>Thank you for your order of {1}! We will see you on <b>{2}</b>. Please try and arrive at <b>Commonwealth Chapel</b> (1836 Park Ave, Richmond VA) close to <b>6pm</b> so you can be seated.<br/><br/>Thanks!<br/>- Shari Davis", order.Name, ticketDisplay, order.PlayDate.ToString("MMM dd, yyyy"));
            Mail.Send(order.Email, order.Name, "Crazy Capers Dinner Theater Order Confirmation", orderConf, false, false, true);

            return true;
        }

        public class Summary
        {
            public int SeatCount { get; set; }
            public int OrderTotal { get; set; }
            public int FriSeatCount { get; set; }
            public int FriOrderTotal { get; set; }
            public int SatSeatCount { get; set; }
            public int SatOrderTotal { get; set; }
            public string SummaryText
            {
                get
                {
                    return string.Format("Friday {0} seats, Saturday {1} seats, Total {2} seats for ${3}. Reply '?' for more commands.", this.FriSeatCount, this.SatSeatCount, this.SeatCount, this.OrderTotal);
                }
            }

            public Summary(List<Models.PlayOrder> orders)
            {
                int seatCount = 0, friSeatCount = 0, satSeatCount = 0;
                int orderTotal = 0, friOrderTotal = 0, satOrderTotal = 0;
                foreach (var order in orders)
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

                this.SeatCount = seatCount;
                this.OrderTotal = orderTotal;
                this.FriOrderTotal = friOrderTotal;
                this.FriSeatCount = friSeatCount;
                this.SatOrderTotal = satOrderTotal;
                this.SatSeatCount = satSeatCount;
            }
        }

        public static Summary SummaryGet()
        {
            var orders = Get();
            return new Summary(orders);
        }

        public static List<Models.PlayOrder> Get()
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = false })
                {
                    var orders = ctx.PlayOrders.Where(x => x.Name != "test" && x.Name != "Test").OrderBy(x => x.CreateDate).ToList();
                    return orders;
                }
            }
            catch
            {
                return null;
            }
        }

        public static Models.PlayOrder Get(int playOrderId)
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = false })
                {
                    return ctx.PlayOrders.FirstOrDefault(x => x.PlayOrderId == playOrderId);
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool Delete(string secret, int playOrderId)
        {
            if (secret.ToLower() != "oblivion")
            {
                return false;
            }

            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    var playOrder = ctx.PlayOrders.FirstOrDefault(x => x.PlayOrderId == playOrderId);
                    ctx.PlayOrders.DeleteOnSubmit(playOrder);
                    ctx.SubmitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static int MarkAsReminded(int playOrderId)
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    var playOrder = ctx.PlayOrders.FirstOrDefault(x => x.PlayOrderId == playOrderId);
                    playOrder.ReminderSent = true;
                    playOrder.ModifyDate = System.DateTime.Now;
                    ctx.SubmitChanges();
                    return playOrderId;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static int MarkAsPaid(int playOrderId)
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    var playOrder = ctx.PlayOrders.FirstOrDefault(x => x.PlayOrderId == playOrderId);
                    playOrder.Paid = true;
                    playOrder.ModifyDate = System.DateTime.Now;
                    ctx.SubmitChanges();
                    return playOrderId;
                }
            }
            catch
            {
                return -1;
            }
        }
    }
}