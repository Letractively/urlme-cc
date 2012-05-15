registerNS("codejkjk.movies.Mobile");

codejkjk.movies.Mobile = {
    // page elements
    Controls: {
        FavoriteTheaterList: function () { return $("#favoriteTheaterList"); }
        , FavoriteTheaterListTemplate: function () { return $("#favoriteTheaterListTemplate"); }
        , IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , InputShowtimesZip: function () { return $("#inputShowtimesZip"); }
        , CurrentShowtimesZip: function () { return $("#currentShowtimesZip"); }
        , CurrentTheater: function () { return $("#currentTheater"); }
        , TheaterLinkSelector: function () { return ".theaterLink"; }
        , ShowtimesHeader: function () { return $("#showtimesHeader"); }
        , ShowtimesOptionsLinkSelector: function () { return "#showtimesOptionsLink"; }
        , ShowtimesOptions: function () { return $("#showtimesOptions"); }
        , TheaterBackLinkSelector: function () { return "#theaterBackLink"; }
        , TheaterHeader: function () { return $("#theaterHeader"); }
        , TheaterList: function () { return $("#theaterList"); }
        , TheaterListTemplate: function () { return $("#theaterListTemplate"); }
        , Theaters: function () { return $("#theaters"); }
        , TheaterTemplate: function () { return $("#theaterTemplate"); }
        , UseCurrentLocationForShowtimesSelector: function () { return "#useCurrentLocationForShowtimes"; }
    },

    Currents: {
        ZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("ZipCode", val);
            } else { // get
                return localStorage.getItem("ZipCode"); // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
            }
        }
        , ShowtimeDay: function (val) {
            if (typeof val != "undefined") { // set
                $("input#currentShowtimeDay").val(val);
            } else { // get
                return $("input#currentShowtimeDay").val();
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
    },

    LoadTheaters: function (postalCode) {
        $.mobile.showPageLoadingMsg();
        var theaters = postalCode.theaters;
        var hiddenTheaterMovies = codejkjk.movies.Mobile.Currents.HiddenTheaterMovies();

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
        if (!codejkjk.movies.Mobile.Currents.Theater()) {
            // no current theater set, so choose first
            var currentTheaterId = null;
            if (favoriteTheaters.length > 0) { currentTheaterId = favoriteTheaters[0].id; }
            else { currentTheaterId = notFavoriteTheaters[0].id; }
            codejkjk.movies.Mobile.Currents.Theater(currentTheaterId);
        } else {
            // make sure the theaterId is in the incoming list of theaters
            var results = $.grep(theaters, function (theater, i) {
                return theater.id.toString() == codejkjk.movies.Mobile.Currents.Theater();
            });
            if (results.length == 0) { // current theaterId does NOT exist in incoming list of theaters, so get the first from favorites or nonfavorites if favorites is empty
                var currentTheaterId = null;
                if (favoriteTheaters.length > 0) { currentTheaterId = favoriteTheaters[0].id; }
                else { currentTheaterId = notFavoriteTheaters[0].id; }
                codejkjk.movies.Mobile.Currents.Theater(currentTheaterId);
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
        codejkjk.movies.Mobile.Controls.FavoriteTheaterList().html(
            codejkjk.movies.Mobile.Controls.FavoriteTheaterListTemplate().render(favoriteTheaters)
        ).listview('refresh');
        codejkjk.movies.Mobile.Controls.TheaterList().html(
            codejkjk.movies.Mobile.Controls.TheaterListTemplate().render(notFavoriteTheaters)
        ).listview('refresh');
        //        codejkjk.movies.Mobile.Controls.CurrentTheaterContainer().html(
        //            codejkjk.movies.Mobile.Controls.CurrentTheaterTemplate().render(theaters)
        //        );

        // codejkjk.movies.Mobile.BuildShowtimeDayLinks();

        if (codejkjk.movies.Mobile.Controls.ShowtimesOptions().is(":visible")) {
            // codejkjk.movies.Mobile.Controls.ChangeOptionsContainer().unmask();
            codejkjk.movies.Mobile.Controls.ShowtimesOptions().slideToggle('fast');
        }

        // now that the theater links are filled, set the currentTheater container's height to match height of theater links container
        // var theaterListHeight = codejkjk.movies.Mobile.Controls.TheaterList().height() + 20;
        // codejkjk.movies.Mobile.Controls.Theaters().css("min-height", theaterListHeight + "px");
        $.mobile.hidePageLoadingMsg();
    },

    BindControls: function () {
        // "options" link for showtimes
        $(document).on('click', codejkjk.movies.Mobile.Controls.ShowtimesOptionsLinkSelector(), function (e) {
            e.preventDefault();
            var clearInput = !codejkjk.movies.Mobile.Controls.ShowtimesOptions().is(":visible");
            if (clearInput) {
                codejkjk.movies.Mobile.Controls.InputShowtimesZip().val("");
            }
            codejkjk.movies.Mobile.Controls.ShowtimesOptions().slideToggle('fast');
        });
        // "use current location" click for showtimes zip
        $(document).on('click', codejkjk.movies.Mobile.Controls.UseCurrentLocationForShowtimesSelector(), function (e) {
            e.preventDefault();
            codejkjk.Geo.GetZipCode(function (zipCode) {
                codejkjk.movies.Mobile.UpdateZip(zipCode);
            });
        });
        // theater link clicks
        $(document).on('click', codejkjk.movies.Mobile.Controls.TheaterLinkSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            var theaterName = link.find("h3").text();
            codejkjk.movies.Mobile.Controls.CurrentTheater().html(theaterName);
            codejkjk.movies.Mobile.Controls.TheaterList().hide();
            codejkjk.movies.Mobile.Controls.FavoriteTheaterList().hide();
            codejkjk.movies.Mobile.Controls.TheaterHeader().show();
            codejkjk.movies.Mobile.Controls.ShowtimesHeader().hide();
        });
        // theater back link
        $(document).on('click', codejkjk.movies.Mobile.Controls.TheaterBackLinkSelector(), function (e) {
            e.preventDefault();
            codejkjk.movies.Mobile.Controls.TheaterList().show();
            codejkjk.movies.Mobile.Controls.FavoriteTheaterList().show();
            codejkjk.movies.Mobile.Controls.TheaterHeader().hide();
            codejkjk.movies.Mobile.Controls.ShowtimesHeader().show();
        });
    },

    Init: function () {
        // init happens only once. make it count.
        codejkjk.movies.Mobile.BindControls();
    },

    InitGooglePlaces: function (inputId) {
        var input = document.getElementById(inputId);
        var autocomplete = new google.maps.places.Autocomplete(input);

        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            // codejkjk.movies.Mobile.Controls.ShowtimesOptions().mask();
            var place = autocomplete.getPlace();
            var formattedAddress = place.formatted_address.replace(", USA", "");
            var latLong = place.geometry.location.toString();
            latLong = latLong.replace("(", "").replace(")", "").replace(" ", "");
            var lat = latLong.split(',')[0];
            var long = latLong.split(',')[1];
            codejkjk.Geo.GetZipCodeFromLatLong(lat, long, function (zipCode) {
                codejkjk.movies.Mobile.UpdateZip(zipCode, formattedAddress);
            });
        });
    },

    PageChanged: function () {
        codejkjk.movies.Mobile.RegisterJsRenderHelpers();

        if ($("#homeShowtimesMobile").is(":visible")) {
            // do showtime stuff
            // init showtime day if it hasn't been set yet
            if (!codejkjk.movies.Mobile.Currents.ShowtimeDay()) {
                codejkjk.movies.Mobile.Currents.ShowtimeDay(Date.today().toString("yyyyMMdd"));
            }

            // InputShowtimesZip mapped to google places api?
            if (!codejkjk.movies.Mobile.Controls.InputShowtimesZip().hasClass("gPlacesLoaded")) {
                codejkjk.movies.Mobile.InitGooglePlaces("inputShowtimesZip");
                codejkjk.movies.Mobile.Controls.InputShowtimesZip().addClass("gPlacesLoaded");
            }

            if (codejkjk.movies.Mobile.Currents.ZipCode()) {
                // zipcode already set
                codejkjk.movies.Mobile.UpdateZip(codejkjk.movies.Mobile.Currents.ZipCode(), codejkjk.movies.Mobile.Currents.ZipCodeFriendlyTitle());
            } else if (!codejkjk.movies.Mobile.Controls.ShowtimesOptions().is(":visible")) {
                // no zipcode set & showtimesoptions container is invisible, so show it
                codejkjk.movies.Mobile.Controls.ShowtimesOptions().slideToggle('fast');
            }
        }
        else {
            // do top box office stuff
        }

        $("img.lazy").lazyload({
            effect: "fadeIn"
        });
    },

    UpdateZip: function (zip, friendlyTitle) {
        codejkjk.movies.Mobile.Currents.ZipCode(zip); // update current zip code
        if (typeof friendlyTitle != "undefined" && friendlyTitle) {
            codejkjk.movies.Mobile.Controls.CurrentShowtimesZip().html(friendlyTitle);
        } else {
            codejkjk.movies.Mobile.Controls.CurrentShowtimesZip().html(zip);
        }
        codejkjk.movies.Mobile.Controls.ShowtimesHeader().show();
        codejkjk.movies.Mobile.Controls.ShowtimesOptions().hide();

        codejkjk.movies.Mobile.Currents.Theater(""); // new zip, so clear out current theater value
        codejkjk.movies.Api.GetTheaters(codejkjk.movies.Mobile.Currents.ShowtimeDay(), zip, codejkjk.movies.Mobile.LoadTheaters);
    },

    RegisterJsRenderHelpers: function () {
        $.views.registerHelpers({
            IsReleased: function (releaseDate) {
                var now = new Date();
                releaseDate = new Date(releaseDate);
                return now >= releaseDate;
            },
            IsCurrentTheater: function (iTheaterId) {
                return iTheaterId.toString() == codejkjk.movies.Mobile.Currents.Theater();
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
            GetIMDbMovieUrl: function (imdbId) {
                return codejkjk.movies.IMDB.GetMovieUrl(imdbId);
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

    GetIMDbData: function () {
        codejkjk.movies.Mobile.Controls.IMDbMoviesNotSet().each(function () {
            var imdb = $(this);
            var q = imdb.attr("data-imdbq");
            codejkjk.movies.Api.GetIMDbMovie(q, function (movie) {
                var ratings = $(".imdbNotSet[data-imdbq='{0}']".format(q));

                if (movie.rating) {
                    ratings.html(movie.rating);
                }

                if (movie.rating && movie.rating !== "N/A" && movie.votes && movie.votes !== "N/A") {
                    var title = "{0} votes on IMDb.com".format(movie.votes);
                    ratings.html(movie.rating).attr("title", title);
                } else {
                    ratings.html("n/a");
                }
                ratings.removeClass("imdbNotSet");
            });
        });
    }
}

$(document).ready(function () {
    codejkjk.movies.Mobile.Init();
});

$(document).on('pagechange', function (e) {
    codejkjk.movies.Mobile.PageChanged();
});

// if load IMDb movies that aren't set from server-side box office and upcoming movie loads
// codejkjk.movies.Mobile.GetIMDbData();