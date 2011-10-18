using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace urlme.Utils.Web.Mvc
{
    public class MethodNotAllowedResult : ActionResult
    {
        public MethodNotAllowedResult()
        { }

        #region Overrides of ActionResult

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusDescription = "HTTP/1.1 405 Method Not Allowed";
            context.HttpContext.Response.Status = "405 Method Not Allowed";
            context.HttpContext.Response.StatusCode = 405;
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }

        #endregion
    }
}