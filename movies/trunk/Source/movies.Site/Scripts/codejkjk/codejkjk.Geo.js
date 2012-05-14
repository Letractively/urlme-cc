﻿registerNS("codejkjk.Geo");

codejkjk.Geo = {
    BaseUrl: "http://api.geonames.org/", // findNearbyPostalCodesJSON?lat=47&lng=9&username=codejkjk&callback=cb
    GetZipCodeFromLatLong: function (lat, long, callback) {
        var url = "{0}findNearbyPostalCodesJSON?lat={1}&lng={2}&username=codejkjk".format(codejkjk.Geo.BaseUrl, lat, long);
        $.ajax({
            url: url,
            dataType: "jsonp",
            success: function (resp) { return callback(resp.postalCodes[0].postalCode); }, // todo: check if postalCodes[0]
            error: function () { return null; }
        });
    },
    GetZipCode: function (callback) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
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
    },
    GetLatLongFromZip: function (zip, callback) {
        var url = "{0}postalCodeSearchJSON?postalCode={1}&username=codejkjk".format(codejkjk.Geo.BaseUrl, zip);
        $.ajax({
            url: url,
            dataType: "jsonp",
            success: function (resp) {
                var lat = resp.postalCodes[0].lat;
                var long = resp.postalCodes[0].lng;
                return callback(lat, long); 
            }, // todo: check if postalCodes[0]
            error: function () { return null; }
        });        
    },
    GetLatLong: function (callback) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                return callback(position.coords.latitude, position.coords.longitude);
            }, function (error) {
                // alert("Error: {0}".format(error.code));
                alert("Error, please try again.");
                return callback(null, null);
            });
        }
    }
};