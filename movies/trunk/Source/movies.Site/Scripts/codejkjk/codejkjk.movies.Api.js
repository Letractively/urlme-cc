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
        codejkjk.movies.Api.AjaxGetMovie(url, callback, cacheKey);
    },
    GetRottenTomatoesMovie: function (rtMovieId, callback) {
        // first, check cache
        var cacheKey = "rt-{0}".format(rtMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}get_rt_movie.json/{1}".format(codejkjk.movies.Api.BaseUrl, rtMovieId);
        codejkjk.movies.Api.AjaxGetMovie(url, callback, cacheKey);
    },
    AjaxGetMovie: function (url, callback, cacheKey) {
        $.ajax({
            url: url,
            dataType: "json",
            success: function (response) {
                if (cacheKey) {
                    $.cacheItem(cacheKey, response, { expires: codejkjk.movies.Defaults.CacheExpires });
                }
                return callback(response);
            },
            error: function () { return null; }
        });
    }
};


