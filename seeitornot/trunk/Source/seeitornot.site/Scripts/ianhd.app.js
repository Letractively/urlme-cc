// @koala-prepend "jquery-1.8.3.min.js"
// @koala-prepend "knockout-2.2.1.js",
// @koala-prepend "knockout.mapping.js",
// @koala-prepend "plugins/bootstrap-dialog/bootstrap-dialog.min.js", //
// @koala-prepend "plugins/iscroll.js",
// @koala-prepend "plugins/jquery.history.js",
// @koala-prepend "plugins/Router.js",
// @koala-prepend "plugins/jsrender.min.js",
// @koala-prepend "plugins/bootstrap-3.1.1/js/bootstrap.min.js",
// @koala-prepend "plugins/date.js",
// @koala-prepend "ianhd.js"

var viewModel = ko.mapping.fromJS({
    theaterName: tn || localStorage.getItem("theaterName"),
    theaterId: tid || localStorage.getItem("theaterId"),
    movieId: mid,
    zip: z || localStorage.getItem("zip"),
    showBack: false,
    view: v,
    date: Date.parse("today"),
    dateLabel: Date.parse("today").toString("dddd, MMM d"),
    dateApiLabel: Date.parse("today").toString("yyyy-MM-dd"),
    dateIdx: 0,
    numDays: 5
});
ko.applyBindings(viewModel);

viewModel.zip.subscribe(function (newVal) {
    localStorage.setItem("zip", newVal);
});
viewModel.theaterName.subscribe(function (newVal) {
    localStorage.setItem("theaterName", newVal);
});
viewModel.theaterId.subscribe(function (newVal) {
    localStorage.setItem("theaterId", newVal);
});
viewModel.date.subscribe(function (newVal) {
    viewModel.dateLabel(newVal.toString("dddd, MMM d"));
    viewModel.dateApiLabel(newVal.toString("yyyy-MM-dd"));
})
viewModel.view.subscribe(function(newVal) {
    console.log("view changed to " + newVal);
    switch (newVal) {
        case "showtimes":
            viewModel.movieId("");
            break;
        case "home":
            viewModel.movieId("");
            break;
        case "movie":
            break;
    }
});

var router = new Router();

