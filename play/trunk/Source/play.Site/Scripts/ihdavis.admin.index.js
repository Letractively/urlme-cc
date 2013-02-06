ihdavis.registerNamespace("admin.index");
ihdavis.admin.index = {
    controls: {
        
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

    }
};

$(function () {
    ihdavis.admin.index.init();
});