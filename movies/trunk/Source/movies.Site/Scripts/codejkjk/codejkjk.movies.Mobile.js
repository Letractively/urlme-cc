registerNS("codejkjk.movies.mobile");

var backUrl = "";

codejkjk.movies.mobile = {
    // page elements
    controls: {
        body: function () { return $("#body"); }
        , FavoriteLinksSelector: function () { return ".favoriteLink"; }
        , FavoriteTheaterList: function () { return $("#showtimes > #theaters div[data-role='content']:first ul"); }
        , FavoriteTheaterListContent: function () { return $("#showtimes > #theaters div[data-role='content']:first"); }
        , FavoriteTheaterListTemplate: function () { return $("#favoriteTheaterListTemplate"); }
        , InputShowtimesZip: function () { return $("#inputShowtimesZip"); }
        , cancelSearch: function () { return $("#cancelSearch"); }
        , CurrentMovie: function () { return $("#movie h2 span"); }
        , CurrentMovieId: function () { return $("#currentMovieId"); }
        , CurrentShowtimesZip: function () { return $("#currentShowtimesZip"); }
        , CurrentTheater: function () { return $("#theater h2:first span"); }
        , CurrentTheaterMovie: function () { return $("#theatermovie h2 span"); }
        , header: function () { return $("#header"); }
        , loading: function () { return $(".loading"); }
        , menu: function () { return $("#menu"); }
        , menuLink: function () { return $("#menuLink"); }
        , Movie: function () { return $("#movie div[data-role='content']"); }
        , MovieBackLink: function () { return $("#movie a"); }
        , searchBox: function () { return $("#search input"); }
        , showtimeDays: function () { return $("#theater #showtimeDays"); }
        , showtimeDaySelector: function () { return "#theater #showtimeDays a" }
        , ShowtimesHeader: function () { return $("#showtimes > #theaters h2"); }
        , ShowtimesOptionsLinkSelector: function () { return "#showtimes > #theaters h2 a"; }
        , ShowtimesOptions: function () { return $("#showtimes > #options"); }
        , TheaterMovie: function () { return $("#theatermovie h2"); }
        , TheaterMovieHeader: function () { return $("#theatermovie h2"); }
        , TheaterMovieBackLink: function () { return $("#theatermovie h2 a"); }
        , TheaterList: function () { return $("#theaters ul:last"); }
        , TheaterLists: function () { return $("#theaters div[data-role='content']"); }
        , TheaterListTemplate: function () { return $("#theaterListTemplate"); }
        , TheaterMovieList: function () { return $("#theater div[data-role='content'] ul"); }
        , TheaterMovieTemplate: function () { return $("#theaterMovieTemplate"); }
        , Theaters: function () { return $("#theaters"); }
        , TheaterTemplate: function () { return $("#theaterTemplate"); }
        , UseCurrentLocationForShowtimesSelector: function () { return "#showtimes div[data-role='content'] a"; }
        , views: function () { return $("section"); }
    },

    currents: {
        ZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("ZipCode", val);
            } else { // get
                return localStorage.getItem("ZipCode"); // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
            }
        }
        , ShowtimeDay: function (val) {
            var input = $("input#currentShowtimeDay");
            if (typeof val != "undefined") { // set
                input.val(val);
            } else { // get
                if (!input.val()) {
                    // showtimeDay hasn't been set yet, so set it and return it
                    var today = Date.today().toString("yyyyMMdd");
                    input.val(today);
                }
                return input.val();
            }
        }
        , ZipCodeFriendlyTitle: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("ZipCodeFriendlyTitle", val);
            } else { // get
                return localStorage.getItem("ZipCodeFriendlyTitle"); // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
            }
        }
        , Theater: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("Theater", val);
            } else { // get
                return localStorage.getItem("Theater") || "";
            }
        }
        , HiddenTheaterMovies: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("HiddenTheaterMovies", val);
            } else {
                var ret = localStorage.getItem("HiddenTheaterMovies");
                return ret ? ret.split(',') : [];
            }
        }
        , MovieId: function () {
            return codejkjk.movies.mobile.controls.CurrentMovieId().val();
        }
    },

    init: function () {
        var History = window.History;

        codejkjk.movies.mobile.initSearch();
        codejkjk.movies.mobile.bindControls();
        codejkjk.movies.mobile.initGooglePlaces("inputShowtimesZip");
        codejkjk.movies.mobile.registerJsRenderHelpers();
        codejkjk.movies.mobile.initHistory(History);
    },

    initSearch: function () {
        var searchBox = codejkjk.movies.mobile.controls.searchBox();
        searchBox.autocomplete({
            minLength: 2,
            source: apiBaseUrl + 'search_movies.json', // change
            focus: function (event, ui) {
                // nothing
            },
            select: function (event, ui) {
                History.pushState(null, null, ui.item.url);
                return false;
            }
        })
		.data("autocomplete")._renderItem = function (ul, item) {
		    return $("<li></li>")
				.data("item.autocomplete", item)
				.append("<a class='group noPush' href='{0}'><img src='{1}' width='61' height='91' /><div class='right'><div>{2} ({3})</div><div class='cast'>{4}</div></div></a>".format(item.url, item.imgUrl, item.title, item.year, item.cast))
				.appendTo(ul);
		};
    },

    initHistory: function (History) {
        // handle any state changes
        History.Adapter.bind(window, 'statechange', function () {
            var state = History.getState();
            codejkjk.movies.mobile.handlePushState(state.url);
        });

        // handle "a" clicks - prevent their default and instead push state
        $(document).on('click', 'a[href^="/"]:not(.noPush)', function (e) {
            e.preventDefault();
            var href = $(this).attr("href");
            var state = History.getState();
            var hash = state.hash;
            if (hash === href) {
                codejkjk.movies.mobile.handlePushState(state.url);
            } else {
                History.pushState(null, null, $(this).attr("href"));
            }
        });

        // handle initial page's load
        var initState = History.getState();
        codejkjk.movies.mobile.handlePushState(initState.url);
    },

    handlePushState: function (url) {
        var paths = url.replace('//', '').split('/');
        var firstPath = '/' + paths[1]; // "", "comingsoon", "showtimes" (all navs), "rb" (redbox movie) or "hunger-games" (movie), /theater

        // not sure why FB does this...
        if (firstPath === "/#_=_") {
            window.location.href = "http://seeitornot.co";
            return;
        }

        // reset menu if it's been messed with
        codejkjk.movies.mobile.controls.menu().removeClass("big").hide();
        codejkjk.movies.mobile.controls.body().removeClass("small").show();

        // primary nav link?
        if (firstPath === "/" || (firstPath === "/redbox" && !paths[2])) {
            codejkjk.movies.mobile.showSection(firstPath);
            backUrl = "/";
        }
        else if (firstPath === "/showtimes") {
            if (codejkjk.movies.mobile.currents.ZipCode()) {
                // we have a zipcode, so display theater list and showtimes header
                codejkjk.movies.mobile.UpdateZip(codejkjk.movies.mobile.currents.ZipCode(), codejkjk.movies.mobile.currents.ZipCodeFriendlyTitle());
            } else {
                // no zipcode, so show options (user can specify zip or use current location)
                codejkjk.movies.mobile.controls.ShowtimesOptions().show();
            }
            codejkjk.movies.mobile.showSection(firstPath);
        }
        else if (firstPath === "/theater") {
            // /theater/{zipCode}/{id}
            var zip = paths[2];
            var theaterId = paths[3];
            codejkjk.movies.mobile.currents.ZipCode(zip);

            codejkjk.movies.mobile.loadTheaterMovies(theaterId);
        }
        else if (firstPath === "/theatermovie") {
            // /theatermovie/{id}
            codejkjk.movies.mobile.toggleLoading();
            var rtMovieId = paths[2];

            // load single theater movie
            codejkjk.movies.Api.GetMovieMobileHtml(rtMovieId, function (html) {
                // slight hack. the html is prefexed with {movieName}^, then the html
                var movieName = $.trim(html.split('^')[0]);
                var htmlStr = $.trim(html.split('^')[1]);
                codejkjk.movies.mobile.controls.CurrentTheaterMovie().html(movieName); // set theater movie name in header
                codejkjk.movies.mobile.controls.TheaterMovie().html(htmlStr);

                // show back link?
                if (backUrl) {
                    codejkjk.movies.mobile.controls.TheaterMovieBackLink().attr("href", backUrl).show();
                } else {
                    codejkjk.movies.mobile.controls.TheaterMovieBackLink().hide();
                }

                codejkjk.movies.mobile.toggleLoading();
                codejkjk.movies.mobile.showSection(firstPath);
            });
        }
        else {
            // /hunger-games/9999888768 (need TODO: redbox movie)
            codejkjk.movies.mobile.toggleLoading();
            var rtMovieId = paths[2];

            // load single movie
            codejkjk.movies.Api.GetMovieMobileHtml(rtMovieId, function (html) {
                // slight hack. the html is prefexed with {movieName}^, then the html
                var movieName = $.trim(html.split('^')[0]);
                var htmlStr = $.trim(html.split('^')[1]);
                codejkjk.movies.mobile.controls.CurrentMovie().html(movieName); // set movie name in header, NEED???
                codejkjk.movies.mobile.controls.Movie().html(htmlStr);

                // show back link?
                if (backUrl) {
                    codejkjk.movies.mobile.controls.MovieBackLink().attr("href", backUrl).show();
                } else {
                    codejkjk.movies.mobile.controls.MovieBackLink().hide();
                }

                codejkjk.movies.mobile.toggleLoading();
                codejkjk.movies.mobile.showSection("movie");
            });
        }
    },

    showSection: function (path) {
        // path = "/", "/showtimes", "/theater" ...
        // primary nav change
        // clear out search val if user previously searched for something
        codejkjk.movies.mobile.controls.searchBox().val("");
        var sectionId = path.replace("/", "") || "whatsHot";

        // show view
        codejkjk.movies.mobile.controls.views().hide();
        var sectionToShow = $('#' + sectionId);
        sectionToShow.show();

        // init image lazyload jquery plugin
        if (!sectionToShow.hasClass("lazyLoaded")) {
            $("img.lazy").lazyload({ effect: "fadeIn" });
            sectionToShow.addClass("lazyLoaded");
        }

        var header = codejkjk.movies.mobile.controls.header();
        if (sectionId == "whatsHot") {
            header.html("What's Hot");
        } else if (sectionId == "showtimes") {
            header.html("Showtimes");
        } else {
            header.html("&nbsp;");
        }

        if (codejkjk.movies.mobile.controls.menu().is(":visible")) {
            codejkjk.movies.mobile.controls.menuLink().trigger('click');
        }
    },

    initGooglePlaces: function (inputId) {
        var input = document.getElementById(inputId);
        var autocomplete = new google.maps.places.Autocomplete(input);

        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            // codejkjk.movies.mobile.controls.ShowtimesOptions().mask();
            var place = autocomplete.getPlace();
            var formattedAddress = place.formatted_address.replace(", USA", "");
            var latLong = place.geometry.location.toString();
            latLong = latLong.replace("(", "").replace(")", "").replace(" ", "");
            var lat = latLong.split(',')[0];
            var long = latLong.split(',')[1];
            codejkjk.Geo.GetZipCodeFromLatLong(lat, long, function (zipCode) {
                codejkjk.movies.mobile.UpdateZip(zipCode, formattedAddress);
            });
        });
    },

    UpdateZip: function (zip, friendlyTitle) {
        codejkjk.movies.mobile.controls.TheaterLists().hide(); // hide theater lists from previous zips
        codejkjk.movies.mobile.toggleLoading();
        codejkjk.movies.mobile.currents.ZipCode(zip); // update current zip code in local storage
        if (typeof friendlyTitle != "undefined" && friendlyTitle) {
            codejkjk.movies.mobile.controls.CurrentShowtimesZip().html(friendlyTitle);
        } else {
            codejkjk.movies.mobile.controls.CurrentShowtimesZip().html(zip);
        }
        codejkjk.movies.mobile.controls.ShowtimesHeader().show();
        codejkjk.movies.mobile.controls.ShowtimesOptions().hide();

        // codejkjk.movies.mobile.currents.Theater(""); // new zip, so clear out current theater value
        codejkjk.movies.Api.GetTheaters(codejkjk.movies.mobile.currents.ShowtimeDay(), zip, codejkjk.movies.mobile.LoadTheaters);
    },

    toggleLoading: function () {
        codejkjk.movies.mobile.controls.loading().toggle();
    },

    loadTheaterMovies: function (theaterId) {
        codejkjk.movies.mobile.toggleLoading();
        codejkjk.movies.Api.GetTheaterMovies(codejkjk.movies.mobile.currents.ShowtimeDay(), codejkjk.movies.mobile.currents.ZipCode(), theaterId, function (theater) {
            codejkjk.movies.mobile.controls.CurrentTheater().html(theater.name);
            codejkjk.movies.mobile.controls.TheaterMovieList().html(
                        codejkjk.movies.mobile.controls.TheaterMovieTemplate().render(theater)
                    ).show();

            codejkjk.movies.mobile.buildShowtimeDays(theaterId);

            // set backUrl
            backUrl = "/theater/{0}/{1}".format(codejkjk.movies.mobile.currents.ZipCode(), theaterId); // set back url to current url

            codejkjk.movies.mobile.toggleLoading();
            codejkjk.movies.mobile.showSection("/theater");
        });
    },

    LoadTheaters: function (postalCode) {
        var theaters = postalCode.theaters;
        var hiddenTheaterMovies = codejkjk.movies.mobile.currents.HiddenTheaterMovies();

        var favoriteTheaterIds = localStorage.getItem("FavoriteTheaters");
        favoriteTheaterIds = favoriteTheaterIds ? favoriteTheaterIds.split(',') : [];

        // determine favorite and not-favorite theaters
        var favoriteTheaters = $.grep(theaters, function (theater, i) {
            return favoriteTheaterIds.indexOf(theater.id.toString()) >= 0;
        });
        var notFavoriteTheaters = $.grep(theaters, function (theater, i) {
            return favoriteTheaterIds.indexOf(theater.id.toString()) == -1;
        });

        // establish current theater id before we render the html, so the jsRender helper can know which items it should show as active
        if (!codejkjk.movies.mobile.currents.Theater()) {
            // no current theater set, so choose first
            var currentTheaterId = null;
            if (favoriteTheaters.length > 0) { currentTheaterId = favoriteTheaters[0].id; }
            else { currentTheaterId = notFavoriteTheaters[0].id; }
            codejkjk.movies.mobile.currents.Theater(currentTheaterId);
        } else {
            // make sure the theaterId is in the incoming list of theaters
            var results = $.grep(theaters, function (theater, i) {
                return theater.id.toString() == codejkjk.movies.mobile.currents.Theater();
            });
            if (results.length == 0) { // current theaterId does NOT exist in incoming list of theaters, so get the first from favorites or nonfavorites if favorites is empty
                var currentTheaterId = null;
                if (favoriteTheaters.length > 0) { currentTheaterId = favoriteTheaters[0].id; }
                else { currentTheaterId = notFavoriteTheaters[0].id; }
                codejkjk.movies.mobile.currents.Theater(currentTheaterId);
            }
        }

        // loop thru theaters and set movieClass for each movie based on whether or not that movie's been previously hidden, per local storage memory
        $.each(theaters, function (i, theater) {
            var numHiddenTheaterMovies = 0;
            $.each(theater.movies, function (j, theaterMovie) {
                var movieClass = hiddenTheaterMovies.indexOf("{0}-{1}".format(theater.id, theaterMovie.id)) >= 0 ? "hidden" : "";
                numHiddenTheaterMovies = movieClass == "hidden" ? ++numHiddenTheaterMovies : numHiddenTheaterMovies;
                theaterMovie.movieClass = movieClass;
            });
            if (numHiddenTheaterMovies) { theater.numHiddenMovies = numHiddenTheaterMovies; }
        });

        // render theaters
        if (favoriteTheaters.length) {
            codejkjk.movies.mobile.controls.FavoriteTheaterList().html(
                codejkjk.movies.mobile.controls.FavoriteTheaterListTemplate().render(favoriteTheaters)
            ).parent().show();
            codejkjk.movies.mobile.controls.FavoriteTheaterListContent().show();
        } else {
            codejkjk.movies.mobile.controls.FavoriteTheaterListContent().hide();
        }
        codejkjk.movies.mobile.controls.TheaterList().html(
            codejkjk.movies.mobile.controls.TheaterListTemplate().render(notFavoriteTheaters)
        ).parent().show();

        if (codejkjk.movies.mobile.controls.ShowtimesOptions().is(":visible")) {
            codejkjk.movies.mobile.controls.ShowtimesOptions().slideToggle('fast');
        }

        codejkjk.movies.mobile.controls.Theaters().show();

        // now that the theater links are filled, set the currentTheater container's height to match height of theater links container
        codejkjk.movies.mobile.toggleLoading();
    },

    registerJsRenderHelpers: function () {
        $.views.registerHelpers({
            IsReleased: function (releaseDate) {
                var now = new Date();
                releaseDate = new Date(releaseDate);
                return now >= releaseDate;
            },
            IsCurrentTheater: function (iTheaterId) {
                return iTheaterId.toString() == codejkjk.movies.mobile.currents.Theater();
            },
            GetHoursAndMinutes: function (minutes) {
                var hrs = Math.floor(minutes / 60);
                var mins = minutes % 60;

                return "{0} hr. {1} min.".format(hrs, mins);
            },
            GetCriticsClass: function (rating) {
                if (!rating) { return ""; }
                return rating.indexOf("Fresh") >= 0 ? "criticsFresh" : "criticsRotten";
            },
            GetAudienceClass: function (rating) {
                if (!rating) { return ""; }
                return rating.indexOf("Upright") >= 0 ? "audienceUpright" : "audienceSpilled";
            },
            FormatReleaseDate: function (date) {
                return Date.parse(date).toString("MMM d, yyyy");
            },
            GetParentalGuideUrl: function (imdbId) {
                return codejkjk.movies.IMDB.GetParentalGuideUrl(imdbId);
            },
            Snippet: function (text, len) {
                return text.snippet(len);
            },
            GetFavoriteLinkClass: function (theaterId) {
                var favoriteTheaters = localStorage.getItem("FavoriteTheaters");
                favoriteTheaters = favoriteTheaters ? favoriteTheaters.split(',') : [];
                return favoriteTheaters.indexOf(theaterId.toString()) >= 0 ? "lit" : "default";
            }
        });
    },

    buildShowtimeDays: function (theaterId) {
        codejkjk.movies.mobile.controls.showtimeDays().html(""); // clear out first

        // only show 3 day links
        for (var i = 0; i < 3; i++) {
            var d = Date.today().add(i).days();
            var label = '';
            var cssClass = '';
            switch (i) {
                case 0:
                    label = "Today";
                    break;
                case 1:
                    label = "Tomorrow";
                    break;
                default:
                    label = d.toString("ddd, MMM dd");
                    break;
            } // end switch

            // if date in iteration matches current showtime day
            if (d.toString("yyyyMMdd") == codejkjk.movies.mobile.currents.ShowtimeDay()) {
                cssClass = "active";
            }

            var showtimeDayLink = "<a href='#' class='{0}' data-date='{1}' data-theaterid='{2}'>{3}</a>".format(cssClass, d.toString("yyyyMMdd"), theaterId, label);
            codejkjk.movies.mobile.controls.showtimeDays().append(showtimeDayLink);
        }
    },

    bindControls: function () {
        // handle menu link click
        codejkjk.movies.mobile.controls.menuLink().click(function (e) {
            e.preventDefault();
            var menu = codejkjk.movies.mobile.controls.menu();
            codejkjk.movies.mobile.controls.body().toggleClass("small");
            menu.toggle();
        });

        codejkjk.movies.mobile.controls.searchBox().focus(function () {
            var menu = codejkjk.movies.mobile.controls.menu();
            if (!menu.hasClass("big")) {
                codejkjk.movies.mobile.controls.body().toggle();
                codejkjk.movies.mobile.controls.menu().toggleClass("big");
            }
        });

        codejkjk.movies.mobile.controls.cancelSearch().click(function (e) {
            e.preventDefault();
            codejkjk.movies.mobile.controls.body().toggle();
            codejkjk.movies.mobile.controls.menu().toggleClass("big");
        });

        // handle showtime day links
        $(document).on('click', codejkjk.movies.mobile.controls.showtimeDaySelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            codejkjk.movies.mobile.currents.ShowtimeDay(link.data().date);

            // refresh theater movies
            codejkjk.movies.mobile.loadTheaterMovies(link.attr("data-theaterid"));
        });

        // handle favorite theater links
        $(document).on('click', codejkjk.movies.mobile.controls.FavoriteLinksSelector(), function (e) {
            e.preventDefault();
            e.stopPropagation();
            var link = $(this);
            var theaterId = link.closest("[data-theaterid]").attr("data-theaterid");
            var favoriteTheaters = localStorage.getItem("FavoriteTheaters");
            favoriteTheaters = favoriteTheaters ? favoriteTheaters.split(',') : [];
            if (link.hasClass("lit")) {
                // currently set to favorite, so remove from favorites list
                if (favoriteTheaters.indexOf(theaterId) >= 0) {
                    favoriteTheaters.splice(favoriteTheaters.indexOf(theaterId), 1);
                }
            } else {
                // currently NOT set to favorite, so add to favorites list
                if (favoriteTheaters.indexOf(theaterId) == -1) {
                    favoriteTheaters.push(theaterId);
                }
            }
            link.toggleClass("lit").toggleClass("default");
            localStorage.setItem("FavoriteTheaters", favoriteTheaters.join(','));

            // refresh theaters
            codejkjk.movies.mobile.toggleLoading();
            codejkjk.movies.Api.GetTheaters(codejkjk.movies.mobile.currents.ShowtimeDay(), codejkjk.movies.mobile.currents.ZipCode(), codejkjk.movies.mobile.LoadTheaters);
        });

        // "options" link for showtimes
        $(document).on('click', codejkjk.movies.mobile.controls.ShowtimesOptionsLinkSelector(), function (e) {
            e.preventDefault();
            var clearInput = !codejkjk.movies.mobile.controls.ShowtimesOptions().is(":visible");
            if (clearInput) {
                codejkjk.movies.mobile.controls.InputShowtimesZip().val("");
            }
            codejkjk.movies.mobile.controls.ShowtimesOptions().slideToggle('fast');
        });
        // "use current location" click for showtimes zip
        $(document).on('click', codejkjk.movies.mobile.controls.UseCurrentLocationForShowtimesSelector(), function (e) {
            e.preventDefault();
            codejkjk.Geo.GetZipCode(function (zipCode) {
                codejkjk.movies.mobile.UpdateZip(zipCode);
            });
        });
    }
}

$(document).ready(function () {
    codejkjk.movies.mobile.init();
});

//var pageInit = false;
//$(document).on('pagebeforechange', function (e, data) {
//    e.preventDefault(); // use jquery mobile just for its nifty css classes, but handle all page transitions on our own using History
//    if (!pageInit) {
//        $("#onlyPage").show().page();
//        pageInit = true;
//    }
//});