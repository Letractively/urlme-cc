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
        public int SeatCount { get; set; }
        public int OrderTotal { get; set; }
        public int SatSeatCount { get; set; }
        public int SunSeatCount { get; set; }
        public int SatOrderTotal { get; set; }
        public int SunOrderTotal { get; set; }
    }
}