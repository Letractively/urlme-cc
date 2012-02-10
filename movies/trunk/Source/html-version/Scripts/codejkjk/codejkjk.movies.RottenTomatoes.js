registerNS("codejkjk.movies.RottenTomatoes");

codejkjk.movies.RottenTomatoes = {
    ApiKey: "pfrwfgnr53tpaydw8pnhrymy",
    BaseUrl: "http://api.rottentomatoes.com/api/public/v1.0/",
//    GetInTheatersMovies: function (callback) {
//        var url = "{0}lists/movies/in_theaters.json?page_limit=20&page=1&country=us&apikey={1}".format(codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey);
//        codejkjk.movies.RottenTomatoes.AjaxGetMovies(url, callback);
//    },
    GetBoxOfficeMovies: function (callback) {
        // first, check cache
        var cacheKey = "rt-GetBoxOfficeMovies";
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}lists/movies/box_office.json?page_limit=20&page=1&country=us&apikey={1}".format(codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey);
        codejkjk.movies.RottenTomatoes.AjaxGetMovies(url, callback, cacheKey);
    },
    GetUpcomingMovies: function (callback) {
        // first, check cache
        var cacheKey = "rt-GetUpcomingMovies";
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}lists/movies/upcoming.json?page_limit=20&page=1&country=us&apikey={1}".format(codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey);
        codejkjk.movies.RottenTomatoes.AjaxGetMovies(url, callback, cacheKey);
    },
    SearchMovies: function (q, callback) {
        // first, check cache
        var cacheKey = "rt-SearchMovies-{0}".format(q);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}movies.json?page_limit=20&page=1&country=us&apikey={1}&q={2}".format(codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey, encodeURI(q));
        codejkjk.movies.RottenTomatoes.AjaxGetMovies(url, callback, cacheKey);
    },
    GetMovie: function (movieId, callback) {
        // first, check cache
        var cacheKey = "rt-GetMovie-{0}".format(movieId);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}movies/{1}.json?apikey={2}".format(codejkjk.movies.RottenTomatoes.BaseUrl, movieId, codejkjk.movies.RottenTomatoes.ApiKey);
        codejkjk.movies.RottenTomatoes.AjaxGetMovie(url, callback, cacheKey);
    },
    AjaxGetMovies: function (url, callback, cacheKey) {
        $.ajax({
            url: url,
            dataType: "jsonp",
            beforeSend: function (xhr) { xhr.setRequestHeader('Access-Control-Allow-Origin', '*'); },
            success: function (response) {
                $.cacheItem(cacheKey, response.movies, { expires: codejkjk.movies.Defaults.CacheExpires });
                return callback(response.movies); 
            },
            error: function () { return null; }
        });
    },
    AjaxGetMovie: function (url, callback, cacheKey) {
        $.ajax({
            url: url,
            dataType: "jsonp",
            success: function (response) {
                $.cacheItem(cacheKey, response, { expires: codejkjk.movies.Defaults.CacheExpires });
                return callback(response); 
            },
            error: function () { return null; }
        });
    }
};