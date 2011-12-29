﻿registerNS("codejkjk.movies.HomeIndex");

// todo:
// - test details view from search results
// - details view not positioned correctly from theater view
// - theater h1, make h2 b/c there should only be one h1 on the page
// - theater view, make width same as width of box office and upcoming views. this way the movie details popup will show up in same location.

codejkjk.movies.HomeIndex = {
    // page elements
    Controls: {
        ActionLinks: function () { return $(".actions").find("a"); }
        , BoxOfficeView: function () { return $("#boxOfficeView"); }
        , ChangeCurrentZipLink: function () { return $("#changeCurrentZipLink"); }
        , ChangeOptionsContainer: function () { return $("#changeOptions"); }
        , CloseMovieDetailsLinkSelector: function () { return "#closeMovieDetailsLink"; }
        , CurrentNavItem: function () { return $("nav > a.selected"); }
        , CurrentShowtimeDay: function () { return $("#currentShowtimeDay"); }
        , CurrentTheater: function () { return $("#currentTheater"); }
        , CurrentTheaterContainer: function () { return $("#currentTheaterContainer"); }
        , CurrentTheaterTemplate: function () { return $("#currentTheaterTemplate"); }
        , CurrentView: function () { return $(".content:visible"); }
        , CurrentZip: function () { return $("#currentZip"); }
        , IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , MovieDetails: function () { return $("#movieDetails"); }
        , MovieDetailsLinksSelector: function () { return ".movieDetailsLink"; }
        , MovieDetailsTemplate: function () { return $("#movieDetailsTemplate"); }
        , MovieListTemplate: function () { return $("#movieListTemplate"); }
        , Nav: function () { return $("nav"); }
        , NavLinks: function () { return $("nav > a"); }
        , NavTemplate: function () { return $("#navTemplate"); }
        , NewPostalCodeInput: function () { return codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().find("input[type=text]"); }
        , PostalCodeContainer: function () { return $("#nearPostalCode"); }
        , RemoveLinks: function () { return $(".actions").find(".removeLink"); }
        , SearchBox: function () { return $("#q"); }
        , SearchResultsView: function () { return $("#searchResultsView"); }
        , SetPostalCodeButton: function () { return codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().find("button"); }
        , ShowRemovedMoviesLinks: function () { return $(".showRemovedMoviesLink"); }
        , ShowtimeDayLinks: function () { return $(".showtimeDays > a"); }
        , ShowtimeDayLinksSelector: function () { return ".showtimeDays > a"; }
        , ShowtimeDayLinksContainer: function () { return $(".showtimeDays"); }
        , TheaterLinksSelector: function () { return "#theaterList > a"; }
        , TheaterList: function () { return $("#theaterList"); }
        , TheaterListTemplate: function () { return $("#theaterListTemplate"); }
        , Theaters: function () { return $(".theater"); }
        , UpcomingListTemplate: function () { return $("#upcomingListTemplate"); }
        , UpcomingView: function () { return $("#upcomingView"); }
        , UseNearbyPostalCodeLink: function () { return $("#useNearbyPostalCode"); }
        , Views: function () { return $(".content"); }
    },

    Init: function () {
        if (typeof localStorage == 'undefined' || !navigator.geolocation) {
            document.writeln("Please use a more recent browser to view this site.  I recommend <a href='http://www.google.com/chrome'>Chrome</a>.");
            return;
        }

        // build nav
        var navItems = [{ text: "Box Office" }, { text: "Showtimes" }, { text: "Upcoming"}];
        var currentNavItem = localStorage.getItem("View") || "Box Office";
        $.each(navItems, function (i, navItem) {
            navItem.className = navItem.text == currentNavItem ? "selected" : "";
        });
        codejkjk.movies.HomeIndex.Controls.Nav().html(
            codejkjk.movies.HomeIndex.Controls.NavTemplate().render(navItems)
        );

        codejkjk.movies.HomeIndex.BindControls();

        // register jsRender helper functions
        $.views.registerHelpers({ HlpIsCurrentTheater: codejkjk.movies.HomeIndex.IsCurrentTheater });
        $.views.registerHelpers({ HlpGetHoursAndMinutes: codejkjk.movies.HomeIndex.GetHoursAndMinutes });

        $.views.registerHelpers({
            IsCurrentTheater: function (i, iTheaterId) {
                return (codejkjk.movies.HomeIndex.Controls.CurrentTheater().val() == iTheaterId
                         || (codejkjk.movies.HomeIndex.Controls.CurrentTheater().val() == "" && i == 1));
            },
            GetHoursAndMinutes: function (minutes) {
                var hrs = Math.floor(minutes / 60);
                var mins = minutes % 60;

                return "{0} hr. {1} min.".format(hrs, mins);
            },
            GetCriticsClass: function (rating) {
                if (!rating) { return ""; }
                return rating.indexOf("Fresh") >= 0 ? "criticsFresh" : "criticsRotten";
            },
            GetAudienceClass: function (rating) {
                if (!rating) { return ""; }
                return rating.indexOf("Upright") >= 0 ? "audienceUpright" : "audienceSpilled";
            },
            FormatReleaseDate: function (date) {
                return Date.parse(date).toString("MMM d, yyyy");
            },
            GetParentalGuideUrl: function (imdbId) {
                return codejkjk.movies.IMDB.GetParentalGuideUrl(imdbId);
            },
            GetIMDbMovieUrl: function (imdbId) {
                return codejkjk.movies.IMDB.GetMovieUrl(imdbId);
            }
        });

        // *** load showtimes view ***
        // init showtime date to today
        codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(Date.today().toString("yyyyMMdd"));
        var postalCode = localStorage.getItem("PostalCode") || 23226; // default to 23226
        codejkjk.movies.HomeIndex.Controls.CurrentZip().html(postalCode);
        codejkjk.movies.Flixster.GetTheaters(codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(), postalCode, codejkjk.movies.HomeIndex.LoadTheaters);

        // *** load box office & upcoming views ***
        codejkjk.movies.RottenTomatoes.GetBoxOfficeMovies(codejkjk.movies.HomeIndex.LoadBoxOfficeMovies);
        codejkjk.movies.RottenTomatoes.GetUpcomingMovies(codejkjk.movies.HomeIndex.LoadUpcomingMovies);

        // load the view that's selected (remembered)
        codejkjk.movies.HomeIndex.Controls.CurrentNavItem().trigger('click');
    },

    BuildShowtimeDayLinks: function () {
        var today = Date.today(), tomorrow = Date.today().add(1).days(), dayAfterTomorrow = Date.today().add(2).days();

        for (var i = 0; i < 5; i++) {
            var d = Date.today().add(i).days();
            var label = '';
            var cssClass = '';
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
            } // end switch

            // if date in iteration matches current showtime day
            if (d.toString("yyyyMMdd") == codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val()) {
                cssClass = "active";
            }

            var showtimeDayLink = "<a href='#' class='{0}' data-date='{1}'>{2}</a>".format(cssClass, d.toString("yyyyMMdd"), label);
            codejkjk.movies.HomeIndex.Controls.ShowtimeDayLinksContainer().append(showtimeDayLink);
        }
    },

    GetIMDbData: function () {
        codejkjk.movies.HomeIndex.Controls.IMDbMoviesNotSet().each(function () {
            var movie = $(this);
            //console.log(movie.data().imdbmovieid);
            codejkjk.movies.IMDB.GetMovie(movie.data().imdbmovieid, codejkjk.movies.HomeIndex.SetIMDbMovieDetails);
        });
    },

    LoadBoxOfficeMovies: function (movies) {
        codejkjk.movies.HomeIndex.Controls.BoxOfficeView().html(
            codejkjk.movies.HomeIndex.Controls.MovieListTemplate().render(movies)
        );
        codejkjk.movies.HomeIndex.GetIMDbData();
    },

    LoadUpcomingMovies: function (movies) {
        codejkjk.movies.HomeIndex.Controls.UpcomingView().html(
            codejkjk.movies.HomeIndex.Controls.UpcomingListTemplate().render(movies)
        );
        codejkjk.movies.HomeIndex.GetIMDbData();
    },

    LoadSearchResults: function (movies) {
        codejkjk.movies.HomeIndex.Controls.NavLinks().removeClass("selected");
        $(".content").hide();
        codejkjk.movies.HomeIndex.Controls.SearchResultsView().html(
            codejkjk.movies.HomeIndex.Controls.MovieListTemplate().render(movies)
        );
        codejkjk.movies.HomeIndex.Controls.SearchResultsView().show();
        codejkjk.movies.HomeIndex.GetIMDbData();
    },

    LoadTheaters: function (theaters) {
        var removedTheaterMovies = localStorage.getItem("RemovedTheaterMovies") != null ? localStorage.getItem("RemovedTheaterMovies").split(',') : [];

        codejkjk.movies.HomeIndex.Controls.TheaterList().html(
            codejkjk.movies.HomeIndex.Controls.TheaterListTemplate().render(theaters)
        );

        codejkjk.movies.HomeIndex.Controls.CurrentTheaterContainer().html(
            codejkjk.movies.HomeIndex.Controls.CurrentTheaterTemplate().render(theaters)
        );

        codejkjk.movies.HomeIndex.BuildShowtimeDayLinks();

        if (codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().is(":visible")) {
            codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().unmask();
            codejkjk.movies.HomeIndex.Controls.ChangeCurrentZipLink().trigger('click');
        }

        // now that the theater links are filled, set the currentTheater container's height to match height of theater links container
        var theaterListHeight = codejkjk.movies.HomeIndex.Controls.TheaterList().height() + 20;
        codejkjk.movies.HomeIndex.Controls.Theaters().css("min-height", theaterListHeight + "px");

        // fill movie data with rotten tomatoes

        // build list of rotten tomato id's
        var rtMovieIdsToLoad = [];
        $.each(theaters, function (i, theater) {
            $.each(theater.movies, function (j, movie) {
                if (rtMovieIdsToLoad.indexOf(movie.rtMovieId) == -1) { rtMovieIdsToLoad.push(movie.rtMovieId); }
            });
        });

        $.each(rtMovieIdsToLoad, function (i, rtMovieIdToLoad) {
            codejkjk.movies.RottenTomatoes.GetMovie(rtMovieIdToLoad, codejkjk.movies.HomeIndex.SetRottenTomatoesMovieDetails);
        });
    },
    SetIMDbMovieDetails: function (imdbMovieId, movie) {
        var movies = $(".movie[data-imdbmovieid='{0}']".format(imdbMovieId));
        var votes = "{0} votes on IMDb.com".format(movie.Votes);

        var rating = movie && movie.Rating;

        if (rating) {
            movies.find(".imdb").html(rating).attr("alt", votes).attr("title", votes).removeClass("imdbNotSet");
            // console.log(imdbMovieId + " is null");
            // return;
        }
    },
    SetRottenTomatoesMovieDetails: function (movie) {
        var movies = $(".movie[data-rtmovieid='{0}']".format(movie.id));

        // set movie poster
        if (typeof movie.posters != 'undefined' && movie.posters.thumbnail != 'undefined') {
            movies.find("img").attr("src", movie.posters.thumbnail);
        }

        // set rating
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
                                         .html("{0}<sup>%</sup>".format(movie.ratings.critics_score))
                                         .attr("alt", criticsAlt)
                                         .attr("title", criticsAlt);
            movies.find('.rt_audience_rating').removeClass('rt_audience_rating')
                                         .addClass(audienceClass)
                                         .html("{0}<sup>%</sup>".format(movie.ratings.audience_score))
                                         .attr("alt", audienceAlt)
                                         .attr("title", audienceAlt);
        } else {
            movies.find('.rt_critics_rating,.rt_audience_rating').addClass("unknownRating"); //.attr("alt,title", "Rating not available");
        }

        // load imdb movie ratings
        if (typeof movie.alternate_ids != 'undefined') {
            movies.attr("data-imdbmovieid", movie.alternate_ids.imdb);
            movies.find(".imdb").attr("href", codejkjk.movies.IMDB.GetMovieUrl(movie.alternate_ids.imdb));
            codejkjk.movies.IMDB.GetMovie(movie.alternate_ids.imdb, codejkjk.movies.HomeIndex.SetIMDbMovieDetails);
            movies.find('.mpaaRating').addClass("external").attr("href", codejkjk.movies.IMDB.GetParentalGuideUrl(movie.alternate_ids.imdb));
        } else {
            movies.find('.mpaaRating').addClass("disabled").click(function (e) { e.preventDefault(); }); // prevent user from clicking on this
            movies.find().addClass("unknownRating");
        }
    },

    UpdateZip: function (postalCode) {
        localStorage.setItem("PostalCode", postalCode);
        codejkjk.movies.HomeIndex.Controls.CurrentZip().html(postalCode);
        codejkjk.movies.HomeIndex.Controls.CurrentTheater().val(""); // new zip, so clear out current theater value
        codejkjk.movies.Flixster.GetTheaters(codejkjk.movies.HomeIndex.Controls.CurrentZip().val(), postalCode, codejkjk.movies.HomeIndex.LoadTheaters);
    },

    BindControls: function () {
        // handle theater link clicks
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.TheaterLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            var theaterId = link.data().theaterid;
            codejkjk.movies.HomeIndex.Controls.CurrentTheater().val(theaterId);
            $(codejkjk.movies.HomeIndex.Controls.TheaterLinksSelector()).removeClass("selected");
            link.addClass("selected");
            codejkjk.movies.HomeIndex.Controls.Theaters().removeClass("selected");
            $(".theater[data-theaterid='{0}']".format(theaterId)).addClass("selected");
        });

        // handle movie details links
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.MovieDetailsLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            var rtMovieId = link.closest("[data-rtmovieid]").data().rtmovieid;
            var top = codejkjk.movies.HomeIndex.Controls.CurrentView().offset().top;
            var left = codejkjk.movies.HomeIndex.Controls.CurrentView().offset().left + 80;
            var width = codejkjk.movies.HomeIndex.Controls.CurrentView().width() - 80;
            codejkjk.movies.HomeIndex.Controls.CurrentView().addClass("seeThrough");

            codejkjk.movies.RottenTomatoes.GetMovie(rtMovieId, function (movie) {
                codejkjk.movies.HomeIndex.Controls.MovieDetails()
                    .css("top", top + "px")
                    .css("left", left + "px")
                    .css("width", width + "px")
                    .html(
                        codejkjk.movies.HomeIndex.Controls.MovieDetailsTemplate().render(movie)
                ).show('slide', { direction: 'right' }, 250);
            });

            // $(this).hide("slide", { direction: "down" }, 1000);
        });

        // handle close movie details link
        $(document).on("click", codejkjk.movies.HomeIndex.Controls.CloseMovieDetailsLinkSelector(), function (e) {
            e.preventDefault();
            codejkjk.movies.HomeIndex.Controls.CurrentView().removeClass("seeThrough");
            codejkjk.movies.HomeIndex.Controls.MovieDetails().hide();
        });

        // handle nav item clicks
        codejkjk.movies.HomeIndex.Controls.NavLinks().click(function (e) {
            e.preventDefault();

            // clear out search val if user previously searched for something
            codejkjk.movies.HomeIndex.Controls.SearchBox().val("");

            var link = $(this);
            var navItemText = link.html();
            codejkjk.movies.HomeIndex.Controls.NavLinks().removeClass("selected");
            link.addClass("selected");
            codejkjk.movies.HomeIndex.Controls.Views().hide();
            $("[data-navitemtext='{0}']".format(navItemText)).show();
            // store this nav tab, so it opens on this one next time user visits this site
            localStorage.setItem("View", link.html());
        });

        // handle showtime day link clicks (Today, Tomorrow, etc)
        $(document).on("click", codejkjk.movies.HomeIndex.Controls.ShowtimeDayLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(link.data().date);

            codejkjk.movies.Flixster.GetTheaters(link.data().date, codejkjk.movies.HomeIndex.Controls.CurrentZip().html(), codejkjk.movies.HomeIndex.LoadTheaters);
        });

        // handle "change" link for zip
        codejkjk.movies.HomeIndex.Controls.ChangeCurrentZipLink().click(function (e) {
            e.preventDefault();
            codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().slideToggle('fast');
        });

        // bind UseNearbyPostalCodeLink
        codejkjk.movies.HomeIndex.Controls.UseNearbyPostalCodeLink().click(function (e) {
            e.preventDefault();
            codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().mask();
            codejkjk.Geo.GetPostalCode(function (postalCode) {
                codejkjk.movies.HomeIndex.UpdateZip(postalCode);
            });
        });

        // handle button that sets manual set of zip
        codejkjk.movies.HomeIndex.Controls.SetPostalCodeButton().click(function (e) {
            e.preventDefault();
            var postalCode = codejkjk.movies.HomeIndex.Controls.NewPostalCodeInput().val();
            codejkjk.movies.HomeIndex.UpdateZip(postalCode);
        });

        codejkjk.movies.HomeIndex.Controls.NewPostalCodeInput().keydown(function (e) {
            if (e.keyCode == 13) {
                codejkjk.movies.HomeIndex.Controls.SetPostalCodeButton().trigger('click');
            }
        });

        codejkjk.movies.HomeIndex.Controls.SearchBox().keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                var q = $(this).val();
                codejkjk.movies.RottenTomatoes.SearchMovies(q, codejkjk.movies.HomeIndex.LoadSearchResults);
                $(this).blur();
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
    },

    BindResultActions2: function () {
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