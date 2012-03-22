﻿registerNS("codejkjk.movies.Api");

codejkjk.movies.Api = {
    BaseUrl: apiBaseUrl, // defined in js-containing view
    GetTheaters: function (dateStr, zip, callback) {
        // first, check cache
        var cacheKey = "flixster-{0}-{1}".format(dateStr, zip);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_showtimes.json/{1}/{2}".format(codejkjk.movies.Api.BaseUrl, dateStr, zip);
        codejkjk.movies.Api.AjaxGet(url, callback, 'json', cacheKey);
    },
    SearchMovies: function (q, callback) {
        // first, check cache
        var cacheKey = "rt-SearchMovies-{0}".format(q);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}search_movies.html?q={1}".format(codejkjk.movies.Api.BaseUrl, encodeURI(q));
        codejkjk.movies.Api.AjaxGet(url, callback, 'html', cacheKey);
    },
    GetIMDbMovie: function (imdbMovieId, callback) {
        // first, check cache
        var cacheKey = "imdb-{0}".format(imdbMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_imdb_movie.json/{1}".format(codejkjk.movies.Api.BaseUrl, imdbMovieId);
        codejkjk.movies.Api.AjaxGetMovie(url, callback, cacheKey);
    },
    GetRottenTomatoesMovie: function (rtMovieId, callback) {
        // first, check cache
        var cacheKey = "rt-{0}".format(rtMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_rt_movie.json/{1}".format(codejkjk.movies.Api.BaseUrl, rtMovieId);
        codejkjk.movies.Api.AjaxGetMovie(url, callback, cacheKey);
    },
    AjaxGet: function (url, callback, responseDataType, cacheKey) {
        $.ajax({
            url: url,
            dataType: responseDataType,
            success: function (response) {
                if (cacheKey) {
                    $.cacheItem(cacheKey, response, { expires: codejkjk.movies.Defaults.CacheExpires });
                }
                return callback(response);
            },
            error: function () { return null; }
        });
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


