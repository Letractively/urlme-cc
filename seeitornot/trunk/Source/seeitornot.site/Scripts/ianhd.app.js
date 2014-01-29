ianhd.registerNamespace("app");
ianhd.app = {
    init: function () {
        // init history
        var History = window.History;

        // check if browser can support this site
        if (typeof localStorage == 'undefined' || !navigator.geolocation || !History.enabled) {
            document.writeln("Please use a more recent browser to view this site.  I recommend <a href='http://www.google.com/chrome'>Chrome</a>.");
            return;
        }

        ianhd.app.initHistory(History);
    },
    handlePushState: function (hash) {
        var parts = hash.split('/');
        var first = parts[0]; // showtimes, {movieTitle}
        var second = parts[1]; // {movieId}

        switch (first) {
            case "":
                break;
            case "showtimes":
                break;
            default:
                alert('loading movie...');
                break;
        }
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