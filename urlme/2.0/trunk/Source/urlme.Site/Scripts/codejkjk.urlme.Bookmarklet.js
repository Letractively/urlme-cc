registerNS("codejkjk.urlme.Bookmarklet");

codejkjk.urlme.Bookmarklet = {
    Init: function () {
        $("#NewPath").focus();
        codejkjk.urlme.Bookmarklet.BindForm();
    },
    BindForm: function () {
        $("#CancelAndClose").click(function () {
            window.close();
        });
        $("#AddAndClose").click(function () { codejkjk.urlme.Bookmarklet.AddAndCloseClicked(); });
    },
    AddAndCloseClicked: function () {
        // todo: VALIDATE
        var url = bookmarkletServiceUrl;
        var newPath = $("#NewPath").val();
        var newDestinationUrl = $("#NewDestinationUrl").val();
        var data = { newPath: newPath, newDestinationUrl: newDestinationUrl };
        $.ajax({
            url: url,
            data: data,
            type: "POST",
            timeout: 5000,
            dataType: "json",
            success: function (resp) { codejkjk.urlme.Bookmarklet.AddAndClose_Callback(resp.Feedback); },
            error: function () { alert("error"); }
        });
    },
    AddAndClose_Callback: function (feedback) {
        if (feedback.indexOf("Error") >= 0 || feedback.indexOf("Fail") >= 0) {
            $("#Error").html(feedback);
            setTimeout(function () { $("#Error").fadeOut(250); }, 4000);
        } else {
            window.close();
        }
    }
};

$(document).ready(function () {
    codejkjk.urlme.Bookmarklet.Init();
});