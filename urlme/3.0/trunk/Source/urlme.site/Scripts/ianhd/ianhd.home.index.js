ianhd.registerNamespace("home.index");

ianhd.home.index = {
	controls: {
	    copyLink: function () { return $("#copyLink"); },
	    shortenUrl: function () { return $("button[type='submit']"); },
	},
	init: function () {
	    ianhd.home.index.bindControls();
	    ianhd.home.index.initZeroClipboard();
	},
	initZeroClipboard: function () {
	    // zero clipboard
	    ZeroClipboard.config({ moviePath: "/scripts/plugins/zeroclipboard/ZeroClipboard.swf" });
	    var client = new ZeroClipboard($("#copyLink"));
	    $("#copyLink").attr("data-clipboard-text", "hiyooo");

	    client.on("load", function (client) {
	        client.on("complete", function (client, args) {
	            alert("Copied text to clipboard: " + args.text);
	        });
	    });
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
                    viewModel.link(resp.id);
                }
            });
		});
	},
};

$(function () {
	ianhd.home.index.init();
});