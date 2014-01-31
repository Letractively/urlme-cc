ianhd.registerNamespace("app");
ianhd.app = {
    controls: {
        movie: function () { return $("#movie"); },
        overlay: function () { return $("#overlay"); }
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
        $(document).on('click', ianhd.app.selectors.closePopup, function (e) {
            History.pushState(null, null, "/");
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