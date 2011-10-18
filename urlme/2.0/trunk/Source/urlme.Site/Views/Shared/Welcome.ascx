<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

Welcome, <strong><%: urlme.Model.User.Current.Email %></strong><span class="separator">|</span><%: Html.ActionLink("Help", "Index", new { controller = "Help" })%><span class="separator">|</span><%: Html.ActionLink("Sign out", "SignOut", new { controller = "Account" })%>
