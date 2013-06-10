using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ianhd.data;

namespace ianhd.admin.ViewModels
{
    public class TaglineIndex : ianhd.core.Mvc.ViewModels.ViewModelBase
    {
        public List<Tagline> Taglines { get; set; }
        public Tagline NewTagline { get; set; }
    }
}