login = {};

login.openPopup = function () {
    const elem = document.getElementById("request-password-reset");
    elem.style.display = "block";
}

login.closePopup = function () {
    const elem = document.getElementById("request-password-reset");
    elem.style.display = "none";
}