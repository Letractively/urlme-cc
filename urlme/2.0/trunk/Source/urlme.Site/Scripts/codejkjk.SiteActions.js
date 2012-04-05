registerNS("codejkjk.SiteActions");

codejkjk.SiteActions = {
    SaveContenteditable: function (elem) {
        var origValElem = elem.next("input[type=hidden]");
        var newVal = elem.is("span") ? elem.text() : elem.val(); // assume select if not span
        var itemId = elem.closest("[data-itemid]").data().itemid; // closest will include itself, if the data-itemid attr is on the elem itself
        var data = {};
        var url = null;

        // set url and data based on whether or not specific ajax url is specified
        var ajaxUrl = elem.attr("ajaxUrl"); // e.g., "/newsletterpage/updatetitle"

        if (ajaxUrl) {

            // use specified ajax url
            var paramName = ajaxUrl.substring(ajaxUrl.indexOf("/update")).replace("/update", "").replace("/", ""); // e.g., title
            data["id"] = itemId;
            data[paramName] = newVal;
            url = ajaxUrl;
        } else {

            // use generic object ajax url
            var typeName = elem.closest("[data-typename]").data().typename;
            var propertyName = elem.data().property;
            data = { itemId: itemId, typeName: typeName, propertyName: propertyName, newValue: newVal };
            url = objectBaseUrl + "setproperty";
        }

        doAjaxPostJson(data, url,
            function (resp) {
                origValElem.val(newVal); $("#contentEditableNote").hide();
            },
            function () {
                // revert to orig db val
                if (elem.is("span")) { elem.text(origValElem.val()); } else { elem.val(origValElem.val()); }
                $("#contentEditableNote").hide();
            }
        );
    },

    WireContenteditables: function () {
        // for each contenteditable, add a hidden input w/ the orig val right next to it, so we can use that to revert later
        $("[contenteditable]").each(function () {
            var ce = $(this);
            var origVal = ce.is("span") ? ce.text() : ce.val(); // assume select if not span
            ce.after("<input type='hidden' value='" + origVal + "' />");
        });

        // fill note
        var noteHtml = "<span class='Note' id='contentEditableNote'>Please hit Enter to save, or Escape to revert.</span>";
        if ($("#NotesContainer").length == 0) {
            $("body").append("<div id='NotesContainer'></div>");
        }
        $("#NotesContainer").append(noteHtml);

        // blur on Enter (which triggers save method), revert on Escape
        $(document).on('keydown', '[contenteditable]', function (e) {
            var elem = $(this);
            var origVal = elem.next("input[type=hidden]").val();

            if (e.keyCode == 13) { // enter
                elem.blur();
                return false;
            } else if (e.keyCode == 27) { // escape
                // reset inner html to orig val
                elem.text(origVal);
                elem.blur();
            }
        });

        // on focus, show note
        $(document).on('focus', '[contenteditable]', function () {
            $("#contentEditableNote").show();
        });

        // on blur, save if value differs from original
        $("[contenteditable]").on('blur', function () {
            var elem = $(this);
            var origVal = elem.next("input[type=hidden]").val();
            var newVal = elem.text();

            if (newVal != origVal) {
                // save this edited element
                Salem.SiteActions.SaveContenteditable(elem);
            } else {
                // no changes made
                $("#contentEditableNote").hide();
            }
        });
    },
};

$(document).ready(function () {
    codejkjk.SiteActions.WireContenteditables();
});