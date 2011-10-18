using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace urlme.Utils.Web.Mvc
{
    public class StringResult : ActionResult
    {
        private string output = string.Empty;
        private string contentType = "text/plain";

        public StringResult(string output, string contentType = "text/plain")
        {
            this.output = output;
            this.contentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = this.contentType;
            context.HttpContext.Response.Write(this.output);
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }
    }
}
