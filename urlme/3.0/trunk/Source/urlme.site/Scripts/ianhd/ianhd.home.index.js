ianhd.registerNamespace("home.index");

ianhd.home.index = {
	controls: {
	    copyLink: function () { return $("#copyLink"); },
	    shortenUrl: function () { return $("button[type='submit']"); },
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
	loadData: function () {
	    if (!viewModel.signedIn()) { return; }
	    $.get('/link', function (resp) {
	        viewModel.items(resp);
	    });
	},
	removeHash: function () {
	    history.pushState("", document.title, window.location.pathname + window.location.search);
	},
};

$(function () {
	ianhd.home.index.init();
});