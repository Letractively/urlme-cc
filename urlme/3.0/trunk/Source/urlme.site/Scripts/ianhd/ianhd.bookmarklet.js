// @koala-prepend "../jquery-1.10.2.min.js"
// @koala-prepend "../knockout-3.1.0.js"
// @koala-prepend "../knockout.mapping.js"
// @koala-prepend "../plugins/zclip/jquery.zclip.min.js"
// @koala-prepend "../plugins/datatables/jquery.dataTables.min.js"
// @koala-prepend "../plugins/datatables/datatables.bootstrap.js"
// @koala-prepend "../plugins/bootstrap-dialog/bootstrap-dialog.min.js"
// @koala-prepend "../bootstrap.min.js",
// @koala-prepend "../respond.js"));
// @koala-prepend "ianhd.js"

var viewModel = ko.mapping.fromJS({ 
    result: '',
    error: '',
    success: '',
    longUrl: longUrl,
    path: '',
    signedIn: si,
    toCopy: '',
    copyTriggerSelector: '',
});
ko.applyBindings(viewModel);

ianhd.registerNamespace("bookmarklet");

ianhd.bookmarklet = {
	controls: {
	    close: function () { return $(".closeWindow"); },
	    shortenUrl: function () { return $("button[type='submit']"); },
	    success: function () { return $(".alert-success.primary"); },
	    theRealCopy: function () { return $("#theRealCopy"); },
	},
	selectors: {
	    copyLinks: '.copy',
	    overwrite: '.overwrite',
	},
	init: function () {
	    ianhd.bookmarklet.initZeroClipboard();
	    ianhd.bookmarklet.bindControls();
	    ianhd.bookmarklet.removeHash();
	    ianhd.bookmarklet.listenForSignedIn();
	},
	initZeroClipboard: function () {
	    ianhd.bookmarklet.controls.theRealCopy().zclip({
	        path: 'ZeroClipboard.swf',
	        copy: function () {
	            return viewModel.toCopy();
	        },
	        afterCopy: function () {
	            var copyTrigger = $(viewModel.copyTriggerSelector());
	            var copyHtml = copyTrigger.html();
	            copyTrigger.html("<span style='text-align: center; width: 46px; display:inline-block; color: green;'><i class='fa fa-check'></i></span>");
	            setTimeout(function () {
	                copyTrigger.html(copyHtml);
	            }, 1500);
	        }
	    });
	    $(document).on('mouseover', ianhd.bookmarklet.selectors.copyLinks, function (e) {
	        e.stopPropagation();

	        var trigger = $(this);

	        // turn on "hover" class
	        trigger.addClass("hover");

	        // set toCopy
	        viewModel.toCopy(viewModel.result());
	        viewModel.copyTriggerSelector(".copy");

	        // move swf to over this element
	        $(".zclip").css("left", trigger.offset().left).css("top", trigger.offset().top);
	    });

	    $(document).on('mouseover', ".addBody", function () {
	        var trigger = $(this);

	        // turn off inner .copy's hover class
	        $(".copy").removeClass("hover");

	        // move zclip back to its original position, somewhere off the page
	        $(".zclip").css("left", -100).css("top", -100);
	    });
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
	                    ianhd.bookmarklet.showSuccess();
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