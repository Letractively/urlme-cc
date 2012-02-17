<%@ Page Title="UrlMe - Bookmarklet" Language="C#" Inherits="System.Web.Mvc.ViewPage<urlme.Model.User>" %>
<%@ Import Namespace="urlme.Site.Helpers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link href="<%= this.Html.ResolveClientUrl("~/Content/Site.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= this.Html.ResolveClientUrl("~/Scripts/jquery-1.7.1.min.js") %>"></script>
    <script src="<%= this.Html.ResolveClientUrl("~/Scripts/codejkjk.js") %>"></script>
    <script src="<%= this.Html.ResolveClientUrl("~/Scripts/plugins/zeroclipboard/zeroclipboard.js") %>"></script>
    <script type="text/javascript">
        ZeroClipboard.setMoviePath('<%= this.Html.ResolveClientUrl("~/Scripts/plugins/zeroclipboard/zeroclipboard.swf") %>');
    </script>
    <script type="text/javascript">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-18542179-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>
</head>
    <body>
    <div class="main-content">
        <a class="Logo" target="_blank" href="<%= this.Html.ResolveClientUrl("~/") %>"></a>

    <% if (this.Model.IsAuthenticated)
       { %>

        <% using (this.Html.BeginForm("Add", "Add", FormMethod.Post, new { id = "BookmarkAddForm" }))
           { 
            %>
            <div class="add-link-form">
                <div style="margin-bottom:10px;"><input value="<%= Request.QueryString["url"] %>" id="NewDestinationUrl" name="NewDestinationUrl" type="text" class="new-destination-url-input" /></div>
                <div style="margin-bottom:10px;"><strong>Add</strong>: <span class="hints">http://urlme.cc/</span><input id="NewPath" name="NewPath" type="text" class="new-path-input" /></div>
                <div><input type="button" id="AddAndClose" value="Add & close" />&nbsp;<input type="button" id="AddCopyAndClose" value="Add, copy & close" />&nbsp;<input type="button" id="CancelAndClose" value="Cancel" />&nbsp;<span id="Error"></span></div>
            </div>
        <% } %>

    <% }
       else
       { %>
       You should sign in first. Visit <a target="_blank" href="http://urlme.cc">http://urlme.cc</a> and sign in, <a href="#" onclick="location.href = location.href;return false;">refresh this window</a>, then try again.
    <% } %>

    </div>

    <script type="text/javascript">
        var bookmarkletServiceUrl = "<%=this.Html.ResolveClientUrl("~/service/add") %>";
    </script>
    
    <script src="<%= this.Html.ResolveClientUrl("~/Scripts/codejkjk.urlme.Bookmarklet.js") %>" type="text/javascript"></script>
    </body>
</html>