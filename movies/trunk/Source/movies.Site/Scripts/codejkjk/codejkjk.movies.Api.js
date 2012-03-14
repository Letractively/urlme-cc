registerNS("codejkjk.movies.Api");

codejkjk.movies.Api = {
    BaseUrl: apiBaseUrl, // defined in js-containing view
    GetTheaters: function (dateStr, zip, callback) {
        // first, check cache
        var cacheKey = "flixster-{0}-{1}".format(dateStr, zip);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}get_showtimes.html/{1}/{2}".format(codejkjk.movies.Api.BaseUrl, dateStr, zip);

        $.ajax({
            url: url,
            success: function (html) {
                var theaters = [];
                var theaterDivs = $('div.theater', html);
                $.each(theaterDivs, function (i, theaterDiv) {
                    var theaterDivElem = $(theaterDiv);
                    var theaterName = theaterDivElem.find("a:first").attr("title");
                    var theaterAddress = $.trim(theaterDivElem.find("span:first").html().split('-')[1]);
                    var theaterMapUrl = theaterDivElem.find("span:first").find("a").attr("href");
                    var theaterUrl = theaterDivElem.find("a:first").attr("href");
                    var theaterId = theaterUrl.substring(theaterUrl.lastIndexOf('/') + 1);

                    var movies = [];
                    theaterDivElem.find('div.showtime').each(function () {
                        var showtime = $(this);
                        var rtMovieId = showtime.find("a.trailer").attr("movieid"); // rottentomatoes movie id
                        if (rtMovieId) {
                            var movieName = $.trim(showtime.find("a:first").html()); // movie title
                            var mpaaRating = $.trim(showtime.find("span:first").html().split(' - ')[0]).replace("- Rated ", "");
                            var movieLength = $.trim(showtime.find("span:first").html().split(' - ')[1]);
                            showtime.find("h3").remove(); // remove header info, which leaves the showtimes as remaining text w/in this showtime div
                            var showtimes = showtime.html();

                            movies.push({ rtMovieId: rtMovieId, name: movieName, showtimes: showtimes, length: movieLength, mpaaRating: mpaaRating });
                        }

                    });

                    theaters.push({ theaterId: theaterId, name: theaterName, address: theaterAddress, mapUrl: theaterMapUrl, movies: movies });
                }); // next theaterDiv

                $.cacheItem(cacheKey, theaters, { expires: { hours: 6} });
                return callback(theaters);
            },
            error: function () { return null; }
        });
    },
    SearchMovies: function (q, callback) {
        // first, check cache
        var cacheKey = "rt-SearchMovies-{0}".format(q);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}search_movies.json?q={1}".format(codejkjk.movies.Api.BaseUrl, encodeURI(q));
        codejkjk.movies.Api.AjaxGet(url, callback, 'html', cacheKey);
    },
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


