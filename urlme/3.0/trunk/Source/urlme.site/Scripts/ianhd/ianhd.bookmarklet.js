﻿ianhd.registerNamespace("bookmarklet");

ianhd.bookmarklet = {
	controls: {
	    close: function () { return $(".closeWindow"); },
	    shortenUrl: function () { return $("button[type='submit']"); },
	    success: function () { return $(".alert-success.primary"); },
	},
	selectors: {
        overwrite: '.overwrite',
	},
	init: function () {
	    ianhd.bookmarklet.bindControls();
	    ianhd.bookmarklet.removeHash();
	    ianhd.bookmarklet.listenForSignedIn();
	},
	bindControls: function () {
	    // shorten url
	    ianhd.bookmarklet.controls.shortenUrl().click(function (e) {
	        e.preventDefault();
	        var data = ko.mapping.toJS(viewModel);

	        // custom path provided?
	        if ($.trim(viewModel.path())) {
	            // create link on server
	            data = { destinationUrl: data.longUrl, path: data.path };

	            $.post("links", data, function (resp) {
	                if (resp.WasSuccessful) {
	                    viewModel.result(resp.Item.ShortUrl);
	                    ianhd.bookmarklet.clearViewModel();
	                } else {
	                    if (resp.ResultEnum === "UserAlreadyHasLink") {
	                        var link = "<a href='{0}' target='_blank' title='{0}'>link</a>".format(resp.Item.DestinationUrl);
	                        var overwrite = "<a href='#' class='overwrite' data-item-id='{0}'>Overwrite it</a>".format(resp.Item.LinkId);
	                        resp.Message = resp.Message
                                .replace("link", link)
                                .replace("Overwrite it", overwrite);
	                    }
	                    viewModel.error(resp.Message);
	                    viewModel.success("");
	                    viewModel.result("");
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
                        ianhd.bookmarklet.clearViewModel();
                    }
                });
	        }
	    });

	    // overwrite
	    $(document).on("click", ianhd.bookmarklet.selectors.overwrite, function (e) {
	        e.preventDefault();
	        var el = $(this);

	        BootstrapDialog.show({
	            title: 'Please Confirm',
	            message: 'Are you sure?',
	            cssClass: 'login-dialog',
	            buttons: [{
	                label: 'Yes, overwrite',
	                cssClass: 'btn-primary',
	                action: function (dialog) {
	                    var itemId = el.attr("data-item-id");
	                    var data = { destinationUrl: viewModel.longUrl(), linkId: itemId };

	                    $.post("links/overwrite", data, function (resp) {
	                        if (resp.WasSuccessful) {
	                            viewModel.result(resp.Item.ShortUrl);
	                            ianhd.bookmarklet.showSuccess();
	                            ianhd.bookmarklet.clearViewModel();
	                        } else {
	                            viewModel.error(resp.Message);
	                            viewModel.success("");
	                        }
	                    });

	                    dialog.close();
	                }
	            }]
	        });
	    });

	    // cancel/close window
	    ianhd.bookmarklet.controls.close().click(function (e) {
	        e.preventDefault();
	        window.close();
	    });
	},
	listenForSignedIn: function() {
	    if (viewModel.signedIn()) return;

	    setInterval(function() {
	        if (!viewModel.signedIn()) {
	            $.get("account/signed-in", function (resp) {
	                if (resp.signedIn) {
	                    viewModel.signedIn(true);
	                    viewModel.success("You've been signed in.");
	                }
	            });
	        }
	    }, 1000);
	},
	clearViewModel: function () {
	    viewModel.longUrl("");
	    viewModel.path("");
	    viewModel.error("");
	    viewModel.success("");
	},
	showSuccess: function () {
	    ianhd.bookmarklet.controls.success().fadeIn(300).delay(2000).fadeOut(500);
	},
	removeHash: function () {
	    history.pushState("", document.title, window.location.pathname + window.location.search);
	},
};

$(function () {
	ianhd.bookmarklet.init();
});