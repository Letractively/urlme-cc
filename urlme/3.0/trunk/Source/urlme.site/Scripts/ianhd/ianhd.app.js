ianhd.registerNamespace("app");

ianhd.app = {
	controls: {
	    shortenUrl: function () { return $("button[type='submit']"); },
	    success: function () { return $(".alert-success"); },
	    theRealCopy: function () { return $("#theRealCopy"); },
	},
	selectors: {
	    copyLinks: '.copy',
	    deleteLinks: '.delete',
        overwrite: '.overwrite',
	},
	init: function () {
	    // $("abbr.timeago").timeago();

	    ianhd.app.loadData();
        ianhd.app.bindControls();
        ianhd.app.initZeroClipboard();
        ianhd.app.removeHash();
	},
	findItemIndex: function (elemInsideOfTr) {
	    var itemId = elemInsideOfTr.closest('tr').attr("data-item-id");
	    var found = false;
	    var itemIdx = -1;
	    var findItem = $.grep(viewModel.items(), function (item, i) {
	        if (!found && item.LinkId == itemId) {
	            found = true;
	            itemIdx = i;
	            return true;
	        }
	    });

	    return itemIdx;
    },
	initZeroClipboard: function () {
	    ianhd.app.controls.theRealCopy().zclip({
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
	    $(document).on('mouseover', ianhd.app.selectors.copyLinks, function (e) {
	        e.stopPropagation();
	        var trigger = $(this);

	        // turn on "hover" class
	        trigger.addClass("hover");

	        // set toCopy
	        if (trigger.hasClass("outputCopy")) {
	            viewModel.toCopy(viewModel.result());
	            viewModel.copyTriggerSelector(".outputCopy");
	        } else {
	            var itemIdx = ianhd.app.findItemIndex(trigger);
	            var item = viewModel.items()[itemIdx];
	            viewModel.toCopy(item.ShortUrl);
	            viewModel.copyTriggerSelector("tr[data-item-id='{0}'] .copy".format(trigger.closest("tr").attr("data-item-id")));
	        }

	        // move swf to over this element
	        $(".zclip").css("left", trigger.offset().left).css("top", trigger.offset().top);
	    });

	    $(document).on('mouseover', "table,.linkFormContainer", function () {
	        var trigger = $(this);

	        // turn off inner .copy's hover class
	        trigger.find(".copy").removeClass("hover");

	        // move zclip back to its original position, somewhere off the page
	        $(".zclip").css("left", -100).css("top", -100);
	    });
	},
	clearViewModel: function () {
	    viewModel.longUrl("");
	    viewModel.path("");
	    viewModel.message("");
	},
	bindControls: function () {
        // overwrite
	    $(document).on("click", ianhd.app.selectors.overwrite, function (e) {
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

	                            // update datatable - get row
	                            var item = $("tr[data-item-id='{0}']".format(itemId))[0];
	                            var rowIndex = dt.fnGetPosition(item);
	                            var newLongUrl = "<a target='_blank' title='{0}' href='{0}'>{1}</a>".format(resp.Item.DestinationUrl, resp.Item.LongUrl);
                                dt.fnUpdate(newLongUrl, rowIndex, 0);

                                ianhd.app.showSuccess();
	                            ianhd.app.clearViewModel();
	                        } else {
	                            viewModel.message(resp.Message);
	                        }
	                    });

	                    dialog.close();
	                }
	            }]
	        });
	    });

	    // delete link
	    $(document).on("click", ianhd.app.selectors.deleteLinks, function (e) {
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
	                    ianhd.app.showSuccess();

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
		ianhd.app.controls.shortenUrl().click(function (e) {
			e.preventDefault();
			var data = ko.mapping.toJS(viewModel);

            // custom path provided?
			if ($.trim(viewModel.path())) {
			    // create link on server
			    data = { destinationUrl: data.longUrl, path: data.path };

			    $.post("links", data, function (resp) {
			        if (resp.WasSuccessful) {
			            viewModel.result(resp.Item.ShortUrl);
			            // if this is the user's first item, then load all the data
			            if (!viewModel.items().length) {
			                ianhd.app.loadData();
			            } else {
			                viewModel.items.push(resp.Item);
			            }
			            ianhd.app.clearViewModel();
			        } else {
			            if (resp.ResultEnum === "UserAlreadyHasLink") {
			                var link = "<a href='{0}' target='_blank' title='{0}'>link</a>".format(resp.Item.DestinationUrl);
			                var overwrite = "<a href='#' class='overwrite' data-item-id='{0}'>Overwrite it</a>".format(resp.Item.LinkId);
			                resp.Message = resp.Message
                                .replace("link", link)
                                .replace("Overwrite it", overwrite);
			            }
			            viewModel.message(resp.Message);
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
                        ianhd.app.clearViewModel();
                    }
                });
			}
		});
	},
	showSuccess: function () {
	    ianhd.app.controls.success().fadeIn(300).delay(2000).fadeOut(500);
	},
	loadData: function () {
	    if (!viewModel.signedIn()) return;
	    $.get('links', function (resp) {
	        viewModel.items(resp);
	        viewModel.loading(false);

            // do not dataTable-ize it until we have items
	        if (!viewModel.items().length) return;

	        // datatable-ize the table
	        dt = $("#example").dataTable({
	            "aaSorting": [[2, "desc"]]
	        });
	    });
	},
	removeHash: function () {
	    history.pushState("", document.title, window.location.pathname + window.location.search);
	},
};

$(function () {
	ianhd.app.init();
});