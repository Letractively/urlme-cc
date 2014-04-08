﻿ianhd.registerNamespace("home.index");

ianhd.home.index = {
	controls: {
	    copyLink: function () { return $("#copyLink"); },
	    shortenUrl: function () { return $("button[type='submit']"); },
	},
	init: function () {
	    $('#example').dataTable();

	    ianhd.home.index.bindControls();
	    ianhd.home.index.initZeroClipboard();
	},
	initZeroClipboard: function () {
	},
	bindControls: function () {
		// shorten url
		ianhd.home.index.controls.shortenUrl().click(function (e) {
			e.preventDefault();
			var data = ko.mapping.toJS(viewModel);

			$.ajax("https://www.googleapis.com/urlshortener/v1/url",
            {
                data: JSON.stringify(data),
                contentType: 'application/json',
                type: 'POST',
                success: function (resp) {
                    viewModel.result(resp.id);
                }
            });
		});
	},
};

$(function () {
	ianhd.home.index.init();
});