registerNS("codejkjk.movies.Mobile");

var jsRenderHelpersRendered= false;

codejkjk.movies.Mobile = {
    // page elements
    Controls: {
        FavoriteLinksSelector: function () { return ".favoriteLink"; }
        , FavoriteTheaterList: function () { return $("#favoriteTheaterListContent ul"); }
        , FavoriteTheaterListContent: function () { return $("#zip div[data-role='content']:first"); }
        , FavoriteTheaterListTemplate: function () { return $("#favoriteTheaterListTemplate"); }
        , IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , InputShowtimesZip: function () { return $("#inputShowtimesZip"); }
        , CurrentShowtimesZip: function () { return $("#currentShowtimesZip"); }
        , CurrentTheater: function () { return $("#theater div[data-role='header'] span"); }
        , CurrentTheaterMovie: function () { return $("#theatermovie div[data-role='header'] span"); }
        , ShowtimesHeader: function () { return $("#zip div[data-role='header']"); }
        , ShowtimesOptionsLinkSelector: function () { return "#zip div[data-role='header'] a"; }
        , ShowtimesOptions: function () { return $("#showtimes div[data-role='content']"); }
        , TheaterMovie: function () { return $("#theatermovie div[data-role='content']"); }
        , TheaterMovieHeader: function () { return $("#theatermovie div[data-role='header']"); }
        , TheaterMovieLinkSelector: function () { return ".theaterMovieLink"; }
        , TheaterLinkSelector: function () { return ".theaterLink"; }
        , TheaterMovieBackLinkSelector: function () { return "#theatermovie div[data-role='header'] a"; }
        , TheaterBackLinkSelector: function () { return "#theater div[data-role='header'] a"; }
        , TheaterHeader: function () { return $("#theater div[data-role='header']"); }
        , TheaterList: function () { return $("#zip ul[data-role='listview']:last"); }
        , TheaterLists: function () { return $("#zip div[data-role='content']"); }
        , TheaterListTemplate: function () { return $("#theaterListTemplate"); }
        , TheaterMovieList: function () { return $("#theater div[data-role='content'] ul"); }
        , TheaterMovieTemplate: function () { return $("#theaterMovieTemplate"); }
        , Theaters: function () { return $("#theaters"); }
        , TheaterTemplate: function () { return $("#theaterTemplate"); }
        , UseCurrentLocationForShowtimesSelector: function () { return "#showtimes div[data-role='content'] a"; }
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
        if (favoriteTheaters.length) {
            codejkjk.movies.Mobile.Controls.FavoriteTheaterList().html(
                codejkjk.movies.Mobile.Controls.FavoriteTheaterListTemplate().render(favoriteTheaters)
            ).listview('refresh').parent().show();
            codejkjk.movies.Mobile.Controls.FavoriteTheaterListContent().show();
        } else {
            codejkjk.movies.Mobile.Controls.FavoriteTheaterListContent().hide();
        }
        codejkjk.movies.Mobile.Controls.TheaterList().html(
            codejkjk.movies.Mobile.Controls.TheaterListTemplate().render(notFavoriteTheaters)
        ).listview('refresh').parent().show();

        if (codejkjk.movies.Mobile.Controls.ShowtimesOptions().is(":visible")) {
            codejkjk.movies.Mobile.Controls.ShowtimesOptions().slideToggle('fast');
        }

        // now that the theater links are filled, set the currentTheater container's height to match height of theater links container
        $.mobile.hidePageLoadingMsg();
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
        if (!jsRenderHelpersRendered) {
            codejkjk.movies.Mobile.RegisterJsRenderHelpers();            
            jsRenderHelpersRendered = true;
        }

        var hash = location.hash;
        if (hash == "#showtimes") {
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
        } else {

        }

        $("img.lazy").lazyload({
            effect: "fadeIn"
        });
    },

    UpdateZip: function (zip, friendlyTitle) {
        codejkjk.movies.Mobile.Controls.TheaterLists().hide(); // hide theater lists from previous zips
        $.mobile.showPageLoadingMsg();
        codejkjk.movies.Mobile.Currents.ZipCode(zip); // update current zip code
        if (typeof friendlyTitle != "undefined" && friendlyTitle) {
            codejkjk.movies.Mobile.Controls.CurrentShowtimesZip().html(friendlyTitle);
        } else {
            codejkjk.movies.Mobile.Controls.CurrentShowtimesZip().html(zip);
        }
        codejkjk.movies.Mobile.Controls.ShowtimesHeader().show();
        codejkjk.movies.Mobile.Controls.ShowtimesOptions().hide();

        // codejkjk.movies.Mobile.Currents.Theater(""); // new zip, so clear out current theater value
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
    },

    BindControls: function () {
        // handle favorite theater links
        $(document).on('click', codejkjk.movies.Mobile.Controls.FavoriteLinksSelector(), function (e) {
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
            $.mobile.showPageLoadingMsg();
            codejkjk.movies.Api.GetTheaters(codejkjk.movies.Mobile.Currents.ShowtimeDay(), codejkjk.movies.Mobile.Currents.ZipCode(), codejkjk.movies.Mobile.LoadTheaters);
        });

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
            $.mobile.showPageLoadingMsg();
            var link = $(this);
            var theaterName = link.find("h3").text();
            var theaterId = link.attr("data-theaterid");
            codejkjk.movies.Mobile.Controls.CurrentTheater().html(theaterName);
            codejkjk.movies.Mobile.Controls.TheaterLists().hide();
            codejkjk.movies.Mobile.Controls.TheaterHeader().show();
            codejkjk.movies.Mobile.Controls.ShowtimesHeader().hide();

            // load theater movies
            codejkjk.movies.Api.GetTheaterMovies(codejkjk.movies.Mobile.Currents.ShowtimeDay(), codejkjk.movies.Mobile.Currents.ZipCode(), theaterId, function (theater) {
                codejkjk.movies.Mobile.Controls.TheaterMovieList().html(
                    codejkjk.movies.Mobile.Controls.TheaterMovieTemplate().render(theater)
                ).show().listview('refresh');
                $.mobile.hidePageLoadingMsg();
            });
        });
        // theater movie link
        $(document).on('click', codejkjk.movies.Mobile.Controls.TheaterMovieLinkSelector(), function (e) {
            e.preventDefault();
            $.mobile.showPageLoadingMsg();
            var link = $(this);
            var movieName = link.find("h3").text();
            var rtMovieId = link.attr("data-rtmovieid");
            codejkjk.movies.Mobile.Controls.CurrentTheaterMovie().html(movieName);
            codejkjk.movies.Mobile.Controls.TheaterHeader().hide();
            codejkjk.movies.Mobile.Controls.TheaterMovieHeader().show();

            // load single theater movie
            codejkjk.movies.Api.GetMovieMobileHtml(rtMovieId, function (html) {
                codejkjk.movies.Mobile.Controls.TheaterMovie().html(html).show(); // show after filling
                $.mobile.hidePageLoadingMsg();
            });
        });
        // theater movie back link
        $(document).on('click', codejkjk.movies.Mobile.Controls.TheaterMovieBackLinkSelector(), function (e) {
            e.preventDefault();
            codejkjk.movies.Mobile.Controls.TheaterHeader().show();
            codejkjk.movies.Mobile.Controls.TheaterMovieList().show();
            codejkjk.movies.Mobile.Controls.TheaterMovie().hide();
        });
        // theater back link
        $(document).on('click', codejkjk.movies.Mobile.Controls.TheaterBackLinkSelector(), function (e) {
            e.preventDefault();
            if (codejkjk.movies.Mobile.Controls.FavoriteTheaterListContent().find("li").length) {
                codejkjk.movies.Mobile.Controls.TheaterLists().show();
            } else {
                codejkjk.movies.Mobile.Controls.TheaterLists().last().show();
            }
            codejkjk.movies.Mobile.Controls.TheaterHeader().hide();
            codejkjk.movies.Mobile.Controls.ShowtimesHeader().show();
            codejkjk.movies.Mobile.Controls.TheaterMovieList().hide();
        });
    }
}

$(document).ready(function () {
    codejkjk.movies.Mobile.Init();
});

$(document).on('pagechange', function (e, o) {
    codejkjk.movies.Mobile.PageChanged();
});

// if load IMDb movies that aren't set from server-side box office and upcoming movie loads
// codejkjk.movies.Mobile.GetIMDbData();