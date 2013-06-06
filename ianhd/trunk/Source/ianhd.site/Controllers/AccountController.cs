//namespace ianhd.site.Controllers
//{
//    using System;
//    using System.Web.Mvc;
//    using DotNetOpenAuth.Messaging;
//    using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
//    using DotNetOpenAuth.OpenId.RelyingParty;
//    using Salem.AllPass.Core;
//    using Salem.AllPass.Core.Diagnostics;
//    using Salem.AllPass.Data;
//    using Salem.AllPass.Data.Models;
//    using Salem.AllPass.Endpoint.ViewModels;

//    using Provider = Salem.AllPass.Data.Models.Provider;
//    using TemporaryUserToken = Salem.AllPass.Data.Models.TemporaryUserToken;
//    using User = Salem.AllPass.Data.Models.User;

//    public class AccountController : Controller
//    {
//        const string RETURN_URL_COOKIE_NAME = "AllPassReturnUrl";
//        const string APP_COOKIE_NAME = "AllPassApp";

//        public ActionResult Index()
//        {
//            return View(new ViewModelBase("Home Page"));
//        }

//        [AcceptVerbs(HttpVerbs.Get), ValidateInput(false)]
//        public ActionResult OAuth(string providerSlug, string returnUrl, string app)
//        {
//            if (string.IsNullOrWhiteSpace(providerSlug) || string.IsNullOrWhiteSpace(returnUrl) || string.IsNullOrWhiteSpace(app))
//            {
//                ErrorHandler.LogError(new Exception(string.Format("OAuth action does not have all required parameters; providerSlug = {0}, returnUrl = {1}, app = {2}", providerSlug, returnUrl, app)));
//                return View("Error", new ViewModelBase("AllPass Login - Error - OAuth Invalid Params"));
//            }

//            //New instance of oAuth object based on providerSlug
//            OAuth oAuth = new OAuth(providerSlug);

//            //Store returnUrl in 10 minute cookie
//            this.HttpContext.Response.Cookies.Add(new System.Web.HttpCookie(RETURN_URL_COOKIE_NAME, returnUrl) { Expires = DateTime.Now.AddMinutes(10) });

//            //Store App value in 10 minute cookie
//            this.HttpContext.Response.Cookies.Add(new System.Web.HttpCookie(APP_COOKIE_NAME, app) { Expires = DateTime.Now.AddMinutes(10) });

//            //Redirect the user back to oAuth provider for authorization.
//            return this.Redirect(oAuth.AuthorizationLinkGet());
//        }

//        [AcceptVerbs(HttpVerbs.Get), ValidateInput(false)]
//        public ActionResult OAuthAuthorized(string providerSlug, string code)
//        {
//            string app = this.HttpContext.Request.Cookies[APP_COOKIE_NAME].Value;
//            string returnUrl = this.HttpContext.Request.Cookies[RETURN_URL_COOKIE_NAME].Value;

//            if (string.IsNullOrEmpty(providerSlug) || string.IsNullOrEmpty(returnUrl) || string.IsNullOrEmpty(app) || string.IsNullOrEmpty(code))
//            {
//                ErrorHandler.LogError(new Exception(string.Format("OAuthAuthorized action does not have all required parameters; providerSlug = {0}, returnUrl = {1}, app = {2}, code = {3}", providerSlug, returnUrl, app, code)));
//                return View("Error", new ViewModelBase("AllPass Login - Error - OAuth Invalid Params"));
//            }

//            OAuth oAuth = new OAuth(providerSlug);

//            //Set ReturnUrl from AllPassReturnUrl cookie
//            oAuth.ReturnUrl = returnUrl;
//            oAuth.Code = code;

//            //Get the access token and secret.
//            oAuth.AccessTokenGet(oAuth.Code);

//            if (oAuth.AccessToken.Length > 0)
//            {
//                //We now have the credentials, so we can start making API calls
//                string url = oAuth.UserEndPoint + oAuth.AccessToken;
//                string jsonInformation = oAuth.WebRequest(Enumerations.Method.Get, url, String.Empty);
//                string clientIp = Core.Utils.GetUserHostAddress();
//                Core.Validation.OperationResult<User> thirdPartyUserOr = oAuth.ParseInformation(oAuth.ProviderId, jsonInformation, app, clientIp);

//                if (thirdPartyUserOr.WasSuccessful && thirdPartyUserOr.Item != null)
//                {
//                    // success - return view that simply has js that sets parent window's location to be returnUrl + "userId=23245"
//                    Guid token = TemporaryUserToken.Add((int)thirdPartyUserOr.Item.UserId);
//                    ViewBag.UserToken = token;
//                    ViewBag.ReturnUrl = oAuth.ReturnUrl;
//                    return View("SetParentWindow", new ViewModelBase("AllPass Login - OAuth Successful"));
//                }
//            }

//            // if it's made it to here, it's failed, so return Error view
//            ErrorHandler.LogError(new Exception(string.Format("OAuthAuthorized action either has oAuth.Token.Length <= 0, thirdPartyUserOr.WasSuccessful = false, or thirdPartyUserOr.Item == null; providerSlug = {0}, code = {1}", providerSlug, code)));
//            ViewBag.TryAgainUrl = Url.Action("OAuth", new { controller = "Login", returnUrl = this.HttpContext.Request.Cookies[RETURN_URL_COOKIE_NAME].Value, providerSlug = providerSlug, app = app });
//            return View("Error", new ViewModelBase("AllPass Login - OAuth Failure"));
//        }
//    }
//}
