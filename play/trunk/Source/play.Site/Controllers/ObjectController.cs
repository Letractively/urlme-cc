using System;
using System.Reflection;
using System.Web.Mvc;

namespace Salem.AllPass.Admin.Controllers
{
    public class ObjectController : Controller
    {
        private Assembly asm = typeof(play.Site.Models.PlayOrder).Assembly; // get assembly of any object w/in our model namespace
        private System.Type type = typeof(play.Site.Models.Object);
        private play.Site.Models.Object obj = new play.Site.Models.Object();

        [HttpPost]
        public JsonResult ToggleProperty(string itemId, string typeName, string propertyName)
        {
            var targetEntityType = asm.GetType(typeName);
            var genericMethodDefinition = type.GetMethod("ToggleProperty");
            var genericMethod = genericMethodDefinition.MakeGenericMethod(targetEntityType);
            bool success = (bool)genericMethod.Invoke(obj, new object[] { itemId, propertyName });

            return this.Json(new { WasSuccessful = success }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult SetProperty(string itemId, string typeName, string propertyName, object newValue)
        {
            var targetEntityType = asm.GetType(typeName);
            var genericMethodDefinition = type.GetMethod("SetProperty");
            var genericMethod = genericMethodDefinition.MakeGenericMethod(targetEntityType);
            bool success = (bool)genericMethod.Invoke(obj, new object[] { itemId, propertyName, newValue });

            return this.Json(new { WasSuccessful = success }, JsonRequestBehavior.DenyGet);
        }
    }
}
