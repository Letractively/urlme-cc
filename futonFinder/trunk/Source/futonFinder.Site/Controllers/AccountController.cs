﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using futonFinder.Core.Extensions;
using futonFinder.Site.Models;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace futonFinder.Site.Controllers
{
    //[Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : BaseController
    {
        //
        // POST: /Account/LogOff

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult SignOut()
        {
            Data.DomainModels.User.SignOut();
            return Redirect("~/");
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            if (provider == "google")
            {
                return new ExternalLoginResult(provider, Url.Action("GoogleLoginCallback", new { ReturnUrl = returnUrl }));
            }
            else // facebook
            {
                returnUrl = Url.Action("FacebookLoginCallback", new { ReturnUrl = returnUrl });
                return Redirect("https://www.facebook.com/dialog/oauth?client_id=" + new Facebook().AppID + "&redirect_uri=" + HttpUtility.HtmlEncode("h" + System.Web.HttpContext.Current.Request.Url.ToString().Substring("h", "/Account") + returnUrl) + "%3F__provider__%3Dfacebook&scope=email");
            }
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult GoogleLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("GoogleLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }
            else
            {
                var email = result.ExtraData["email"];
                Data.DomainModels.User.IssueAuthTicket(email, true);
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult FacebookLoginCallback(string returnUrl)
        {
            string code = Request.QueryString["code"];
            string returnUrl1 = Url.Action("FacebookLoginCallback", new { ReturnUrl = returnUrl });

            IDictionary<string, string> userData = new Facebook().GetUserData(code, HttpUtility.HtmlEncode("h" + System.Web.HttpContext.Current.Request.Url.ToString().Substring("h", "/Account") + returnUrl1));
            AuthenticationResult result = new AuthenticationResult(isSuccessful: true, provider: "facebook", providerUserId: userData["id"], userName: userData["username"], extraData: userData);

            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }
            else
            {
                //futonFinder.Data.User
                Data.DomainModels.User.IssueAuthTicket(userData["email"], true);
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion
    }
}
