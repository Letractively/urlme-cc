registerNS("codejkjk.urlme.Default");

codejkjk.urlme.Default = {
    Init: function () {
        codejkjk.urlme.Default.BindDeleteLinks();
    },

    BindDeleteLinks: function () {
        $("tr").hoverIntent({
            over: function () {
                $(this).find(".delete-link").show();
            },
            out: function () {
                $(this).find(".delete-link").hide(); // fadeOut(50);
            }
        });
    }
};

$(document).ready(function () {
    codejkjk.urlme.Default.Init();
});