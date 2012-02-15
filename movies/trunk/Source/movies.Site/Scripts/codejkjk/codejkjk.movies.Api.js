registerNS("codejkjk.movies.Api");

codejkjk.movies.Api = {
    BaseUrl: apiBaseUrl, // defined in js-containing view
    GetIMDbMovie: function (imdbMovieId, callback) {
        // first, check cache
        var cacheKey = "imdb-{0}".format(imdbMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}get_imdb_movie.json/{1}".format(codejkjk.movies.Api.BaseUrl, imdbMovieId);
        $.ajax({
            url: url,
            dataType: "json",
            success: function (response) {
                $.cacheItem(cacheKey, response, { expires: codejkjk.movies.Defaults.CacheExpires });
                return callback(response);
            },
            error: function () { return null; }
        });
    },
    GetRottenTomatoesMovie: function(rtMovieId, callback) {
        // first, check cache
        var cacheKey = "rt-{0}".format(rtMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}get_rt_movie.json/{1}".format(codejkjk.movies.Api.BaseUrl, rtMovieId);
        $.ajax({
            url: url,
            dataType: "json",
            success: function (response) {
                $.cacheItem(cacheKey, response, { expires: codejkjk.movies.Defaults.CacheExpires });
                return callback(response);
            },
            error: function () { return null; }
        });
    }
};


//AjaxGetMovie: function (url, callback, cacheKey) {
//    $.ajax({
//        url: url,
//        dataType: "jsonp",
//        success: function (response) {
//            $.cacheItem(cacheKey, response, { expires: codejkjk.movies.Defaults.CacheExpires });
//            return callback(response);
//        },
//        error: function () { return null; }
//    });
//}