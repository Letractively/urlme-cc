using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;

namespace UrlMe.cc
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadLinks();
        }

        protected void lbSignOut_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnAddEdit_Click(object sender, EventArgs e)
        {
            DateTime expirationDate = DateTime.Now;
            switch (ddlExpiresIn.SelectedValue.ToLower())
            {
                case "1yr":
                    expirationDate = expirationDate.AddYears(1);
                    break;                
                case "1wk":
                    expirationDate = expirationDate.AddDays(7);
                    break;
                case "1d":
                    expirationDate = expirationDate.AddDays(1);
                    break;
            }
            int success = Library.Data.LinkData.NewLink(int.Parse(HttpContext.Current.User.Identity.Name.Split("|".ToCharArray())[0]), txtPath.Text, txtDestinationUrl.Text, bool.Parse(ddlPublicPrivate.SelectedValue), expirationDate);
            if (success != 0)
                Response.Write("Failed.");
            else
            {
                LoadLinks();
            }
        }

        /// <summary>
        /// Called from the end of LoadLinks()
        /// </summary>
        private void ClearForm()
        {
            txtPath.Text = ""; txtPath.Focus();
            txtDestinationUrl.Text = "";
            ddlExpiresIn.SelectedIndex = 0;
            ddlPublicPrivate.SelectedIndex = 0;
        }

        private void LoadLinks()
        {
            DataSet dsLinks = Library.Data.LinkData.GetLinksByUserID(int.Parse(HttpContext.Current.User.Identity.Name.Split("|".ToCharArray())[0]));
            if (dsLinks != null)
            {
                gvLinks.DataSource = dsLinks;
                gvLinks.DataBind();
            }
            ClearForm();
        }

        #region lbToggleActiveInd_Click
        public void lbToggleActiveInd_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            Library.Data.LinkData.ToggleActiveInd(int.Parse(lb.CommandArgument));
            LoadLinks();
        }
        #endregion

        #region lbTogglePublicInd_Click
        public void lbTogglePublicInd_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            Library.Data.LinkData.TogglePublicInd(int.Parse(lb.CommandArgument));
            LoadLinks();
        }
        #endregion

        protected void gvLinks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)e.Row;
            if (gvr.DataItem != null) // make sure it's not a header/footer
            {
                HiddenField hid = (HiddenField)gvr.FindControl("hidPublicInd");
                LinkButton lb = (LinkButton)gvr.FindControl("lbTogglePublicInd");
                Literal lit = (Literal)gvr.FindControl("litPublicPrivate");

                if (hid.Value.ToLower() == "true")
                {
                    lit.Text = "Public";
                    lb.ToolTip = "Click to make Private";
                }
                else
                {
                    lit.Text = "<span class='Private'>Private</span>";
                    lb.ToolTip = "Click to make Public";
                }
            }
        }

        protected void gvLinks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower()) {
                case "edit":
                    txtPath.Text = "hi there";
                    //GridViewRow gvr = (GridViewRow)gvLinks.Rows[int.Parse(e.CommandArgument.ToString())];
                    //LinkButton lb = (LinkButton)gvr.FindControl("lbTogglePublicInd");
                    //lb.Visible = false;
                    //LoadLinks();
                    break;

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
