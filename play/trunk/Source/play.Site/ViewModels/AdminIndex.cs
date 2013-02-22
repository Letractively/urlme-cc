using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace play.Site.ViewModels
{
    public class AdminIndex
    {
        public List<Models.PlayOrder> Orders { get; set; }
        public Models.PlayOrder Order { get; set; }
    }
}