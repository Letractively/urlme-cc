registerNS("codejkjk.movies.Mobile");

codejkjk.movies.Mobile = {
    // page elements
    Controls: {
        IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , Theaters: function () { return $("#theaters"); }
        , TheaterTemplate: function () { return $("#theaterTemplate"); }
    },

    Currents: {
        ZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("ZipCode", val);
            } else { // get
                return localStorage.getItem("ZipCode") || "23226"; // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
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

    Init: function () {
        codejkjk.movies.Mobile.RegisterJsRenderHelpers();

        if (codejkjk.movies.Mobile.Controls.Theaters().length) {
            codejkjk.movies.Mobile.LoadTheaters();
        }
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

$(document).on('pageinit', function (e) {
    // alert('loading imdb');
    // if load IMDb movies that aren't set from server-side box office and upcoming movie loads
    codejkjk.movies.Mobile.GetIMDbData();
});

// if load IMDb movies that aren't set from server-side box office and upcoming movie loads
codejkjk.movies.Mobile.GetIMDbData();