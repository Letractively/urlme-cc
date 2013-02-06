ihdavis.registerNamespace("admin.index");
ihdavis.admin.index = {
    controls: {
        markAsSeatedLinks: function () { return $(".markAsSeated"); }
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
        ihdavis.admin.index.controls.markAsSeatedLinks().click(function (e) {
            e.preventDefault();
            var link = $(this);
            var seatedCell = link.closest("td.seated");

            var data = {};
            data.playOrderId = link.closest("[data-item-id]").attr("data-item-id");

            $.ajax({
                url: markAsSeatedUrl,
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8',
                success: function (resp) {
                    if (resp.success) {
                        window.location.reload();
                        //seatedCell.text("<span class=\"active\">✔</span>");
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