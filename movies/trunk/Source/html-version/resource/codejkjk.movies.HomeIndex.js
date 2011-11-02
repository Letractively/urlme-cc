registerNS("codejkjk.movies.HomeIndex");

codejkjk.movies.HomeIndex = {
    // page elements
    Controls: {
        MoviesContainer: function () { return $("#movies"); }
        , FiltersContainer: function () { return $("#filters"); }
        , TodayShowtimes: function () { return $("#today"); }
        , TomorrowShowtimes: function () { return $("#tomorrow"); }
        , DayAfterTomorrowShowtimes: function () { return $("#dayAfterTomorrow"); }
        , ShowtimeDayLinks: function () { return $("#showtimeDays").find("a"); }
        , TheatersContainer: function () { return $("#theaters"); }
        , LoadingContainer: function () { return $("#loading"); }
        , LoadingMessage: function () { return $("#loadingMessage"); }
        , SearchBox: function () { return $("#q"); }
        , SearchButton: function () { return $("#go"); }
        , ActionLinks: function () { return $(".actions").find("a"); }
        , RemoveLinks: function () { return $(".actions").find(".removeLink"); }
        , ShowRemovedMoviesLinks: function () { return $(".showRemovedMoviesLink"); }
        , UnPinnedMovies: function () { return $(".movie:not(.pinned)"); }
        , Ratings: function () { return $(".rating"); }
    },

    Init: function () {

        if (typeof localStorage == 'undefined') {
            document.writeln("Please use a more recent browser to view this site.  I recommend <a href='http://www.google.com/chrome'>Chrome</a>.");
            return;
        }

        codejkjk.movies.HomeIndex.Controls.SearchBox().focus(); // cuz some browsers are stupid and don't yet support html5's autofocus attr
        codejkjk.movies.HomeIndex.InitShowtimeDates();
        codejkjk.movies.HomeIndex.BindFormActions();
        codejkjk.movies.HomeIndex.HandleFeedback();

        codejkjk.movies.HomeIndex.ShowLoading("Loading...");
        codejkjk.movies.Flixster.GetTheaters(codejkjk.movies.HomeIndex.Controls.TodayShowtimes().attr("date"), 23226, codejkjk.movies.HomeIndex.LoadTheaters);
    },

    InitShowtimeDates: function () {
        var today = Date.today();
        var tomorrow = Date.today().add(1).days();
        var dayAfterTomorrow = Date.today().add(2).days();

        codejkjk.movies.HomeIndex.Controls.TodayShowtimes().attr("date", today.toString("yyyyMMdd"));
        codejkjk.movies.HomeIndex.Controls.TomorrowShowtimes().attr("date", tomorrow.toString("yyyyMMdd"));
        codejkjk.movies.HomeIndex.Controls.DayAfterTomorrowShowtimes().attr("date", dayAfterTomorrow.toString("yyyyMMdd")).html(dayAfterTomorrow.toString("ddd, MMM dd"));

        for(var i = 0; i < 5, i++) {
            var d = Date.today().add(i).days();
            var label = '';
            switch (i) {
                case 0:
                    label = "Today";
                    break;
                case 1:
                    label = "Tomorrow";
                    break;
                default:
                    label = d.toString("ddd, MMM dd");
                    break;
            }
        }

        codejkjk.movies.HomeIndex.Controls.FiltersContainer().show();
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
        var rtMovieIdsToLoad = [];

        var collapsedTheaters = localStorage.getItem("CollapsedTheaters") != null ? localStorage.getItem("CollapsedTheaters").split(',') : [];
        var removedTheaterMovies = localStorage.getItem("RemovedTheaterMovies") != null ? localStorage.getItem("RemovedTheaterMovies").split(',') : [];

        $.each(theaters, function (i, theater) {
            var collapserState = collapsedTheaters.indexOf(theater.theaterId) >= 0 ? 'collapsed' : 'expanded';
            var collapseeState = collapserState == 'collapsed' ? 'hidden' : '';

            // determine if "Show removed movies" link should show, along w/ the number of hidden movies if applicable
            var removedMoviesCount = 0;
            $.each(removedTheaterMovies, function (i, removedTheaterMovie) {
                if (removedTheaterMovie.indexOf(String.format("{0}-", theater.theaterId)) == 0) {
                    removedMoviesCount++;
                }
            });
            var showRemovedMoviesLinkState = removedMoviesCount > 0 ? '' : 'hidden';

            html += String.format("<div class='theater' id='{0}'>", theater.theaterId);
            html += String.format("<a href='#' class='theaterHeader collapser {0}' collapsee='.collapsee-{1}'>{2}</a>", collapserState, theater.theaterId, theater.name);
            html += String.format("<div class='collapsee-{0} {1}'>{2} - <a href='{3}' target='_blank' class='external'>Map</a> <a href='#' class='showRemovedMoviesLink {4}'>Show removed movies (<span class='removedMoviesCount'>{5}</span>)</a>", theater.theaterId, collapseeState, theater.address, theater.mapUrl, showRemovedMoviesLinkState, removedMoviesCount);
            html += "<div class='movies'>";
            $.each(theater.movies, function (j, movie) {
                var theaterMovieId = String.format("{0}-{1}", theater.theaterId, movie.rtMovieId);
                var movieState = removedTheaterMovies.indexOf(theaterMovieId) >= 0 ? 'hidden' : '';

                if (movie.rtMovieId != null) {
                    if (rtMovieIdsToLoad.indexOf(movie.rtMovieId) == -1) { rtMovieIdsToLoad.push(movie.rtMovieId); }
                    html += String.format("<div class='movie rtNotSet imdbNotSet {0}' rtMovieId='{1}' theaterMovieId='{2}'>", movieState, movie.rtMovieId, theaterMovieId); // init to being invisible, since there's really nothing in here yet for the user to see
                } else {
                    html += String.format("<div class='movie' style='display:none;'>", movie.rtMovieId); // init to being invisible, since there's really nothing in here yet for the user to see
                }
                html += String.format("<h3>{0}</h3><a href='#' class='mpaaRating' target='_blank' alt='{1}' title='{1}'></a>", String.snippet(movie.title, 40), "Link to Parents Guide on IMDb.com");
                html += "<div class='ratings'><a class='imdb' target='_blank'></a><a class='rt_critics_rating rottenTomato' target='_blank'></a><a class='rt_audience_rating rottenTomato' target='_blank'></a></div>"
                html += String.format("<div>{0}</div>", movie.showtimes);
                html += "<div class='actions'><a href='#' class='removeLink'>Remove</a></div>";
                html += "</div>"; // close movie
            });
            html += "</div>"; // close movies
            html += "</div>"; // close theaterContents
            html += "</div>"; // close theater
        });
        codejkjk.movies.HomeIndex.Controls.TheatersContainer().html(html);
        codejkjk.movies.HomeIndex.BindResultActions2();

        // make the api calls to fill the theaters with the list of unique rt movie id's
        $.each(rtMovieIdsToLoad, function (i, rtMovieIdToLoad) {
            codejkjk.movies.RottenTomatoes.GetMovie(rtMovieIdToLoad, codejkjk.movies.HomeIndex.SetRottenTomatoesMovieDetails);
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
        if ($(".imdbNotSet").length == 0 && $(".rtNotSet").length == 0) {
            codejkjk.movies.HomeIndex.HideLoading();
        }
    },
    SetRottenTomatoesMovieDetails: function (movie) {
        var movies = $(".movie[rtMovieId=" + movie.id + "]");

        movies.find('.mpaaRating').html(movie.mpaa_rating);

        if (typeof movie.links != 'undefined') {
            movies.find('.rt_critics_rating,.rt_audience_rating').attr("href", movie.links.alternate);
        } else {
            movies.find('.rt_critics_rating,.rt_audience_rating').unbind().click(function (e) { e.preventDefault(); });
        }

        if (typeof movie.ratings != 'undefined'
            && typeof movie.ratings.critics_rating != 'undefined'
            && typeof movie.ratings.audience_rating != 'undefined'
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
            movies.find(".imdb").attr("href", codejkjk.movies.IMDB.GetMovieUrl(movie.alternate_ids.imdb));
            codejkjk.movies.IMDB.GetMovie(movie.alternate_ids.imdb, codejkjk.movies.HomeIndex.SetIMDbMovieDetails2);
            movies.find('.mpaaRating').addClass("external").attr("href", codejkjk.movies.IMDB.GetParentalGuideUrl(movie.alternate_ids.imdb));
        } else {
            movies.find('.mpaaRating').addClass("disabled").click(function (e) { e.preventDefault(); }); // prevent user from clicking on this
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
        codejkjk.movies.HomeIndex.Controls.ShowtimeDayLinks().click(function (e) {
            e.preventDefault();
            var showtimeDayLink = $(this);
            codejkjk.movies.HomeIndex.ShowLoading("Loading...");
            codejkjk.movies.Flixster.GetTheaters(showtimeDayLink.attr("date"), 23226, codejkjk.movies.HomeIndex.LoadTheaters);
            codejkjk.movies.HomeIndex.Controls.ShowtimeDayLinks().removeClass("active");
            showtimeDayLink.addClass("active");
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

    BindResultActions2: function () {
        // wire the collapsers
        codejkjk.movies.SiteActions.WireCollapsers();

        codejkjk.movies.HomeIndex.Controls.RemoveLinks().click(function (e) {
            e.preventDefault();
            var link = $(this);
            var movie = link.closest(".movie");
            var theater = link.closest(".theater");
            var removedTheaterMovies = localStorage.getItem("RemovedTheaterMovies") != null ? localStorage.getItem("RemovedTheaterMovies").split(',') : [];
            removedTheaterMovies.pushIfDoesNotExist(movie.attr("theaterMovieId"));
            movie.addClass('hidden');
            var removedMoviesCount = theater.find(".movie.hidden").length;
            theater.find(".showRemovedMoviesLink").find(".removedMoviesCount").html(removedMoviesCount);
            theater.find(".showRemovedMoviesLink").show();

            localStorage.setItem("RemovedTheaterMovies", removedTheaterMovies.join(','));
        });

        codejkjk.movies.HomeIndex.Controls.ShowRemovedMoviesLinks().click(function (e) {
            e.preventDefault();
            var removedTheaterMovies = localStorage.getItem("RemovedTheaterMovies") != null ? localStorage.getItem("RemovedTheaterMovies").split(',') : [];

            var link = $(this);
            var theater = link.closest(".theater");
            theater.find(".movie.hidden").each(function () {
                var movie = $(this);
                var movieTheaterId = movie.attr("theaterMovieId");
                removedTheaterMovies.removeByElement(movieTheaterId);
                movie.removeClass('hidden');
            });

            if (removedTheaterMovies.length > 0) {
                localStorage.setItem("RemovedTheaterMovies", removedTheaterMovies.join(','));
            } else {
                localStorage.removeItem("RemovedTheaterMovies");
            }

            link.hide();
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