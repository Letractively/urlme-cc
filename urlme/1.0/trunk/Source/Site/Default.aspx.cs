using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using UrlMe.cc.Model.Enums;
using UrlMe.cc.Model;

namespace UrlMe.cc
{
    public partial class _Default : System.Web.UI.Page
    {
        private string AddPath { get { return Request.Form["AddPath"]; } }
        private string AddDestinationUrl { get { return Request.Form["AddDestinationUrl"]; } }
        private int LinkIdToDelete { get { return int.Parse(Request.Form["LinkIdToDelete"]); } }
        private string LinkIdsToDelete { get { return Request.Form["LinkIdsToDelete"]; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            CrudLinkResults result = CrudLinkResults.Success;

            if (!string.IsNullOrEmpty(Request.Form["FormAction"]))
            {
                // TODO: case switch
                switch (Request.Form["FormAction"])
                {
                    case "AddLink":
                        // todo: create a User.Current with user props, push this in the data calls rather than right here
                        result = Link.CreateUserLink(int.Parse(HttpContext.Current.User.Identity.Name.Split('|')[0]), this.AddPath, this.AddDestinationUrl);
                        Message.InnerHtml = result.ToString();
                        break;
                    case "UpdateLinks": // not being used
                        Message.InnerHtml = "Updating&nbsp;...";
                        break;
                    case "DeleteLink":
                        result = Link.DeleteLink(this.LinkIdToDelete);
                        Message.InnerHtml = result.ToString();
                        break;
                    case "DeleteLinks":
                        result = Link.DeleteLinks(this.LinkIdsToDelete);
                        Message.InnerHtml = result.ToString();
                        break;
                }
            }
            this.LoadLinks();
        }

        protected void lbSignOut_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        }

        private void LoadLinks()
        {
            List<Link> links = Link.GetLinksByUser(int.Parse(HttpContext.Current.User.Identity.Name.Split('|')[0]));
            
            if (links != null)
            {
                LinksRepeater.DataSource = links;
                LinksRepeater.DataBind();
            }
        }

        #region MakeSnippet
        // put this in Utils
        public string MakeSnippet(string str, int threshold)
        {
            if (str.Length > threshold)
                return str.Substring(0, threshold) + "...";
            else
                return str;
        }
        #endregion
    }
}
