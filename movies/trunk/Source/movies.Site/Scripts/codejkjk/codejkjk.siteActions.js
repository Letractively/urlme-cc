registerNS("codejkjk.siteActions");

codejkjk.siteActions = {
    // make html 5 features work in browsers that don't support it innately, like IE
    html5ize: function () {
        if (!codejkjk.browserCompatability.testAttribute('input', 'placeholder')) {
            codejkjk.form.smartTextBox($("input[placeholder]"), 'placeholder');
        }
    },

    wireReleaseDates: function () {
        $("[data-releasedate]:not(.wired)").each(function (i, rd) {
            var rdContainer = $(rd);
            var releaseDateStr = rdContainer.attr("data-releasedate"); // 06/24/2012
            var rdDisplay = rdContainer.find("span");
            var releaseDate = Date.parse(releaseDateStr);
            var today = Date.parse("today");
            var todayStr = today.toString("MM/dd/yyyy");
            var tomorrow = Date.parse("tomorrow");
            var tomorrowStr = tomorrow.toString("MM/dd/yyyy");
            var nextFriday = Date.parse("next friday");
            var nextFridayStr = nextFriday.toString("MM/dd/yyyy");
            if (releaseDateStr == todayStr) {
                rdDisplay.addClass("alert");
                rdDisplay.text("Today!");
            }
            else if (releaseDateStr == tomorrowStr) {
                rdDisplay.addClass("alert");
                rdDisplay.text("Tomorrow");
            }
            else if (releaseDateStr == nextFridayStr) {
                rdDisplay.text("This Friday");
            } else {
                rdDisplay.text(releaseDate.toString("MMM d, yyyy"));
            }
            rdDisplay.addClass("wired");
        });
    }
}

$(document).ready(function () {
    codejkjk.siteActions.html5ize();

    codejkjk.siteActions.wireReleaseDates();
});