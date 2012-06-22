/*
TABLE OF CONTENTS

=salem.registerNamespace
=salem.ajax.service
=salem.string
=salem.sounds
and more...

*/

/* =salem.registerNamespace */
if (typeof f == "undefined" || !salem) {
    var salem = {};
}

salem.registerNamespace = function () {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i++) {
        d = a[i].split(".");
        o = salem;

        // salem is implied, so it is ignored if it is included
        for (j = (d[0] == "salem") ? 1 : 0; j < d.length; j++) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};

// example: "{0} - {1}".format("hello", "world") results in "hello - world"
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined' ? args[number] : match;
    });
};

/* =search.cookie */
salem.registerNamespace("search.cookie");
salem.search.cookie = function (value) {
    var cookieName = "search.cookie";
    if (typeof value != 'undefined') {
        // setter
        $.cookie(cookieName, value);
    } else {
        // getter
        var ret = "";
        if ($.cookie(cookieName) != null)
            ret = $.cookie(cookieName);

        // null out the cookie
        $.cookie(cookieName, null);

        // set cookie to blank after get
        return ret;
    }
};

salem.bind = function (obj, fun, args) {
    return function () {
        if (obj === true)
            obj = this;
        var f = typeof fun === "string" ? obj[fun] : fun;

        return f.apply(obj, Array.prototype.slice.call(args || [])
        .concat(Array.prototype.slice.call(arguments)));
    };
}

/* =salem.ajax.service */
salem.registerNamespace("ajax.service");
salem.ajax.service = function AjaxService(serviceUrl) {
    var Instance = this;
    this.serviceUrl = serviceUrl;
    //Get Wrapper
    this.JsonGet = function (method, data, callback, error) {
        this.Invoke("GET", method, data, callback, error, false);
    };
    //Post Wrapper
    this.JsonPost = function (method, data, callback, error) {
        this.Invoke("POST", method, data, callback, error, false);
    };
    //Call a wrapped object
    this.Invoke = function (action, method, data, callback, error, bare) {
        //The service endpoint URL        
        var url = Instance.serviceUrl + method;
        $.ajax({
            url: url,
            data: data,
            type: action,
            processData: true,
            contentType: "application/json",
            timeout: 5000,
            dataType: "json",
            success: function (response) {
                if (!bare) {
                    for (var property in response) {
                        //WCF returns all json as a wrapped object. This drops the outer layer that isn't needed if it exists
                        response = response[property];
                        break;
                    }
                }
                //Interpret JSON objects set the response as that object
                //We do this verses Eal because tha could result in an unsafe execution of code
                response = JSON.parse(response);
                //Return the object
                callback(response);
            },
            error: error
        });
    }
}

/* =salem.string */
salem.registerNamespace("string");
salem.string = {

    isNullOrEmpty: function () {
        var ret = false;
        for (var i = 0; i < arguments.length; i++) {
            if (ret == false && (arguments[i] == null || salem.string.trim(arguments[i].toString()).length == 0)) { ret = true; }
        }
        return ret;
    },

    trim: function (str) {
        return str.replace(/^\s+|\s+$/g, "");
    }

};

/* =salem.Validation */
salem.registerNamespace("validation");
salem.validation = {
    isValidEmail: function (email) {
        var reg = new RegExp('^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$');
        return reg.test(email);
    }
};

/* =salem.sounds */
salem.registerNamespace("sound");
salem.sound = {
    Play: function (sound, debug) {
        var embed = document.getElementById('__SoundFrame');
        if (embed != null) {
            embed.parentNode.removeChild(embed);
        }

        embed = document.createElement('embed');
        embed.setAttribute('id', '__SoundFrame');
        embed.setAttribute('src', sound);
        embed.setAttribute('loop', 'false');
        embed.setAttribute('volume', '200');
        embed.setAttribute('autostart', 'true');
        if (debug == null || debug == false) {
            embed.setAttribute('hidden', 'true');
        }
        document.body.appendChild(embed);
    }
};

/* =salem.animation */
salem.registerNamespace("animation");
salem.animation = {
    HighlightThenFade: function (elem, postFadeBgColor) {
        elem.removeClass("Highlight");
        elem.addClass("Highlight");

        setTimeout(function () {
            elem.animate({ backgroundColor: postFadeBgColor }, 500);
        }, 3000);
    }
};

/* =salem.browserCompatability */
salem.registerNamespace("browserCompatability");
salem.browserCompatability = {
    // e.g., if (!salem.browserCompatability.testAttribute('input', 'placeholder')) { salem.form.smartTextBox($("input[type='text']"),'placeholder'); }
    testAttribute: function (element, attribute) {
        var test = document.createElement(element);
        return attribute in test;
    }
};

/* =salem.form */
salem.registerNamespace("form");
salem.form = {
    // e.g., smartTextBox($(".input[type='text']"), "blurred")
    smartTextBox: function (elements, blurredClass) {
        $.each(elements, function (i, element) {
            var elem = $(element);
            var defaultVal = elem.attr("placeholder") || elem.val();
            elem.val(defaultVal); // set default val
            elem.addClass(blurredClass)
                .focus(function () {
                    $(this).val("").removeClass(blurredClass);
                })
                .blur(function () {
                    if ($(this).val() == defaultVal || $(this).val() == "")
                        $(this).val(defaultVal).addClass(blurredClass);
                });
        });
    }
}