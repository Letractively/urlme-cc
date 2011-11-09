﻿registerNS("codejkjk.Geo");

codejkjk.Geo = {
    BaseUrl: "http://api.geonames.org/", // findNearbyPostalCodesJSON?lat=47&lng=9&username=codejkjk&callback=cb
    GetPostalCode: function (callback) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var url = String.format("{0}findNearbyPostalCodesJSON?lat={1}&lng={2}&username=codejkjk", codejkjk.Geo.BaseUrl, position.coords.latitude, position.coords.longitude);
                $.ajax({
                    url: url,
                    dataType: "jsonp",
                    success: function (resp) { return callback(resp.postalCodes[0].postalCode); }, // todo: check if postalCodes[0]
                    error: function () { return null; }
                });
            });
        }
    }
};