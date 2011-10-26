registerNS("codejkjk.movies.HomeIndex");

codejkjk.movies.HomeIndex = {
    // page elements
    Controls: {
        MoviesContainer: function () { return $("#movies"); }
        , TheatersContainer: function () { return $("#theaters"); }
        , LoadingContainer: function () { return $("#loading"); }
        , LoadingMessage: function () { return $("#loadingMessage"); }
        , SearchBox: function () { return $("#q"); }
        , SearchButton: function () { return $("#go"); }
        , ActionLinks: function () { return $(".actions").find("a"); }
        , UnPinnedMovies: function () { return $(".movie:not(.pinned)"); }
        , Ratings: function () { return $(".rating"); }
    },

    Init: function () {
        codejkjk.movies.HomeIndex.Controls.SearchBox().focus(); // cuz some browsers are stupid and don't yet support html5's autofocus attr
        codejkjk.movies.HomeIndex.BindFormActions();
        codejkjk.movies.HomeIndex.HandleFeedback();

        codejkjk.movies.HomeIndex.ShowLoading("Loading Flixster.com info...");

        codejkjk.movies.Flixster.GetTheaters("20111025", 23226, codejkjk.movies.HomeIndex.LoadTheaters);
    },

    ShowLoading: function (msg) {
        codejkjk.movies.HomeIndex.Controls.LoadingContainer().show();
        codejkjk.movies.HomeIndex.Controls.LoadingMessage().html(msg);
    },

    HideLoading: function () {
        codejkjk.movies.HomeIndex.Controls.LoadingContainer().hide();
    },

    LoadTheaters: function (theaters) {
        var html = "";
        $.each(theaters, function (i, theater) {
            html += "<div class='theater'>";
            html += String.format("<h2>{0}</h2>{1} - <a href='{2}'>Map</a>", theater.name, theater.address, theater.mapUrl);
            html += "<div class='movies'>";
            $.each(theater.movies, function (j, movie) {
                html += String.format("<div class='movie {0} rtNotSet imdbNotSet'>", movie.rtMovieId); // init to being invisible, since there's really nothing in here yet for the user to see
                html += String.format("<h3>{0}</h3><span class='mpaaRating'></span>", movie.title);
                html += "<div class='ratings'><a class='imdb' target='_blank'></a><a class='rt_critics_rating rottenTomato' target='_blank'></a><a class='rt_audience_rating rottenTomato' target='_blank'></a></div>"
                html += String.format("<div>{0}</div>", movie.showtimes);
                html += "</div>"; // close movie
            });
            html += "</div>"; // close movies
            html += "</div>"; // close theater
        });
        codejkjk.movies.HomeIndex.Controls.TheatersContainer().append(html);
        codejkjk.movies.HomeIndex.FillTheaters(theaters);
    },

    FillTheaters: function (theaters) {
        codejkjk.movies.HomeIndex.ShowLoading("Loading RottenTomatoes.com info...");
        var movieIdsSet = [];
        $.each(theaters, function (i, theater) {
            $.each(theater.movies, function (j, movie) {
                if (movieIdsSet.indexOf(movie.rtMovieId) == -1) { // if we haven't set this movie yet
                    codejkjk.movies.RottenTomatoes.GetMovie(movie.rtMovieId, codejkjk.movies.HomeIndex.SetRottenTomatoesMovieDetails);
                    movieIdsSet.push(movie.rtMovieId); // add this movie to the list of movies that have been set
                }
            });
        });
    },

    LoadMovies: function (movies) {
        var html = "";
        $.each(movies, function (index, movie) {
            if (movie.ratings.critics_score != -1 && movie.ratings.audience_score != -1 && typeof movie.alternate_ids != 'undefined') {
                var criticsClass = movie.ratings.critics_rating.indexOf("Fresh") >= 0 ? "criticsFresh" : "criticsRotten";
                var audienceClass = movie.ratings.audience_rating.indexOf("Upright") >= 0 ? "audienceUpright" : "audienceSpilled";

                html += String.format("<div class='movie unPinned imdbNotSet' id='{0}'>", movie.alternate_ids.imdb);
                html += String.format("<img src='{0}'/>", movie.posters.profile);
                html += String.format("<div class='details'><span class='title'>{0}</span>{1}", movie.title, movie.mpaa_rating);
                html += "<div class='ratings'>";
                html += String.format("<a href='{0}' target='_blank' class='imdb_rating star'></a>", codejkjk.movies.IMDB.GetMovieUrl(movie.alternate_ids.imdb));
                html += String.format("<a href='{4}' target='_blank' alt='{5}' title='{5}' class='{0} rottenTomato'>{1}<sup>%</sup></a><a href='{4}' target='_blank' alt='{6}' title='{6}' class='{2} rottenTomato'>{3}<sup>%</sup></a>", criticsClass, movie.ratings.critics_score, audienceClass, movie.ratings.audience_score, movie.links.alternate, "Critics score on RottenTomatoes", "Audience score on RottenTomatoes");
                html += "</div>"; // close ratings
                // , movie.links.alternate
            } else { // not yet released / not rated
                html += "<div class='movie unPinned notYetReleased'>";
                html += String.format("<img src='{0}'/>", movie.posters.thumbnail);
                html += String.format("<div class='details'><span class='title'>{0}</span>{1}", movie.title, movie.mpaa_rating);
                html += "<div class='notYetReleasedMessage'>Info not available</div>";
            }
            html += "</div>"; // close details
            html += "<div class='actions'><a href='#' class='removeLink'>Remove</a><a href='#' class='pinLink'>Freeze in results</a></div>";
            html += "</div>"; // close movie
        });
        codejkjk.movies.HomeIndex.Controls.MoviesContainer().append(html);
        codejkjk.movies.HomeIndex.BindResultActions();
        codejkjk.movies.HomeIndex.GetIMDBData();
    },

    GetIMDBData: function () {
        codejkjk.movies.HomeIndex.ShowLoading("Loading IMDb.com info...");
        codejkjk.movies.HomeIndex.Controls.MoviesContainer().find(".movie").each(function () {
            if (!$(this).hasClass("notYetReleased")) {
                var imdbMovieId = $(this).attr("id");
                codejkjk.movies.IMDB.GetMovie(imdbMovieId, codejkjk.movies.HomeIndex.SetIMDbMovieDetails);
            }
        });
    },
    SetIMDbMovieDetails: function (imdbMovieId, movie) {
        var movieContainer = $("#" + imdbMovieId);
        var votes = String.format("{0} votes on IMDb.com", movie.Votes);
        movieContainer.find(".imdb_rating").html(movie.Rating).attr("alt", votes).attr("title", votes).removeClass(".imdb_rating");
        movieContainer.removeClass("imdbNotSet");
        if ($(".imdbNotSet").length == 0) {
            codejkjk.movies.HomeIndex.HideLoading();
        }
    },
    SetIMDbMovieDetails2: function (imdbMovieId, movie) {
        var movies = $(".movie[imdbMovieId=" + imdbMovieId + "]");

        var votes = String.format("{0} votes on IMDb.com", movie.Votes);

        movies.find(".imdb").html(movie.Rating).attr("alt", votes).attr("title", votes);
        movies.removeClass("imdbNotSet");
    },
    SetRottenTomatoesMovieDetails: function (movie) {
        var movies = $("." + movie.id);

        movies.find('.mpaaRating').html(movie.mpaa_rating);

        movies.find('.rt_critics_rating,.rt_audience_rating').attr("href", movie.links.alternate);

        if (typeof movie.ratings != 'undefined'
            && movie.ratings.critics_score != -1
            && movie.ratings.audience_score != -1
            ) {
            var criticsClass = movie.ratings.critics_rating.indexOf("Fresh") >= 0 ? "criticsFresh" : "criticsRotten";
            var audienceClass = movie.ratings.audience_rating.indexOf("Upright") >= 0 ? "audienceUpright" : "audienceSpilled";
            var criticsAlt = "Critics score on RottenTomatoes";
            var audienceAlt = "Audience score on RottenTomatoes";

            movies.find('.rt_critics_rating').removeClass('rt_critics_rating')
                                         .addClass(criticsClass)
                                         .html(String.format("{0}<sup>%</sup>", movie.ratings.critics_score))
                                         .attr("alt", criticsAlt)
                                         .attr("title", criticsAlt);
            movies.find('.rt_audience_rating').removeClass('rt_audience_rating')
                                         .addClass(audienceClass)
                                         .html(String.format("{0}<sup>%</sup>", movie.ratings.audience_score))
                                         .attr("alt", audienceAlt)
                                         .attr("title", audienceAlt);
        } else {
            movies.find('.rt_critics_rating,.rt_audience_rating').addClass("unknownRating"); //.attr("alt,title", "Rating not available");
        }

        // load imdb movie ratings
        if (typeof movie.alternate_ids != 'undefined') {
            movies.attr("imdbMovieId", movie.alternate_ids.imdb);
            codejkjk.movies.IMDB.GetMovie(movie.alternate_ids.imdb, codejkjk.movies.HomeIndex.SetIMDbMovieDetails2);
        } else {
            movies.find().addClass("unknownRating");
        }

        movies.removeClass('rtNotSet');
    },
    BindFormActions: function () {
        codejkjk.movies.HomeIndex.Controls.SearchButton().click(function (e) {
            e.preventDefault();

            // prep result list
            // remove non-pinned items
            codejkjk.movies.HomeIndex.Controls.UnPinnedMovies().remove();

            codejkjk.movies.HomeIndex.ShowLoading("Loading RottenTomatoes.com info...");
            var q = codejkjk.movies.HomeIndex.Controls.SearchBox().val();
            codejkjk.movies.RottenTomatoes.SearchMovies(q, codejkjk.movies.HomeIndex.LoadMovies);
            $(this).blur();
        });
        codejkjk.movies.HomeIndex.Controls.SearchBox().keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                codejkjk.movies.HomeIndex.Controls.SearchButton().trigger('click');
            }
        });
    },
    BindResultActions: function () {
        codejkjk.movies.HomeIndex.Controls.ActionLinks().unbind().click(function (e) {
            e.preventDefault();
            var link = $(this);
            var movie = link.closest(".movie");
            if (link.hasClass("removeLink"))
                movie.remove();
            else if (link.hasClass("pinLink")) {
                if (movie.hasClass("pinned")) { // already frozen, so unfreeze
                    link.html("Freeze in results");
                } else { // not already frozen, so freeze
                    link.html("Frozen in results");
                }
                movie.toggleClass("unPinned").toggleClass('pinned');
                link.blur();
            }
        });

        // prevent that weird dotted gray box to show around the rating links that go to imdb.com and rottentomatoes.com after the user clicks on them.
        codejkjk.movies.HomeIndex.Controls.Ratings().unbind().click(function () {
            $(this).blur();
            return true;
        });
    },
    HandleFeedback: function () {
        //        if (feedback != "") {
        //            if (feedback.indexOf("Error") >= 0 || feedback.indexOf("Fail") >= 0) {
        //                $("#Error").html(feedback);
        //                setTimeout(function () { $("#Error").fadeOut(250); }, 4000);
        //            } else {
        //                $.growlUI(feedback, null);
        //            }
        //        }
    }
}

$(document).ready(function () {
    codejkjk.movies.HomeIndex.Init();
});