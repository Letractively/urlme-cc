<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<urlme.Site.ViewModels.HomeViewModel>" %>
<%@ Import Namespace="urlme.Site.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UrlMe
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if (this.Model.CurrentUser.IsAuthenticated) { %>

        <% this.Html.RenderPartial("~/views/home/displaytemplates/link.ascx", this.Model.NewLink); %>

    <% } %>

    <% if (this.Model.CurrentUser.IsAuthenticated && this.Model.Links.Count > 0)
       {
           this.Html.RenderPartial("SortBy", this.Model.Sort);
           this.Html.RenderPartial("LinkList", this.Model.Links);
       }%>

    <script type="text/javascript">
        var feedback = '<%= TempData["Feedback"] ?? string.Empty %>';
    </script>
    <script src="<%= this.Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script src="<%= this.Url.Content("~/Scripts/jquery.dataTables.min.js") %>" type="text/javascript"></script>
    <script src="<%= this.Url.Content("~/Scripts/codejkjk.urlme.HomeIndex.js") %>" type="text/javascript"></script>
</asp:Content>
