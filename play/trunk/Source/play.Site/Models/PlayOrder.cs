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
                    playOrder.Status = "Pending";
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

        public static int MarkAsPaid(int playOrderId)
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    var playOrder = ctx.PlayOrders.FirstOrDefault(x => x.PlayOrderId == playOrderId);
                    playOrder.Status = "Paid";
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