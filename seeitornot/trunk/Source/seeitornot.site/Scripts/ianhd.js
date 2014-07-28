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

String.prototype.isValidEmail = function () {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(this);
}

String.prototype.stripHtml = function () {
    return $.trim(this.replace(/<(?:.|\n)*?>/gm, ''));
}

// ex: (new Date).addHours(4) gives now plus 4 hours
Date.prototype.addHours = function (h) {
    this.setHours(this.getHours() + h);
    return this;
}

/* =ianhd.registerNamespace */
if (typeof ianhd === "undefined" || !ianhd) {
    var ianhd = {};
}

ianhd.registerNamespace = function () {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i++) {
        d = a[i].split(".");
        o = ianhd;

        // ianhd is implied, so it is ignored if it is included
        for (j = (d[0] == "ianhd") ? 1 : 0; j < d.length; j++) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};

ianhd.registerNamespace("ajax");
ianhd.ajax = {
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
                    ianhd.feedback.showSuccess();
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

ianhd.registerNamespace("pluginHelpers.ko");
ianhd.pluginHelpers.ko = {
    bind: function (vm) {
        ko.applyBindings(vm);
    }
	, getViewModel: function (data) {
	    return ko.mapping.fromJS(data);
	}
	, refresh: function (vm, data) {
	    ko.mapping.fromJS(data, vm);
	}
};

ianhd.registerNamespace("feedback");
ianhd.feedback = {
    showSuccess: function () {
        $.growlUI("Success!", null);
    }
};

ianhd.registerNamespace("math");
ianhd.math = {
    getRandomFromZeroThru: function (x) {
        return Math.floor(Math.random() * (x + 1));
    }
};

// not sure if this is used or what it's for, but at one point it was probably for something awesome.
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