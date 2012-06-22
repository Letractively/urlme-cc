registerNS("codejkjk.siteActions");

codejkjk.siteActions = {
    // make html 5 features work in browsers that don't support it innately, like IE
    html5ize: function () {
        if (!codejkjk.browserCompatability.testAttribute('input', 'placeholder')) {
            codejkjk.form.smartTextBox($("input[placeholder]"), 'placeholder');
        }
    }
}

$(document).ready(function () {
    codejkjk.siteActions.html5ize();
});