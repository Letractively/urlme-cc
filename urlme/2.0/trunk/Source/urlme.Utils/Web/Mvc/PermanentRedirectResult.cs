using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace urlme.Utils.Web.Mvc
{
    public class PermanentRedirectResult : ActionResult
    {
        private string _url { get; set; }

        public PermanentRedirectResult(string url)
        {
            _url = url;
        }

        #region Overrides of ActionResult

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Status = "301 Moved Permanently";
            context.HttpContext.Response.StatusCode = 301;
            context.HttpContext.Response.AddHeader("Location", _url);
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }

        #endregion
    }
}
