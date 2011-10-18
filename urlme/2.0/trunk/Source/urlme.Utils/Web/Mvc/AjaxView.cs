using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Mvc;

namespace urlme.Utils.Web.Mvc
{
    public class AjaxView : IView, IViewDataContainer
    {
        private TextWriter writer = new StringWriter();
        private ViewContext viewContext = null;
        private HtmlHelper htmlHelper = null;

        public ViewDataDictionary ViewData
        {
            get;
            set;
        }

        private AjaxView(ControllerContext controllerContext)
        {
            this.viewContext = new ViewContext(controllerContext, this, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, this.writer);
            this.htmlHelper = new HtmlHelper(this.viewContext, this);
        }

        public HtmlHelper Html
        {
            get
            {
                return this.htmlHelper;
            }
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {

        }

        public static AjaxView Create(ControllerContext controllerContext)
        {
            return new AjaxView(controllerContext);
        }

        public override string ToString()
        {
            return this.writer.ToString();
        }
    }
}
