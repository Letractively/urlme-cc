// $(".Search-Box input").keyup(function (event) { if (event.keyCode == 13) { var idPrefix = "#" + $(this).closest(".Column").attr("id") + " "; $(idPrefix + "#SearchButton").trigger("click"); } });
ianhd.registerNamespace("tagline.index");
ianhd.tagline.index = {
    controls: {

    },
    init: function () {
        ianhd.tagline.index.bindControls();
    },
    bindControls: function () {
        var controls = ianhd.tagline.index.controls;

    }
};

$(function () {
    ianhd.tagline.index.init();
});