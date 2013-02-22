ihdavis.registerNamespace("admin.index");
ihdavis.admin.index = {
    controls: {
        moreInfo: function () { return $("#moreInfo"); }
        , moreInfoLinks: function () { return $(".moreInfo"); }
        , newCount: function () { return $("#newCount"); }
        , newNotif: function () { return $("#notif"); }
        , newPlural: function () { return $("#newPlural"); }
        , toggleLinks: function () { return $("a[href*='toggle']"); }
    }
    , init: function() {
        $("#orders").dataTable({
            "sPaginationType": "full_numbers",
            "bSort": true,
            "bJQueryUI": true,
            "iDisplayLength": 25
        });

        ihdavis.admin.index.initCountChecks();

        ihdavis.admin.index.bindControls();

        ihdavis.admin.index.controls.moreInfo().dialog({ autoOpen: false, modal: true });
    }
    , initCountChecks: function () {
        setInterval(function () {
            ihdavis.ajax.get("/order/getcount", function (resp) {
                if (resp.count > orderCount) {
                    var overflow = resp.count - orderCount;
                    var newPluralText = overflow > 1 ? "(s)" : "";
                    var controls = ihdavis.admin.index.controls;
                    controls.newCount().text(overflow);
                    controls.newPlural().text(newPluralText);
                    controls.newNotif().show();
                }
            });
        }, 5000);
    }
    , bindControls: function () {
        ihdavis.admin.index.controls.moreInfoLinks().click(function (e) {
            e.preventDefault();
            ihdavis.ajax.get($(this).attr("href"), function (resp) {
                resp.MailTo = "mailto:" + resp.Email;
                ko.mapping.fromJS(resp, viewModel);
                ihdavis.admin.index.controls.moreInfo().dialog('open');
            });
        });

        $('.ui-widget-overlay').click(function () { $("#moreInfo").dialog("close"); });

        ihdavis.admin.index.controls.toggleLinks().click(function (e) {
            e.preventDefault();
            var link = $(this);
            var icon = link.find(".icon")
                , stateText = link.find(".stateText")
                , itemRow = link.closest("[data-item-id]")
                , display = itemRow.find("." + link.attr("data-display-class"))
                , currentStateText = stateText.text()
                , states = link.attr("data-states").split(",")
                , iconClasses = link.attr("data-icon-classes").split(",")
                , ajaxUrl = link.attr("href");

            var data = {};
            data.playOrderId = itemRow.attr("data-item-id");

            ihdavis.ajax.post(ajaxUrl, data, function (resp) {
                // udpate display & link text
                display.find("span").toggleClass("hidden");
                icon.toggleClass(iconClasses[0]).toggleClass(iconClasses[1]);
                if (currentStateText == states[0])
                    stateText.text(states[1]);
                else
                    stateText.text(states[0]);
            });
        });
    }
};

$(function () {
    ihdavis.admin.index.init();
});