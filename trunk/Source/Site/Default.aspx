<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UrlMe.cc._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="SiteInfo/Styles/main.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~/") %>/SiteInfo/javascript/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="sm" />
    <asp:UpdateProgress runat="server" id="up">
        <ProgressTemplate>
            <div class="messageBox">
                <div id="messageContent">Loading&nbsp;...</div>
            </div>
        </ProgressTemplate>    
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="upd">
        <ContentTemplate>
            <div>
	            <div id="divLogo">
	                <a href="<%= ResolveUrl("~/") %>"><img style="border:none;" src="<%=ResolveUrl(Library.Configuration.Site.Logo) %>" /></a>
	            </div><!-- /divLogo -->
                <div style="float:right; width: 300px; text-align:right;">
                    <b><%= HttpContext.Current.User.Identity.Name.Split("|".ToCharArray())[1]%></b>
                    &nbsp;|&nbsp;
                    <asp:LinkButton ID="lbSignOut" runat="server" Text="Sign out" onclick="lbSignOut_Click" />
                </div>
                <br clear="all" />
                <br />
                <!-- ### ADD FORM ### -->
                <strong>Add:</strong>
                <div class="Form Add">
                    <%=Library.Configuration.Site.UrlNoEndingSlash %>/&nbsp;<asp:TextBox ID="txtPath" style="width:50px" runat="server" />
                    &nbsp;redirects&nbsp;to&nbsp;
                    <asp:TextBox ID="txtDestinationUrl" runat="server" style="width:300px;" />
                    <span style="display:none">
                        &nbsp;expiring&nbsp;in&nbsp;
                        <asp:DropDownList ID="ddlExpiresIn" runat="server">
                            <asp:ListItem Text="1 Year" Value="1Yr" />
                            <asp:ListItem Text="1 Week" Value="1Wk" />
                            <asp:ListItem Text="1 Day" Value="1D" />
                        </asp:DropDownList>
                    </span>
                    &nbsp;set&nbsp;to&nbsp;
                    <asp:DropDownList ID="ddlPublicPrivate" runat="server">
                        <asp:ListItem Text="Private" Value="false" />
                        <asp:ListItem Text="Public" Value="true" />
                    </asp:DropDownList>            
                    <asp:button ID="btnAddEdit" runat="server" Text="Add" 
                        onclick="btnAddEdit_Click" />
                </div>
                <!-- /### ADD FORM ### -->
                <div style="padding:10px;"></div>
                <div id="debug"></div>
                
                <!-- ### MANAGE LINKS ### -->
                <asp:GridView ID="gvLinks" 
                              runat="server" 
                              AutoGenerateColumns="false"
                              CssClass="GridView"
                              AlternatingRowStyle-BackColor="#E1E0E0"
                              OnRowDataBound="gvLinks_RowDataBound"
                              OnRowCommand="gvLinks_RowCommand"
                >
                    <Columns>
                    <asp:TemplateField HeaderText="Link">
                        <ItemTemplate>
                            <div style="width:200px;" onmouseover="showTextBox(<%# Eval("LinkID") %>);" onmouseout="hideTextBox(<%# Eval("LinkID") %>);">
                                <span id="spnLinkTextContainer<%# Eval("LinkID") %>"><%# String.Format("{0}/<b>{1}</b>",Library.Configuration.Site.UrlNoEndingSlash, Eval("Path")) %></span>
                                <span style="display:none;" id="spnLinkTextBoxContainer<%# Eval("LinkID") %>"><%# String.Format("<input readonly=\"readonly\" onclick=\"this.select();\" class=\"txtBoxLarge\" id=\"txtLink{0}\" type=\"text\" value=\"{1}/{2}\"/>",Eval("LinkID"),Library.Configuration.Site.UrlNoEndingSlash, Eval("Path")) %></span>
                            </div>
                            <div id="divEditPath" runat="server" visible="false"><%=Library.Configuration.Site.UrlNoEndingSlash + "/"%><asp:TextBox ID="txtPath" CssClass="txtBoxLarge" runat="server" Text='<%# Eval("Path") %>' /></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Destination Url">
                        <ItemTemplate>
                            <%# String.Format("<a href=\"{0}\">{0}</a>",MakeSnippet(Eval("DestinationUrl").ToString(),Library.Configuration.Page.DestinationUrlSnippetLength)) %>
                        </ItemTemplate>
                    </asp:TemplateField>                
                    <asp:TemplateField HeaderText="Public?">
                        <ItemTemplate>
                            <asp:HiddenField ID="hidPublicInd" runat="server" Value='<%# Eval("PublicInd") %>' />
                            <asp:Literal ID="litPublicPrivate" runat="server" />&nbsp;[<asp:LinkButton OnClick="lbTogglePublicInd_Click" ID="lbTogglePublicInd" runat="server" CommandArgument='<%# Eval("LinkID") %>' Text="&nbsp;&Delta;&nbsp;" />]
                        </ItemTemplate>
                    </asp:TemplateField>                 
                    <asp:TemplateField HeaderText="Expires On">
                        <ItemTemplate>
                            <%# DateTime.Parse(Eval("ExpirationDate").ToString()).ToString("MM/dd/yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton OnClick="lbToggleActiveInd_Click" OnClientClick="return confirm('Are you sure?');" CommandArgument='<%# Eval("LinkId") %>' ID="lbToggleActiveInd" runat="server" Text="Release" />
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:ButtonField Visible="false" CommandName="Edit" Text="Edit" ButtonType="Link" />
                    </Columns>
                </asp:GridView>
                <!-- /### MANAGE LINKS ### -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>    
    <script type="text/javascript" language="javascript">
        init();
        function init() {
            // set focus to email input
            gel('<%=txtPath.ClientID %>').focus();
        }
        function showTextBox(linkID) {
            showInlineId("spnLinkTextBoxContainer" + linkID);
            hideId("spnLinkTextContainer" + linkID);
            gel("txtLink" + linkID).focus();
            gel("txtLink" + linkID).select();
        }
        function hideTextBox(linkID) {
            hideId("spnLinkTextBoxContainer" + linkID);
            showInlineId("spnLinkTextContainer" + linkID);
        }
    </script>
</body>
</html>
