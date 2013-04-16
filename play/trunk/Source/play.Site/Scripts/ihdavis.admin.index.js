﻿/* constants */
var constants = {
    actionBaseUrl: "/order/"
};

ihdavis.registerNamespace("admin.index");
ihdavis.admin.index = {
    controls: {
        moreInfo: function () { return $("#moreInfo"); }
        , moreInfoLinks: function () { return $(".moreInfo"); }
        , newCount: function () { return $("#newCount"); }
        , newNotif: function () { return $("#notif"); }
        , newPlural: function () { return $("#newPlural"); }
        , sendConfirmation: function () { return $(".sendConfirmation"); }
        , search: function () { return $("#orders_filter input"); }
        , toggleLinks: function () { return $("a[href*='toggle']"); }
        , totalFilters: function () { return $(".header a"); }
        , deleteLinks: function () { return $("a[href*='delete']"); }
    }
    , init: function() {
        $("#orders").dataTable({
            "sPaginationType": "full_numbers",
            "bSort": true,
            "bJQueryUI": true,
            "iDisplayLength": 25
        });

        ihdavis.admin.index.controls.search()
            .attr("data-intro", "Remember, you can press 'q' and filter on the orders")
            .attr("data-step", "1");

        setTimeout(function () {
            if (!localStorage.getItem("introRan")) {
                introJs().start();
                localStorage.setItem("introRan", "true, on " + new Date());
            }
        }, 1000);

        shortcut.add("q", function () {
            ihdavis.admin.index.controls.search().focus();
        }, { "disable_in_input": true });

        ihdavis.admin.index.initCountChecks();

        ihdavis.admin.index.bindControls();

        ihdavis.admin.index.controls.moreInfo().dialog({ autoOpen: false, modal: true });
    }
    , initCountChecks: function () {
        setInterval(function () {
            ihdavis.ajax.get(constants.actionBaseUrl + "getcount", function (resp) {
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
        ihdavis.admin.index.controls.totalFilters().click(function (e) {
            e.preventDefault();
            var link = $(this);
            var seats = link.parent().next().find(".seatCount");
            var total = link.parent().next().find(".orderTotal");
            link.parent().find("a").removeClass("active");
            link.addClass("active");
            var txt = link.text();
            if (txt === "Both") {
                seats.text(bothSeatCount);
                total.text(bothOrderTotal);
            }
            else if (txt === "Saturday") {
                seats.text(satSeatCount);
                total.text(satOrderTotal);
            } else {
                seats.text(sunSeatCount);
                total.text(sunOrderTotal);
            }
        });

        ihdavis.admin.index.controls.moreInfoLinks().click(function (e) {
            e.preventDefault();
            ihdavis.ajax.get(constants.actionBaseUrl + $(this).attr("href"), function (resp) {
                resp.MailTo = "mailto:" + resp.Email;
                resp.ResendConfText = "Resend confirmation to " + resp.Name;
                ko.mapping.fromJS(resp, viewModel);
                var dialog = ihdavis.admin.index.controls.moreInfo();
                dialog.dialog('option', 'title', resp.Name).dialog('open');
                dialog.find("a").blur(); // this is annoying to have to un-auto focus
            });
        });

        ihdavis.admin.index.controls.deleteLinks().click(function (e) {
            e.preventDefault();
            var secret = prompt("A(n) ______ gave way to Team ShariIan for Nerts the first time @ Shockoe.");
            if (secret) {
                var link = $(this);
                var itemRow = link.closest("[data-item-id]");
                var data = { secret: secret, playOrderId: itemRow.attr("data-item-id") };

                ihdavis.ajax.post(constants.actionBaseUrl + "delete", data, function () {
                    itemRow.remove();
                    orderCount--;
                });
            }
        });

        $('.ui-widget-overlay').click(function () { $("#moreInfo").dialog("close"); });

        ihdavis.admin.index.controls.sendConfirmation().click(function (e) {
            e.preventDefault();
            var el = $(this);
            var itemId = el.closest("[data-item-id]").attr("data-item-id")
                , ajaxUrl = constants.actionBaseUrl + el.attr("href")
                , data = { playOrderId: itemId };

            ihdavis.ajax.post(ajaxUrl, data);
        });

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
                , ajaxUrl = constants.actionBaseUrl + link.attr("href");

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