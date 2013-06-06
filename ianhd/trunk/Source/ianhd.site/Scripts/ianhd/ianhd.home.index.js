ianhd.registerNamespace("home.index");
ianhd.home.index = {
    controls: {
    },
    init: function () {
        ianhd.home.index.initTaglines();
    },
    initTaglines: function () {
        var taglines = $(".tagline span");
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