ianhd.registerNamespace("app");
ianhd.app = {
    controls: {
        changeDate: function () { return $(".datePicker a"); },
        enterZip: function () { return $(".enterZip"); },
        logo: function () { return $("#logo"); },
        menu: function () { return $("nav"); },
        menuIcon: function () { return $(".fa-bars"); },
        movie: function () { return $("#movie"); },
        nearMe: function() { return $("#nearMe"); },
        overlay: function () { return $("#overlay"); },
        searchBox: function() { return $("input.search"); },
        searchIcon: function () { return $(".fa-search"); },
        singleMovie: function () { return $(".singleMovie"); },
        theater: function () { return $(".theater"); },
        theatersWithMovies: function () { return $("#theatersWithMovies"); },
        zip: function () { return $(".zip"); },
    },
    selectors: {
        back: ".back",
        closePopup: "#overlay,.closePopup",
        routeTrigger: '.triggerRoute',
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
        var lsKey = "theaters-" + viewModel.zip(); // local storage key
        var theaters = localStorage.getItem(lsKey);
        var url = "{0}api/theaters?zip={1}".format(apiBaseUrl, viewModel.zip());
        var message = "";
        var haveData = false;
        if (theaters) {
            console.log("Retrieved theaters from localStorage.");
            message = ianhd.app.getTheaterListHtml(JSON.parse(theaters));
            haveData = true;
            // update local storage w/ latest from server (todo: do this every once in a while)
            $.get(url, function (theaters) {
                localStorage.setItem(lsKey, JSON.stringify(theaters));
                console.log("Updated theaters in localStorage after localStorage retrieval.");
            });
        } else {
            console.log("Theaters not found in localStorage; retrieving from server.");
            message = "<span class='hint loading'></span>";
            // leave haveData = false  
        } 

        BootstrapDialog.show({
            title: "Select a theater",
            message: message,
            cssClass: "selectTheater",
            buttons: [{
                label: 'Cancel',
                cssClass: 'btn-default',
                action: function (dialog) {
                    dialog.close();
                }
            }],
            onshown: function (dialog) {
                if (!haveData) {
                    $.get(url, function (theaters) {
                        dialog.setMessage(
                            ianhd.app.getTheaterListHtml(theaters)
                        );
                        localStorage.setItem(lsKey, JSON.stringify(theaters));
                    });
                }
            }
        });
    },
    getTheaterListHtml: function(theaters) {
        var html = "";
        $.each(theaters, function (i, theater) {
            html += "<a href='/showtimes/{0}/{1}'>{2}</a>".format(viewModel.zip(), theater.id, theater.name);
        });
        return html;
    },
    loadShowtimes: function () {
        var zip = viewModel.zip();
        var theaterId = viewModel.theaterId();
        var date = viewModel.dateApiLabel();
        console.log("Loading showtimes...");
        
        var url = "{0}api/theaters-with-movies?zip={1}&theaterId={2}&date={3}".format(apiBaseUrl, zip, theaterId, date);
        var targetOutput = ianhd.app.controls.theatersWithMovies();
        targetOutput.html("<span class='hint loading'></span>");

        $.get(url, function (theaters) {

            // combine any movies that have standard + 3d sets of showtimes
            $.each(theaters, function(h, theater) {
                $.each(theater.movies, function (i, movie) {
                    if (!movie.is3d) return true; // continue

                    // we have a 3d movie, so find it's standard movie equivalent, which we'll modify for the view
                    var standardMovie = $.grep(theater.movies, function (tm, j) {
                        return tm.id === movie.id && !tm.is3d;
                    })[0];

                    if (standardMovie) {
                        standardMovie.threeDShowtimes = movie.showtimes;
                        movie.remove = true;
                    } else { // if there is no standard equivalent to this 3d movie, then mark the 3d movie as "is3dOnly"
                        movie.is3dOnly = true;
                    }
                });

                theater.movies = $.grep(theater.movies, function(tm, j) {
                    return !tm.remove;
                });
            });

            // mark any times that are earlier than now
            //var now = new Date();
            //now = parseInt(now.getHours() + "" + now.getMinutes());
            //$.each(theaters, function (i, theater) {
            //    $.each(theater.movies, function (j, movie) {

            //        var showtimes = [];
            //        $.each(movie.showtimes, function (k, showtime) {
            //            var hours = Number(showtime.match(/^(\d+)/)[1]);
            //            var minutes = Number(showtime.match(/:(\d+)/)[1]);
            //            minutes = minutes < 10 ? "0" + minutes : minutes;
            //            var isPM = showtime.indexOf("am") == -1;
            //            hours = hours != 12 && isPM ? hours + 12 : hours;
            //            var showtime24hr = parseInt(hours + "" + minutes);
            //            var isPast = now > showtime24hr;
            //            showtimes.push({ isPast: isPast, showtime: showtime });
            //        });

            //        movie.showtimes = showtimes;
            //    });
            //});
            
            var html = $.templates("#theaterTmpl").render(theaters);
            targetOutput.html(html);
        });
    },
    loadMovie: function () {
        var movieId = viewModel.movieId();
        console.log("Loading movie...");
        
        var url = "{0}api/movie/{1}".format(apiBaseUrl, movieId);
        var target = ianhd.app.controls.singleMovie();
        target.html("<span class='hint loading'></span>");
        $.get(url, function (html) {
            target.html(html);
            if (viewModel.showBack()) {
                target.find(".backContainer").show();
            }
        });
    },
    bindControls: function () {
        // select an actual theater
        $(document).on('click', ianhd.app.selectors.selectTheater, function (e) {
            e.preventDefault();
            var trigger = $(this);
            viewModel.theaterName(trigger.html());
            BootstrapDialog.closeAll(); // close any open dialogs. this is nice that BootstrapDialog provides this.
            router.navigate(trigger.attr("href"));
        });

        // route triggers
        $(document).on('click', ianhd.app.selectors.routeTrigger, function (e) {
            e.preventDefault();
            router.navigate($(this).attr("href"));
        });

        // change date
        ianhd.app.controls.changeDate().click(function (e) {
            e.preventDefault();
            var trigger = $(this);
            if (!trigger.hasClass("enabled")) return; // do nothing if this link is not enabled

            var isNext = trigger.hasClass("next");
            var add = 1;
            if (isNext) {
                viewModel.dateIdx(viewModel.dateIdx() + 1);
            } else {
                viewModel.dateIdx(viewModel.dateIdx() - 1);
                add = -1;
            }
            viewModel.date(viewModel.date().add(add).days());
            ianhd.app.loadShowtimes();
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
                    alert("Please enter a 5-digit zip code.");
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

            var menu = ianhd.app.controls.menu();
            menu.toggleClass("expanded");
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
        
        // go back
        $(document).on('click', ianhd.app.selectors.back, function (e) {
            e.preventDefault();
            router.go(-1);
        });
    },
    initHistory: function () {
        router.route('/showtimes/:zip/:theaterId', function (zip, theaterId) {
            viewModel.showBack(true);
            viewModel.zip(zip);
            viewModel.theaterId(theaterId);
            ianhd.app.loadShowtimes();
            viewModel.view("showtimes");
        });
        router.route('/:movieSlug/:movieId', function (movieSlug, movieId) {
            viewModel.showBack(true);
            viewModel.movieId(movieId);
            ianhd.app.loadMovie();
            viewModel.view("movie");
        });
        router.route('/', function () {
            viewModel.showBack(true);
            viewModel.view("home");
        });

        // handle initial route
        switch (viewModel.view()) {
            case "showtimes":
                localStorage.setItem("zip", viewModel.zip());
                localStorage.setItem("theaterId", viewModel.theaterId());
                localStorage.setItem("theaterName", viewModel.theaterName());
                ianhd.app.loadShowtimes();
                break;
            case "movie":
                ianhd.app.loadMovie();
                break;
            case "home":
                if (viewModel.zip() && viewModel.theaterId()) {
                    ianhd.app.loadShowtimes();
                }
                break;
        }
    }
};

$(function () {
    ianhd.app.init();
});