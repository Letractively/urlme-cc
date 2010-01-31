<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UrlMe.cc._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="Resource/Styles/main.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~/") %>Resource/javascript/common.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/") %>Resource/javascript/bakersdozen.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/") %>Resource/javascript/jquery-1.3.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="messageBox" style="display:none;">
        <div class="messageContent" id="Message" runat="server"></div>
    </div>
    
    <div>
        <div id="divLogo">
            <a href="<%= ResolveUrl("~/") %>"><img style="border:none;" src="<%=ResolveUrl(Library.Configuration.Site.Logo) %>" /></a>
        </div><!-- /divLogo -->
        <div id="divWelcome">
            <b><%= HttpContext.Current.User.Identity.Name.Split("|".ToCharArray())[1]%></b>
            &nbsp;|&nbsp;
            <a href="<%= ResolveUrl("~/LogOut.ashx") %>">Sign out</a>
        </div>
        <br clear="all" />

        <!-- ### ADD FORM ### -->
        <div class="Form Add">
            <form id="AddLinkForm" method="post">
                <strong>Add:</strong>
                <%=Library.Configuration.Site.UrlNoEndingSlash %>/&nbsp;<input class="TextBox AddPathTextBox" type="text" name="AddPath" id="AddPath" />
                <%=string.Format("{0}redirects{0}to{0}","&nbsp;") %>
                <input type="text" id="AddDestinationUrl" name="AddDestinationUrl" style="width:300px;" />
                <input type="submit" value="Add" />
                <input type="hidden" name="FormAction" value="AddLink" />
            </form>
        </div>
        <!-- /### ADD FORM ### -->
        
        <div style="padding:5px;"></div>
        <div id="debug"></div>
        
        <!-- /### MANAGE LINKS ### -->
        <asp:Repeater runat="server" ID="LinksRepeater">
            <HeaderTemplate>
                <form id="EditLinksForm" method="post">
                <input type="hidden" id="EditLinksFormAction" name="FormAction" value="" />
                <input type="hidden" id="LinkIdToDelete" name="LinkIdToDelete" value="" />
                <input type="hidden" id="LinkIdsToDelete" name="LinkIdsToDelete" value="" />
                <input type="hidden" id="LinkIdsToUpdate" name="LinkIdsToUpdate" value="" />
                <div class="TableControls">
                    <input type="button" id="DeleteChecked" value="Delete checked" /><input type="checkbox" id="GlobalCheckbox" />
                </div>
                <div>
                <table class="GridView">
                    <tr>
                        <th>Link</th><th>Destination Url</th><th>&nbsp;</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id="DisplayLinkRow-<%# Eval("LinkID") %>" class="DisplayRow">
                        <td style="width: 225px"><%# String.Format("{0}/<b>{1}</b>",Library.Configuration.Site.UrlNoEndingSlash, Eval("Path")) %></td>
                        <td style="width: 450px"><%# String.Format("<a href=\"{0}\">{0}</a>",MakeSnippet(Eval("DestinationUrl").ToString(),Library.Configuration.Page.DestinationUrlSnippetLength)) %></td>    
                        <td style="width: 90px"><a href="#" onclick="EditRow(<%# Eval("LinkID") %>); return false;">Edit</a>&nbsp;|&nbsp;<a href="#" onclick="DeleteRow(<%# Eval("LinkID") %>); return false;">Delete</a>&nbsp;<input class="RowCheckbox" type="checkbox" id="Checkbox-<%# Eval("LinkID") %>" name="Checkbox-<%# Eval("LinkID") %>" /></td>
                    </tr>
                    <tr id="EditLinkRow-<%# Eval("LinkID") %>" class="EditRow">
                        <td><%= Library.Configuration.Site.UrlNoEndingSlash %>/&nbsp;<input class="TextBox PathTextBox" type="text" name="Path-<%# Eval("LinkID") %>" value="<%# Eval("Path") %>" /></td>
                        <td><input class="UrlTextBox" type="text" name="Url-<%# Eval("LinkID") %>" value="<%# Eval("DestinationUrl") %>" /></td>
                        <td><a class="SaveLink" href="#">Save</a> | <a href="#" onclick="CancelEditRow(<%# Eval("LinkID") %>); return false;">Cancel</a></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
                </div>
                </form>
            </FooterTemplate>
        </asp:Repeater>
        <!-- /### MANAGE LINKS ### -->
    </div>
    
    <script type="text/javascript" language="javascript">
        var proxyUrl = '<%=ResolveUrl("~/resource/serviceproxy/linkproxy.svc") %>/';
    </script>
    <script src="<%=ResolveUrl("~/") %>Resource/javascript/default.js" type="text/javascript"></script>
</body>
</html>
