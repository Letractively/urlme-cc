ianhd.registerNamespace("bookmarklet");

ianhd.bookmarklet = {
	controls: {
	    close: function () { return $(".close"); },
	},
	selectors: {
        overwrite: '.overwrite',
	},
	init: function () {
	    ianhd.bookmarklet.bindControls();
	    ianhd.bookmarklet.removeHash();
	},
	bindControls: function () {
	    // cancel/close window
	    ianhd.bookmarklet.controls.close().click(function (e) {
	        e.preventDefault();
	        window.close();
	    });
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