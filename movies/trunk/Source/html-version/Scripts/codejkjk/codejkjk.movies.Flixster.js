registerNS("codejkjk.movies.Flixster");

codejkjk.movies.Flixster = {
    BaseUrl: "http://opensocial.flixster.com/igoogle/showtimes", // ?date=20111025&postal=23226
    GetTheaters: function (dateStr, zip, callback) {
        // first, check cache
        var cacheKey = "flixster-{0}-{1}".format(dateStr, zip);
        var cached = $.cacheItem(cacheKey);
        if (cached) {
            return callback(cached);
        }

        var url = "{0}?date={1}&postal={2}".format(codejkjk.movies.Flixster.BaseUrl, dateStr, zip);

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
                        var movieName = $.trim(showtime.find("a:first").html()); // movie title
                        var rtMovieId = showtime.find("a.trailer").attr("movieid"); // rottentomatoes movie id
                        var mpaaRating = $.trim(showtime.find("span:first").html().split(' - ')[0]).replace("- Rated ", "");
                        var movieLength = $.trim(showtime.find("span:first").html().split(' - ')[1]);
                        showtime.find("h3").remove(); // remove header info, which leaves the showtimes as remaining text w/in this showtime div
                        var showtimes = showtime.html();

                        movies.push({ rtMovieId: rtMovieId, name: movieName, showtimes: showtimes, length: movieLength, mpaaRating: mpaaRating });
                    });

                    theaters.push({ theaterId: theaterId, name: theaterName, address: theaterAddress, mapUrl: theaterMapUrl, movies: movies });
                }); // next theaterDiv

                $.cacheItem(cacheKey, theaters, { expires: { hours: 6} });
                return callback(theaters);
            },
            error: function () { return null; }
        });
    }
};