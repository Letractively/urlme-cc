ianhd.registerNamespace("home.index");
ianhd.home.index = {
    controls: {
    },
    init: function () {
        ianhd.home.index.bindControls();
        ianhd.home.index.initTaglines();
        ianhd.home.index.initShortcuts();
    },
    bindControls: function () {
        var controls = ianhd.home.index.controls;
    },
    initShortcuts: function () {
        shortcut.add("r", function () {
            alert('Stay tuned...');
        }, { "disable_in_input": true });

        //shortcut.add("Ctrl+s", function () {
        //  ...
        //}, { "propagate": false });
    },
    initTaglines: function () {
        var taglines = $(".tagline span");
        taglines.attr("title", "Tap 'r' on your keyboard to refresh tagline");

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