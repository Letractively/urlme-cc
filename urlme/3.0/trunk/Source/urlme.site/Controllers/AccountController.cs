using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

namespace urlme.site.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : BaseController
    {
        private static readonly string redirectUriRoot = "http://urlme.cc/";
        private static readonly long fbAppId = 97294448011;
        private static readonly string fbAppSecret = "d0140785285682511eb137104673586a";
        private static readonly string fbSignIn = "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}";
        private static readonly string fbAccessToken = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}";
        private static readonly string fbUserInfo = "https://graph.facebook.com/me?access_token=";

        [Route("facebook-sign-in")]
        public ActionResult FacebookSignIn(string returnUrl)
        {
            var fbRedirectUri = redirectUriRoot + returnUrl;
            var scope = "email";
            var redirectUrl = string.Format(fbSignIn, fbAppId, fbRedirectUri, scope);

            return Redirect(redirectUrl);
        }

        [Route("facebook-sign-in-callback")]
        public ActionResult FacebookSignInCallback(string code, string error)
        {
            return HandleFacebookSignInCallback(code, "account/facebook-sign-in-callback");
        }

        [Route("facebook-sign-in-callback-from-add")]
        public ActionResult FacebookSignInCallbackFromAdd(string code, string error)
        {
            return HandleFacebookSignInCallback(code, "account/facebook-sign-in-callback-from-add");
        }

        private ActionResult HandleFacebookSignInCallback(string code, string redirectUri)
        {
            var url = string.Format(fbAccessToken, fbAppId, redirectUriRoot + redirectUri, fbAppSecret, code);
            var response = ianhd.core.Net.HttpWebRequest.GetResponse(url); // access_token={access-token}&expires={seconds-til-expiration} || error message
            var accessToken = response.Split('&')[0].Split('=')[1];

            var userInfoResponse = ianhd.core.Net.HttpWebRequest.GetResponse(fbUserInfo + accessToken);
            var json = JObject.Parse(userInfoResponse);
            var email = (string)json["email"];

            urlme.data.Models.User.IssueAuthTicket(email, true);
            
            if (redirectUri.Contains("from-add")) {
                return Redirect("~/add-sign-in");
            }

            return Redirect("~/");
        }

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

                        urlme.data.Models.User.IssueAuthTicket(email, true);

                        if (!string.IsNullOrEmpty(returnUrl))
                            return Redirect("~/" + returnUrl);

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