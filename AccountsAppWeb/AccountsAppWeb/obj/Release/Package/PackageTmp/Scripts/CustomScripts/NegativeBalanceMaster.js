var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadNegavtiveBalanceList();
    });
});
function loadNegavtiveBalanceList() {
    if ($.fn.DataTable.isDataTable("#tblNegativeBalance")) {
        $('#tblNegativeBalance').DataTable().draw();
        $('#tblNegativeBalance').DataTable().destroy();
        $('#tblNegativeBalance tbody').empty();
    }
    $('#tblNegativeBalance').DataTable({
        bProcessing: true,
        info: false,
        language: {
            search: "",
            searchPlaceholder: "Search records",
            sEmptyTable: "Congratulation, there is no Negative Balance."
        },
        ajax: {
            url: "GetNegativeCashBalance",
            dataSrc: ''
        },       
        columns: [
            { data: "MyTransactinDate", name: "Transaction Date" },
            { data: "Credit", name: "Credit" },
        ]
    });  
}