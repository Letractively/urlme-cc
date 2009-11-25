function gel(id) {
    return document.getElementById(id);
}
function show(ctl) {
    ctl.style.display = "block";
}
function hide(ctl) {
    ctl.style.display = "none";
}
function hideId(id) {
    gel(id).style.display = "none";
}
function showInline(ctl) {
    ctl.style.display = "inline";
}
function showInlineId(id) {
    gel(id).style.display = "inline";
}
function emailIsValid(value) {
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailPattern.test(value);
}
function varIsNothing(value) {
    return (value == null || value == "" || value == " ");
}