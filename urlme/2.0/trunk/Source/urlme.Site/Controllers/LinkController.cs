using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urlme.Model.Enums;

namespace urlme.Site.Controllers
{
    public class LinkController : Controller
    {
        //
        // GET: /Link/

        public ActionResult Delete(int id)
        {
            CrudLinkResults result = Model.Link.DeleteLink(id);
            TempData.Add("Feedback", (result == CrudLinkResults.Success) ? "Success!" : "Error. Please try again.");
            return Redirect("~/");
        }

        public void RedirectToDestinationUrl(string path)
        {
            string destinationUrl = Model.Link.GetDestinationUrlByPathAndIncrementHitCount(path);
            Response.Redirect(destinationUrl);
        }

        public void RedirectToDestinationUrlWithQueryString(string invitationCode, string path = "ido-_invitationCode_")
        {
            string destinationUrl = Model.Link.GetDestinationUrlByPathAndIncrementHitCount(path);
            destinationUrl = destinationUrl.Replace("_invitationCode_", "invitationCode=" + invitationCode);
            Response.Redirect(destinationUrl);
        }
    }
}
