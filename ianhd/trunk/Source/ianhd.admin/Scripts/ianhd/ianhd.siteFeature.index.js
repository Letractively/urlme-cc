// $(".Search-Box input").keyup(function (event) { if (event.keyCode == 13) { var idPrefix = "#" + $(this).closest(".Column").attr("id") + " "; $(idPrefix + "#SearchButton").trigger("click"); } });
ianhd.registerNamespace("siteFeature.index");
ianhd.siteFeature.index = {
    controls: {

    },
    init: function () {
        ianhd.siteFeature.index.bindControls();
    },
    bindControls: function () {
        var controls = ianhd.siteFeature.index.controls;

    }
};

$(function () {
    ianhd.siteFeature.index.init();
});