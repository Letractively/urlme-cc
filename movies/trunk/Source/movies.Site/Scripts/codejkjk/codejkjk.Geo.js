﻿registerNS("codejkjk.Geo");

codejkjk.Geo = {
    BaseUrl: "http://api.geonames.org/", // findNearbyPostalCodesJSON?lat=47&lng=9&username=codejkjk&callback=cb
    GetZipCode: function (callback) {
        if (navigator.geolocation) {
            console.log("navigator.geolocation " + navigator.geolocation);
            navigator.geolocation.getCurrentPosition(function (position) {
                console.log("in getCurrentPosition callback");
                var url = "{0}findNearbyPostalCodesJSON?lat={1}&lng={2}&username=codejkjk".format(codejkjk.Geo.BaseUrl, position.coords.latitude, position.coords.longitude);
                $.ajax({
                    url: url,
                    dataType: "jsonp",
                    success: function (resp) { return callback(resp.postalCodes[0].postalCode); }, // todo: check if postalCodes[0]
                    error: function () { return null; }
                });
            }, function (error) {
                // alert("Error: {0}".format(error.code));
                alert("Error, please try again.");
                return callback(localStorage.getItem("ZipCode") || 23226);
            });
        }
    }
};