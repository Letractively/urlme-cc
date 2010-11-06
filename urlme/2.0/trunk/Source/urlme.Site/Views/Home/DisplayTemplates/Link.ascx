<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<urlme.Model.Link>" %>

    <% using (Html.BeginForm()) {%>
        <div class="add-link-form">
            <strong>Add</strong>: <span class="hints">http://urlme.cc/</span><%: this.Html.TextBoxFor(m => m.Path, new { @class = "new-path-input" }) %><span class="hints"> redirects to </span><%: this.Html.TextBoxFor(m => m.DestinationUrl, new { @class = "new-destination-url-input" }) %>
            <input type="submit" value="Add" class="add-button" />&nbsp;<span id="Error"></span>
        </div>

        <%: Html.ValidationMessageFor(m => m.Path) %>
        <%: Html.ValidationMessageFor(m => m.DestinationUrl) %>
    <% } %>

