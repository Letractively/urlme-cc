registerNS("codejkjk.movies.RottenTomatoes");

codejkjk.movies.RottenTomatoes = {
    ApiKey: "pfrwfgnr53tpaydw8pnhrymy",
    BaseUrl: "http://api.rottentomatoes.com/api/public/v1.0/",
    GetInTheatersMovies: function (callback) {
        var url = String.format("{0}lists/movies/in_theaters.json?page_limit=16&page=1&country=us&apikey={1}", codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey);
        codejkjk.movies.RottenTomatoes.AjaxGetMovies(url, callback);
    },
    GetBoxOfficeMovies: function (callback) {
        var url = String.format("{0}lists/movies/box_office.json?page_limit=16&page=1&country=us&apikey={1}", codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey);
        codejkjk.movies.RottenTomatoes.AjaxGetMovies(url, callback);
    },
    SearchMovies: function (q, callback) {
        var url = String.format("{0}movies.json?page_limit=16&page=1&country=us&apikey={1}&q={2}", codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey, encodeURI(q));
        codejkjk.movies.RottenTomatoes.AjaxGetMovies(url, callback);
    },
    GetMovie: function (movieId, callback) {
        // if (movieId == 771092231) { alert('about to ajax get'); };
        var url = String.format("{0}movies/{1}.json?apikey={2}", codejkjk.movies.RottenTomatoes.BaseUrl, movieId, codejkjk.movies.RottenTomatoes.ApiKey);
        codejkjk.movies.RottenTomatoes.AjaxGetMovie(url, callback);
    },
    AjaxGetMovies: function (url, callback) {
        $.ajax({
            url: url,
            dataType: "jsonp",
            success: function (response) { return callback(response.movies); },
            error: function () { return null; }
        });
    },
    AjaxGetMovie: function (url, callback) {
        $.ajax({
            url: url,
            dataType: "jsonp",
            success: function (response) { return callback(response); },
            error: function () { return null; }
        });
    }
};