using System.Collections.Generic;
using movies.Model;

namespace movies.Site.ViewModels.Shared
{
    public class PageHeader
    {
        public Enumerations.MobileMenuItems ActiveMenu { get; set; }
        public bool UseAjaxForLinks { get; set; }
        public bool PrefetchLinks { get; set; }
    }
}