registerNS("codejkjk.movies.IMDB");

codejkjk.movies.IMDB = {
    BaseUrl: "http://www.imdbapi.com/?i=tt",
    GetMovie: function (movieId, callback) {
        var url = String.format("{0}{1}", codejkjk.movies.IMDB.BaseUrl, movieId);
        $.ajax({
            url: url,
            dataType: "jsonp",
            success: function (response) { return callback(movieId, response); },
            error: function () { return null; }
        });
    },
    GetMovieUrl: function (movieId) {
        return String.format("http://www.imdb.com/title/tt{0}/", movieId);
    },
    GetParentalGuideUrl: function (movieId) {
        if (movieId.indexOf("tt") != 0)
            movieId = "tt" + movieId;
        return String.format("http://www.imdb.com/title/{0}/parentalguide", movieId);
    }
};