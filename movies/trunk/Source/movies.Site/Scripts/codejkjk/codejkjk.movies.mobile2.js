registerNS("codejkjk.movies.mobile");

var backUrl = "";

codejkjk.movies.mobile = {
    // page elements
    controls: {
        searchButton: function () { return $(".m.searchButton"); },
        searchBar: function () { return $(".m.searchBar"); }
    },

    init: function () {
        codejkjk.movies.mobile.bindControls();
    },

    bindControls: function () {
        // toggle search bar
        codejkjk.movies.mobile.controls.searchButton().click(function (e) {
            e.preventDefault();
            var searchBar = codejkjk.movies.mobile.controls.searchBar();
            searchBar.toggle("fast", function () {
                if (searchBar.is(":visible")) {
                    searchBar.find("input").focus();
                }
            });
        });
    }
}

$(document).ready(function () {
    codejkjk.movies.mobile.init();
});