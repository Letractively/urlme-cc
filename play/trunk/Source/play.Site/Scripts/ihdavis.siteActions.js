﻿ihdavis.registerNamespace("siteActions");
ihdavis.siteActions = {
    wireDialogs: function () {
        $(document).on('click', ".ui-widget-overlay", function () {
            $(".ui-dialog-titlebar-close").trigger('click');
        });
    },
    wireTooltips: function () {
        if ($(".tooltip").length) {
            $(".tooltip").tooltip();
        }
    }
};

$(function () {
    ihdavis.siteActions.wireDialogs();
    ihdavis.siteActions.wireTooltips();
});