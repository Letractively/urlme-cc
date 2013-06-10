using System.Web.Mvc;
using ianhd.core.Mvc.ViewModels;

namespace ianhd.core.Mvc.Controllers
{
    public class BaseController : System.Web.Mvc.Controller
    {
        public string OpenGraphTitle { get; set; }
        public string OpenGraphImage { get; set; }
        public string OpenGraphDescription { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var baseController = filterContext.Controller as BaseController;
            var baseModel = baseController.ViewData.Model as ViewModelBase;

            // Assign site to base view model.
            if (baseModel != null)
            {
                baseModel.OpenGraphTitle = baseController.OpenGraphTitle;
                baseModel.OpenGraphImage = baseController.OpenGraphImage;
                baseModel.OpenGraphDescription = baseController.OpenGraphDescription;
            }

            base.OnActionExecuted(filterContext);
        }

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    // Site Lookup
        //    if (Site.Current == null)
        //    {
        //        Site.Current = this.FindSite(filterContext.HttpContext);
        //    }

        //    // Get our CMS Site
        //    if (this.CmsSite == null)
        //    {
        //        this.CmsSite = CMS.API.Site.GetSiteById(Site.Current.SiteCode);
        //    }

        //    base.OnActionExecuting(filterContext);
        //}        
    }
}
