// $(".Search-Box input").keyup(function (event) { if (event.keyCode == 13) { var idPrefix = "#" + $(this).closest(".Column").attr("id") + " "; $(idPrefix + "#SearchButton").trigger("click"); } });
ianhd.registerNamespace("siteFeature.index");
ianhd.siteFeature.index = {
    constants: {
        noEndDateKey: "noEndDate"
        , startWheneverKey: "startWhenever"
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
        // submit form
        ianhd.siteFeature.index.controls.submit().on('click', function (e) {
            // validate
            var controls = ianhd.siteFeature.index.controls;
            var startDate = controls.startDate();
            var whenever = viewModel.startWhenever();
            if (whenever.is(":checked")) {
                startDate.val("");
            }
        });
    }
};

$(function () {
    ianhd.siteFeature.index.init();
});