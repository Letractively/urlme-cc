<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<urlme.Model.Enums.SortOptions>" %>
<%@ Import Namespace="urlme.Site.Helpers" %>

<div class="sort-by">
    <% if (Model == urlme.Model.Enums.SortOptions.path)
       { // active = path %>
            Sort by:&nbsp;&nbsp;<%: this.Html.ActionLink("Latest", "Index", new { controller = "Home" })%><span class="separator">|</span><strong>Link A-Z</strong><span class="separator">|</span><a href="<%= this.Html.ResolveClientUrl("~/?hits") %>"># Hits</a>
    <% }
       else if (Model == urlme.Model.Enums.SortOptions.hits)
       { // active = hits %>
            Sort by:&nbsp;&nbsp;<%: this.Html.ActionLink("Latest", "Index", new { controller = "Home" })%><span class="separator">|</span><a href="<%= this.Html.ResolveClientUrl("~/?path") %>">Link A-Z</a><span class="separator">|</span><strong># Hits</strong>
    <% } else { // active = latest %>
            Sort by:&nbsp;&nbsp;<strong>Latest</strong><span class="separator">|</span><a href="<%= this.Html.ResolveClientUrl("~/?path") %>">Link A-Z</a><span class="separator">|</span><a href="<%= this.Html.ResolveClientUrl("~/?hits") %>"># Hits</a>
    <% } %>
</div>

