ianhd.registerNamespace("bookmarklet");

ianhd.bookmarklet = {
	controls: {
	    close: function () { return $(".closeWindow"); },
	    shortenUrl: function () { return $("button[type='submit']"); },
	},
	selectors: {
        overwrite: '.overwrite',
	},
	init: function () {
	    ianhd.bookmarklet.bindControls();
	    ianhd.bookmarklet.removeHash();
	},
	bindControls: function () {
	    // shorten url
	    ianhd.bookmarklet.controls.shortenUrl().click(function (e) {
	        e.preventDefault();
	        alert('clicked');
	        var data = ko.mapping.toJS(viewModel);

	        // custom path provided?
	        if ($.trim(viewModel.path())) {
	            // create link on server
	            data = { destinationUrl: data.longUrl, path: data.path };

	            $.post("links", data, function (resp) {
	                if (resp.WasSuccessful) {
	                    viewModel.result(resp.Item.ShortUrl);
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

	    // cancel/close window
	    ianhd.bookmarklet.controls.close().click(function (e) {
	        e.preventDefault();
	        window.close();
	    });

	},
	clearViewModel: function () {
	    viewModel.longUrl("");
	    viewModel.path("");
	    viewModel.message("");
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