// first, some extensions
String.prototype.endsWith = function (suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};

// example: "{0} - {1}".format("hello", "world") results in "hello - world"
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined' ? args[number] : match;
    });
};

/* =ihdavis.registerNamespace */
if (typeof ihdavis === "undefined" || !ihdavis) {
    var ihdavis = {};
}

ihdavis.registerNamespace = function () {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i++) {
        d = a[i].split(".");
        o = ihdavis;

        // ihdavis is implied, so it is ignored if it is included
        for (j = (d[0] == "ihdavis") ? 1 : 0; j < d.length; j++) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};

ihdavis.registerNamespace("ajax");
ihdavis.ajax = {
    post: function (url, jsonObj, success) {
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(jsonObj),
            contentType: 'application/json; charset=utf-8',
            success: function (resp) {
                if (resp.success && typeof success === "function") {
                    success(resp);
                    ihdavis.feedback.showSuccess();
                } else if (!resp.success) {
                    alert("Error. Please try again.");
                }
            },
            error: function (xhr) {
                alert("Ajax error. Please try again.");
            }
        });
    }
    , get: function (url, success) {
        $.get(url, function (resp) {
            if (typeof success === "function") {
                success(resp);
            }
        });
    }
};

ihdavis.registerNamespace("form");
ihdavis.form = {
    emailIsValid: function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }
};

ihdavis.registerNamespace("feedback");
ihdavis.feedback = {
    showSuccess: function () {
        $.growlUI("Success!", null);
    }
};

var clearSelection = function () {
    if (window.getSelection) {
        if (window.getSelection().empty) {  // Chrome
            window.getSelection().empty();
        } else if (window.getSelection().removeAllRanges) {  // Firefox
            window.getSelection().removeAllRanges();
        }
    } else if (document.selection) {  // IE?
        document.selection.empty();
    }
};