function registerNS(ns) {
    var nsParts = ns.split(".");
    var root = window;

    for (var i = 0; i < nsParts.length; i++) {
        if (typeof root[nsParts[i]] == "undefined")
            root[nsParts[i]] = new Object();

        root = root[nsParts[i]];
    }
}

String = {
    format: function () {
        var s = arguments[0];
        for (var i = 0; i < arguments.length - 1; i++) {
            var reg = new RegExp("\\{" + i + "\\}", "gm");
            s = s.replace(reg, arguments[i + 1]);
        }

        return s;
    },
    snippet: function (s, len) {
        if (s.length > len) {
            return String.format("<span alt='{0}' title='{0}'>{1}...</span>", s, s.substring(0, len));
        }
        return s;
    }
}

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

Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
};
    
Date.prototype.toFormat = function (format) {
    var d = this;
    var yyyy = this.getFullYear();
    var MM = (this.getMonth() + 1) < 10 ? "0" + this.getMonth() + 1 : this.getMonth() + 1;
    var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();

    switch (format) {
        case "yyyyMMdd":
            return yyyy.toString() + MM.toString() + dd.toString();
            break;
    }
};

// orig:
//Date.prototype.addDays = function(days)
// {
//     var dat = new Date(this.valueOf())
//     dat.setDate(dat.getDate() + days);
//     return dat;
// }