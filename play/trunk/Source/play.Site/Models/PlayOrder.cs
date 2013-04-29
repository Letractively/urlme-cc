using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace play.Site.Models
{
    public partial class PlayOrder
    {
        public static int Save(Models.PlayOrder playOrder) {
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
            if (secret.ToLower() != "oblivion") {
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