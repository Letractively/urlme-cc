<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<urlme.Model.Enums.SortOptions>" %>
<%@ Import Namespace="urlme.Site.Helpers" %>

<div class="sort-by">
    <% if (Model == urlme.Model.Enums.SortOptions.path)
       { %>
            Sort by:&nbsp;&nbsp;<%: this.Html.ActionLink("# Hits", "Index", new { controller = "Stats" })%><span class="separator">|</span><a href="<%= this.Html.ResolveClientUrl("~/stats/?latest") %>">Latest</a><span class="separator">|</span><strong>Link A-Z</strong>
    <% }
       else if (Model == urlme.Model.Enums.SortOptions.latest)
       { %>
            Sort by:&nbsp;&nbsp;<%: this.Html.ActionLink("# Hits", "Index", new { controller = "Stats" })%><span class="separator">|</span><strong>Latest</strong><span class="separator">|</span><a href="<%= this.Html.ResolveClientUrl("~/stats/?path") %>">Link A-Z</a>
    <% }        
       else
       { %>
            Sort by:&nbsp;&nbsp;<strong># Hits</strong><span class="separator">|</span><a href="<%= this.Html.ResolveClientUrl("~/stats/?latest") %>">Latest</a><span class="separator">|</span><a href="<%= this.Html.ResolveClientUrl("~/stats/?path") %>">Link A-Z</a>
    <% } %>
</div>

