ihdavis.registerNamespace("siteActions");
ihdavis.siteActions = {
    wireDialogs: function () {
        $(document).on('click', ".ui-widget-overlay", function () {
            $(".ui-dialog-titlebar-close").trigger('click');
        });
    }
};

$(function () {
    ihdavis.siteActions.wireDialogs();
});