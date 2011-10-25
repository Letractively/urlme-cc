registerNS("codejkjk.movies.Flixster");

codejkjk.movies.Flixster = {
    BaseUrl: "http://opensocial.flixster.com/igoogle/showtimes", // ?date=20111025&postal=23226
    GetTheaters: function (dateStr, zip, callback) {
        var url = String.format("{0}?date={1}&postal={2}", codejkjk.movies.Flixster.BaseUrl, dateStr, zip);

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

                    var movies = [];
                    theaterDivElem.find('div.showtime').each(function () {
                        var showtime = $(this);
                        var rtMovieId = showtime.find("a.trailer").attr("movieid"); // rottentomatoes movie id
                        showtime.find("h3").remove(); // remove header info, which leaves the showtimes as remaining text w/in this showtime div
                        var showtimes = showtime.html();
                        movies.push({ rtMovieId: rtMovieId, showtimes: showtimes });
                    });

                    theaters.push({ name: theaterName, address: theaterAddress, mapUrl: theaterMapUrl, movies: movies });
                }); // next theaterDiv

                return callback(theaters);
            },
            error: function () { return null; }
        });
    }
};