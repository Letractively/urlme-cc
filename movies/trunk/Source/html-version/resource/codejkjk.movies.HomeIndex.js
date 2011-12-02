registerNS("codejkjk.movies.HomeIndex");

codejkjk.movies.HomeIndex = {
    // page elements
    Controls: {
        ShowtimeDayLinksContainer: function () { return $("#showtimeDays"); }
        , ShowtimeDayLinks: function () { return $("#showtimeDays").find("a"); }
        , TheatersContainer: function () { return $("#theaters"); }
        , SearchBox: function () { return $("#q"); }
        , SearchButton: function () { return $("#go"); }
        , ActionLinks: function () { return $(".actions").find("a"); }
        , RemoveLinks: function () { return $(".actions").find(".removeLink"); }
        , ShowRemovedMoviesLinks: function () { return $(".showRemovedMoviesLink"); }
        , UnPinnedMovies: function () { return $(".movie:not(.pinned)"); }
        , Ratings: function () { return $(".rating"); }
        , PostalCode: function () { return $("#postalCode"); }
        , PostalCodeContainer: function () { return $("#nearPostalCode"); }
        , ChangePostalCodeLink: function () { return $("#changePostalCodeLink"); }
        , ChangeOptionsContainer: function () { return $("#changeOptions"); }
        , UseNearbyPostalCodeLink: function () { return $("#useNearbyPostalCode"); }
        , SetPostalCodeButton: function () { return codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().find("button"); }
        , NewPostalCodeInput: function () { return codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().find("input[type=text]"); }
    },

    Init: function () {
        if (typeof localStorage == 'undefined' || !navigator.geolocation) {
            document.writeln("Please use a more recent browser to view this site.  I recommend <a href='http://www.google.com/chrome'>Chrome</a>.");
            return;
        }

        codejkjk.movies.HomeIndex.BuildFilters();
        codejkjk.movies.HomeIndex.BindFormActions();

        var postalCode = localStorage.getItem("PostalCode") || 23226; // default to 23226
        codejkjk.movies.HomeIndex.Controls.PostalCode().html(postalCode);
        codejkjk.movies.HomeIndex.Controls.PostalCodeContainer().show();
        codejkjk.movies.Flixster.GetTheaters(Date.today().toString("yyyyMMdd"), postalCode, codejkjk.movies.HomeIndex.LoadTheaters);
    },

    BuildFilters: function () {
        var today = Date.today(), tomorrow = Date.today().add(1).days(), dayAfterTomorrow = Date.today().add(2).days();

        for (var i = 0; i < 5; i++) {
            var d = Date.today().add(i).days();
            var label = '';
            var cssClass = '';
            switch (i) {
                case 0:
                    label = "Today";
                    cssClass = 'active';
                    break;
                case 1:
                    label = "Tomorrow";
                    break;
                default:
                    label = d.toString("ddd, MMM dd");
                    break;
            } // end switch
            var showtimeDayLink = $('<a/>', { href: '#', text: label, class: cssClass }).attr("date", d.toString("yyyyMMdd"));
            codejkjk.movies.HomeIndex.Controls.ShowtimeDayLinksContainer().append(showtimeDayLink);
        }
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

            $("#theaterList").append(String.format("<li><div class='title'>{0}</div><div class='address'>{1}</div></li>", String.snippet(theater.name, 30), String.snippet(theater.address, 36)));

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
                    html += "<div class='movie' style='display:none;'>"; // init to being invisible, since there's really nothing in here yet for the user to see
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
        // codejkjk.movies.HomeIndex.Controls.TheatersContainer().html(html);
        codejkjk.movies.HomeIndex.BindResultActions2();

        // make the api calls to fill the theaters with the list of unique rt movie id's
        $.each(rtMovieIdsToLoad, function (i, rtMovieIdToLoad) {
            codejkjk.movies.RottenTomatoes.GetMovie(rtMovieIdToLoad, codejkjk.movies.HomeIndex.SetRottenTomatoesMovieDetails);
        });
    },
    SetIMDbMovieDetails: function (imdbMovieId, movie) {
        var movieContainer = $("#" + imdbMovieId);
        var votes = String.format("{0} votes on IMDb.com", movie.Votes);
        movieContainer.find(".imdb_rating").html(movie.Rating).attr("alt", votes).attr("title", votes).removeClass(".imdb_rating");
        movieContainer.removeClass("imdbNotSet");
    },
    SetIMDbMovieDetails2: function (imdbMovieId, movie) {
        var movies = $(".movie[imdbMovieId=" + imdbMovieId + "]");

        var votes = String.format("{0} votes on IMDb.com", movie.Votes);

        movies.find(".imdb").html(movie.Rating).attr("alt", votes).attr("title", votes);
        movies.removeClass("imdbNotSet");
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
        codejkjk.movies.HomeIndex.Controls.ShowtimeDayLinks().click(function (e) {
            e.preventDefault();
            var link = $(this);
            codejkjk.movies.Flixster.GetTheaters(link.attr("date"), 23226, codejkjk.movies.HomeIndex.LoadTheaters);
            codejkjk.movies.HomeIndex.Controls.ShowtimeDayLinks().removeClass("active");
            link.addClass("active");
        });
        // bind postal code control
        codejkjk.movies.HomeIndex.Controls.ChangePostalCodeLink().click(function (e) {
            e.preventDefault();
            codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().slideToggle('fast');
        });

        // bind UseNearbyPostalCodeLink, SetPostalCodeButton, NewPostalCodeInput
        codejkjk.movies.HomeIndex.Controls.UseNearbyPostalCodeLink().click(function (e) {
            e.preventDefault();
            codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().mask();
            codejkjk.Geo.GetPostalCode(function (postalCode) {
                localStorage.setItem("PostalCode", postalCode);
                codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().unmask();
                codejkjk.movies.HomeIndex.Controls.PostalCode().html(postalCode);
                codejkjk.movies.HomeIndex.Controls.ChangePostalCodeLink().trigger('click');
                codejkjk.movies.Flixster.GetTheaters(Date.today().toString("yyyyMMdd"), postalCode, codejkjk.movies.HomeIndex.LoadTheaters);
            });
        });
        codejkjk.movies.HomeIndex.Controls.SetPostalCodeButton().click(function (e) {
            e.preventDefault();
            var postalCode = codejkjk.movies.HomeIndex.Controls.NewPostalCodeInput().val();
            localStorage.setItem("PostalCode", postalCode);
            codejkjk.movies.HomeIndex.Controls.PostalCode().html(postalCode);
            codejkjk.movies.HomeIndex.Controls.ChangePostalCodeLink().trigger('click');
            codejkjk.movies.Flixster.GetTheaters(Date.today().toString("yyyyMMdd"), postalCode, codejkjk.movies.HomeIndex.LoadTheaters);
        });
        codejkjk.movies.HomeIndex.Controls.NewPostalCodeInput().keydown(function (e) {
            if (e.keyCode == 13) {
                codejkjk.movies.HomeIndex.Controls.SetPostalCodeButton().trigger('click');
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
    }
}

$(document).ready(function () {
    codejkjk.movies.HomeIndex.Init();
});