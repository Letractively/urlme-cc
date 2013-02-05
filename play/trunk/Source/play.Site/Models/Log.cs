using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace play.Site.Models
{
    public partial class Log
    {
        public static bool Save(string msg) {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    var log = new Models.Log
                    {
                        CreateDate = System.DateTime.Now,
                        LogType = "Error",
                        Message = msg
                    };
                    ctx.Logs.InsertOnSubmit(log);
                    ctx.SubmitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}