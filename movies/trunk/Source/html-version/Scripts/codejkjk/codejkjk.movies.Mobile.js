registerNS("codejkjk.movies.Mobile");

codejkjk.movies.Mobile = {
    // page elements
    Controls: {
    },

    Currents: {
        HiddenTheaterMovies: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("HiddenTheaterMovies", val);
            } else {
                var ret = localStorage.getItem("HiddenTheaterMovies");
                return ret ? ret.split(',') : [];
            }
        }
    },

    PageBeforeChange: function (e, data) {
        // handle changepage where the caller is asking us to load a page by url
        console.log("typeof data.toPage = {0}".format(typeof data.toPage));
        if (typeof data.toPage === "string") {
            var u = $.mobile.path.parseUrl(data.toPage), re = /^#movie-details/;
            if (u.hash.search(re) !== -1) {

            }
        }        
    }
}

$(function () {
    $("#movieList").html(
        $("#movieTemplate").render([{ title: "t1" }, { title: "t2" }])
    ).listview('refresh');
});

$(document).bind("pagebeforechange", function (e, data) {
    codejkjk.movies.Mobile.PageBeforeChange(e, data);
});