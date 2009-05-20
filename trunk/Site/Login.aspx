<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UrlMe.cc.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="SiteInfo/Styles/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
	    <div style="float:left;">
	        <div style="float:left;"><a href="<%= Library.Configuration.Site.ApplicationUrlRoot %>"><img style="border:none;" src="<%=ResolveUrl(Library.Configuration.Site.Logo) %>" /></a></div>
            <br clear="all" />
            <br />	    
	        <div id="divSiteUpdates">
	            <asp:GridView ID="gvSiteUpdates"
	                          runat="server" 
	                          CssClass="GridView"
	                          AutoGenerateColumns="false"
                              AlternatingRowStyle-BackColor="#E1E0E0"
	                          >
	                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <%# DateTime.Parse(Eval("UpdateDate").ToString()).ToString("MM/dd/yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Site Update">
                        <ItemTemplate>
                            <%# GetNewNotice(DateTime.Parse(Eval("UpdateDate").ToString())) %><%# Eval("Text").ToString() %>
                        </ItemTemplate>
                    </asp:TemplateField>                        
	                </Columns>
	            </asp:GridView>
	        </div>
	    </div><!-- logo & siteupdates container -->
	    <div id="divSignInCreateAccountContainer">
            <iframe style="display:none" src="https://urlme-cc.rpxnow.com/openid/embed?token_url=http://localhost/urlme.www01/login.aspx"
              scrolling="no" frameBorder="no" style="width:310px;height:240px;">
            </iframe>
            <br clear="all" />
	        <div id="divSignInCreateAccount">
		        <div id="divEmail" class="inputField">
        	        Email<span id="spnReqEmail" style="display:none">*</span>:<br />
        	        <asp:TextBox id="txtEmail" runat="server" />
                </div>
		        <div id="divPassword" class="inputField">
        	        Password<span id="spnReqPassword" style="display:none">*</span>:<br />
                    <asp:TextBox TextMode="Password" id="txtPassword" runat="server" />
                </div>
		        <div id="divRetypePassword" class="inputField" style="display:none">
        	        Re-type password*:<br />
                    <asp:textbox TextMode="Password" id="txtRetypePassword" runat="server" />
                </div>
		        <div id="divPasswordHint" class="inputField" style="display:none">
        	        Password hint*:<br />
                    <asp:textbox id="txtPasswordHint" runat="server" />
                </div>
                <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember me" Checked="true" />
                <div style="padding:5px"></div>                
		        <div id="divSignIn" style="display:block">
        	        <asp:Button ID="btnSignIn" OnClientClick="return validateSignIn()" runat="server" Text="Sign in" onclick="btnSignIn_Click" />&nbsp;&nbsp;or&nbsp;&nbsp;<a href="javascript:switchView('createAccount')">create account</a>
                </div>
                <div id="divCreateAccount" name="divCreateAccount" style="display:none">
        	        <asp:Button ID="btnCreateAccount" OnClientClick="return validateCreateAccount()" runat="server" Text="Create account" OnClick="btnCreateAccount_Click" />&nbsp;&nbsp;or&nbsp;&nbsp;<a href="javascript:switchView('signIn')">back to sign in</a>
                </div>
		        <div id="msgSignInCreateAccount_div" style="display:none;margin-top:10px;width:100%"></div>
	        </div><!-- /divSignInCreateAccount -->        
	    </div><!-- /divSignInCreateAccountContainer -->    
    </div>
    </form>
    <script type="text/javascript" language="javascript">
        init();
        function init() {
            // set focus to email input
            gel('<%=txtEmail.ClientID %>').focus();
        }
        function gel(id) {
            return document.getElementById(id);
        }
        function show(ctl) {
            ctl.style.display = "block";
        }
        function hide(ctl) {
            ctl.style.display = "none";
        }
        function showInline(ctl) {
            ctl.style.display = "inline";
        }
        function emailIsValid(value) {
            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            return emailPattern.test(value);
        }
        function varIsNothing(value) {
            return (value == null || value == "" || value == " ");
        }
        function switchView(view) {
            switch (view.toLowerCase()) {
                case 'signin':
                    show(gel('divSignIn'));
                    hide(gel('divCreateAccount'));
                    hide(gel('divPasswordHint'));
                    hide(gel('divRetypePassword'));
                    gel('<%=txtEmail.ClientID %>').focus();
                    // clearMessages(); // clear out any messages
                    hide(gel('spnReqEmail')); // hide required * for inputs
                    hide(gel('spnReqPassword'));
                    // // gel("btnSignIn").disabled = false; // done loading; re-enable button							
                    break;
                case 'createaccount':
                    document.getElementById("divCreateAccount").style.display = "block";
                    show(gel('divCreateAccount'));
                    hide(gel('divSignIn'));
                    show(gel('divPasswordHint'));
                    show(gel('divRetypePassword'));
                    gel('<%=txtEmail.ClientID %>').focus();
                    // displayMessage("<i>Please provide an email, password and password hint to create your account.</i>");
                    showInline(gel('spnReqEmail')); // show required * for inputs
                    showInline(gel('spnReqPassword'));
                    // // gel("btnCreateAccount").disabled = false; // done loading; re-enable button
                    break;
                // default:   
            } // switch
            // currentView = view;
        } // switchView(...)
        function validateSignIn() {
            var inputIsValid = (!varIsNothing(gel('<%=txtEmail.ClientID %>').value) && emailIsValid(gel('<%=txtEmail.ClientID %>').value) && !varIsNothing(gel('<%=txtPassword.ClientID %>').value));
            // displayMessage("<font color='red'>Error authenticating user remotely, please try again.</font>", true);
            if (!inputIsValid)
                alert("sign in failed - bad or insufficient input");
            return inputIsValid;
        } // signIn()
        function validateCreateAccount() {
            var inputIsValid = (!varIsNothing(gel('<%=txtEmail.ClientID %>').value) && emailIsValid(gel('<%=txtEmail.ClientID %>').value) && !varIsNothing(gel('<%=txtPassword.ClientID %>').value) && !varIsNothing(gel('<%=txtRetypePassword.ClientID %>').value) && !varIsNothing(gel('<%=txtPasswordHint.ClientID %>').value) && (gel('<%=txtPassword.ClientID %>').value == gel('<%=txtRetypePassword.ClientID %>').value));
            if (!inputIsValid)
                alert("account create failed - bad or insufficient input");
            return inputIsValid;
        } // createAccount()               
    </script>    
</body>
</html>
