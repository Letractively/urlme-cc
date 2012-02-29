﻿registerNS("codejkjk.urlme.Default");

codejkjk.urlme.Default = {
    Init: function () {
        codejkjk.urlme.Default.BindDeleteLinks();
        codejkjk.urlme.Default.WireCopyLinks();
    },

    WireCopyLinks: function () {
        $(".copyButton").each(function () {
            var copyButton = $(this);
            var copyButtonId = copyButton.attr("id");
            var textToCopy = copyButton.closest("tr").find(".copyText").text();

            // for now, say "not working yet"
            copyButton.click(function (e) {
                e.preventDefault();
                alert("not working yet");
            });

            // the following makes the browser extremely slow :/ disabling for now
            //            var clip = new ZeroClipboard.Client();
            //            clip.setText(textToCopy);
            //            clip.glue(copyButtonId);
            //            clip.addEventListener('complete', function (client, text) {
            //                copyButton.text("✔ Copied!")
            //                setTimeout(function() {
            //                    copyButton.text("Copy urlme");
            //                }, 2500);
            //            });
        });
    },

    BindDeleteLinks: function () {
    }
};

$(document).ready(function () {
    codejkjk.urlme.Default.Init();
});