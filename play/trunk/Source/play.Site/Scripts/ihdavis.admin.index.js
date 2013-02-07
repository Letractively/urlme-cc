ihdavis.registerNamespace("admin.index");
ihdavis.admin.index = {
    controls: {
        toggleSeatedLinks: function () { return $(".toggleSeated"); }
    }
    , init: function() {
        $("#orders").dataTable({
            "sPaginationType": "full_numbers",
            "bSort": true,
            "bJQueryUI": true,
            "iDisplayLength": 25
        });

        ihdavis.admin.index.bindControls();
    }
    , bindControls: function () {
        ihdavis.admin.index.controls.toggleSeatedLinks().click(function (e) {
            e.preventDefault();
            var link = $(this);
            var itemRow = link.closest("[data-item-id]");
            var seatedCell = itemRow.find(".seatedCell");

            var data = {};
            data.playOrderId = itemRow.attr("data-item-id");

            $.ajax({
                url: toggleSeatedUrl,
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8',
                success: function (resp) {
                    if (resp.success) {
                        // udpate display
                        seatedCell.find("span").toggleClass("hidden");
                        // update link text
                        var stateText = link.find(".stateText");
                        var currentStateText = stateText.text();
                        var states = stateText.attr("data-states").split(",");
                        if (currentStateText == states[0])
                            stateText.text(states[1]);
                        else
                            stateText.text(states[0]);
                    } else {
                        alert("Error. Please try again.");
                    }
                },
                error: function (xhr) {
                    alert("Ajax error. Please try again.");
                }
            });
        });
    }
};

$(function () {
    ihdavis.admin.index.init();
});