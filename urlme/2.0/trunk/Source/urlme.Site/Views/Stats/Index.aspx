<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<urlme.Site.ViewModels.HomeViewModel>" %>
<%@ Import Namespace="urlme.Site.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UrlMe - Stats
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Stats</h2>

    <% if (this.Model.CurrentUser.IsAuthenticated && this.Model.Links.Count > 0)
       {
           this.Html.RenderPartial("SortStatsBy", this.Model.Sort);
           this.Html.RenderPartial("LinkStatList", this.Model.Links);
       }%>
</asp:Content>
