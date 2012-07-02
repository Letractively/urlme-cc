registerNS("codejkjk.movies.desktop");

// todo:
// - theater h1, make h2 b/c there should only be one h1 on the page
// - theater view, make width same as width of box office and upcoming views. this way the movie details popup will show up in same location.
// - setting min-height of theaterlist() yields zero. why? - maybe cuz at first it's invisible?
// - Favorites list should not have borders if there are none
// - combine favorite theater list with not-favorite theater list with if condition for favoriteLink class (default vs. lit)
// - critics & audience titles not always showing up
// - search for Sherlock - divs need max height, and error with parsing date
// - square off the theater list on the left hand side.
// - green plus and get a bold one, more like an icon for show movie details.
// - movie details popup - make % fonts similar to movie listings page (all bold, blue links)
// - movie details page, make overlay visible and popup visible on load so it doesn't take a sec to popup
// - movie details pages, make these links and not div.show()'s
// - no more "Top Box Office," instead, "In theaters", AND mark recent movies with Just Released - Today / 2 Days Ago (maybe w/ a star or an icon??)
// - hide "poster not found" movies
// - get rid of "noPush" class. instead, do :not[href(startsWith)'http']

codejkjk.movies.desktop = {
    // page elements
    controls: {
        ActionLinks: function () { return $(".actions").find("a"); }
        , BackToMovieDetailsLinkSelector: function () { return ".backToMovieDetails"; }
        , BoxOfficeView: function () { return $("#boxOfficeView"); }
        , BrowseLinkSelector: function () { return "#browse"; }
        , changeShowtimeZipLink: function () { return $("#changeCurrentZipLink"); }
        , CloseMovieDetailsLinkSelector: function () { return ".closeMovieDetails"; }
        , CopyButton: function () { return $("#copyButton"); }
        , CopySuccess: function () { return $("#copySuccess"); }
        , CurrentNavItem: function () { return $("nav > a.selected"); }
        , CurrentMovieId: function () { return $("#currentMovieId"); }
        , CurrentTheaterContainer: function () { return $("#currentTheaterContainer"); }
        , CurrentTheaterTemplate: function () { return $("#currentTheaterTemplate"); }
        , CurrentView: function () { return $(".content:visible"); }
        , CurrentZip: function () { return $("#currentZip"); }
        , DefaultNavItem: function () { return $("nav > a:first"); }
        , FavoriteLinksSelector: function () { return ".favoriteLink"; }
        , FavoriteTheaterList: function () { return $("#favoriteTheaterList"); }
        , FavoriteTheaterListTemplate: function () { return $("#favoriteTheaterListTemplate"); }
        , FirstNavLink: function () { return $("nav > a:first"); }
        , HideMovieLinksSelector: function () { return ".hideMovieLink"; }
        , IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , Logo: function () { return $("#logo"); }
        , MovieDetailsPopup: function () { return $("#movieDetailsPopup"); }
        , MovieDetails: function () { return $("#movieDetails"); }
        , MovieListTemplate: function () { return $("#movieListTemplate"); }
        , MovieShowtimes: function () { return $("#movieShowtimes"); }
        , MovieUrl: function () { return $("#movieUrl"); }
        , Nav: function () { return $("nav"); }
        , NavLinks: function () { return $("nav > a"); }
        , NearbyRedboxLinksSelector: function () { return ".nearbyRedboxLink"; }
        , InputShowtimesZip: function () { return $("#inputShowtimesZip"); }
        , Overlay: function () { return $("#overlay"); }
        , OverlaySelector: function () { return "#overlay"; }
        , RedboxAvails: function () { return $("#redboxAvails"); }
        , RedboxAvailsList: function () { return $("#redboxAvailsList"); }
        , Redboxes: function () { return $("#redboxes"); }
        , RedboxLocationChooser: function () { return $("#redboxLocationChooser"); }
        , RemoveLinks: function () { return $(".actions").find(".removeLink"); }
        , RedboxZipCodeInput: function () { return $("#redboxZip"); }
        , searchBox: function () { return $("#q > input[type='text']"); }
        , SearchRedboxZipCodeButton: function () { return $("#redboxZip").next("button"); }
        , SearchResultsView: function () { return $("#searchResultsView"); }
        , SeeNearbyRedboxesSelector: function () { return "#seeNearbyRedboxes"; }
        , ShowHiddenMoviesLinksSelector: function () { return ".showHiddenMoviesLink"; }
        , ShowtimeDayLinks: function () { return $(".showtimeDays > a"); }
        , ShowtimeDayLinksSelector: function () { return ".showtimeDays > a"; }
        , ShowtimeDayLinksContainer: function () { return $(".showtimeDays"); }
        , showtimes: function () { return $("#showtimes"); }
        , ShowtimesLinksSelector: function () { return ".showtimesLink"; }
        , showtimesLoading: function () { return $("#showtimesLoading"); }
        , showtimeUseMyLocations: function () { return $(".showtimeZipOptions a"); }
        , showtimeZipOptions: function () { return $(".showtimeZipOptions"); }
        , showtimeZipOptionsBig: function () { return $("#showtimesView .showtimeZipOptions:first"); }
        , showtimeZipOptionsSmall: function () { return $("#showtimesView .showtimeZipOptions:last"); }
        , TheatersForMovieList: function () { return $("#theatersForMovieList"); }
        , TheaterLinksSelector: function () { return ".theaterList > a"; }
        , TheaterList: function () { return $("#theaterList"); }
        , TheaterListTemplate: function () { return $("#theaterListTemplate"); }
        , Theaters: function () { return $(".theater"); }
        , trailer: function () { return $("#trailer"); }
        , UnsetIMDbMovieIds: function () { return $("[data-imdbmovieid='']"); }
        , UpcomingView: function () { return $("#upcomingView"); }
        , Views: function () { return $(".content"); }
        , watchTrailerLinkSelector: function () { return "#watchTrailerLink"; }
    },

    currents: {
        ZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("ZipCode", val);
            } else { // get
                return localStorage.getItem("ZipCode"); // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
            }
        }
        , FriendlyZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("FriendlyZipCode", val);
            } else { // get
                return localStorage.getItem("FriendlyZipCode"); // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
            }
        }
        , RedboxZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("RedboxZipCode", val);
            } else { // get
                return localStorage.getItem("RedboxZipCode") || "23226"; // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
            }
        }
        , Redbox: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("Redbox", val);
            } else { // get
                return localStorage.getItem("Redbox") || "";
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
            return codejkjk.movies.desktop.controls.CurrentMovieId().val();
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
    },

    init: function () {
        // check if browser can support this site
        var History = window.History;
        if (typeof localStorage == 'undefined' || !navigator.geolocation || !History.enabled) {
            document.writeln("Please use a more recent browser to view this site.  I recommend <a href='http://www.google.com/chrome'>Chrome</a>.");
            return;
        }

        codejkjk.movies.desktop.initHotkeys();
        codejkjk.movies.desktop.initSearch();
        codejkjk.movies.desktop.initGooglePlaces("inputShowtimesZipSmall");
        codejkjk.movies.desktop.initGooglePlaces("inputShowtimesZipBig");
        codejkjk.movies.desktop.bindControls();
        codejkjk.movies.desktop.registerJsRenderHelpers();
        codejkjk.movies.desktop.initHistory(History);
    },

    initHistory: function (History) {
        // handle any state changes
        History.Adapter.bind(window, 'statechange', function () {
            var state = History.getState();
            codejkjk.movies.desktop.handlePushState(state.url);
        });

        // handle "a" clicks - prevent their default and instead push state
        $(document).on('click', 'a[href^="/"]:not(.noPush)', function (e) {
            e.preventDefault();
            History.pushState(null, null, $(this).attr("href"));
        });

        // handle initial page's load
        var initState = History.getState();
        codejkjk.movies.desktop.handlePushState(initState.url);
    },

    handlePushState: function (url) {
        var paths = url.replace('//', '').split('/');
        var firstPath = '/' + paths[1]; // "", "comingsoon", "showtimes" (all navs), "rb" (redbox movie) or "hunger-games" (movie)

        // not sure why FB does this...
        if (firstPath === "/#_=_") {
            window.location.href = "http://seeitornot.co";
            return;
        }

        // check for static pages, in which case do nothing (let them render as-is)
        if (firstPath === "/about" || firstPath === "/reviews") {
            var link = codejkjk.movies.desktop.controls.Nav().find("a[href='{0}']".format(firstPath)); // logo does not have inner html, which is what we use later to select view to show
            link.addClass("selected");
            return;
        }

        // primary nav link?
        if (firstPath === "/" || firstPath === "/comingsoon" || (firstPath === "/redbox" && !paths[2])) {
            codejkjk.movies.desktop.showSection(firstPath);
        }
        else if (firstPath === "/showtimes") {
            if (codejkjk.movies.desktop.currents.ZipCode()) {
                // zipcode is remembered, so load movies w/ zipcode
                codejkjk.movies.desktop.updateShowtimesZip(codejkjk.movies.desktop.currents.ZipCode());
            } else {
                // no zipcode in memory, so display form where user can specify zip (or use current location)
                codejkjk.movies.desktop.controls.showtimeZipOptionsBig().show();
                codejkjk.movies.desktop.controls.showtimes().hide();
            }
            codejkjk.movies.desktop.showSection(firstPath);
        }
        else {
            // /redbox/wreckage or /hunger-games/9999888768
            // movie details link
            var part1 = paths[1];
            if (part1 === "redbox") { // redbox movie
                var rbSlug = paths[2]; // redbox product id
                if (codejkjk.movies.desktop.currents.MovieId()) {
                    // movie details is already in dom, b/c user went to movie link directly
                    codejkjk.movies.desktop.showSection("/");
                    codejkjk.movies.desktop.showMovieDetails("rb");
                } else {
                    codejkjk.movies.desktop.showMovieDetails("rb", rbSlug);
                }
            } else { // rt movie
                var rtMovieId = paths[2]; // rt movie id
                if (codejkjk.movies.desktop.currents.MovieId()) {
                    // movie details is already in dom, b/c user went to movie link directly
                    codejkjk.movies.desktop.showSection("/");
                    codejkjk.movies.desktop.showMovieDetails("rt");
                } else {
                    codejkjk.movies.desktop.showMovieDetails("rt", rtMovieId);
                }
            }
        }
    },

    adminize: function (facebookUserId) {
        codejkjk.movies.Api.authUser(facebookUserId, function (js) {
            $("body").append(js);
        });
    },

    initSearch: function () {
        var searchBox = codejkjk.movies.desktop.controls.searchBox();
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
				.append("<a class='group noPush' href='{0}'><img src='{1}' /><div class='right'><div>{2} ({3})</div><div class='cast'>{4}</div></div></a>".format(item.url, item.imgUrl, item.title, item.year, item.cast))
				.appendTo(ul);
		};
    },

    initGooglePlaces: function (inputId) {
        var input = document.getElementById(inputId);
        if (!input) {
            return;
        }

        var autocomplete = new google.maps.places.Autocomplete(input);

        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            codejkjk.movies.desktop.controls.showtimeZipOptionsSmall().mask();
            codejkjk.movies.desktop.controls.showtimeZipOptionsBig().mask();
            var place = autocomplete.getPlace();
            var formattedAddress = place.formatted_address.replace(", USA", "");
            var latLong = place.geometry.location.toString();
            latLong = latLong.replace("(", "").replace(")", "").replace(" ", "");
            var lat = latLong.split(',')[0];
            var long = latLong.split(',')[1];
            codejkjk.Geo.GetZipCodeFromLatLong(lat, long, function (zipCode) {
                codejkjk.movies.desktop.updateShowtimesZip(zipCode, formattedAddress);
            });
        });
    },

    showSection: function (path) {
        // primary nav change
        // clear out search val if user previously searched for something
        codejkjk.movies.desktop.controls.searchBox().val("");

        var link = codejkjk.movies.desktop.controls.Nav().find("a[href='{0}']".format(path)); // logo does not have inner html, which is what we use later to select view to show
        codejkjk.movies.desktop.controls.NavLinks().removeClass("selected");
        link.addClass("selected");
        codejkjk.movies.desktop.controls.Views().hide();

        // show view
        var sectionToShow = $('section[data-navitemtext="{0}"]'.format(link.html()));
        sectionToShow.show();

        // init image lazyload jquery plugin
        if (!sectionToShow.hasClass("lazyLoaded")) {
            $("img.lazy").lazyload({
                effect: "fadeIn"
            });
            sectionToShow.addClass("lazyLoaded");
        }
    },

    registerJsRenderHelpers: function () {
        $.views.registerHelpers({
            IsReleased: function (releaseDate) {
                var now = new Date();
                releaseDate = new Date(releaseDate);
                return now >= releaseDate;
            },
            IsCurrentTheater: function (iTheaterId) {
                return iTheaterId.toString() == codejkjk.movies.desktop.currents.Theater();
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

    BuildShowtimeDayLinks: function () {
        var today = Date.today(), tomorrow = Date.today().add(1).days(), dayAfterTomorrow = Date.today().add(2).days();

        for (var i = 0; i < 5; i++) {
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
            if (d.toString("yyyyMMdd") == codejkjk.movies.desktop.currents.ShowtimeDay()) {
                cssClass = "active";
            }

            var showtimeDayLink = "<a href='#' class='{0}' data-date='{1}'>{2}</a>".format(cssClass, d.toString("yyyyMMdd"), label);
            codejkjk.movies.desktop.controls.ShowtimeDayLinksContainer().append(showtimeDayLink);
        }
    },

    GetIMDbData: function () {
        return;
        codejkjk.movies.desktop.controls.IMDbMoviesNotSet().each(function () {
            var imdb = $(this);
            var imdbMovieId = imdb.attr("data-imdbmovieid");
            codejkjk.movies.Api.GetIMDbMovie(imdbMovieId, function (movie) {
                var ratings = $(".imdb[data-imdbmovieid='{0}']".format(imdbMovieId));

                if (movie.rating && movie.rating !== "N/A" && movie.votes && movie.votes !== "N/A") {
                    var title = "{0} votes on IMDb.com".format(movie.votes);
                    ratings.html(movie.rating).attr("title", title);
                } else {
                    ratings.html("n/a");
                }
                ratings.removeClass("imdbNotSet");
            });
        });
    },

    LoadSearchResults: function (html) {
        codejkjk.movies.desktop.controls.NavLinks().removeClass("selected");
        if ($.trim(html)) {
            codejkjk.movies.desktop.controls.SearchResultsView().html(html);
        } else {
            codejkjk.movies.desktop.controls.SearchResultsView().html("No results found.");
        }
        codejkjk.movies.desktop.GetIMDbData();

        $("img.lazy").lazyload({
            effect: "fadeIn"
        });
    },

    LoadTheaters: function (postalCode) {
        var theaters = postalCode.theaters;

        var hiddenTheaterMovies = codejkjk.movies.desktop.currents.HiddenTheaterMovies();

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
        if (!codejkjk.movies.desktop.currents.Theater()) {
            // no current theater set, so choose first
            var currentTheaterId = null;
            if (favoriteTheaters.length > 0) { currentTheaterId = favoriteTheaters[0].id; }
            else { currentTheaterId = notFavoriteTheaters[0].id; }
            codejkjk.movies.desktop.currents.Theater(currentTheaterId);
        } else {
            // make sure the theaterId is in the incoming list of theaters
            var results = $.grep(theaters, function (theater, i) {
                return theater.id.toString() == codejkjk.movies.desktop.currents.Theater();
            });
            if (results.length == 0) { // current theaterId does NOT exist in incoming list of theaters, so get the first from favorites or nonfavorites if favorites is empty
                var currentTheaterId = null;
                if (favoriteTheaters.length > 0) { currentTheaterId = favoriteTheaters[0].id; }
                else { currentTheaterId = notFavoriteTheaters[0].id; }
                codejkjk.movies.desktop.currents.Theater(currentTheaterId);
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
        codejkjk.movies.desktop.controls.FavoriteTheaterList().html(
            codejkjk.movies.desktop.controls.FavoriteTheaterListTemplate().render(favoriteTheaters)
        );
        codejkjk.movies.desktop.controls.TheaterList().html(
            codejkjk.movies.desktop.controls.TheaterListTemplate().render(notFavoriteTheaters)
        );
        codejkjk.movies.desktop.controls.CurrentTheaterContainer().html(
            codejkjk.movies.desktop.controls.CurrentTheaterTemplate().render(theaters)
        );

        // move hidden movies to the end of their theaters, so the css doesn't looked jacked up for nth-child col's
        $(".movie.hidden").each(function (i, movie) {
            movie = $(movie);
            var theater = movie.closest(".theater");
            movie.appendTo(theater);
        });

        // fill label for current zip code
        var zipToDisplay = codejkjk.movies.desktop.currents.FriendlyZipCode() || codejkjk.movies.desktop.currents.ZipCode();
        codejkjk.movies.desktop.controls.CurrentZip().html(zipToDisplay);

        codejkjk.movies.desktop.BuildShowtimeDayLinks();

        // show/hide showtimes zip change containers
        codejkjk.movies.desktop.controls.showtimeZipOptions().unmask();
        codejkjk.movies.desktop.controls.showtimeZipOptionsBig().hide();
        if (codejkjk.movies.desktop.controls.showtimeZipOptionsSmall().is(":visible")) {
            codejkjk.movies.desktop.controls.changeShowtimeZipLink().trigger('click');
        }
        codejkjk.movies.desktop.controls.showtimesLoading().hide(); // in case it was shown
        codejkjk.movies.desktop.controls.showtimes().show(); // in case it was hidden

        // now that the theater links are filled, set the currentTheater container's height to match height of theater links container
        var theaterListHeight = codejkjk.movies.desktop.controls.TheaterList().height() + 20;
        codejkjk.movies.desktop.controls.Theaters().css("min-height", theaterListHeight + "px");
    },

    updateShowtimesZip: function (zipCode, friendlyTitle) {
        // update zip and friendly title in memory
        codejkjk.movies.desktop.currents.ZipCode(zipCode);
        if (typeof friendlyTitle != "undefined") {
            codejkjk.movies.desktop.currents.FriendlyZipCode(friendlyTitle);
        } else {
            codejkjk.movies.desktop.currents.FriendlyZipCode("");
        }
        codejkjk.movies.desktop.currents.Theater(""); // new zip, so clear out current theater value

        // if either option containers are visible, mask them
        if (codejkjk.movies.desktop.controls.showtimeZipOptionsBig().is(":visible") || codejkjk.movies.desktop.controls.showtimeZipOptionsSmall().is(":visible")) {
            codejkjk.movies.desktop.controls.showtimeZipOptions().mask();
        } else { // user did not come from option container form, so display loading gif
            codejkjk.movies.desktop.controls.showtimesLoading().show();
        }

        codejkjk.movies.Api.GetTheaters(codejkjk.movies.desktop.currents.ShowtimeDay(), zipCode, codejkjk.movies.desktop.LoadTheaters);
    },

    UpdateRedboxZip: function (zipCode) {
        codejkjk.movies.desktop.currents.RedboxZipCode(zipCode); // update current redbox zip code
        // codejkjk.movies.desktop.controls.CurrentZip().html(zipCode);
        codejkjk.movies.desktop.currents.Redbox(""); // new zip, so clear out current redbox store id value

        codejkjk.Geo.GetLatLongFromZip(zipCode, function (lat, long) {
            codejkjk.movies.Api.GetRedboxesHtml(lat, long, function (html) {
                codejkjk.movies.desktop.controls.Redboxes().html(html);
            });
        });
    },

    showMovieDetails: function (rbOrRt, movieIdToAjaxLoad) {
        // show overlay
        var overlayHeight = $(document).height() + "px";
        var overlayWidth = $(document).width() + "px";
        codejkjk.movies.desktop.controls.Overlay().css("height", overlayHeight).css("width", overlayWidth).show();

        if (movieIdToAjaxLoad) {
            // user went to movie link by clicking on a poster, so ajax-load it
            codejkjk.movies.desktop.controls.MovieDetailsPopup().html("<div class='loading'></div>");
            codejkjk.movies.desktop.controls.MovieDetailsPopup().show();

            if (rbOrRt === "rb") {
                codejkjk.movies.Api.GetRedboxMovieHtml(movieIdToAjaxLoad, function (html) {
                    codejkjk.movies.desktop.controls.MovieDetailsPopup().html(html);
                    FB.XFBML.parse();
                    codejkjk.movies.desktop.InitZeroClipboard();
                    codejkjk.siteActions.wireReleaseDates();
                });
            } else {
                codejkjk.movies.Api.GetMovieHtml(movieIdToAjaxLoad, function (html) {
                    codejkjk.movies.desktop.controls.MovieDetailsPopup().html(html);
                    FB.XFBML.parse();
                    codejkjk.movies.desktop.InitZeroClipboard();
                    codejkjk.siteActions.wireReleaseDates();
                    if (typeof refreshAdmin === "function") { refreshAdmin(); }
                });
            }
        } else {
            // movie details are already in dom, so just init zeroclipboard b/c it's ready to go
            codejkjk.movies.desktop.controls.MovieDetailsPopup().show();
            codejkjk.movies.desktop.InitZeroClipboard();
            codejkjk.siteActions.wireReleaseDates();
            if (typeof refreshAdmin === "function") { refreshAdmin(); }
        }
    },

    InitZeroClipboard: function () {
        var clip = new ZeroClipboard.Client();
        clip.setText(codejkjk.movies.desktop.controls.MovieUrl().val());
        clip.glue('copyButton');

        clip.addEventListener('complete', function (client, text) {
            codejkjk.movies.desktop.controls.CopySuccess().show().delay(2500).fadeOut('fast');
        });
    },

    bindControls: function () {
        // handle "Browse Nearby Redbox"
        $(document).on('click', codejkjk.movies.desktop.controls.BrowseLinkSelector(), function (e) {
            e.preventDefault();
            $(this).next("div").toggleClass("hidden");
        });

        // handle "see nearby redboxes" links
        $(document).on('click', codejkjk.movies.desktop.controls.SeeNearbyRedboxesSelector(), function (e) {
            e.preventDefault();
            codejkjk.Geo.GetLatLong(function (lat, long) {
                codejkjk.movies.Api.GetRedboxesHtml(lat, long, function (html) {
                    codejkjk.movies.desktop.controls.Redboxes().html(html);
                });
            });
        });

        // handle favorite theater links
        $(document).on('click', codejkjk.movies.desktop.controls.FavoriteLinksSelector(), function (e) {
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
            codejkjk.movies.Api.GetTheaters(codejkjk.movies.desktop.currents.ShowtimeDay(), codejkjk.movies.desktop.currents.ZipCode(), codejkjk.movies.desktop.LoadTheaters);
        });

        // handle theater link clicks
        $(document).on('click', codejkjk.movies.desktop.controls.TheaterLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            var theaterId = link.attr("data-theaterid");
            codejkjk.movies.desktop.currents.Theater(theaterId);
            $(codejkjk.movies.desktop.controls.TheaterLinksSelector()).removeClass("selected glowing");
            link.addClass("selected glowing");
            codejkjk.movies.desktop.controls.Theaters().removeClass("selected glowing");
            $(".theater[data-theaterid='{0}']".format(theaterId)).addClass("selected glowing");
        });

        // handle hide movie links
        $(document).on('click', codejkjk.movies.desktop.controls.HideMovieLinksSelector(), function (e) {
            e.preventDefault();
            var movie = $(this);
            var theater = movie.closest(".theater");
            var hiddenTheaterMovies = codejkjk.movies.desktop.currents.HiddenTheaterMovies();
            hiddenTheaterMovies.pushIfDoesNotExist(movie.data().theatermovie);
            codejkjk.movies.desktop.currents.HiddenTheaterMovies(hiddenTheaterMovies.join(','));

            movie.closest(".movie").fadeOut('fast', function () {
                $(this).addClass("hidden").removeAttr("style").appendTo(theater); // append to end of list of movies
                theater.find(".numHiddenMovies").text(theater.find(".movie.hidden").length);
                theater.find(".showHiddenMoviesLinkContainer").removeClass("hidden");
            });

        });

        // handle "show hidden movies" links
        $(document).on('click', codejkjk.movies.desktop.controls.ShowHiddenMoviesLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            var theater = link.closest(".theater");
            theater.find(".movie.hidden").removeClass("hidden");
            link.parent().addClass("hidden");

            // update localStorage; trim out the theater that this movie belongs to, all of its hidden movies
            var hiddenTheaterMovies = codejkjk.movies.desktop.currents.HiddenTheaterMovies();
            hiddenTheaterMovies = $.grep(hiddenTheaterMovies, function (hiddenTheaterMovie, i) {
                return hiddenTheaterMovie.split('-')[0] != theater.data().theaterid;
            });
            codejkjk.movies.desktop.currents.HiddenTheaterMovies(hiddenTheaterMovies.join(','))
        });

        // handle close movie details link AND overlay click
        $(document).on("click", codejkjk.movies.desktop.controls.CloseMovieDetailsLinkSelector(), function (e) {
            codejkjk.movies.desktop.controls.Overlay().hide();
            codejkjk.movies.desktop.controls.MovieDetailsPopup().hide().html("");
            var currentNav = codejkjk.movies.desktop.controls.CurrentNavItem();
            if (currentNav && currentNav.hasClass("noPush")) {
                History.pushState(null, null, currentNav.attr("href"));
            } else {
                currentNav.trigger('click');
            }
        });

        // handle showtime day link clicks (Today, Tomorrow, etc)
        $(document).on("click", codejkjk.movies.desktop.controls.ShowtimeDayLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            codejkjk.movies.desktop.currents.ShowtimeDay().val(link.data().date);

            codejkjk.movies.Api.GetTheaters(link.data().date, codejkjk.movies.desktop.controls.CurrentZip().html(), codejkjk.movies.desktop.LoadTheaters);
        });

        // handle "change" link for zip
        codejkjk.movies.desktop.controls.changeShowtimeZipLink().click(function (e) {
            e.preventDefault();
            var setFocus = !codejkjk.movies.desktop.controls.showtimeZipOptionsSmall().is(":visible");
            if (setFocus) {
                codejkjk.movies.desktop.controls.InputShowtimesZip().val("");
            }
            codejkjk.movies.desktop.controls.showtimeZipOptionsSmall().slideToggle('fast', function () {
                if (setFocus) {
                    codejkjk.movies.desktop.controls.InputShowtimesZip().focus();
                }
            });
        });

        // handle "back to The Hunger Games" link on movie showtimes link / redbox availability on movie popup
        $(document).on('click', codejkjk.movies.desktop.controls.BackToMovieDetailsLinkSelector(), function (e) {
            e.preventDefault();

            codejkjk.movies.desktop.controls.MovieDetails().show();
            $(this).closest("div").addClass("hidden");
        });

        // handle showtimes links on movie detail popups
        $(document).on('click', codejkjk.movies.desktop.controls.ShowtimesLinksSelector(), function (e) {
            e.preventDefault();

            var link = $(this);
            var movieShowtimesContainer = codejkjk.movies.desktop.controls.MovieShowtimes();
            var theaterList = codejkjk.movies.desktop.controls.TheatersForMovieList();

            if (movieShowtimesContainer.hasClass("loaded")) { // already been loaded previously, so just show it
                codejkjk.movies.desktop.controls.MovieDetails().hide();
                movieShowtimesContainer.removeClass("hidden");
            } else {
                // first thing, add loading class
                link.addClass("loading");
                codejkjk.Geo.GetZipCode(function (zipCode) {
                    codejkjk.movies.Api.GetTheatersForMovie(codejkjk.movies.desktop.currents.ShowtimeDay(), zipCode, codejkjk.movies.desktop.currents.MovieId(), function (theatersHtml) {
                        theaterList.html(theatersHtml);

                        // remove loading class
                        link.removeClass("loading");
                        codejkjk.movies.desktop.controls.MovieDetails().hide();
                        movieShowtimesContainer.removeClass("hidden").addClass("loaded");
                    });
                });
            }
        });

        // handle watch trailer link on movie detail popups
        $(document).on('click', codejkjk.movies.desktop.controls.watchTrailerLinkSelector(), function (e) {
            e.preventDefault();

            var link = $(this);
            var ivaPublishedId = link.attr("data-ivapublishedid");
            var trailerContainer = codejkjk.movies.desktop.controls.trailer();

            if (trailerContainer.hasClass("loaded")) { // already been loaded previously, so just show it
                codejkjk.movies.desktop.controls.MovieDetails().hide();
                trailerContainer.removeClass("hidden");
            } else {
                var swfUrl = 'http://www.videodetective.net/flash/players/movieapi/?publishedid={0}'.format(ivaPublishedId);
                // var flashVars = {};
                // flashVars.allowFullScreen = "true";
                // swfobject.embedSWF(swfUrl, "trailerGoesHere", "400", "300", "9.0.0", "", flashVars); // 500 x 380 is larger but fuzzy
                swfobject.embedSWF(swfUrl, "trailerGoesHere", "400", "280", "9.0.0"); // 500 x 380 is larger but fuzzy
                codejkjk.movies.desktop.controls.MovieDetails().hide();
                trailerContainer.removeClass("hidden").addClass("loaded");
            }
        });

        // handle showtimes links on movie detail popups
        $(document).on('click', codejkjk.movies.desktop.controls.NearbyRedboxLinksSelector(), function (e) {
            e.preventDefault();

            var link = $(this);
            var redboxAvailsContainer = codejkjk.movies.desktop.controls.RedboxAvails();
            var redboxAvailsList = codejkjk.movies.desktop.controls.RedboxAvailsList();

            if (redboxAvailsContainer.hasClass("loaded")) {
                // already been loaded previously, so just show it
                codejkjk.movies.desktop.controls.MovieDetails().hide();
                redboxAvailsContainer.removeClass("hidden");
            } else {
                // first thing, add loading class
                link.addClass("loading");
                redboxAvailsList.html("This feature coming soon...");

                // remove loading class
                link.removeClass("loading");
                codejkjk.movies.desktop.controls.MovieDetails().hide();
                redboxAvailsContainer.removeClass("hidden").addClass("loaded");
                // });
                // });
            }
        });

        // bind showtimeUseMyLocations
        codejkjk.movies.desktop.controls.showtimeUseMyLocations().click(function (e) {
            e.preventDefault();
            codejkjk.movies.desktop.controls.showtimeZipOptionsSmall().mask();
            codejkjk.movies.desktop.controls.showtimeZipOptionsBig().mask();
            codejkjk.Geo.GetZipCode(function (zipCode) {
                codejkjk.movies.desktop.updateShowtimesZip(zipCode);
            });
        });

        // handle button that sets manual set of zip for redbox search
        codejkjk.movies.desktop.controls.SearchRedboxZipCodeButton().click(function (e) {
            e.preventDefault();
            var zipCode = codejkjk.movies.desktop.controls.RedboxZipCodeInput().val();
            codejkjk.movies.desktop.UpdateRedboxZip(zipCode);
        });

        // handle Enter key on manual zip input box for redbox search - triggers "Search" button click
        codejkjk.movies.desktop.controls.RedboxZipCodeInput().keydown(function (e) {
            if (e.keyCode == 13) {
                codejkjk.movies.desktop.controls.SearchRedboxZipCodeButton().trigger('click');
            }
        });

        // handle Enter key on search box
        codejkjk.movies.desktop.controls.searchBox().keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                var q = $(this).val();
                // loading 
                codejkjk.movies.desktop.controls.SearchResultsView().html("<div class='loading'></div>");
                $(".content").hide();
                codejkjk.movies.desktop.controls.SearchResultsView().show();
                codejkjk.movies.Api.SearchMovies(q, codejkjk.movies.desktop.LoadSearchResults);
                $(this).blur();
            }
        });
    },

    initHotkeys: function () {
        $(document).bind('keyup', 'q', function () {
            if (!codejkjk.movies.desktop.controls.searchBox().is(":focus")) {
                codejkjk.movies.desktop.controls.searchBox().focus();
            }
        });
        $(document).bind('keyup', 'esc', function () {
            if (codejkjk.movies.desktop.currents.MovieId()) {
                $(codejkjk.movies.desktop.controls.CloseMovieDetailsLinkSelector()).trigger('click');
            }
        });
        $(document).bind('keyup', 'Alt_a', function () {
            // alert('alt+a');
        });
    }
}

$(document).ready(function () {
    codejkjk.movies.desktop.init();
});