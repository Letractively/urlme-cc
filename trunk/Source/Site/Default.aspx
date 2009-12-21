<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UrlMe.cc._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="SiteInfo/Styles/main.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~/") %>/SiteInfo/javascript/common.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/") %>/SiteInfo/javascript/jquery-1.3.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="messageBox">
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
                <%=Library.Configuration.Site.UrlNoEndingSlash %>/&nbsp;<input class="TextBox AddPathTextBox" type="text" id="AddPath" />
                &nbsp;redirects&nbsp;to&nbsp;
                <input type="text" id="AddDestinationUrl" style="width:300px;" />
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
                <input type="hidden" name="FormAction" value="UpdateLink" />
                <table class="GridView">
                    <tr>
                        <th>Link</th><th>Destination Url</th><th>&nbsp;</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id="DisplayLinkRow-<%# Eval("LinkID") %>">
                        <td><%# String.Format("{0}/<b>{1}</b>",Library.Configuration.Site.UrlNoEndingSlash, Eval("Path")) %></td>
                        <td><%# String.Format("<a href=\"{0}\">{0}</a>",MakeSnippet(Eval("DestinationUrl").ToString(),Library.Configuration.Page.DestinationUrlSnippetLength)) %></td>    
                        <td><a href="#" onclick="EditRow(<%# Eval("LinkID") %>); return false;">Edit</a> | <a href="#" onclick="DeleteRow(<%# Eval("LinkID") %>); return false;">Delete</a></td>
                    </tr>
                    <tr id="EditLinkRow-<%# Eval("LinkID") %>" class="EditRow">
                        <td><%= Library.Configuration.Site.UrlNoEndingSlash %>/&nbsp;<input class="TextBox PathTextBox" type="text" name="Path-<%# Eval("LinkID") %>" value="<%# Eval("Path") %>" /></td>
                        <td><input class="UrlTextBox" type="text" name="Url-<%# Eval("LinkID") %>" value="<%# Eval("DestinationUrl") %>" /></td>
                        <td><input type="submit" name="Save" value="Save" /> | <a href="#" onclick="CancelEditRow(<%# Eval("LinkID") %>); return false;">Cancel</a></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
                </form>
            </FooterTemplate>
        </asp:Repeater>
        <!-- /### MANAGE LINKS ### -->
    </div>
    
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            // set focus to email input
            $('#AddPath').focus();

        });

        function EditRow(linkId) {
            $("#DisplayLinkRow-" + linkId).hide();
            $("#EditLinkRow-" + linkId).show();
        }

        function CancelEditRow(linkId) {
            $("#DisplayLinkRow-" + linkId).show();
            $("#EditLinkRow-" + linkId).hide();
        }        
    </script>
</body>
</html>
