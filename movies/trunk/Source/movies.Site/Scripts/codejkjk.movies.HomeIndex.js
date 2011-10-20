registerNS("codejkjk.movies.HomeIndex");

codejkjk.movies.HomeIndex = {
    // page elements
    Controls: {
        MoviesContainer: function () { return $("#movies"); }
        , LoadingContainer: function () { return $("#loading"); }
        , LoadingMessage: function () { return $("#loadingMessage"); }
        , SearchBox: function () { return $("#q"); }
        , SearchButton: function () { return $("#go"); }
        , PinLinks: function () { return $(".pinLink"); }
        , RemoveLinks: function () { return $(".removeLink"); }
    },

    Init: function () {
        codejkjk.movies.HomeIndex.Controls.SearchBox().focus(); // cuz some browsers are stupid and don't yet support html5's autofocus attr
        codejkjk.movies.HomeIndex.BindFormActions();
        codejkjk.movies.HomeIndex.HandleFeedback();

        codejkjk.movies.HomeIndex.Controls.MoviesContainer().show();
        codejkjk.movies.HomeIndex.ShowLoading("Loading RottenTomatoes.com info...");
        codejkjk.movies.RottenTomatoes.GetBoxOfficeMovies(codejkjk.movies.HomeIndex.LoadMovies);
    },

    ShowLoading: function (msg) {
        codejkjk.movies.HomeIndex.Controls.LoadingContainer().show();
        codejkjk.movies.HomeIndex.Controls.LoadingMessage().html(msg);
    },

    HideLoading: function () {
        codejkjk.movies.HomeIndex.Controls.LoadingContainer().hide();
    },

    LoadMovies: function (movies) {
        var html = "";
        $.each(movies, function (index, movie) {
            if (movie.ratings.critics_score != -1 && movie.ratings.audience_score != -1) {
                var criticsClass = movie.ratings.critics_rating.indexOf("Fresh") >= 0 ? "criticsFresh" : "criticsRotten";
                var audienceClass = movie.ratings.audience_rating.indexOf("Upright") >= 0 ? "audienceUpright" : "audienceSpilled";

                html += String.format("<div class='movie imdbNotSet' id='{0}'>", movie.alternate_ids.imdb);
                html += String.format("<img src='{0}'/>", movie.posters.profile);
                html += String.format("<div class='details'><span class='title'>{0}</span>{1}", movie.title, movie.mpaa_rating);
                html += "<div class='ratings'>";
                html += "<span class='imdb_rating star rating'></span>";
                html += String.format("<span class='{0} rottenTomato rating'>{1}<sup>%</sup></span><span class='{2} rottenTomato rating'>{3}<sup>%</sup></span>", criticsClass, movie.ratings.critics_score, audienceClass, movie.ratings.audience_score);
                html += "</div>"; // close ratings
                html += String.format("<div class='links'><a href='{0}' class='external' target='_blank'>IMDb</a>  <a href='{1}' class='external' target='_blank'>RottenTomatoes</a></div>", codejkjk.movies.IMDB.GetMovieUrl(movie.alternate_ids.imdb), movie.links.alternate);
                html += "</div>"; // close details
                html += "<div class='actions'><a href='#' class='removeLink'>Remove</a><a href='#' class='pinLink'>Pin it</a></div>";
                html += "</div>"; // close movie
            } else {
                html += "<div class='movie notYetReleased'>";
                html += String.format("<img src='{0}'/>", movie.posters.thumbnail);
                html += String.format("<div class='details'><span class='title'>{0}</span>{1}", movie.title, movie.mpaa_rating);
                html += "<div class='notYetReleasedMessage'>Not yet released / no rating available</div>";
                html += "</div>"; // close details
                html += "<div class='actions'><a href='#' class='removeLink'>Remove</a></div>";
                html += "</div>"; // close movie
            }
        });
        codejkjk.movies.HomeIndex.Controls.MoviesContainer().html(html);
        codejkjk.movies.HomeIndex.BindResultActions();
        codejkjk.movies.HomeIndex.GetIMDBData();
    },

    GetIMDBData: function () {
        codejkjk.movies.HomeIndex.Controls.MoviesContainer().show();
        codejkjk.movies.HomeIndex.ShowLoading("Loading IMDb.com info...");
        codejkjk.movies.HomeIndex.Controls.MoviesContainer().find(".movie").each(function () {
            if (!$(this).hasClass("notYetReleased")) {
                var imdbMovieId = $(this).attr("id");
                codejkjk.movies.IMDB.GetMovie(imdbMovieId, codejkjk.movies.HomeIndex.SetIMDBDetails);
            }
        });
    },
    SetIMDBDetails: function (movieId, movie) {
        var movieContainer = $("#" + movieId);
        var votes = String.format("{0} votes", movie.Votes);
        movieContainer.find(".imdb_rating").html(movie.Rating).attr("alt", votes).attr("title", votes).removeClass(".imdb_rating");
        movieContainer.removeClass("imdbNotSet");
        if ($(".imdbNotSet").length == 0) {
            codejkjk.movies.HomeIndex.HideLoading();
        }
    },
    BindFormActions: function () {
        codejkjk.movies.HomeIndex.Controls.SearchButton().click(function (e) {
            e.preventDefault();
            codejkjk.movies.HomeIndex.Controls.MoviesContainer().hide();
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
        codejkjk.movies.HomeIndex.Controls.RemoveLinks().click(function (e) {
            e.preventDefault();
            var link = $(this);
            var movie = link.closest(".movie");
            movie.remove();
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