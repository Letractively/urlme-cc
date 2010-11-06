<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div style="display:none;">
<% using (this.Html.BeginForm(new { controller = "Account", action = "OpenIdLogin" }))
    { %>
    <input id="openid_identifier" name="openid_identifier" type="hidden" value="https://www.google.com/accounts/o8/id" />             
    <input class="login-button" type="submit" value="Sign in with Google" />
<% } %>
</div>

<form action="account/openidlogin" method="post">
    <input id="Hidden1" name="openid_identifier" type="hidden" value="https://www.google.com/accounts/o8/id" />             
    <input class="login-button" type="submit" value="Sign in with Google" />
</form>
