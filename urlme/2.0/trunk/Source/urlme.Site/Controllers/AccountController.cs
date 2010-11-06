using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using System.Web.Security;

namespace urlme.Site.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult SignOut()
        {
            Model.User.SignOut();
            return Redirect("~/");
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get), ValidateInput(false)]
        public ActionResult OpenIdLogOn(string returnUrl)
        {
            var openId = new OpenIdRelyingParty();
            var response = openId.GetResponse();
            if (response == null)  // Initial operation
            {
                // make request
                var req = openId.CreateRequest(Request.Form["openid_identifier"]);

                var fields = new ClaimsRequest();
                fields.Email = DemandLevel.Require;
                req.AddExtension(fields);

                return req.RedirectingResponse.AsActionResult();
            }
            else  // receiving response
            {
                // Step 2: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var claim = response.GetExtension<ClaimsResponse>();
                        string email = (claim != null) ? claim.Email : null;

                        string identifier = response.ClaimedIdentifier;

                        Model.User.IssueAuthTicket(email, true);

                        if (!string.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);

                        return Redirect("~/");
                }
            }
            return new EmptyResult();
        }
    }
}
