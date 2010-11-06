<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SandBox.aspx.cs" Inherits="UrlMe.cc.SandBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="<%=ResolveUrl("~/") %>Resource/javascript/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/") %>Resource/javascript/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
    <style type="text/css">
	    #sortable { list-style-type: none; margin: 0; padding: 0; width: 60%; }
	    #sortable li { margin: 0 5px 5px 5px; padding: 5px; /* font-size: 1.2em; height: 1.5em; */ border: solid 1px black; }
	    html>body #sortable li { /* height: 1.5em; line-height: 1.2em; */ }
	    .ui-state-highlight { height: 1.5em; line-height: 1.2em; }
    </style>    
</head>
<body>
    <div class="demo">
        <ul id="sortable">
	        <li class="ui-state-default">Item 1</li>
	        <li class="ui-state-default">Item 2</li>
	        <li class="ui-state-default">Item 3</li>
	        <li class="ui-state-default">Item 4</li>
	        <li class="ui-state-default">Item 5</li>
	        <li class="ui-state-default">Item 6</li>
	        <li class="ui-state-default">Item 7</li>
        </ul>
    </div><!-- End demo -->

    <div class="demo-description">
        <p>
	        When dragging a sortable item to a new location, other items will make room
	        for the that item by shifting to allow white space between them. Pass a
	        class into the <code>placeholder</code> option to style that space to
	        be visible.  Use the boolean <code>forcePlaceholderSize</code> option
	        to set dimensions on the placeholder.
        </p>
    </div><!-- End demo-description -->
    
	<script type="text/javascript">
	    $(document).ready(function() {
	        $("#sortable").sortable({
	            placeholder: 'ui-state-highlight'
	        });
	        $("#sortable").disableSelection();
	        $('#sortable').bind('sortupdate', function(event, ui) {
	            alert('updated!');
	        });
	    });
	</script>        
</body>
</html>
