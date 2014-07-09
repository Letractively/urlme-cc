ianhd.registerNamespace("app");
ianhd.app = {
    controls: {
        logo: function () { return $("#logo"); },
        menu: function () { return $("nav"); },
        menuIcon: function () { return $(".fa-bars"); },
        movie: function () { return $("#movie"); },
        overlay: function () { return $("#overlay"); },
        searchBox: function() { return $("input.search"); },
        searchIcon: function () { return $(".fa-search"); },
        theater: function () { return $(".theater"); },
        zip: function () { return $(".zip"); }
    },
    selectors: {
        closePopup: "#overlay,.closePopup"
    },
    init: function () {
        // init history
        var History = window.History;

        // check if browser can support this site
        if (typeof localStorage == 'undefined' || !navigator.geolocation || !History.enabled) {
            document.writeln("Please use a more recent browser to view this site.  I recommend <a href='http://www.google.com/chrome'>Chrome</a>.");
            return;
        }

        // set height of overlay
        ianhd.app.controls.overlay().css("height", $(document).height());

        ianhd.app.initHistory(History);
        ianhd.app.bindControls();
    },
    handlePushState: function (hash) {
        var parts = hash.split('/');
        var first = parts[1]; // showtimes, {movieTitle}
        var second = parts[2]; // {movieId}

        switch (first) {
            case "": // homepage
                viewModel.overlayMovieId("");
                console.log("Loading homepage");
                break;
            case "showtimes":
                console.log("Loading showtimes");
                break;
            default:
                if (viewModel.overlayMovieId()) return; // movie is already loaded from server-side, so leave it alone
                $.get('/movie/' + second, function (resp) {
                    ianhd.app.controls.movie().html(resp);
                    viewModel.overlayMovieId("some-movie");
                });
                console.log("Loading movie");
                break;
        }
    },
    bindControls: function () {
        // close popup
        $(document).on('click', ianhd.app.selectors.closePopup, function (e) {
            History.pushState(null, null, "/");
        });

        // theater prompt
        ianhd.app.controls.theater().click(function (e) {
            e.preventDefault();
            BootstrapDialog.show({
                title: "Select a theater",
                message: "<a href='#'>* All Theaters *</a><a href='#'>Theater 1</a>",
                cssClass: "selectTheater",
                buttons: [{
                    label: 'Cancel',
                    cssClass: 'btn-default',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
        });

        // zip prompt
        ianhd.app.controls.zip().click(function (e) {
            e.preventDefault();
            BootstrapDialog.show({
                title: "Select a zip code",
                message: "<form class='form-inline'><input class='form-control' class='enterZip' placeholder='Enter zip code...' /><button class='btn btn-primary'>Go</button></form>",
                cssClass: "selectZip",
                buttons: [{
                    label: 'Cancel',
                    cssClass: 'btn-default',
                    action: function (dialog) {
                        dialog.close();
                    }
                }]
            });
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
    initHistory: function (History) {
        // handle any state changes
        History.Adapter.bind(window, 'statechange', function () {
            var state = History.getState();
            ianhd.app.handlePushState(state.hash);
        });

        // handle "a" clicks - prevent their default and instead push state
        $(document).on('click', 'a[href^="/"]:not(.noPush)', function (e) {
            e.preventDefault();
            History.pushState(null, null, $(this).attr("href"));
        });

        // handle initial page's load
        var initState = History.getState();
        ianhd.app.handlePushState(initState.hash);
    }
};

$(function () {
    ianhd.app.init();
});