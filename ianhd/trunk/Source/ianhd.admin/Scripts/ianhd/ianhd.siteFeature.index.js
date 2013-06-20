﻿// $(".Search-Box input").keyup(function (event) { if (event.keyCode == 13) { var idPrefix = "#" + $(this).closest(".Column").attr("id") + " "; $(idPrefix + "#SearchButton").trigger("click"); } });
ianhd.registerNamespace("siteFeature");
ianhd.siteFeature = {
    constants: {
    },
    controls: {
        buttonSet: function () { return $(".buttonset"); }
        , buttonSetRadios: function () { return $(".buttonset input[type='radio']"); }
        , startDate: function () { return $("input[name='NewSiteFeature.StartDate']"); }
        , submit: function () { return $("input[type='submit']"); }
        , whenever: function () { return $("#whenever"); }
    },
    selectors: {
        buttonSetLabels: ".buttonset label"
    },
    init: function () {
        ianhd.siteFeature.bindControls();
        ianhd.siteFeature.initDateOptions();
        ianhd.siteFeature.controls.buttonSet().buttonset(); // todo: siteActions
    },
    initDateOptions: function () {
        // Whenever true? then make it selected in button set

        // else, select Specific days
    },
    bindControls: function () {
        ianhd.siteFeature.controls.buttonSetRadios().change(function () {
            var buttonSet = ianhd.siteFeature.controls.buttonSet();
            var selectedVal = buttonSet.find("input[type='radio']:checked").val();
            console.log(selectedVal);
        });

        // submit form
        ianhd.siteFeature.controls.submit().on('click', function (e) {
            // validate
            var controls = ianhd.siteFeature.controls;

            // can do all below using subscribe, shooooooot.

            //var startDate = controls.startDate();
            //var whenever = viewModel.startWhenever();
            //if (whenever.is(":checked")) {
            //    startDate.val("");
            //}
        });
    }
};

$(function () {
    ianhd.siteFeature.init();
});