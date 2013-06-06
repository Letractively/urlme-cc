ianhd.registerNamespace("home.index");
ianhd.home.index = {
    controls: {
        taglineSelector: function () { return ".tagline span:visible"; }
    },
    init: function () {
        ianhd.home.index.bindControls();
        ianhd.home.index.initTaglines();
    },
    bindControls: function () {
        var controls = ianhd.home.index.controls;

        $(document).on('click', controls.taglineSelector(), function (e) {
            alert('Stay tuned...');
        });
    },
    initTaglines: function () {
        var taglines = $(".tagline span");
        taglines.attr("title", "Click to refresh tagline");

        var lastIdxKey = "lastTaglineIdx";
        var lastIdx = localStorage.getItem(lastIdxKey);
        var nextIdx = lastIdx == null ? 0 : parseInt(lastIdx) + 1;
        nextIdx = nextIdx >= taglines.length ? 0 : nextIdx;
        localStorage.setItem(lastIdxKey, nextIdx);
        taglines.eq(nextIdx).show();
        // todo: enable user to click on tagline to auto refresh with random idx
    }
};

$(function () {
    ianhd.home.index.init();
});