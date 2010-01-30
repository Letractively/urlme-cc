﻿using System;
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
        private string AddPath { get { return Request.Form["AddPath"]; } }
        private string AddDestinationUrl { get { return Request.Form["AddDestinationUrl"]; } }
        private int LinkIdToDelete { get { return int.Parse(Request.Form["LinkIdToDelete"]); } }
        private string LinkIdsToDelete { get { return Request.Form["LinkIdsToDelete"]; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            int success = -1; // data calls return 0 if success. todo: make this more flexible (could return a specific error msg like "path already exists")

            if (!string.IsNullOrEmpty(Request.Form["FormAction"]))
            {
                // TODO: case switch
                switch (Request.Form["FormAction"])
                {
                    case "AddLink":
                        // todo: create a User.Current with user props, push this in the data calls rather than right here
                        success = Library.Data.LinkData.NewLink(int.Parse(HttpContext.Current.User.Identity.Name.Split("|".ToCharArray())[0]), this.AddPath, this.AddDestinationUrl);
                        if (success != 0)
                            Message.InnerHtml = "Failed.";
                        else
                        {
                            // todo: add "want to edit?" or "you just added the path X"
                            Message.InnerHtml = "Success!";
                        }
                        break;
                    case "UpdateLinks":
                        Message.InnerHtml = "Updating&nbsp;...";
                        break;
                    case "DeleteLink":
                        success = Library.Data.LinkData.DeleteLink(this.LinkIdToDelete);
                        if (success != 0)
                            Message.InnerHtml = "Failed.";
                        else 
                            Message.InnerHtml = "Success!";
                        break;
                    case "DeleteLinks":
                        success = Library.Data.LinkData.DeleteLinks(this.LinkIdsToDelete);
                        if (success != 0)
                            Message.InnerHtml = "Failed.";
                        else
                            Message.InnerHtml = "Success!";
                        break;
                }
            }
            LoadLinks();
        }

        protected void lbSignOut_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnAddEdit_Click(object sender, EventArgs e)
        {
            //int success = Library.Data.LinkData.NewLink(int.Parse(HttpContext.Current.User.Identity.Name.Split("|".ToCharArray())[0]), txtPath.Text, txtDestinationUrl.Text);
            //if (success != 0)
            //    Response.Write("Failed.");
            //else
            //{
            //    LoadLinks();
            //}
        }

        /// <summary>
        /// Called from the end of LoadLinks()
        /// </summary>
        private void ClearForm()
        {
            //txtPath.Text = ""; 
            //// txtPath.Focus();
            //txtDestinationUrl.Text = "";
        }

        private void LoadLinks()
        {
            DataSet dsLinks = Library.Data.LinkData.GetLinksByUserID(int.Parse(HttpContext.Current.User.Identity.Name.Split("|".ToCharArray())[0]));
            if (dsLinks != null)
            {
                LinksRepeater.DataSource = dsLinks;
                LinksRepeater.DataBind();
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
            //LinkButton lb = (LinkButton)sender;
            //Library.Data.LinkData.TogglePublicInd(int.Parse(lb.CommandArgument));
            //LoadLinks();
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
                    // txtPath.Text = "hi there";
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
