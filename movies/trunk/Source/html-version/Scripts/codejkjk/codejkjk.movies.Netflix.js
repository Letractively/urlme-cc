registerNS("codejkjk.movies.Netflix");

codejkjk.movies.Netflix = {
    ApiKey: "9xjstfkk374wutrkjw7xups8",
    BaseUrl: "http://api.netflix.com/",
    GetMovie: function (movieId, callback) {
        // first, check cache
        var cacheKey = "rt-GetMovie-{0}".format(movieId);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}movies/{1}.json?apikey={2}".format(codejkjk.movies.Netflix.BaseUrl, movieId, codejkjk.movies.Netflix.ApiKey);
        codejkjk.movies.Netflix.AjaxGetMovie(url, callback, cacheKey);
    },
    AjaxGetMovies: function (url, callback, cacheKey) {
        $.ajax({
            url: url,
            dataType: "jsonp",
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