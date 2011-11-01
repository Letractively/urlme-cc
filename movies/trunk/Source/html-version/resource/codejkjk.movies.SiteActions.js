registerNS("codejkjk.movies.SiteActions");

codejkjk.movies.SiteActions = {
    WireCollapsers: function () {
        $(".collapser").unbind().click(function (e) {
            e.preventDefault();

            var collapser = $(this);
            var collapsee = $($(collapser.attr("collapsee")));
            var collapsedTheaterIds = localStorage.getItem("CollapsedTheaterIds") != null ? localStorage.getItem("CollapsedTheaterIds").split(',') : [];
            var theaterId = collapser.closest(".theater").attr("id");

            if (collapser.hasClass("collapsed")) {
                collapsedTheaterIds.removeByElement(theaterId);
            } else {
                collapsedTheaterIds.pushIfDoesNotExist(theaterId);
            }

            collapsee.slideToggle('fast');
            collapser.toggleClass("collapsed").toggleClass("expanded");
            collapser.blur();

            if (collapsedTheaterIds.length > 0) {
                localStorage.setItem("CollapsedTheaterIds", collapsedTheaterIds.join(','));
            } else {
                localStorage.removeItem("CollapsedTheaterIds");
            }
        });
    }
};

$(document).ready(function () {

    codejkjk.movies.SiteActions.WireCollapsers();

});