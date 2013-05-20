using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace play.Site.ViewModels
{
    public class ToolsMail
    {
        public string Body { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public bool ToAllTicketHolders { get; set; }
        public string Template { get; set; }
        public List<Models.AdHoc> AdHocs { get; set; }
    }
}