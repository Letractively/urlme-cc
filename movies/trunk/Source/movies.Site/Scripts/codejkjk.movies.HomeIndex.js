registerNS("codejkjk.movies.HomeIndex");

codejkjk.movies.HomeIndex = {
    // page elements
    MoviesContainer: function () { return $("#movies"); },
    LoadingContainer: function () { return $("#loading"); },
    LoadingMessage: function () { return $("#loadingMessage"); },

    Init: function () {
        codejkjk.movies.HomeIndex.BindFormActions();
        codejkjk.movies.HomeIndex.HandleFeedback();

        codejkjk.movies.HomeIndex.ShowLoading("Loading RottenTomatoes.com info...");
        codejkjk.movies.RottenTomatoes.GetBoxOfficeMovies(codejkjk.movies.HomeIndex.LoadMovies);
    },

    ShowLoading: function (msg) {
        codejkjk.movies.HomeIndex.LoadingContainer().show();
        codejkjk.movies.HomeIndex.LoadingMessage().html(msg);
    },

    HideLoading: function () {
        codejkjk.movies.HomeIndex.LoadingContainer().hide();
    },

    LoadMovies: function (movies) {
        codejkjk.movies.HomeIndex.MoviesContainer().hide(); // hide it; show it after injecting imdb data
        var html = "";
        $.each(movies, function (index, movie) {
            html += String.format("<div class='movie imdbNotSet' id='{0}'>", movie.alternate_ids.imdb);
            html += String.format("<img src='{0}'/>", movie.posters.profile);
            html += String.format("<div class='details'><span class='title'>{0}</span>{1}", movie.title, movie.mpaa_rating);
            html += String.format("<div class='ratings'><span class='imdb_rating star rating'></span> (<span class='imdb_votes'></span> votes)</div>");
            html += String.format("<div class='ratings'><span class='critics-fresh rating'>{0}%</span> <span class='audience-upright rating'>{1}%</span></div>", movie.ratings.critics_score, movie.ratings.audience_score);
            html += String.format("<div class='links'><a href='{0}' class='external' target='_blank'>IMDb</a>, <a href='{1}' class='external' target='_blank'>RottenTomatoes</a></div>", codejkjk.movies.IMDB.GetMovieUrl(movie.alternate_ids.imdb), movie.links.alternate);
            html += "</div>"; // close details
            html += "</div>"; // close movie
        });
        codejkjk.movies.HomeIndex.MoviesContainer().html(html);
        codejkjk.movies.HomeIndex.GetIMDBData();
    },

    GetIMDBData: function () {
        codejkjk.movies.HomeIndex.ShowLoading("Loading IMDb.com info...");
        codejkjk.movies.HomeIndex.MoviesContainer().find(".movie").each(function () {
            var imdbMovieId = $(this).attr("id");
            codejkjk.movies.IMDB.GetMovie(imdbMovieId, codejkjk.movies.HomeIndex.SetIMDBDetails);
        });
    },
    SetIMDBDetails: function (movieId, movie) {
        var movieContainer = $("#" + movieId);
        movieContainer.find(".imdb_rating").html(movie.Rating);
        movieContainer.find(".imdb_votes").html(addCommas(movie.Votes));
        movieContainer.removeClass("imdbNotSet");
        if ($(".imdbNotSet").length == 0) {
            codejkjk.movies.HomeIndex.MoviesContainer().show();
            codejkjk.movies.HomeIndex.HideLoading();
        }
    },
    BindFormActions: function () {
        $("#go").click(function (e) {
            e.preventDefault();
            var q = $("#q").val();
            codejkjk.movies.RottenTomatoes.SearchMovies(q, codejkjk.movies.HomeIndex.LoadMovies);
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