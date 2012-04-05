using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace urlme.Site.Controllers
{
    public class ObjectController : Controller
    {
        //private readonly Linq2SqlAdminRepository repo = new Linq2SqlAdminRepository();

        //[HttpPost]
        //public JsonResult ToggleProperty(string itemId, string typeName, string propertyName)
        //{
        //    Assembly asm = typeof(Salem.AllPass.Data.Newsletter).Assembly; // get assembly of any object w/in salem.allpass.data namespace (NS). all obj's in that NS share the same assembly
        //    Type targetEntityType = asm.GetType(typeName);
        //    MethodInfo genericMethodDefinition = typeof(Linq2SqlAdminRepository).GetMethod("ToggleProperty");
        //    MethodInfo genericMethod = genericMethodDefinition.MakeGenericMethod(targetEntityType);
        //    bool success = (bool)genericMethod.Invoke(repo, new object[] { itemId, propertyName });

        //    return this.Json(new OperationResult() { WasSuccessful = success }, JsonRequestBehavior.DenyGet);
        //}

        //[HttpPost]
        //public JsonResult SetProperty(string itemId, string typeName, string propertyName, object newValue)
        //{
        //    Assembly asm = typeof(Salem.AllPass.Data.Newsletter).Assembly; // get assembly of any object w/in salem.allpass.data namespace (NS). all obj's in that NS share the same assembly
        //    Type targetEntityType = asm.GetType(typeName);
        //    MethodInfo genericMethodDefinition = typeof(Linq2SqlAdminRepository).GetMethod("SetProperty");
        //    MethodInfo genericMethod = genericMethodDefinition.MakeGenericMethod(targetEntityType);
        //    bool success = (bool)genericMethod.Invoke(repo, new object[] { itemId, propertyName, newValue });

        //    return this.Json(new OperationResult() { WasSuccessful = success }, JsonRequestBehavior.DenyGet);
        //}
    }
}
