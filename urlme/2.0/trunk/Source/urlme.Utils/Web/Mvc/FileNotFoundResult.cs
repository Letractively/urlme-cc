using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace urlme.Utils.Web.Mvc
{
    public class FileNotFoundResult : ActionResult
    {
        public FileNotFoundResult()
        {
        }

        #region Overrides of ActionResult

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = 404;
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }

        #endregion
    }
}
