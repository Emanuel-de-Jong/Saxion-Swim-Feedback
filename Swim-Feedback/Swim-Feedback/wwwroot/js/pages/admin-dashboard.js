adminDashboard = {};

adminDashboard.init = function() {
    adminDashboard.logTable = new DataTable("#log-table", {
        pageLength: 9,
        order: [[ 0, 'desc' ]]
    });
};

adminDashboard.openPasswordPopup = function () {
    const elem = document.getElementById("admin-import-data");
    elem.style.display = "block";
}

adminDashboard.closePasswordPopup = function () {
    const elem = document.getElementById("admin-import-data");
    elem.style.display = "none";
}

adminDashboard.openImportPopup = function () {
    const elem = document.getElementById("admin-import-data");
    elem.style.display = "block";
}

adminDashboard.closeImportPopup = function () {
    const elem = document.getElementById("admin-import-data");
    elem.style.display = "none";
}