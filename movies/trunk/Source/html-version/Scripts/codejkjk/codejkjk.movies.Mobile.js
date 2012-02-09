registerNS("codejkjk.movies.Mobile");

codejkjk.movies.Mobile = {
    // page elements
    Controls: {
        IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , MovieList: function () { return $("#movieList"); }
        , MovieTemplate: function () { return $("#movieTemplate"); }
    },

    Currents: {
        HiddenTheaterMovies: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("HiddenTheaterMovies", val);
            } else {
                var ret = localStorage.getItem("HiddenTheaterMovies");
                return ret ? ret.split(',') : [];
            }
        }
    },

    Init: function () {
        codejkjk.movies.Mobile.RegisterJsRenderHelpers();

        var pathName = location.pathname;
        if (pathName.indexOf("mobile.htm") >= 0) {
            // load box office
            codejkjk.movies.RottenTomatoes.GetBoxOfficeMovies(function (movies) {

                codejkjk.movies.Mobile.Controls.MovieList().html(
                    codejkjk.movies.Mobile.Controls.MovieTemplate().render(movies)
                ).listview('refresh');
                codejkjk.movies.Mobile.GetIMDbData();

            });
        } else if (pathName.indexOf("mobile_comingsoon.htm") >= 0) {
            // load upcoming
            codejkjk.movies.RottenTomatoes.GetUpcomingMovies(function (movies) {

                codejkjk.movies.Mobile.Controls.MovieList().html(
                    codejkjk.movies.Mobile.Controls.MovieTemplate().render(movies)
                ).listview('refresh');
                codejkjk.movies.Mobile.GetIMDbData();

            });
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
                return iTheaterId.toString() == codejkjk.movies.HomeIndex.Currents.Theater();
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
            codejkjk.movies.IMDB.GetMovie(imdb.attr("data-imdbmovieid"), function (imdbMovieId, movie) {
                var ratings = $(".imdb[data-imdbmovieid='{0}']".format(imdbMovieId));

                var rating = movie && movie.Rating;
                if (rating) {
                    ratings.html(rating);
                }
                ratings.removeClass("imdbNotSet");                
            });
        });
    },

    PageBeforeChange: function (e, data) {
        // handle changepage where the caller is asking us to load a page by url
        console.log("typeof data.toPage = {0}".format(typeof data.toPage));
        if (typeof data.toPage === "string") {
            var u = $.mobile.path.parseUrl(data.toPage), re = /^#movie-details/;
            if (u.hash.search(re) !== -1) {

            }
        }
    }
}

$(document).bind("pagebeforechange", function (e, data) {
    codejkjk.movies.Mobile.PageBeforeChange(e, data);
});

$(document).ready(function () {
    codejkjk.movies.Mobile.Init();
});