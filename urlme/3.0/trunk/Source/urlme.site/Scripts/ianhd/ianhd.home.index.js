ianhd.registerNamespace("home.index");

ianhd.home.index = {
	controls: {
		shortenUrl: function () { return $("button[type='submit']"); },
	},
	init: function () {
		ianhd.home.index.bindControls();
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