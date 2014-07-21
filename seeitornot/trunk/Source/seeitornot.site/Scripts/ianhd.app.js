viewModel.zip.subscribe(function (newVal) {
    localStorage.setItem("zip", newVal);
});
viewModel.theaterName.subscribe(function (newVal) {
    localStorage.setItem("theaterName", newVal);
});
viewModel.theaterId.subscribe(function (newVal) {
    localStorage.setItem("theaterId", newVal);
});

var router = new Router();

ianhd.registerNamespace("app");
ianhd.app = {
    controls: {
        enterZip: function () { return $(".enterZip"); },
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
        theatersWithMovies: function () { return $("#theatersWithMovies"); },
        zip: function () { return $(".zip"); },
    },
    selectors: {
        closePopup: "#overlay,.closePopup",
        selectMovie: "#theatersWithMovies .movie a",
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
    loadTheaters: function () {
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
                var url = "{0}api/theaters?zip={1}".format(apiBaseUrl, viewModel.zip());
                $.get(url, function (theaters) {
                    var dialogBody = ianhd.app.controls.selectTheaterBody();
                    dialogBody.html(""); // clear out any "Loading..." messages
                    // dialogBody.append("<a href='/showtimes/{0}/all' data-theater-id='all'>* All Theaters *</a>".format(viewModel.zip()));
                    $.each(theaters, function (i, theater) {
                        dialogBody.append("<a href='/showtimes/{0}/{1}' data-theater-id='{1}'>{2}</a>".format(viewModel.zip(), theater.id, theater.name));
                    });
                });
            }
        });
    },
    loadShowtimes: function (zip, theaterId) {
        console.log("Loading showtimes...");
        var url = "{0}api/theaters-with-movies?zip={1}&theaterId={2}".format(apiBaseUrl, zip, theaterId);
        var targetOutput = ianhd.app.controls.theatersWithMovies();
        targetOutput.html("<span class='hint'>Loading...</span>")

        $.get(url, function (theaters) {

            // mark any times that are earlier than now
            var now = new Date();
            now = parseInt(now.getHours() + "" + now.getMinutes());
            $.each(theaters, function (i, theater) {
                $.each(theater.movies, function (j, movie) {

                    var showtimes = [];
                    $.each(movie.showtimes, function (k, showtime) {
                        var hours = Number(showtime.match(/^(\d+)/)[1]);
                        var minutes = Number(showtime.match(/:(\d+)/)[1]);
                        minutes = minutes < 10 ? "0" + minutes : minutes;
                        var isPM = showtime.indexOf("am") == -1;
                        hours = hours != 12 && isPM ? hours + 12 : hours;
                        var showtime24hr = parseInt(hours + "" + minutes);
                        var isPast = now > showtime24hr;
                        showtimes.push({ isPast: isPast, showtime: showtime });
                    });

                    movie.showtimes = showtimes;
                });
            });
            
            var html = $.templates("#theaterTmpl").render(theaters);
            targetOutput.html(html);
        });
    },
    loadMovie: function (movieSlug, movieId) {
        console.log("Loading movie...");

    },
    bindControls: function () {
        // select an actual theater
        $(document).on('click', ianhd.app.selectors.selectTheater, function (e) {
            e.preventDefault();
            var trigger = $(this);
            viewModel.theaterId(trigger.attr('data-theater-id'));
            viewModel.theaterName(trigger.html());
            BootstrapDialog.closeAll(); // close any open dialogs. this is nice that BootstrapDialog provides this.
            router.navigate(trigger.attr("href"));
        });

        // select an actual movie
        $(document).on('click', ianhd.app.selectors.selectMovie, function (e) {
            e.preventDefault();
            var trigger = $(this);
            viewModel.movieId(trigger.attr('data-movie-id'));
            router.navigate(trigger.attr("href"));
        });

        // use my location button
        ianhd.app.controls.nearMe().click(function (e) {
            var btn = $(this);
            btn.button('loading');
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var url = "http://api.geonames.org/findNearbyPostalCodesJSON?lat={0}&lng={1}&username=codejkjk".format(position.coords.latitude, position.coords.longitude);
                    $.ajax({
                        url: url,
                        dataType: "jsonp",
                        success: function (resp) {
                            var zip = resp.postalCodes[0].postalCode;
                            viewModel.theaterId("");
                            viewModel.theaterName("");
                            viewModel.zip(zip);
                            ianhd.app.loadTheaters();
                            btn.button('reset');
                        }, // todo: check if postalCodes[0]
                        error: function () { }
                    });
                }, function (error) {
                    alert("Error, please try again.");
                });
            }
        });

        // enter zip textbox
        ianhd.app.controls.enterZip().keyup(function (e) {
            var trigger = $(this);
            if (e.keyCode == 13) {
                var val = $.trim(trigger.val());
                if (isNaN(val) || val.length !== 5) {
                    alert("Please enter a 5-digit zip code.")
                    trigger.focus();
                    return;
                }

                viewModel.theaterId("");
                viewModel.theaterName("");
                viewModel.zip(val);
                ianhd.app.loadTheaters();
            }
        });

        // theater prompt
        ianhd.app.controls.theater().click(function (e) {
            e.preventDefault();
            ianhd.app.loadTheaters();
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
            menu.slideToggle();
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
            // todo: set zip and theater id to localStorage
            ianhd.app.loadShowtimes(zip, theaterId);
        });
        router.route('/:movieSlug/:movieId', function (movieSlug, movieId) {
            console.log('route /:movieSlug/:movieId');
            ianhd.app.loadMovie(movieSlug, movieId);
        });
        router.route('/', function (zip, theaterId) {
            console.log('route /');
            viewModel.movieId("");
        });

        // gotta be a better way to trigger initial route
        var initPath = window.location.pathname;
        if (initPath.indexOf("showtimes") >= 0) {
            initPath = initPath.substr(initPath.indexOf("showtimes"));
            var parts = initPath.split('/');
            var zip = parts[1];
            var theaterId = parts[2];
            viewModel.zip(zip);
            ianhd.app.loadShowtimes(zip, theaterId);
        } else if (initPath !== "/") { // movie
            var parts = initPath.split('/');
            var movieSlug = parts[1];
            var movieId = parts[2];
            viewModel.movieId(movieId);
            ianhd.app.loadMovie(movieSlug, movieId);
        } else if (viewModel.zip() && viewModel.theaterId()) {
            ianhd.app.loadShowtimes(viewModel.zip(), viewModel.theaterId());
        }

        // trigger initial route
        //router.navigate(window.location.pathname);
    }
};

$(function () {
    ianhd.app.init();
});