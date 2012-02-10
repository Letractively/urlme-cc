registerNS("codejkjk.movies.Mobile");

codejkjk.movies.Mobile = {
    // page elements
    Controls: {
        BoxOffice: function () { return $("#topBoxOffice"); }
        , IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , MovieTemplate: function () { return $("#movieTemplate"); }
        , Upcoming: function () { return $("#comingSoon"); }
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

    LoadBoxOfficeMovies: function () {
        codejkjk.movies.RottenTomatoes.GetBoxOfficeMovies(function (movies) {
            codejkjk.movies.Mobile.Controls.BoxOffice().html(
                    codejkjk.movies.Mobile.Controls.MovieTemplate().render(movies)
                ).listview('refresh');
            codejkjk.movies.Mobile.GetIMDbData();
        });
    },

    LoadUpcomingMovies: function () {
        codejkjk.movies.RottenTomatoes.GetUpcomingMovies(function (movies) {
            codejkjk.movies.Mobile.Controls.Upcoming().html(
                    codejkjk.movies.Mobile.Controls.MovieTemplate().render(movies)
                ).listview('refresh');
            codejkjk.movies.Mobile.GetIMDbData();
        });
    },

    LoadShowtimes: function () {

    },

    Init: function () {
        // this function is run once
        codejkjk.movies.Mobile.RegisterJsRenderHelpers();

        codejkjk.movies.Mobile.LoadUrl(location.pathname);
    },

    LoadUrl: function (url) {
        if (url.indexOf("mobile.htm") >= 0) {
            // load box office
            console.log("loading box office");
            codejkjk.movies.Mobile.LoadBoxOfficeMovies();
        } else if (url.indexOf("mobile_comingsoon.htm") >= 0) {
            // load upcoming
            console.log("loading upcoming");
            codejkjk.movies.Mobile.LoadUpcomingMovies();
        } else if (url.indexOf("mobile_showtimes.htm") >= 0) {
            // load showtimes
            console.log("loading showtimes");
            codejkjk.movies.Mobile.LoadShowtimes();
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

    //    PageChange: function (e, data) {
    //        var pathName = $.mobile.path.parseUrl(data.toPage).pathname;
    //    },

    PageBeforeChange: function (e, data) {
        // handle changepage where the caller is asking us to load a page by url
        if (typeof data.toPage === "string") {
            var pathName = $.mobile.path.parseUrl(data.toPage).pathname;

            // console.log('in pagebefore change, pathname = ' + pathName);

            // codejkjk.movies.Mobile.LoadUrl(pathName);

            // e.preventDefault(); // is this a good thing?

            //            var u = $.mobile.path.parseUrl(data.toPage), re = /^#movie-details/;
            //            if (u.hash.search(re) !== -1) {

            //            }
        }
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
    }
}

//$(document).bind("pageinit", function (e, data) {
//    // codejkjk.movies.Mobile.PageBeforeChange(e, data);
//    console.log('in pageinit, loc.pathname = ' + location.pathname + ', page1div len = ' + $("#topBoxOffice").length + ' page3div len = ' + $("#comingSoon").length);
//});

//$(document).bind("pagebeforechange", function (e, data) {
//    // codejkjk.movies.Mobile.PageBeforeChange(e, data);
//    console.log('in pagebeforechange, loc.pathname = ' + location.pathname + ', page1div len = ' + $("#topBoxOffice").length + ' page3div len = ' + $("#comingSoon").length);
//});

//$(document).bind("pagechange", function (e, data) {
//    // codejkjk.movies.Mobile.PageChange(e, data);
//    console.log('in pagechange, loc.pathname = ' + location.pathname + ', page1div len = ' + $("#topBoxOffice").length + ' page3div len = ' + $("#comingSoon").length);
//});

//$(document).bind("pageload", function (e, data) {
//    console.log('in pageload, data.url.pathname = ' + $.mobile.path.parseUrl(data.url).pathname + ', page1div len = ' + $("#topBoxOffice").length + ' page3div len = ' + $("#comingSoon").length);
//});

//$(document).bind("pageshow", function () {
//    console.log('in pageshow, loc.pathname = ' + location.pathname + ', page1div len = ' + $("#topBoxOffice").length + ' page3div len = ' + $("#comingSoon").length);
//});

$(document).ready(function () {
    // this is called once, when user visits any of the mobile pages
    codejkjk.movies.Mobile.Init();
});