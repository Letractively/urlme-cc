registerNS("codejkjk.movies.Mobile");

codejkjk.movies.Mobile = {
    // page elements
    Controls: {
        IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , InputShowtimesZip: function () { return $("#inputShowtimesZip"); }
        , CurrentShowtimesZip: function () { return $("#currentShowtimesZip"); }
        , ShowtimesHeader: function () { return $("#showtimesHeader"); }
        , ShowtimesOptionsLinkSelector: function () { return "#showtimesOptionsLink"; }
        , ShowtimesOptions: function () { return $("#showtimesOptions"); }
        , Theaters: function () { return $("#theaters"); }
        , TheaterTemplate: function () { return $("#theaterTemplate"); }
    },

    Currents: {
        ZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("ZipCode", val);
            } else { // get
                return localStorage.getItem("ZipCode"); // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
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

    LoadTheaters: function () {
        // codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(Date.today().toString("yyyyMMdd"));
        // codejkjk.movies.HomeIndex.Controls.CurrentZip().html(codejkjk.movies.HomeIndex.Currents.ZipCode());
        // codejkjk.movies.Api.GetTheaters(codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(), codejkjk.movies.HomeIndex.Currents.ZipCode(), codejkjk.movies.HomeIndex.LoadTheaters);
        // codejkjk.movies.Api.GetTheaters(Date.today().toString("yyyyMMdd"), "23226", 
    },

    BindControls: function () {
        $(document).on('click', codejkjk.movies.Mobile.Controls.ShowtimesOptionsLinkSelector(), function (e) {
            e.preventDefault();
            var clearInput = !codejkjk.movies.Mobile.Controls.ShowtimesOptions().is(":visible");
            if (clearInput) {
                codejkjk.movies.Mobile.Controls.InputShowtimesZip().val("");
            }
            codejkjk.movies.Mobile.Controls.ShowtimesOptions().slideToggle('fast');
        });
    },

    Init: function () {
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

        // codejkjk.movies.HomeIndex.Currents.Theater(""); // new zip, so clear out current theater value
        // codejkjk.movies.Api.GetTheaters(codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(), zip, codejkjk.movies.HomeIndex.LoadTheaters);
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