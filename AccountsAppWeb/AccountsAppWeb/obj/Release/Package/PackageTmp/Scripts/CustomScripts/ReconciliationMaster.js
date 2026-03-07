var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadLedgerList();
        loadReconciliationLedgerList();
    });
});

function loadLedgerList() {
    if ($.fn.DataTable.isDataTable("#tblLedgerList")) {
        $('#tblLedgerList').DataTable().draw();
        $('#tblLedgerList').DataTable().destroy();
        $('#tblLedgerList tbody').empty();
    }
    $('#tblLedgerList').DataTable({
        bProcessing: true,
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        ajax: {
            url: "GetAccountLedgerList",
            dataSrc: ''
        },
        columns: [
            { data: "LedgerId", name: "LedgerId" },
            { data: "LedgerName", name: "Ledger Name" },
            {
                "title": "Check it",
                "data": "AssetID",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return '<input id=' + full.LedgerId + ' type =\"checkbox\" />';
                }
            }
        ]
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#tblLedgerList tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        if ($(this).is(":checked")) {
            $('input[type="checkbox"]').prop("checked", false);
            $(this).prop("checked", true);
        }
    });
}

function loadReconciliationLedgerList() {
    if ($.fn.DataTable.isDataTable("#tblReconciliationLedgerList")) {
        $('#tblReconciliationLedgerList').DataTable().draw();
        $('#tblReconciliationLedgerList').DataTable().destroy();
        $('#tblReconciliationLedgerList tbody').empty();
    }
    $('#tblReconciliationLedgerList').DataTable({
        bProcessing: true,
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        ajax: {
            url: "GetReconciliationLedgerList",
            dataSrc: ''
        },
        columns: [
            { data: "Inst_Title", name: "Institution" },
            { data: "ClientLedgerName", name: "Inst./Dept/.Ledger Name" },
            { data: "ToInstLedgerName", name: "My Ledger Name" },
            {
                "title": "Save",
                "data": "AccountGroupId",
                "searchable": false,
                "sortable": false,
                "className": "tr-edit",
                "render": function (data, type, full, meta) {
                    return '<a href="#" onClick="SaveReconciliationLedger(' + full.Id + ')" class="btn btn-primary btn-padding">Save</a>';
                }
            },
            {
                "title": "Delete",
                "data": "AssetID",
                "searchable": false,
                "sortable": false,
                "className": "tr-edit",
                "render": function (data, type, full, meta) {
                    return '<a href="#" onClick="DeleteReconciliationLedger(' + full.Id + ')" class="btn btn-danger btn-padding">Delete</a>';
                }
            }
        ]
    });
}

function SaveReconciliationLedger(id) {
    var chkbox_checked = $('tbody input[type="checkbox"]:checked', '#tblLedgerList');
    if (chkbox_checked.length == 0) {
        alert('Please select one Ledger');
        return false;
    }
    else {
        var notificationToLedgerId = chkbox_checked[0].id;
        $.ajax({
            type: "GET",
            url: '/Admin/ReconciliationLedgerConfirmedUpdate',
            contentType: "application/json; charset=utf-8",
            data: { notificationToLedgerId: notificationToLedgerId, id: id },
            dataType: "json",
            beforeSend: function () {
                ShowLoading();
            },
            success: function (data) {
                alert('Record has been saved');
                loadLedgerList();
                loadReconciliationLedgerList();
            },
            error: function (error) { console.log(error); },
            complete: function () {
                HideLoading();
            }
        });
    }
}
function DeleteReconciliationLedger(id) {
    $.ajax({
        type: "GET",
        url: '/Admin/ReconciliationLedgerConfirmedDelete',
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            alert('Record has been deleted');
            loadLedgerList();
            loadReconciliationLedgerList();
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}