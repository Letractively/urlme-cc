registerNS("codejkjk.movies.redbox");

codejkjk.movies.redbox = {
    // page elements
    controls: {
        browseLinkSelector: function () { return "#browse"; }
        , nearbyRedboxLinksSelector: function () { return ".nearbyRedboxLink"; }
        , redboxAvails: function () { return $("#redboxAvails"); }
        , redboxAvailsList: function () { return $("#redboxAvailsList"); }
        , redboxes: function () { return $("#redboxes"); }
        , redboxOptions: function () { return $("#redboxOptions"); }
        , redboxZipCodeInput: function () { return $("#redboxZip"); }
        , searchRedboxZipCodeButton: function () { return $("#redboxZip").next("button"); }
        , seeNearbyRedboxesSelector: function () { return "#seeNearbyRedboxes"; }
    },

    currents: {
        redboxZipCode: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("RedboxZipCode", val);
            } else { // get
                return localStorage.getItem("RedboxZipCode") || "23226"; // return str b/c if we ever want to change it to 02322, this will get converted to str as "3222" if we return as int
            }
        }
        , redbox: function (val) {
            if (typeof val != "undefined") { // set
                localStorage.setItem("Redbox", val);
            } else { // get
                return localStorage.getItem("Redbox") || "";
            }
        }
    },

    init: function () {
        // codejkjk.movies.desktop.initGooglePlaces("inputShowtimesZipSmall");
        // codejkjk.movies.desktop.initGooglePlaces("inputShowtimesZipBig");
        codejkjk.movies.redbox.bindControls();
    },

    UpdateRedboxZip: function (zipCode) {
        codejkjk.movies.redbox.currents.redboxZipCode(zipCode); // update current redbox zip code
        // codejkjk.movies.redbox.controls.CurrentZip().html(zipCode);
        codejkjk.movies.redbox.currents.redbox(""); // new zip, so clear out current redbox store id value

        codejkjk.Geo.GetLatLongFromZip(zipCode, function (lat, long) {
            codejkjk.movies.Api.GetRedboxesHtml(lat, long, function (html) {
                codejkjk.movies.redbox.controls.redboxes().html(html);
            });
        });
    },

    bindControls: function () {
        // handle "Browse Nearby Redbox"
        $(document).on('click', codejkjk.movies.redbox.controls.browseLinkSelector(), function (e) {
            e.preventDefault();
            codejkjk.movies.redbox.controls.redboxOptions().toggleClass("hidden");
        });

        // handle "see nearby redboxes" links
        $(document).on('click', codejkjk.movies.redbox.controls.seeNearbyRedboxesSelector(), function (e) {
            e.preventDefault();
            codejkjk.Geo.GetLatLong(function (lat, long) {
                codejkjk.movies.Api.GetRedboxesHtml(lat, long, function (html) {
                    codejkjk.movies.redbox.controls.redboxes().html(html);
                });
            });
        });

        // handle showtimes links on movie detail popups
        $(document).on('click', codejkjk.movies.redbox.controls.nearbyRedboxLinksSelector(), function (e) {
            e.preventDefault();

            var link = $(this);
            var redboxAvailsContainer = codejkjk.movies.redbox.controls.redboxAvails();
            var redboxAvailsList = codejkjk.movies.redbox.controls.redboxAvailsList();

            if (redboxAvailsContainer.hasClass("loaded")) {
                // already been loaded previously, so just show it
                codejkjk.movies.redbox.controls.MovieDetails().hide();
                redboxAvailsContainer.removeClass("hidden");
            } else {
                // first thing, add loading class
                link.addClass("loading");
                redboxAvailsList.html("This feature coming soon...");

                // remove loading class
                link.removeClass("loading");
                codejkjk.movies.redbox.controls.MovieDetails().hide();
                redboxAvailsContainer.removeClass("hidden").addClass("loaded");
                // });
                // });
            }
        });

        // handle button that sets manual set of zip for redbox search
        codejkjk.movies.redbox.controls.searchRedboxZipCodeButton().click(function (e) {
            e.preventDefault();
            var zipCode = codejkjk.movies.redbox.controls.redboxZipCodeInput().val();
            codejkjk.movies.redbox.UpdateRedboxZip(zipCode);
        });

        // handle Enter key on manual zip input box for redbox search - triggers "Search" button click
        codejkjk.movies.redbox.controls.redboxZipCodeInput().keydown(function (e) {
            if (e.keyCode == 13) {
                codejkjk.movies.redbox.controls.searchRedboxZipCodeButton().trigger('click');
            }
        });
    }
}

$(document).ready(function () {
    codejkjk.movies.redbox.init();
});