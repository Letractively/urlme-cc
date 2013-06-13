// $(".Search-Box input").keyup(function (event) { if (event.keyCode == 13) { var idPrefix = "#" + $(this).closest(".Column").attr("id") + " "; $(idPrefix + "#SearchButton").trigger("click"); } });
ianhd.registerNamespace("siteFeature.index");
ianhd.siteFeature.index = {
    constants: {
        startWheneverKey: "startWhenever"
    },
    controls: {
        startDate: function () { return $("input[name='NewSiteFeature.StartDate']"); }
        , submit: function () { return $("input[type='submit']"); }
        , whenever: function () { return $("#whenever"); }
    },
    init: function () {
        ianhd.siteFeature.index.bindControls();
    },
    bindControls: function () {
        // change "starting whenever" checkbox
        //ianhd.siteFeature.index.controls.whenever().on('change', function () {
        //    var startWheneverKey = ianhd.siteFeature.index.constants.startWheneverKey;
        //    var controls = ianhd.siteFeature.index.controls;
        //    var chk = $(this);
        //    if (chk.is(":checked")) {
        //        controls.startDate().attr("disabled", "disabled");
        //        localStorage.setItem(startWheneverKey, "true");
        //    } else {
        //        controls.startDate().removeAttr("disabled");
        //        localStorage.setItem(startWheneverKey, "false");
        //    }
        //});

        // submit form
        ianhd.siteFeature.index.controls.submit().on('click', function (e) {
            // validate
            var controls = ianhd.siteFeature.index.controls;
            var startDate = controls.startDate();
            var whenever = controls.whenever();
            if (whenever.is(":checked")) {
                startDate.val("");
            }
        });
    }
};

$(function () {
    ianhd.siteFeature.index.init();
});