registerNS("codejkjk.movies.SiteActions");

codejkjk.movies.SiteActions = {
    WireCollapsers: function () {
        $(".collapser").unbind().click(function (e) {
            e.preventDefault();
            var collapser = $(this);
            var collapsee = $($(collapser.attr("collapsee")));
//            if (collapsee.hasClass("collapsed")) {
//                collapsee.fadeIn();
//            } else {
//                collapsee.fadeOut();
//            }
            collapsee.toggle()
            collapser.toggleClass("collapsed").toggleClass("expanded");
            collapser.blur();
        });
    }
};

$(document).ready(function () {

    codejkjk.movies.SiteActions.WireCollapsers();

});