registerNS("codejkjk.movies.HomeIndex");

// todo:
// - theater h1, make h2 b/c there should only be one h1 on the page
// - theater view, make width same as width of box office and upcoming views. this way the movie details popup will show up in same location.
// - setting min-height of theaterlist() yields zero. why? - maybe cuz at first it's invisible?
// - be careful of .data().rtmovieid, which will return "234" if it's "0234"
// - Favorites list should not have borders if there are none
// - clicking on the left col stars is buggy, does not maintain selected theater
// - combine favorite theater list with not-favorite theater list with if condition for favoriteLink class (default vs. lit)
// - critics & audience titles not always showing up
// - search for Sherlock - divs need max height, and error with parsing date
// - square off the theater list on the left hand side.
// - green plus and get a bold one, more like an icon for show movie details.
// - movie details popup - make % fonts similar to movie listings page (all bold, blue links)
// - movie details page, make overlay visible and popup visible on load so it doesn't take a sec to popup
// - movie details pages, make these links and not div.show()'s
// - no more "Top Box Office," instead, "In theaters", AND mark recent movies with Just Released - Today / 2 Days Ago (maybe w/ a star or an icon??)
// - hide "poster not found" movies
// - get rid of "noPush" class. instead, do :not[href(startsWith)'http']

codejkjk.movies.HomeIndex = {
    // page elements
    Controls: {
        ActionLinks: function () { return $(".actions").find("a"); }
        , BackToMovieDetailsLinkSelector: function () { return ".backToMovieDetails"; }
        , BoxOfficeView: function () { return $("#boxOfficeView"); }
        , BrowseLinkSelector: function () { return "#browse"; }
        , ChangeCurrentZipLink: function () { return $("#changeCurrentZipLink"); }
        , ChangeOptionsContainer: function () { return $("#changeOptions"); }
        , CloseMovieDetailsLinkSelector: function () { return ".closeMovieDetails"; }
        , CopyButton: function () { return $("#copyButton"); }
        , CopySuccess: function () { return $("#copySuccess"); }
        , CurrentNavItem: function () { return $("nav > a.selected"); }
        , CurrentRtMovieId: function () { return $("#currentRtMovieId"); }
        , CurrentShowtimeDay: function () { return $("input#currentShowtimeDay"); }
        , CurrentTheaterContainer: function () { return $("#currentTheaterContainer"); }
        , CurrentTheaterTemplate: function () { return $("#currentTheaterTemplate"); }
        , CurrentView: function () { return $(".content:visible"); }
        , CurrentZip: function () { return $("#currentZip"); }
        , DefaultNavItem: function () { return $("nav > a:first"); }
        , FavoriteLinksSelector: function () { return ".favoriteLink"; }
        , FavoriteTheaterList: function () { return $("#favoriteTheaterList"); }
        , FavoriteTheaterListTemplate: function () { return $("#favoriteTheaterListTemplate"); }
        , FirstNavLink: function () { return $("nav > a:first"); }
        , HideMovieLinksSelector: function () { return ".hideMovieLink"; }
        , IMDbMoviesNotSet: function () { return $(".imdbNotSet"); }
        , Logo: function () { return $("#logo"); }
        , MovieDetailsPopup: function () { return $("#movieDetailsPopup"); }
        , MovieDetails: function () { return $("#movieDetails"); }
        , MovieListTemplate: function () { return $("#movieListTemplate"); }
        , MovieShowtimes: function () { return $("#movieShowtimes"); }
        , MovieUrl: function () { return $("#movieUrl"); }
        , Nav: function () { return $("nav"); }
        , NavLinks: function () { return $("nav > a"); }
        , NearbyRedboxLinksSelector: function () { return ".nearbyRedboxLink"; }
        , NewZipCodeInput: function () { return codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().find("input[type=text]"); }
        , Overlay: function () { return $("#overlay"); }
        , OverlaySelector: function () { return "#overlay"; }
        , RedboxAvails: function () { return $("#redboxAvails"); }
        , RedboxAvailsList: function () { return $("#redboxAvailsList"); }
        , RedboxLocationChooser: function () { return $("#redboxLocationChooser"); }
        , RemoveLinks: function () { return $(".actions").find(".removeLink"); }
        , SearchBox: function () { return $("#q"); }
        , SearchResultsView: function () { return $("#searchResultsView"); }
        , SeeNearbyRedboxesSelector: function () { return "#seeNearbyRedboxes"; }
        , SetZipCodeButton: function () { return codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().find("button"); }
        , ShowHiddenMoviesLinksSelector: function () { return ".showHiddenMoviesLink"; }
        , ShowtimeDayLinks: function () { return $(".showtimeDays > a"); }
        , ShowtimeDayLinksSelector: function () { return ".showtimeDays > a"; }
        , ShowtimeDayLinksContainer: function () { return $(".showtimeDays"); }
        , ShowtimesLinks: function () { return $(".showtimesLink"); }
        , ShowtimesLinksSelector: function () { return ".showtimesLink"; }
        , TheatersForMovieList: function () { return $("#theatersForMovieList"); }
        , TheaterLinksSelector: function () { return ".theaterList > a"; }
        , TheaterList: function () { return $("#theaterList"); }
        , TheaterListTemplate: function () { return $("#theaterListTemplate"); }
        , Theaters: function () { return $(".theater"); }
        , UnsetIMDbMovieIds: function () { return $("[data-imdbmovieid='']"); }
        , UpcomingView: function () { return $("#upcomingView"); }
        , UseNearbyZipCodeLink: function () { return $("#useNearbyZipCode"); }
        , Views: function () { return $(".content"); }
    },

    Currents: {
        ZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("ZipCode", val);
            } else { // get
                return localStorage.getItem("ZipCode") || "23226"; // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
            }
        }
        , Theater: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("Theater", val);
            } else { // get
                return localStorage.getItem("Theater") || "";
            }
        }
        , HiddenTheaterMovies: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("HiddenTheaterMovies", val);
            } else {
                var ret = localStorage.getItem("HiddenTheaterMovies");
                return ret ? ret.split(',') : [];
            }
        }
        , RtMovieId: function () {
            return codejkjk.movies.HomeIndex.Controls.CurrentRtMovieId().val();
        }
    },

    Init: function () {
        // check if browser can support this site
        var History = window.History;
        if (typeof localStorage == 'undefined' || !navigator.geolocation || !History.enabled) {
            document.writeln("Please use a more recent browser to view this site.  I recommend <a href='http://www.google.com/chrome'>Chrome</a>.");
            return;
        }

        codejkjk.movies.HomeIndex.BindControls();
        codejkjk.movies.HomeIndex.RegisterJsRenderHelpers();

        // *** load showtimes view ***
        // init showtime date to today
        codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(Date.today().toString("yyyyMMdd"));
        codejkjk.movies.HomeIndex.Controls.CurrentZip().html(codejkjk.movies.HomeIndex.Currents.ZipCode());
        codejkjk.movies.Api.GetTheaters(codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(), codejkjk.movies.HomeIndex.Currents.ZipCode(), codejkjk.movies.HomeIndex.LoadTheaters);

        // setup History
        // handle any state changes
        History.Adapter.bind(window, 'statechange', function () {
            var state = History.getState();
            codejkjk.movies.HomeIndex.HandlePushState(state.url);
        });

        // handle "a" clicks - prevent their default and isntead push state
        $(document).on('click', 'a[href^="/"]', function (e) {
            e.preventDefault();
            History.pushState(null, null, $(this).attr("href"));
        });

        // handle this page's load
        var initState = History.getState();
        codejkjk.movies.HomeIndex.HandlePushState(initState.url);
    },

    ShowSection: function (path) {
        // primary nav change
        // clear out search val if user previously searched for something
        codejkjk.movies.HomeIndex.Controls.SearchBox().val("");

        var link = codejkjk.movies.HomeIndex.Controls.Nav().find("a[href='{0}']".format(path)); // logo does not have inner html, which is what we use later to select view to show
        codejkjk.movies.HomeIndex.Controls.NavLinks().removeClass("selected glowing rounded");
        link.addClass("selected glowing rounded");
        codejkjk.movies.HomeIndex.Controls.Views().hide();

        // show view
        $('section[data-navitemtext="{0}"]'.format(link.html())).show();
    },

    HandlePushState: function (url) {
        var paths = url.replace('//', '').split('/');
        var firstPath = '/' + paths[1]; // "", "comingsoon", "showtimes" (all navs), or "hunger-games" (movie)
        if (firstPath === "/" || firstPath === "/comingsoon" || firstPath === "/showtimes" || firstPath === "/redbox") {
            codejkjk.movies.HomeIndex.ShowSection(firstPath);
        }
        else {
            // movie details link
            if (codejkjk.movies.HomeIndex.Currents.RtMovieId()) {
                // movie details is already in dom, b/c user went to movie link directly
                codejkjk.movies.HomeIndex.ShowSection("/");
                codejkjk.movies.HomeIndex.ShowMovieDetails();
            } else {
                codejkjk.movies.HomeIndex.ShowMovieDetails(paths[2]);
            }
        }
    },

    RegisterJsRenderHelpers: function () {
        $.views.registerHelpers({
            IsReleased: function (releaseDate) {
                var now = new Date();
                releaseDate = new Date(releaseDate);
                return now >= releaseDate;
            },
            IsCurrentTheater: function (iTheaterId) {
                return iTheaterId.toString() == codejkjk.movies.HomeIndex.Currents.Theater();
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
            Snippet: function (text, len) {
                return text.snippet(len);
            },
            GetFavoriteLinkClass: function (theaterId) {
                var favoriteTheaters = localStorage.getItem("FavoriteTheaters");
                favoriteTheaters = favoriteTheaters ? favoriteTheaters.split(',') : [];
                return favoriteTheaters.indexOf(theaterId.toString()) >= 0 ? "lit" : "default";
            }
        });
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
        return;
        codejkjk.movies.HomeIndex.Controls.IMDbMoviesNotSet().each(function () {
            var imdb = $(this);
            var imdbMovieId = imdb.attr("data-imdbmovieid");
            codejkjk.movies.Api.GetIMDbMovie(imdbMovieId, function (movie) {
                var ratings = $(".imdb[data-imdbmovieid='{0}']".format(imdbMovieId));

                if (movie.rating && movie.rating !== "N/A" && movie.votes && movie.votes !== "N/A") {
                    var title = "{0} votes on IMDb.com".format(movie.votes);
                    ratings.html(movie.rating).attr("title", title);
                } else {
                    ratings.html("n/a");
                }
                ratings.removeClass("imdbNotSet");
            });
        });
    },

    LoadSearchResults: function (html) {
        codejkjk.movies.HomeIndex.Controls.NavLinks().removeClass("selected glowing rounded");
        $(".content").hide();
        codejkjk.movies.HomeIndex.Controls.SearchResultsView().html(html);
        codejkjk.movies.HomeIndex.Controls.SearchResultsView().show();
        codejkjk.movies.HomeIndex.GetIMDbData();
    },

    LoadTheaters: function (postalCode) {
        var theaters = postalCode.theaters;

        var hiddenTheaterMovies = codejkjk.movies.HomeIndex.Currents.HiddenTheaterMovies();

        var favoriteTheaterIds = localStorage.getItem("FavoriteTheaters");
        favoriteTheaterIds = favoriteTheaterIds ? favoriteTheaterIds.split(',') : [];

        // determine favorite and not-favorite theaters
        var favoriteTheaters = $.grep(theaters, function (theater, i) {
            return favoriteTheaterIds.indexOf(theater.id.toString()) >= 0;
        });
        var notFavoriteTheaters = $.grep(theaters, function (theater, i) {
            return favoriteTheaterIds.indexOf(theater.id.toString()) == -1;
        });

        // establish current theater id before we render the html, so the jsRender helper can know which items it should show as active
        if (!codejkjk.movies.HomeIndex.Currents.Theater()) {
            // no current theater set, so choose first
            var currentTheaterId = null;
            if (favoriteTheaters.length > 0) { currentTheaterId = favoriteTheaters[0].id; }
            else { currentTheaterId = notFavoriteTheaters[0].id; }
            codejkjk.movies.HomeIndex.Currents.Theater(currentTheaterId);
        } else {
            // make sure the theaterId is in the incoming list of theaters
            var results = $.grep(theaters, function (theater, i) {
                return theater.id.toString() == codejkjk.movies.HomeIndex.Currents.Theater();
            });
            if (results.length == 0) { // current theaterId does NOT exist in incoming list of theaters, so get the first from favorites or nonfavorites if favorites is empty
                var currentTheaterId = null;
                if (favoriteTheaters.length > 0) { currentTheaterId = favoriteTheaters[0].id; }
                else { currentTheaterId = notFavoriteTheaters[0].id; }
                codejkjk.movies.HomeIndex.Currents.Theater(currentTheaterId);
            }
        }

        // loop thru theaters and set movieClass for each movie based on whether or not that movie's been previously hidden, per local storage memory
        $.each(theaters, function (i, theater) {
            var numHiddenTheaterMovies = 0;
            $.each(theater.movies, function (j, theaterMovie) {
                var movieClass = hiddenTheaterMovies.indexOf("{0}-{1}".format(theater.id, theaterMovie.id)) >= 0 ? "hidden" : "";
                numHiddenTheaterMovies = movieClass == "hidden" ? ++numHiddenTheaterMovies : numHiddenTheaterMovies;
                theaterMovie.movieClass = movieClass;
            });
            if (numHiddenTheaterMovies) { theater.numHiddenMovies = numHiddenTheaterMovies; }
        });

        // render theaters
        codejkjk.movies.HomeIndex.Controls.FavoriteTheaterList().html(
            codejkjk.movies.HomeIndex.Controls.FavoriteTheaterListTemplate().render(favoriteTheaters)
        );
        codejkjk.movies.HomeIndex.Controls.TheaterList().html(
            codejkjk.movies.HomeIndex.Controls.TheaterListTemplate().render(notFavoriteTheaters)
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
    },

    UpdateZip: function (zipCode) {
        codejkjk.movies.HomeIndex.Currents.ZipCode(zipCode); // update current zip code
        codejkjk.movies.HomeIndex.Controls.CurrentZip().html(zipCode);
        codejkjk.movies.HomeIndex.Currents.Theater(""); // new zip, so clear out current theater value
        codejkjk.movies.Api.GetTheaters(codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(), zipCode, codejkjk.movies.HomeIndex.LoadTheaters);
    },

    ShowMovieDetails: function (rtMovieIdToAjaxLoad) {
        // show overlay
        var overlayHeight = $(document).height() + "px";
        var overlayWidth = $(document).width() + "px";
        codejkjk.movies.HomeIndex.Controls.Overlay().css("height", overlayHeight).css("width", overlayWidth).show();

        if (rtMovieIdToAjaxLoad) {
            // user went to movie link by clicking on a poster, so ajax-load it
            codejkjk.movies.HomeIndex.Controls.MovieDetailsPopup().html("<div class='loading'></div>");
            codejkjk.movies.HomeIndex.Controls.MovieDetailsPopup().show();

            codejkjk.movies.Api.GetMovieHtml(rtMovieIdToAjaxLoad, function (html) {
                codejkjk.movies.HomeIndex.Controls.MovieDetailsPopup().html(html);
                FB.XFBML.parse();
                codejkjk.movies.HomeIndex.InitZeroClipboard();
            });
        } else {
            // movie details are already in dom, so just init zeroclipboard b/c it's ready to go
            codejkjk.movies.HomeIndex.Controls.MovieDetailsPopup().show();
            codejkjk.movies.HomeIndex.InitZeroClipboard();
        }
    },

    InitZeroClipboard: function () {
        var clip = new ZeroClipboard.Client();
        clip.setText(codejkjk.movies.HomeIndex.Controls.MovieUrl().val());
        clip.glue('copyButton');

        clip.addEventListener('complete', function (client, text) {
            codejkjk.movies.HomeIndex.Controls.CopySuccess().show().delay(2500).fadeOut('fast');
        });
    },

    BindControls: function () {
        // handle "Browse Nearby Redbox"
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.BrowseLinkSelector(), function (e) {
            e.preventDefault();
            $(this).next("div").toggleClass("hidden");
        });

        $(document).on('click', codejkjk.movies.HomeIndex.Controls.SeeNearbyRedboxesSelector(), function (e) {
            e.preventDefault();
            codejkjk.Geo.GetLatLong(function (lat, long) {
                codejkjk.movies.Api.GetRedboxesHtml(lat, long, function (html) {
                    alert(html);
                });
            });
        });

        // handle favorite theater links
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.FavoriteLinksSelector(), function (e) {
            e.preventDefault();
            e.stopPropagation();
            var link = $(this);
            var theaterId = link.closest("[data-theaterid]").attr("data-theaterid");
            var favoriteTheaters = localStorage.getItem("FavoriteTheaters");
            favoriteTheaters = favoriteTheaters ? favoriteTheaters.split(',') : [];
            if (link.hasClass("lit")) {
                // currently set to favorite, so remove from favorites list
                if (favoriteTheaters.indexOf(theaterId) >= 0) {
                    favoriteTheaters.splice(favoriteTheaters.indexOf(theaterId), 1);
                }
            } else {
                // currently NOT set to favorite, so add to favorites list
                if (favoriteTheaters.indexOf(theaterId) == -1) {
                    favoriteTheaters.push(theaterId);
                }
            }
            link.toggleClass("lit").toggleClass("default");
            localStorage.setItem("FavoriteTheaters", favoriteTheaters.join(','));

            // refresh theaters
            codejkjk.movies.Api.GetTheaters(codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(), codejkjk.movies.HomeIndex.Currents.ZipCode(), codejkjk.movies.HomeIndex.LoadTheaters);
        });

        // handle theater link clicks
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.TheaterLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            var theaterId = link.attr("data-theaterid");
            codejkjk.movies.HomeIndex.Currents.Theater(theaterId);
            $(codejkjk.movies.HomeIndex.Controls.TheaterLinksSelector()).removeClass("selected glowing");
            link.addClass("selected glowing");
            codejkjk.movies.HomeIndex.Controls.Theaters().removeClass("selected glowing");
            $(".theater[data-theaterid='{0}']".format(theaterId)).addClass("selected glowing");
        });

        // handle hide movie links
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.HideMovieLinksSelector(), function (e) {
            e.preventDefault();
            var movie = $(this);
            var theater = movie.closest(".theater");
            var hiddenTheaterMovies = codejkjk.movies.HomeIndex.Currents.HiddenTheaterMovies();
            hiddenTheaterMovies.pushIfDoesNotExist(movie.data().theatermovie);
            codejkjk.movies.HomeIndex.Currents.HiddenTheaterMovies(hiddenTheaterMovies.join(','));

            movie.closest(".movie").fadeOut('fast', function () {
                $(this).addClass("hidden").removeAttr("style");
                theater.find(".numHiddenMovies").text(theater.find(".movie.hidden").length);
                theater.find(".showHiddenMoviesLinkContainer").removeClass("hidden");
            });

        });

        // handle "show hidden movies" links
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.ShowHiddenMoviesLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            var theater = link.closest(".theater");
            theater.find(".movie.hidden").removeClass("hidden");
            link.parent().addClass("hidden");

            // update localStorage; trim out the theater that this movie belongs to, all of its hidden movies
            var hiddenTheaterMovies = codejkjk.movies.HomeIndex.Currents.HiddenTheaterMovies();
            hiddenTheaterMovies = $.grep(hiddenTheaterMovies, function (hiddenTheaterMovie, i) {
                return hiddenTheaterMovie.split('-')[0] != theater.data().theaterid;
            });
            codejkjk.movies.HomeIndex.Currents.HiddenTheaterMovies(hiddenTheaterMovies.join(','))
        });

        // handle close movie details link AND overlay click
        $(document).on("click", codejkjk.movies.HomeIndex.Controls.CloseMovieDetailsLinkSelector(), function (e) {
            codejkjk.movies.HomeIndex.Controls.Overlay().hide();
            codejkjk.movies.HomeIndex.Controls.MovieDetailsPopup().hide().html("");
            codejkjk.movies.HomeIndex.Controls.CurrentNavItem().trigger('click');
        });

        // handle showtime day link clicks (Today, Tomorrow, etc)
        $(document).on("click", codejkjk.movies.HomeIndex.Controls.ShowtimeDayLinksSelector(), function (e) {
            e.preventDefault();
            var link = $(this);
            codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(link.data().date);

            codejkjk.movies.Api.GetTheaters(link.data().date, codejkjk.movies.HomeIndex.Controls.CurrentZip().html(), codejkjk.movies.HomeIndex.LoadTheaters);
        });

        // handle "change" link for zip
        codejkjk.movies.HomeIndex.Controls.ChangeCurrentZipLink().click(function (e) {
            e.preventDefault();
            codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().slideToggle('fast');
        });

        // handle "back to The Hunger Games" link on movie showtimes link / redbox availability on movie popup
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.BackToMovieDetailsLinkSelector(), function (e) {
            e.preventDefault();

            codejkjk.movies.HomeIndex.Controls.MovieDetails().show();
            $(this).closest("div").addClass("hidden");
        });

        // handle showtimes links on movie detail popups
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.ShowtimesLinksSelector(), function (e) {
            e.preventDefault();

            var link = $(this);
            var movieShowtimesContainer = codejkjk.movies.HomeIndex.Controls.MovieShowtimes();
            var theaterList = codejkjk.movies.HomeIndex.Controls.TheatersForMovieList();

            if (movieShowtimesContainer.hasClass("loaded")) { // already been loaded previously, so just show it
                codejkjk.movies.HomeIndex.Controls.MovieDetails().hide();
                movieShowtimesContainer.removeClass("hidden");
            } else {
                // first thing, add loading class
                link.addClass("loading");
                codejkjk.Geo.GetZipCode(function (zipCode) {
                    codejkjk.movies.Api.GetTheatersForMovie(codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(), zipCode, codejkjk.movies.HomeIndex.Currents.RtMovieId(), function (theatersHtml) {
                        theaterList.html(theatersHtml);

                        // remove loading class
                        link.removeClass("loading");
                        codejkjk.movies.HomeIndex.Controls.MovieDetails().hide();
                        movieShowtimesContainer.removeClass("hidden").addClass("loaded");
                    });
                });
            }
        });

        // handle showtimes links on movie detail popups
        $(document).on('click', codejkjk.movies.HomeIndex.Controls.NearbyRedboxLinksSelector(), function (e) {
            e.preventDefault();

            var link = $(this);
            var redboxAvailsContainer = codejkjk.movies.HomeIndex.Controls.RedboxAvails();
            var redboxAvailsList = codejkjk.movies.HomeIndex.Controls.RedboxAvailsList();

            if (redboxAvailsContainer.hasClass("loaded")) {
                // already been loaded previously, so just show it
                codejkjk.movies.HomeIndex.Controls.MovieDetails().hide();
                redboxAvailsContainer.removeClass("hidden");
            } else {
                // first thing, add loading class
                link.addClass("loading");
                // codejkjk.Geo.GetZipCode(function (zipCode) {
                // codejkjk.movies.Api.GetTheatersForMovie(codejkjk.movies.HomeIndex.Controls.CurrentShowtimeDay().val(), zipCode, codejkjk.movies.HomeIndex.Currents.RtMovieId(), function (theatersHtml) {
                redboxAvailsList.html("This feature coming soon...");

                // remove loading class
                link.removeClass("loading");
                codejkjk.movies.HomeIndex.Controls.MovieDetails().hide();
                redboxAvailsContainer.removeClass("hidden").addClass("loaded");
                // });
                // });
            }
        });

        // bind UseNearbyZipCodeLink
        codejkjk.movies.HomeIndex.Controls.UseNearbyZipCodeLink().click(function (e) {
            e.preventDefault();
            codejkjk.movies.HomeIndex.Controls.ChangeOptionsContainer().mask();
            codejkjk.Geo.GetZipCode(function (zipCode) {
                codejkjk.movies.HomeIndex.UpdateZip(zipCode);
            });
        });

        // handle button that sets manual set of zip
        codejkjk.movies.HomeIndex.Controls.SetZipCodeButton().click(function (e) {
            e.preventDefault();
            var zipCode = codejkjk.movies.HomeIndex.Controls.NewZipCodeInput().val();
            codejkjk.movies.HomeIndex.UpdateZip(zipCode);
        });

        // handle Enter key on manual zip input box - triggers "Set" button click
        codejkjk.movies.HomeIndex.Controls.NewZipCodeInput().keydown(function (e) {
            if (e.keyCode == 13) {
                codejkjk.movies.HomeIndex.Controls.SetZipCodeButton().trigger('click');
            }
        });

        // handle Enter key on search box
        codejkjk.movies.HomeIndex.Controls.SearchBox().keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                var q = $(this).val();
                codejkjk.movies.Api.SearchMovies(q, codejkjk.movies.HomeIndex.LoadSearchResults);
                $(this).blur();
            }
        });
    }
}

$(document).ready(function () {
    codejkjk.movies.HomeIndex.Init();
});