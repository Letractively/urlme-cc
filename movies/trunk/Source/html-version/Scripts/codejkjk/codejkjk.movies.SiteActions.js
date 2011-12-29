registerNS("codejkjk.movies.SiteActions");

codejkjk.movies.SiteActions = {
    WireCollapsers: function () {
        $(".collapser").unbind().click(function (e) {
            e.preventDefault();

            var collapser = $(this);
            var collapsee = $($(collapser.attr("collapsee")));
            var collapsedTheaters = localStorage.getItem("CollapsedTheaters") != null ? localStorage.getItem("CollapsedTheaters").split(',') : [];
            var theaterId = collapser.closest(".theater").attr("id");

            if (collapser.hasClass("collapsed")) {
                collapsedTheaters.removeByElement(theaterId);
            } else {
                collapsedTheaters.pushIfDoesNotExist(theaterId);
            }

            collapsee.slideToggle('fast');
            collapser.toggleClass("collapsed").toggleClass("expanded");
            collapser.blur();

            if (collapsedTheaters.length > 0) {
                localStorage.setItem("CollapsedTheaters", collapsedTheaters.join(','));
            } else {
                localStorage.removeItem("CollapsedTheaters");
            }
        });
    }
};

$(document).ready(function () {

    codejkjk.movies.SiteActions.WireCollapsers();

});