registerNS("codejkjk.movies.RottenTomatoes");

codejkjk.movies.RottenTomatoes = {
    ApiKey: "pfrwfgnr53tpaydw8pnhrymy",
    BaseUrl: "http://api.rottentomatoes.com/api/public/v1.0/",
    GetInTheatersMovies: function (callback) {
        var url = String.format("{0}lists/movies/in_theaters.json?page_limit=16&page=1&country=us&apikey={1}", codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey);
        codejkjk.movies.RottenTomatoes.AjaxGet(url, callback);
    },
    GetBoxOfficeMovies: function (callback) {
        var url = String.format("{0}lists/movies/box_office.json?page_limit=16&page=1&country=us&apikey={1}", codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey);
        codejkjk.movies.RottenTomatoes.AjaxGet(url, callback);
    },
    SearchMovies: function (q, callback) {
        var url = String.format("{0}movies.json?page_limit=16&page=1&country=us&apikey={1}&q={2}", codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey, encodeURI(q));
        codejkjk.movies.RottenTomatoes.AjaxGet(url, callback);
    },
    AjaxGet: function (url, callback) {
        $.ajax({
            url: url,
            dataType: "jsonp",
            success: function (response) { return callback(response.movies); },
            error: function () { return null; }
        });
    }
};