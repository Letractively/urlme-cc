ihdavis.registerNamespace("order.index");
ihdavis.order.index = {
    controls: {
        coupleCount: function () { return $(ihdavis.order.index.controls.envClassPrefix() + "coupleCount").first(); }
        , coupleTotal: function () { return $(ihdavis.order.index.controls.envIdPrefix() + "coupleTotal"); }
        , countButtons: function () { return $(".countButton"); }
        , email: function () { return ihdavis.order.index.controls.order().find("input[name='email']"); }
        , envClassPrefix: function () { return ihdavis.order.index.controls.order().hasClass("m") ? ".m_" : ".dt_"; }
        , envIdPrefix: function () { return ihdavis.order.index.controls.order().hasClass("m") ? "#m_" : "#dt_"; }
        , formSubmitButton: function () { return ihdavis.order.index.controls.payButton().find("input[name='submit']"); }
        , hidAmount: function () { return $("input[name='amount']"); }
        , hidItemId: function () { return $("input[name='custom']"); }
        , howDidYouHear: function () { return ihdavis.order.index.controls.order().find("select[name='howDidYouHear']"); }
        , indivCount: function () { return $(ihdavis.order.index.controls.envClassPrefix() + "individualCount").first(); }
        , indivTotal: function () { return $(ihdavis.order.index.controls.envIdPrefix() + "individualTotal"); }
        , name: function () { return ihdavis.order.index.controls.order().find("input[name='name']"); }
        , order: function () { return $(".orderIndex.m").is(":visible") ? $(".orderIndex.m") : $(".orderIndex.dt"); }
        , payButton: function () { return ihdavis.order.index.controls.order().find(".payButton form"); }
        , platform: function () { return ihdavis.order.index.controls.order().hasClass("m") ? "mobile" : "desktop"; }
        , playDate: function() { return ihdavis.order.index.controls.order().find("input[name='playDate']:checked"); }
    }
    , calculatedVals: {
        ticketTotal: function () { return parseInt(ihdavis.order.index.controls.coupleTotal().text()) + parseInt(ihdavis.order.index.controls.indivTotal().text()); }
        , ticketCount: function () { return parseInt(ihdavis.order.index.controls.coupleCount().text()) + parseInt(ihdavis.order.index.controls.indivCount().text()); }
    }
    , init: function() {
        if (!ihdavis.order.index.controls.order().hasClass("iPhone") || ihdavis.order.index.controls.order().hasClass("dt")) {
            $(".buttonset").buttonset(); // todo: siteActions
        }
        ihdavis.order.index.bindControls();
    }
    , resetPayButton: function () {
        var name = $.trim(ihdavis.order.index.controls.name().val())
            , email = $.trim(ihdavis.order.index.controls.email().val())
            , playDate = ihdavis.order.index.controls.playDate().val()
            , ticketCount = ihdavis.order.index.calculatedVals.ticketCount()
            , ticketTotal = ihdavis.order.index.calculatedVals.ticketTotal()
            , payButton = ihdavis.order.index.controls.payButton()
            , emailEl = ihdavis.order.index.controls.email();

        if (email && !ihdavis.form.emailIsValid(email) && !emailEl.is(":focus")) {
            emailEl.css("border-color", "red");
        } else {
            emailEl.css("border-color", "#ccc");
        }

        if (name && email && playDate && ticketCount) {
            payButton.show();
            
            // update hidAmount
            ihdavis.order.index.controls.hidAmount().val(ticketTotal);
            if (name.toLowerCase() === "test")
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
            ihdavis.order.index.resetPayButton();
        });

        ihdavis.order.index.controls.formSubmitButton().click(function (e) {
            e.preventDefault();

            var payButtonForm = ihdavis.order.index.controls.payButton();
            if (payButtonForm.hasClass("working")) {
                alert("Please only click the button once.");
                return;
            }
            payButtonForm.addClass("working");

            // first, submit the order in our db so we can get the order id to send along w/ paypal payment
            var data = {};
            data.Name = $.trim(ihdavis.order.index.controls.name().val());
            data.Email = $.trim(ihdavis.order.index.controls.email().val());
            data.CoupleTicketCount = parseInt(ihdavis.order.index.controls.coupleCount().text());
            data.IndividualTicketCount = parseInt(ihdavis.order.index.controls.indivCount().text());
            data.PlayDate = ihdavis.order.index.controls.playDate().val();
            data.UserAgent = userAgent;
            data.Platform = ihdavis.order.index.controls.platform();
            data.HowDidYouHear = ihdavis.order.index.controls.howDidYouHear().find("option:selected").val();

            $.ajax({
                url: submitOrderUrl,
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8',
                success: function (resp) {
                    if (resp.orderId > 0) {
                        ihdavis.order.index.controls.hidItemId().val(resp.orderId);
                        ihdavis.order.index.controls.payButton().submit(); // now that we've set the orderId for the IPN callback, we can submit the paypal form
                    } else {
                        alert("Error. Please try again.");
                    }
                },
                error: function (xhr) {
                    alert("Ajax error. Please try again.");
                }
            });
        });

        // keyup on name textbox - show/hide payButton
        $("input[name='name'],input[name='email']").keyup(function () {
            ihdavis.order.index.resetPayButton();
        });
        $("input[name='email']").blur(function () {
            ihdavis.order.index.resetPayButton();
        });

        // click a counter up/down (+/-) button
        ihdavis.order.index.controls.countButtons().click(function (e) {
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
            ihdavis.order.index.resetPayButton();
        });
    }
};

$(function () {
    ihdavis.order.index.init();
});