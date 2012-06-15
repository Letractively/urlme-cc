salem.registerNamespace("siteActions");
salem.siteActions = {
    // make html 5 features work in browsers that don't support it innately, like IE
    html5ize: function () {
        if (!salem.browserCompatability.testAttribute('input', 'placeholder')) {
            salem.form.smartTextBox($("input[placeholder]"), 'html5placeholder');
        }
    },

    wireSortables: function () {
        $("[contenteditable]").mousedown(function (e) {
            e.stopPropagation();
        });

        // construct save order note that the user will click on when sortupdate occurs
        var noteHtml = "<span class='Note' id='SaveOrRevertNote'><a href='#' id='SaveLink'>Save</a><span class='divider'>|</span><a href='#' class='revertLink'>Revert</a></span>";
        if ($("#NotesContainer").length == 0) {
            $("body").append("<div id='NotesContainer'></div>");
        }
        $("#NotesContainer").append(noteHtml);

        // handle save sort order link
        $(document).on('click', '#SaveLink', function (e) {
            e.preventDefault();

            var idsInOrder = [];
            $("[data-itemid]").each(function (i, item) {
                item = $(item);
                var id = item.attr("data-itemid");
                idsInOrder.push(id);
            });

            // use generic object ajax url
            var typeName = $("[data-typename]:first").data().typename;
            var url = objectBaseUrl + "setsortorders";
            var data = { itemIdsInOrder: idsInOrder.join(','), typeName: typeName };

            salem.ajax.doAjaxPost(data, url,
                function (resp) {
                    $("#SaveOrRevertNote").hide();
                },
                function () {
                    // revert to orig db val
                    alert('Error'); // need to make this nicer and less annoying.
                }
            );
        });

        $("ul.sortable").each(function () {
            var ul = $(this);
            var hideNoteOnUpdate = ul.hasClass("hideNoteOnUpdate");
            var useHandle = ul.hasClass("useHandle");
            var usePlaceholder = ul.hasClass("usePlaceholder");

            ul.sortable({
                update: function (event, ui) {
                    if (!hideNoteOnUpdate) {
                        $(ui.item).addClass("ChangedItem");
                    }
                },
                forcePlaceholderSize: 'true',
                items: "li:not(.ui-state-disabled)"
            });

            // connectwith specified?
            var connectwith = ul.data().connectwith;
            if (connectwith) {
                ul.sortable("option", "connectWith", connectwith);
            }

            if (useHandle) {
                ul.sortable("option", "handle", '.handle');
            }

            if (usePlaceholder) {
                ul.sortable("option", "placeholder", 'placeholder');
            }

            // on update, show the save order note
            if (!hideNoteOnUpdate) {
                ul.bind('sortupdate', function (event, ui) {
                    $("#SaveOrRevertNote").show();
                });
            }
        });
    },

    saveContenteditable: function (elem) {
        var origValElem = elem.next("input[type=hidden]");
        var origVal = origValElem.val();
        var newVal = elem.is("span") ? elem.text() : elem.val(); // assume select or input box (like datepicker) if not span

        if (newVal == origVal) {
            // no changes made
            $("#contentEditableNote").hide();
            return;
        }

        // validate function provided?
        var validateJs = elem.closest("[data-oncontenteditablesavevalidate]");
        if (validateJs.length) {
            var isValid = eval(validateJs.attr("data-oncontenteditablesavevalidate"));
            if (!isValid) {
                // revert to orig db val
                if (elem.is("span")) { elem.text(origValElem.val()); } else { elem.val(origValElem.val()); }
                $("#contentEditableNote").hide();
                return;
            }
        }

        var itemId = elem.closest("[data-itemid]").data().itemid; // closest will include itself, if the data-itemid attr is on the elem itself
        var typeName = elem.closest("[data-typename]").data().typename;
        var propertyName = elem.data().property;
        var data = { itemId: itemId, typeName: typeName, propertyName: propertyName, newValue: newVal };
        var url = objectBaseUrl + "setproperty";

        salem.ajax.doAjaxPost(data, url,
            function () {
                origValElem.val(newVal);
                $("#contentEditableNote").hide(); // in case it's visible

                // add'l success callback function provided?
                var contenteditableSaveSuccessJs = elem.closest("[data-oncontenteditablesavesuccess]");
                if (contenteditableSaveSuccessJs.length) {
                    eval(contenteditableSaveSuccessJs.attr("data-oncontenteditablesavesuccess"));
                }
            },
            function () {
                // revert to orig db val
                if (elem.is("span")) { elem.text(origValElem.val()); } else { elem.val(origValElem.val()); }
                $("#contentEditableNote").hide();
            }
        );
    },

    wireContentEditables: function () {
        // for each contenteditable, add a hidden input w/ the orig val right next to it, so we can use that to revert later
        $("[contenteditable],.contenteditable").each(function () {
            var ce = $(this);
            var origVal = ce.is("span") ? ce.text() : ce.val(); // assume select or input textbox (like datepicker) if not span
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
        $(document).on('blur', '[contenteditable]', function () {
            salem.siteActions.saveContenteditable($(this));
        });

        // on select change, trigger update
        $(document).on('change', 'select.contenteditable', function () {
            salem.siteActions.saveContenteditable($(this));
        });
    },

    wireDeleteEntityLinks: function () {
        $(".deleteEntityLink").attr("title", "Delete");
        $(document).on('click', '.deleteEntityLink', function (e) {
            e.preventDefault();
            var link = $(this);
            var item = link.closest("[data-itemid]");
            var id = item.data().itemid;
            var typeName = link.closest("[data-typename]").data().typename;

            salem.ajax.doAjaxPost({ itemId: id, typeName: typeName }, objectBaseUrl + "deleteentity",
                function (resp) {
                    // remove the item form the dom
                    item.remove();

                    // add'l success callback function provided?
                    var deleteEntitySuccessJs = link.closest("[data-ondeleteentitysuccess]");
                    if (deleteEntitySuccessJs.length) {
                        eval(deleteEntitySuccessJs.attr("data-ondeleteentitysuccess"));
                    }
                },
                function () {
                    // todo: make this nicer
                    alert("Error");
                }
            );
        });
    },

    wireTogglePropertyLinks: function () {
        // add titles if they don't already have them specified
        $(".togglePropertyLink").each(function (i, link) {
            link = $(link);
            if (!link.attr("title")) {
                link.attr("title", "Click to toggle");        
            }
        });

        $(document).on('click', '.togglePropertyLink', function (e) {
            e.preventDefault();
            var link = $(this);
            var id = link.closest("[data-itemid]").data().itemid;
            var typeName = link.closest("[data-typename]").data().typename;
            // if data-property isn't provided, assume propertyname = Active
            var propertyName = link.data().property || "Active";

            link.mask();

            salem.ajax.doAjaxPost({ itemId: id, typeName: typeName, propertyName: propertyName }, objectBaseUrl + "toggleproperty",
                function (resp) {
                    link.unmask();
                    link.closest("span").toggleClass("active").toggleClass("inactive");
                    if (link.data().truefalse) {
                        var trueText = link.data().truefalse.split(',')[0];
                        var falseText = link.data().truefalse.split(',')[1];
                        var newLinkHtml = link.html() == trueText ? falseText : trueText;
                        link.html(newLinkHtml);
                    }

                    // add'l success callback function provided?
                    var togglePropertySuccessJs = link.closest("[data-ontogglepropertysuccess]");
                    if (togglePropertySuccessJs.length) {
                        eval(togglePropertySuccessJs.attr("data-ontogglepropertysuccess"));
                    }
                },
                function () {
                    link.unmask();
                }
            );
        });
    },

    wireRevertLinks: function () {
        $(document).on('click', '.revertLink', function (e) {
            e.preventDefault();
            location.href = location.href; // refresh page, vs. managing what was originally in the dom before the user changed things around
        });
    },

    validateForms: function () {
        $("form.validate").each(function () {
            $(this).validate();
        });
    },

    wireIPhoneCheckBoxes: function () {
        $(".iPhoneCheckBox").each(function () {
            var chk = $(this);
            var yesLabel = chk.data().yesno.split(',')[0];
            var noLabel = chk.data().yesno.split(',')[1];
            var onChangeJs = chk.data().onchangejs;
            chk.iphoneStyle({
                checkedLabel: yesLabel,
                uncheckedLabel: noLabel,
                onChange: function (e, value) {
                    if (onChangeJs) {
                        eval(onChangeJs);
                    }
                }
            });
        });
    },

    wireDatePickers: function () {
        $(".datepicker").datepicker({
            onSelect: function (dateText, inst) {
                var input = $(this);
                if (input.hasClass("contenteditable")) {
                    salem.siteActions.saveContenteditable(input);
                }
            }
        });
    },

    removeSortable: function () {
        $(".DeleteSortable").on('click', function (e) {
            e.preventDefault();
            var $li = $(this).closest("li");
            $li.find("input[name$='MarkedForDeletion']:first").val(true);
            $li.hide();
            $("#SaveOrRevertNote").show();
        });
    },

    validateForms: function () {
        $("form.validate").each(function () {
            $(this).validate();
        });
    },

    wireDataTables: function () {
        $("table.dataTable").each(function () {
            var aaSorting = $("table.dataTable").hasClass("noSort") ? [] : [[0, 'asc']];

            $(this).dataTable({
                "sDom": '<"Pagination"p>lfrt<"Pagination"p>li',
                "sPaginationType": "full_numbers",
                "bAutoWidth": false,
                "aLengthMenu": [[10, 15, 25, 50, -1], [10, 15, 25, 50, "All"]],
                "oLanguage": {
                    "oPaginate": {
                        "sFirst": "&lt;&lt;",
                        "sPrevious": "&lt;",
                        "sNext": "&gt;",
                        "sLast": "&gt;&gt;"
                    }
                },
                "aaSorting": aaSorting,
                "bStateSave": true
            });
        });
    },

    wireSearchBoxes: function () {
        $(".search").attr("placeholder", "Search...");

        $(".search").each(function (i) {
            var search = $(this);
            var searchContainer = search.parent();
            searchContainer.append("<span></span>"); // close icon
            searchContainer.find("span").click(function (e) {
                search.val("");
                search.trigger('keyup');
            });
        });

        $(".search").keyup(function (e) {
            var q = $(this).val();
            var items = $("[data-searchable='true']").closest('li');
            var sortHandles = items.find('.handle');
            var cancelSearch = $("#searchDiv span");
            if (q) {
                cancelSearch.show();
                items.hide(); // hide all
                sortHandles.hide(); // hide sort handles
                $("[data-searchable='true']:contains({0})".format(q)).closest('li').show(); // show ones that contain search string
            }
            else {
                cancelSearch.hide();
                items.show(); // show all
                sortHandles.show(); // show sort handles
            }
        });
    },

    wireChosenDropdowns: function () {
        $(".chosen").chosen();
        $(".chosenSingleDeselect").chosen({ allow_single_deselect: true });
    },

    handleSuccessFeedback: function () {
        // successFeedback is defined in _Layout.cshtml
        if (successFeedback != "") {
            salem.feedback.showSuccess();
        }
    }
}

$(document).ready(function () {
    salem.siteActions.validateForms();

    salem.siteActions.wireDataTables();

    salem.siteActions.handleSuccessFeedback();

    salem.siteActions.wireTogglePropertyLinks();

    salem.siteActions.wireDeleteEntityLinks();

    salem.siteActions.wireContentEditables();

    salem.siteActions.wireRevertLinks();

    salem.siteActions.wireSortables();

    salem.siteActions.removeSortable();

    salem.siteActions.validateForms();

    salem.siteActions.wireChosenDropdowns();

    salem.siteActions.wireDatePickers();

    salem.siteActions.wireIPhoneCheckBoxes();

    salem.siteActions.wireSearchBoxes();

    // do this at end, in case any functions above add/remove any html5 attr's, like placeholder
    salem.siteActions.html5ize();
});