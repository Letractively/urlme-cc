registerNS("codejkjk.movies.Api");

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
    authUser: function (facebookUserId, callback) {
        var url = "{0}auth_user.js/{1}".format(codejkjk.movies.Api.BaseUrl, facebookUserId)
        codejkjk.movies.Api.AjaxGet(url, callback, 'html');
    },
    GetTheaterMovies: function (dateStr, zip, theaterId, callback) {
        // first, check cache
        var cacheKey = "flixster-{0}-{1}-{2}".format(dateStr, theaterId, zip);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_theater_movies.json/{1}/{2}/{3}".format(codejkjk.movies.Api.BaseUrl, dateStr, zip, theaterId);
        codejkjk.movies.Api.AjaxGet(url, callback, 'json', cacheKey);
    },
    GetTheatersForMovie: function (dateStr, zip, rtMovieId, callback) {
        // first, check cache
        var cacheKey = "flixster-{0}-{1}-{2}".format(dateStr, zip, rtMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }
        var url = "{0}get_showtimes_for_movie.html/{1}/{2}/{3}".format(codejkjk.movies.Api.BaseUrl, dateStr, zip, rtMovieId);
        codejkjk.movies.Api.AjaxGet(url, callback, 'html', cacheKey);
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
    GetMovieReviewForReviewer: function (rtMovieId, callback) {
        var url = "{0}get_movie_review.json/{1}".format(codejkjk.movies.Api.BaseUrl, rtMovieId);
        codejkjk.movies.Api.AjaxGet(url, callback, 'json');
    },
    GetMovieHtml: function (rtMovieId, callback) {
        // first, check cache
        var cacheKey = "rt-html-{0}".format(rtMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_rt_movie.html/{1}".format(codejkjk.movies.Api.BaseUrl, rtMovieId);
        codejkjk.movies.Api.AjaxGet(url, callback, 'html', cacheKey);
    },
    GetMovieSimpleHtml: function (rtMovieId, callback) {
        // first, check cache
        var cacheKey = "rt-html-simple-{0}".format(rtMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_rt_movie_simple.html/{1}".format(codejkjk.movies.Api.BaseUrl, rtMovieId);
        codejkjk.movies.Api.AjaxGet(url, callback, 'html', cacheKey);
    },
    GetRedboxMovieHtml: function (rbSlug, callback) {
        // first, check cache
        var cacheKey = "rb-html-{0}".format(rbSlug);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_rb_movie.html/{1}".format(codejkjk.movies.Api.BaseUrl, rbSlug);
        codejkjk.movies.Api.AjaxGet(url, callback, 'html', cacheKey);
    },
    GetMovieMobileHtml: function (rtMovieId, callback) {
        // first, check cache
        var cacheKey = "rt-mobile-html-{0}".format(rtMovieId);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_rt_movie_mobile.html/{1}".format(codejkjk.movies.Api.BaseUrl, rtMovieId);
        codejkjk.movies.Api.AjaxGet(url, callback, 'html', cacheKey);
    },
    GetRedboxesHtml: function (lat, long, callback) {
        // first, check cache
        var cacheKey = "rb-html-{0}-{1}".format(lat, long);
        var cached = $.cacheItem(cacheKey);
        if (cached && codejkjk.movies.Defaults.AllowCache) {
            return callback(cached);
        }

        var url = "{0}get_rbs.html/{1},{2}".format(codejkjk.movies.Api.BaseUrl, lat, long);
        codejkjk.movies.Api.AjaxGet(url, callback, 'html', cacheKey);
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


