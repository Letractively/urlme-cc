function registerNS(ns) {
    var nsParts = ns.split(".");
    var root = window;

    for (var i = 0; i < nsParts.length; i++) {
        if (typeof root[nsParts[i]] == "undefined")
            root[nsParts[i]] = new Object();

        root = root[nsParts[i]];
    }
}

registerNS("codejkjk.feedback");
codejkjk.feedback = {
    showSuccess: function () {
        $.growlUI("Success!", null);
    }
//    ,
//    clearMessage: function () {
//        $("#message").removeClass("success").removeClass("failure").hide();
//    },
//    errorMessage: function (html) {
//        $("#message").addClass("failure").removeClass("success").html(html).show();
//    },
//    successMessage: function (html) {
//        $("#message").addClass("success").removeClass("failure").html(html).show();
//    }
};

registerNS("codejkjk.browserCompatability");
codejkjk.browserCompatability = {
    testAttribute: function (element, attribute) {
        var test = document.createElement(element);
        return attribute in test;
    }
};

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined' ? args[number] : match;
    });
};

String.prototype.snippet = function (len) {
    var s = this;
    if (s.length > len) {
        return "<span alt='{0}' title='{0}'>{1}...</span>".format(s, s.substring(0, len));
    }
    return s;
};

//String = {
//    snippet: function (s, len) {
//        if (s.length > len) {
//            return "<span alt='{0}' title='{0}'>{1}...</span>".format(s, s.substring(0, len));
//        }
//        return s;
//    }
//}

function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

Array.prototype.removeByElement = function (elem) {
    var arr = this;
    $.each(arr, function (i, arrElem) {
        if (arrElem == elem) {
            arr.splice(i, 1);
        }
    });
};

Array.prototype.pushIfDoesNotExist = function (elem) {
    if (this.indexOf(elem) == -1) {
        this.push(elem);
    }
};
