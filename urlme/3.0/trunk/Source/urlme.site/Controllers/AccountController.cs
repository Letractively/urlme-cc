using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
//using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
//using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace urlme.site.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : BaseController
    {
        [Route("openid-sign-in")]
        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get), ValidateInput(false)]
        public ActionResult OpenIdLogOn(string returnUrl)
        {
            var openId = new OpenIdRelyingParty();
            var response = openId.GetResponse();
            if (response == null)  // Initial operation
            {
                // make request
                var req = openId.CreateRequest(Request.Form["openid_identifier"]);

                //var fields = new ClaimsRequest();
                //fields.Email = DemandLevel.Require;
                //req.AddExtension(fields);

                var fetch = new FetchRequest();
                fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                req.AddExtension(fetch);

                return req.RedirectingResponse.AsActionResult();
            }
            else  // receiving response
            {
                // Step 2: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var fetch = response.GetExtension<FetchResponse>();
                        string email = string.Empty;
                        if (fetch != null)
                        {
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                        }

                        //var claim = response.GetExtension<ClaimsResponse>();
                        //string email = (claim != null) ? claim.Email : null;

                        //string identifier = response.ClaimedIdentifier;

                        urlme.data.Models.User.IssueAuthTicket(email, true);

                        if (!string.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);

                        return Redirect("~/");
                }
            }
            return new EmptyResult();
        }

        [Route("sign-in")]
        public ActionResult SignIn(string returnUrl)
        {
            return Content("You need to sign in to do that.");
        }

        [Route("signed-in")]
        public JsonResult SignedIn()
        {
            return this.Json(new { signedIn = base.User.IsAuthenticated }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Account/LogOff
        [Route("logoff")]
        [HttpPost]
        public ActionResult LogOff()
        {
            urlme.data.Models.User.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}