ianhd.registerNamespace("home.index");

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
	                        url: "/link/" + itemId,
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
	showSuccess: function () {
	    ianhd.home.index.controls.success().fadeIn(300).delay(1500).fadeOut(500);
	},
	loadData: function () {
	    if (!viewModel.signedIn()) { return; }
	    $.get('/link', function (resp) {
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