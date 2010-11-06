<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UrlMe - Help
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Help</h2>

    <h3>How do I add a bookmarklet to add a urlme link for the website I'm on?</h3>
    <ol class="First">
        <li>Drag 'n' drop the following link into the Links toolbar of your browser: <a href="javascript:(function(){ window.open('http://urlme.cc/add/?url=' + encodeURIComponent(document.location), 'UrlMe', 'status=1,height=218,width=400'); })();" class="Bookmarklet">+urlme</a></li>
        <li>Click on that guy whenever you want to add a urlme link on the fly, specify what path you want to use, and you're a Go!</li>
        <li>While you're at it, add this to get to all your links: <a href="http://urlme.cc" class="Bookmarklet">view urlme's</a></li>
    </ol>

    <h3>How do I sign in with a different Google account?</h3>
    <ol>
        <li>Click "Revoke Access" next to urlme.cc on this page (opens in new tab): <a target="_blank" href="http://urlme.cc/revoke">http://urlme.cc/revoke</a></li>
        <li>Come back here, and click Sign out in the upper-right corner, then sign back into urlme.cc</li>
        <li>Provide a different Google username/password, or click "Sign in as a different user" to change your Google account if you're already signed into Google</li>
        <li>That should do it!</li>
    </ol>

</asp:Content>
