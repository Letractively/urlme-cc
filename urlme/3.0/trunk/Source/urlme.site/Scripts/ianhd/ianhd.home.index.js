ianhd.registerNamespace("home.index");

ianhd.home.index = {
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

	    ianhd.home.index.loadData();
        ianhd.home.index.bindControls();
        ianhd.home.index.initZeroClipboard();
        ianhd.home.index.removeHash();
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
	    ianhd.home.index.controls.theRealCopy().zclip({
	        path: 'ZeroClipboard.swf',
	        copy: function () {
	            return viewModel.toCopy();
	        },
	        afterCopy: function () {
	            ianhd.home.index.showSuccess();
	        }
	    });
	    $(document).on('mouseover', ianhd.home.index.selectors.copyLinks, function () {
	        var trigger = $(this);

	        // turn on "hover" class
	        //trigger.addClass("hover");

	        // set toCopy
	        var itemIdx = ianhd.home.index.findItemIndex(trigger);
	        var item = viewModel.items()[itemIdx];
	        viewModel.toCopy(item.ShortUrl);

	        // move swf to over this element
	        $(".zclip").css("left", trigger.offset().left).css("top", trigger.offset().top);
	    });

	    $(document).on('mouseout', ianhd.home.index.selectors.copyLinks, function () {
	        var trigger = $(this);

	        // turn off "hover" class
	        //trigger.removeClass("hover");
	    });
	},
	clearViewModel: function () {
	    viewModel.longUrl("");
	    viewModel.path("");
	    viewModel.message("");
	},
	bindControls: function () {
        // overwrite
	    $(document).on("click", ianhd.home.index.selectors.overwrite, function (e) {
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

                                ianhd.home.index.showSuccess();
	                            ianhd.home.index.clearViewModel();
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

			    $.post("links", data, function (resp) {
			        if (resp.WasSuccessful) {
			            viewModel.result(resp.Item.ShortUrl);
			            // if this is the user's first item, then load all the data
			            if (!viewModel.items().length) {
			                ianhd.home.index.loadData();
			            } else {
			                viewModel.items.push(resp.Item);
			            }
			            ianhd.home.index.clearViewModel();
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
                        ianhd.home.index.clearViewModel();
                    }
                });
			}
		});
	},
	showSuccess: function () {
	    ianhd.home.index.controls.success().fadeIn(300).delay(2000).fadeOut(500);
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
	ianhd.home.index.init();
});