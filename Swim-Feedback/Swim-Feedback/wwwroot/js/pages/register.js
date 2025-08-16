register = {};

register.openPopup = function () {
    const elem = document.getElementById("terms-and-conditions");
    elem.style.display = "block";
}

register.closePopup = function () {
    const elem = document.getElementById("terms-and-conditions");
    elem.style.display = "none";
}