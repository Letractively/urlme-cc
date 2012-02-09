registerNS("codejkjk.movies.Mobile");

codejkjk.movies.Mobile = {
    // page elements
    Controls: {
        MovieList: function () { return $("#movieList"); }
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
        // load box office
        codejkjk.movies.RottenTomatoes.GetBoxOfficeMovies(function (movies) {
            codejkjk.movies.Mobile.Controls.MovieList().html(
                codejkjk.movies.Mobile.Controls.MovieTemplate().render(movies)
            ).listview('refresh');
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