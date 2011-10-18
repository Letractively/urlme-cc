<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<urlme.Model.Link>>" %>
<%@ Import Namespace="urlme.Site.Helpers" %>

<table id="Links">
    <thead>
        <tr>
            <th class="link">
                UrlMe Link
            </th>
            <th class="delete"></th>
            <th>
                Destination Url
            </th>
            <th class="hits"># Hits</th>
            <th>
                Created
            </th>        
        </tr>
    </thead>
    <tbody>
<%  
    for (int i = 0; i < Model.Count(); i++)
    {
        var item = Model.ElementAt(i);
        %>
    
    <tr <%= (i % 2 == 1) ? "class=\"even\"" : "" %> id="<%= item.LinkId %>">
        <td>
            <%= string.Format("<a href=\"{0}\">http://urlme.cc/{1}</a>", item.DestinationUrl, item.Path)%>&nbsp;&nbsp;&nbsp;(<%= string.Format("<a href=\"{0}\" class=\"external\" target=\"_blank\">new tab</a>", item.DestinationUrl)%>)
        </td>
        <td>
            <%: Html.ActionLink("Delete", "delete", new { controller = "link", id = item.LinkId }, new { @class = "delete-link" })%>
        </td>
        <td>
            <%: this.Html.MakeSnippet(item.DestinationUrl, 60)%>
        </td>
        <td>
            <%: item.HitCount.ToString("#,#0") %>
        </td>
        <td class="created">
            <%: item.CreateDate.ToString("MMM dd, yyyy") %>
        </td>
    </tr>
    <% } %>
    </tbody>
</table>
<script src="../../Scripts/jquery.hoverIntent.min.js" type="text/javascript"></script>
<script src="../../Scripts/codejkjk.urlme.LinkList.js" type="text/javascript"></script>
