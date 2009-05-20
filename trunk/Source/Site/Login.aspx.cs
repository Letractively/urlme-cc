using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace UrlMe.cc
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // load site updates
                gvSiteUpdates.DataSource = Library.Data.SiteData.GetSiteUpdatesSiteCD("URLME");
                gvSiteUpdates.DataBind();
            }
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            int userID = Library.Data.UserData.AuthenticateUser(txtEmail.Text,txtPassword.Text);
            if (userID != -1)
                FormsAuthentication.RedirectFromLoginPage(userID.ToString() + "|" + txtEmail.Text, chkRememberMe.Checked);
            else
                Response.Write("Wrong email and/or password. Please try again.");
        }
        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            int userID = Library.Data.UserData.NewUser(txtEmail.Text, txtPassword.Text,txtPasswordHint.Text);
            if (userID != -1)
                FormsAuthentication.RedirectFromLoginPage(userID.ToString() + "|" + txtEmail.Text, chkRememberMe.Checked);
            else
                Response.Write("Something broke on account creation. Undo.");
        }
        #region GetNewNotice
        // put this in Utils
        public string GetNewNotice(DateTime dateUpdated)
        {
            TimeSpan ts = dateUpdated - DateTime.Now;
            if (Math.Abs(ts.Days) < Library.Configuration.Site.SiteUpdateNewNoticeDaySpan)
                return "<b><font color=\"green\">New!</font></b>&nbsp;";
            else
                return "";
        }
        #endregion
    }
}
