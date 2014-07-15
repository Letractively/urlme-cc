﻿viewModel.zip.subscribe(function (newVal) {
    localStorage.setItem("zip", newVal);
});

var router = new Router();

ianhd.registerNamespace("app");
ianhd.app = {
    controls: {
        logo: function () { return $("#logo"); },
        menu: function () { return $("nav"); },
        menuIcon: function () { return $(".fa-bars"); },
        movie: function () { return $("#movie"); },
        nearMe: function() { return $("#nearMe"); },
        overlay: function () { return $("#overlay"); },
        searchBox: function() { return $("input.search"); },
        searchIcon: function () { return $(".fa-search"); },
        selectTheaterBody: function () { return $(".selectTheater .bootstrap-dialog-message"); },
        theater: function () { return $(".theater"); },
        zip: function () { return $(".zip"); },
    },
    selectors: {
        closePopup: "#overlay,.closePopup",
        selectTheater: ".selectTheater a",
    },
    init: function () {
        // check if browser can support this site
        if (typeof localStorage == 'undefined' || !navigator.geolocation) {
            document.writeln("Please use a more recent browser to view this site.  I recommend <a href='http://www.google.com/chrome'>Chrome</a>.");
            return;
        }

        // set height of overlay
        ianhd.app.controls.overlay().css("height", $(document).height());

        ianhd.app.initHistory();
        ianhd.app.bindControls();
    },
    loadShowtimes: function (zip, theaterId) {
        //console.log('loading showtimes...');
    },
    bindControls: function () {
        // close popup
        $(document).on('click', ianhd.app.selectors.closePopup, function (e) {
            //History.pushState(null, null, "/");
        });

        $(document).on('click', ianhd.app.selectors.selectTheater, function (e) {
            e.preventDefault();
            router.navigate($(this).attr("href"));
        });

        // use my location button
        ianhd.app.controls.nearMe().click(function (e) {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var url = "http://api.geonames.org/findNearbyPostalCodesJSON?lat={0}&lng={1}&username=codejkjk".format(position.coords.latitude, position.coords.longitude);
                    $.ajax({
                        url: url,
                        dataType: "jsonp",
                        success: function (resp) { viewModel.zip(resp.postalCodes[0].postalCode); }, // todo: check if postalCodes[0]
                        error: function () { }
                    });
                }, function (error) {
                    alert("Error, please try again.");
                });
            }
        });

        // theater prompt
        ianhd.app.controls.theater().click(function (e) {
            e.preventDefault();
            BootstrapDialog.show({
                title: "Select a theater",
                message: "<span class='hint'>Loading...</span>",
                cssClass: "selectTheater",
                buttons: [{
                    label: 'Cancel',
                    cssClass: 'btn-default',
                    action: function (dialog) {
                        dialog.close();
                    }
                }],
                onshown: function () {
                    $.get(apiBaseUrl + "api/theaters?zip=" + viewModel.zip(), function (theaters) {
                        var dialogBody = ianhd.app.controls.selectTheaterBody();
                        dialogBody.html(""); // clear out any "Loading..." messages
                        dialogBody.append("<a href='/showtimes/{0}/all'>* All Theaters *</a>".format(viewModel.zip()));
                        $.each(theaters, function (i, theater) {
                            dialogBody.append("<a href='/showtimes/{0}/{1}'>{2}</a>".format(viewModel.zip(), theater.id, theater.name));
                        });
                    });
                }
            });
        });

        // zip prompt
        ianhd.app.controls.zip().click(function (e) {
            e.preventDefault();
            viewModel.zip("");
            router.navigate("/", false);
        });

        // menu icon
        ianhd.app.controls.menuIcon().click(function (e) {
            e.preventDefault();
            var trigger = $(this);
            trigger.toggleClass("fa-bars fa-times");

            ianhd.app.controls.logo().toggle();
            ianhd.app.controls.searchIcon().toggle();

            var menu = ianhd.app.controls.menu()
            if (trigger.hasClass("fa-times")) {
                menu.slideDown();
            } else {
                menu.slideUp();
            }
        });

        // search icon
        ianhd.app.controls.searchIcon().click(function (e) {
            e.preventDefault();
            var trigger = $(this);
            trigger.toggleClass("fa-search fa-times");

            var searchBox = ianhd.app.controls.searchBox();
            searchBox.toggleClass("display");
            ianhd.app.controls.logo().toggle();

            if (searchBox.hasClass("display")) {
                searchBox.focus();
            }
        });
    },
    initHistory: function () {
        router.route('/showtimes/:zip/:theaterId', function (zip, theaterId) {
            console.log('route /showtimes/:zip/:theaterId');
            ianhd.app.loadShowtimes(zip, theaterId);
        });
        router.route('/', function (zip, theaterId) {
            console.log('route /');
        });

        // trigger initial route
        //router.navigate(window.location.pathname);
    }
};

$(function () {
    ianhd.app.init();
});