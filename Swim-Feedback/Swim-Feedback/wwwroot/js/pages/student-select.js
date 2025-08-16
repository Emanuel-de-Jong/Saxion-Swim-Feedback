studentSelect = {};

studentSelect.openPopup = function () {
    const elem = document.getElementById("student-select-back");
    elem.style.display = "block";
}

studentSelect.closePopup = function () {
    const elem = document.getElementById("student-select-back");
    elem.style.display = "none";
    console.log("test");
}