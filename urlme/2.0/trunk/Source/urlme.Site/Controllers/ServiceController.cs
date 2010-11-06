using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urlme.Model.Enums;

namespace urlme.Site.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/
        
        public JsonResult Add(string newPath, string newDestinationUrl)
        {
            CrudLinkResults result = Model.Link.CreateLink(newPath, newDestinationUrl);
            string feedback = string.Empty;
            switch (result)
            {
                case CrudLinkResults.Success:
                    feedback = "Success!";
                    break;
                case CrudLinkResults.PathAlreadyExists:
                    feedback = "Error: path already exists.";
                    break;
                case CrudLinkResults.InsufficientInput:
                    feedback = "Error: please provide both.";
                    break;
                default:
                    feedback = "Error. Please try again.";
                    break;
            }
            var resp = new { Feedback = feedback };
            return this.Json(resp);
        }

    }
}
