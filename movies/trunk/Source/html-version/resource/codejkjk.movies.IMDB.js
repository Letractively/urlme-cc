registerNS("codejkjk.movies.IMDB");

codejkjk.movies.IMDB = {
    BaseUrl: "http://www.imdbapi.com/?i=tt",
    GetMovie: function (movieId, callback) {
        var url = "{0}{1}".format(codejkjk.movies.IMDB.BaseUrl, movieId);
        $.ajax({
            url: url,
            dataType: "jsonp",
            success: function (response) { return callback(movieId, response); },
            error: function () { return null; }
        });
    },
    GetMovieUrl: function (movieId) {
        return "http://www.imdb.com/title/tt{0}/".format(movieId);
    },
    GetParentalGuideUrl: function (movieId) {
        if (movieId.indexOf("tt") != 0)
            movieId = "tt" + movieId;
        return "http://www.imdb.com/title/{0}/parentalguide".format(movieId);
    }
};