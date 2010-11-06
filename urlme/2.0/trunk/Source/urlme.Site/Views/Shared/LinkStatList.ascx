﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<urlme.Model.Link>>" %>
<%@ Import Namespace="urlme.Site.Helpers" %>

    <table>
        <tr>
            <th>
                UrlMe Link
            </th>
            <th>
                Destination Url
            </th>
            <th>
                Created
            </th>
            <th>
                # Hits
            </th>
        </tr>

    <% int i = 0; 
       foreach (var item in Model)
       { %>
    
        <tr <%= (i % 2 == 1) ? "class=\"even\"" : "" %>>
            <td>
                <%= string.Format("<a href=\"{0}\">http://urlme.cc/{1}</a>", item.DestinationUrl, item.Path)%>&nbsp;&nbsp;&nbsp;(<%= string.Format("<a href=\"{0}\" class=\"external\" target=\"_blank\">new tab</a>", item.DestinationUrl)%>)
            </td>
            <td>
                <%: this.Html.MakeSnippet(item.DestinationUrl, 60)%>
            </td>
            <td class="created">
                <%: item.CreateDate.ToString("MMM dd, yyyy") %>
            </td>
            <td>
                <%: item.HitCount.ToString("#,#0") %>
            </td>
        </tr>
    
    <% i++; 
       } %>

    </table>