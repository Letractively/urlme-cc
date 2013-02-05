ihdavis.registerNamespace("order.index");
ihdavis.order.index = {
    controls: {
        coupleCount: function () { return $(ihdavis.order.index.controls.envClassPrefix() + "coupleCount").first(); }
        , coupleTotal: function () { return $(ihdavis.order.index.controls.envIdPrefix() + "coupleTotal"); }
        , email: function () { return ihdavis.order.index.controls.order().find("input[name='email']"); }
        , envClassPrefix: function () { return ihdavis.order.index.controls.order().hasClass("m") ? ".m_" : ".dt_"; }
        , envIdPrefix: function () { return ihdavis.order.index.controls.order().hasClass("m") ? "#m_" : "#dt_"; }
        , hidAmount: function () { return $("input[name='amount']"); }
        , indivCount: function () { return $(ihdavis.order.index.controls.envClassPrefix() + "individualCount").first(); }
        , indivTotal: function () { return $(ihdavis.order.index.controls.envIdPrefix() + "individualTotal"); }
        , name: function() { return ihdavis.order.index.controls.order().find("input[name='name']"); }
        , order: function () { return $(".orderIndex.m").is(":visible") ? $(".orderIndex.m") : $(".orderIndex.dt"); }
        , payButton: function () { return ihdavis.order.index.controls.order().find(".payButton form"); }
        , playDate: function() { return ihdavis.order.index.controls.order().find("input[name='playDate']:checked"); }
    }
    , calculatedVals: {
        ticketTotal: function () { return parseInt(ihdavis.order.index.controls.coupleTotal().text()) + parseInt(ihdavis.order.index.controls.indivTotal().text()); }
        , ticketCount: function () { return parseInt(ihdavis.order.index.controls.coupleCount().text()) + parseInt(ihdavis.order.index.controls.indivCount().text()); }
    }
    , init: function() {
        $(".buttonset").buttonset(); // todo: siteActions
        ihdavis.order.index.bindControls();
    }
    , showHidePayButton: function () {
        var name = $.trim(ihdavis.order.index.controls.name().val())
            , email = $.trim(ihdavis.order.index.controls.email().val())
            , playDate = ihdavis.order.index.controls.playDate().val()
            , ticketCount = ihdavis.order.index.calculatedVals.ticketCount()
            , ticketTotal = ihdavis.order.index.calculatedVals.ticketTotal()
            , payButton = ihdavis.order.index.controls.payButton();
        if (name && email && playDate && ticketCount) {
            payButton.show();
            
            // update hidAmount
            ihdavis.order.index.controls.hidAmount().val(ticketTotal);
            ihdavis.order.index.controls.hidAmount().val("0.01");
        }
        else {
            payButton.hide();
        }
    }
    , bindControls: function () {
        // change play date, deselect all selections in document
        $("input[name='playDate']").change(function () {
            document.getSelection().removeAllRanges();
            ihdavis.order.index.showHidePayButton();
        });

        // keyup on name textbox - show/hide payButton
        $("input[name='name']").keyup(function () {
            ihdavis.order.index.showHidePayButton();
        });

        // click a counter up/down (+/-) button
        $(".countButton").click(function (e) {
            e.preventDefault();
            var btn = $(this);
            var countUp = btn.hasClass("countUp");
            var counter = btn.closest(".counter");
            var display = $("." + counter.attr("data-display-class"));
            var total = $("#" + counter.attr("data-total-id"));
            var pricePer = parseInt($("#" + counter.attr("data-price-per-id")).text());
            var currentCount = parseInt(display.first().text());
            if (!currentCount && !countUp)
                return; // zero. can't get lower than zero.
            if (countUp) {
                currentCount++;
            } else {
                currentCount--;
            }
            display.text(currentCount);
            total.text(currentCount * pricePer);
            ihdavis.order.index.showHidePayButton();
        });
    }
};

$(function () {
    ihdavis.order.index.init();
});