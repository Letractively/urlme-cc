﻿ianhd.registerNamespace("home.index");

ianhd.home.index = {
	controls: {
	    copyLink: function () { return $("#copyLink"); },
	    shortenUrl: function () { return $("button[type='submit']"); },
	    success: function () { return $(".alert-success"); },
	},
	selectors: {
	    deleteLinks: ".delete"
	},
	init: function () {
	    $("abbr.timeago").timeago();
	    //$('#example').dataTable();

	    ianhd.home.index.loadData();
        ianhd.home.index.bindControls();
        ianhd.home.index.initZeroClipboard();
        ianhd.home.index.removeHash();
	},
	initZeroClipboard: function () {
	},
	clearViewModel: function () {
	    viewModel.longUrl("");
	    viewModel.path("");
	    viewModel.message("");
	},
	bindControls: function () {
	    // delete link
	    $(document).on("click", ianhd.home.index.selectors.deleteLinks, function (e) {
	        e.preventDefault();
	        var el = $(this);

	        BootstrapDialog.show({
	            title: 'Please Confirm',
	            message: 'Are you sure?',
	            cssClass: 'login-dialog',
	            buttons: [{
	                label: 'Yes, delete',
	                cssClass: 'btn-primary',
	                action: function (dialog) {
	                    var item = el.closest('tr')[0];
	                    var itemId = item.attributes["data-item-id"].value;
	                    var rowIndex = dt.fnGetPosition(item);
	                    dt.fnDeleteRow(rowIndex);
	                    ianhd.home.index.showSuccess();

	                    $.ajax({
	                        url: "links/" + itemId,
	                        type: 'DELETE',
	                        success: function (resp) { /* silent */ },
	                        error: function () { alert("Error :/"); }
	                    });
	                    dialog.close();
	                }
	            }]
	        });
	    });

	    // shorten url
		ianhd.home.index.controls.shortenUrl().click(function (e) {
			e.preventDefault();
			var data = ko.mapping.toJS(viewModel);

            // custom path provided?
			if ($.trim(viewModel.path())) {
			    // create link on server
			    data = { destinationUrl: data.longUrl, path: data.path };

			    // use goo.gl to create link
			    $.ajax("links",
                {
                    data: JSON.stringify(data),
                    contentType: 'application/json',
                    type: 'POST',
                    success: function (resp) {
                        if (resp.WasSuccessful) {
                            viewModel.result('http://urlme.cc/' + resp.Item.ShortUrl);
                            ianhd.home.index.clearViewModel();
                        } else {
                            if (resp.ResultEnum === "UserAlreadyHasLink") {
                                var link = "<a href='{0}' target='_blank'>link</a>".format(resp.Item.DestinationUrl);
                                resp.Message = resp.Message.replace("link", link);
                            }
                            viewModel.message(resp.Message);
                        }
                    }
                });
			} else {
			    // trim data obj to only what goo.gl needs
			    data = { longUrl: data.longUrl };

			    // use goo.gl to create link
			    $.ajax("https://www.googleapis.com/urlshortener/v1/url",
                {
                    data: JSON.stringify(data),
                    contentType: 'application/json',
                    type: 'POST',
                    success: function (resp) {
                        viewModel.result(resp.id);
                        ianhd.home.index.clearViewModel();
                    }
                });
			}
		});
	},
	showSuccess: function () {
	    ianhd.home.index.controls.success().fadeIn(300).delay(1500).fadeOut(500);
	},
	loadData: function () {
	    if (!viewModel.signedIn()) { return; }
	    $.get('links', function (resp) {
	        viewModel.items(resp);
	        // datatable-ize the table
	        dt = $("#example").dataTable();
	    });
	},
	removeHash: function () {
	    history.pushState("", document.title, window.location.pathname + window.location.search);
	},
};

$(function () {
	ianhd.home.index.init();
});