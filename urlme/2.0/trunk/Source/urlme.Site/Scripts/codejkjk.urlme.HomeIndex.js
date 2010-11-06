registerNS("codejkjk.urlme.HomeIndex");

codejkjk.urlme.HomeIndex = {
    Init: function () {
        $("#Path").focus();
        codejkjk.urlme.HomeIndex.BindTableActions();
        codejkjk.urlme.HomeIndex.InitDataTable();
        codejkjk.urlme.HomeIndex.HandleFeedback();
    },
    BindTableActions: function () {
        $(".delete-link").click(function () {
            return confirm("Are you sure?");
        });
    },
    InitDataTable: function () {
        $("#Links_DISABLE").dataTable({
            "bPaginate": true,
            "bLengthChange": true,
            "bFilter": false,
            "bSort": false,
            "bInfo": true,
            "bAutoWidth": true,
            "bJQueryUI": true,
            "sPaginationType": "full_numbers"
        });
    },
    HandleFeedback: function () {
        if (feedback != "") {
            if (feedback.indexOf("Error") >= 0 || feedback.indexOf("Fail") >= 0) {
                $("#Error").html(feedback);
                setTimeout(function () { $("#Error").fadeOut(250); }, 4000);
            } else {
                $.growlUI(feedback, null);
            }
        }
    }
}

$(document).ready(function () {
    codejkjk.urlme.HomeIndex.Init();
});