var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadNotificationPendingLedgerList();
        loadNotificationConfirmedLedgerList();
        loadDropdownforNoification();
    });
});
function loadDropdownforNoification() {
    $.ajax({
        type: "GET",
        url: '/Admin/GetDepartmentsList',     
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            var optionhtml = '<option value="">Select Department</option>';
            $("#ddlInstitute").append(optionhtml);
            $.each(data, function (i) {
                var optionhtml = '<option value="' +
                    data[i].Inst_Id + '">' + data[i].Inst_ShortTitle + '</option>';
                $("#ddlInstitute").append(optionhtml);
            });
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
function loadNotificationPendingLedgerList() {
    if ($.fn.DataTable.isDataTable("#tblNotificationPendingLedger")) {
        $('#tblNotificationPendingLedger').DataTable().draw();
        $('#tblNotificationPendingLedger').DataTable().destroy();
        $('#tblNotificationPendingLedger tbody').empty();
    }
    $('#tblNotificationPendingLedger').DataTable({
        bProcessing: true,
        pageLength: 50,
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        ajax: {
            url: "GetNotificationPendingLedgerList",
            dataSrc: ''
        },
        columns: [
            { data: "LedgerName", name: "Ledger Name" },
            { data: "AccountGroupName", name: "Under Group" },
            {
                "title": "Edit",
                "data": "LedgerId",
                "searchable": false,
                "sortable": false,
                "className": "tr-edit",
                "render": function (data, type, full, meta) {
                    return '<a href="#" onClick="AddPendingAccountLedger(' + full.Id + ')" class="btn btn-primary btn-padding">Add</a>';
                }
            }
        ]
    });
}
function loadNotificationConfirmedLedgerList() {
    if ($.fn.DataTable.isDataTable("#tblNotificationConfirmedLedger")) {
        $('#tblNotificationConfirmedLedger').DataTable().draw();
        $('#tblNotificationConfirmedLedger').DataTable().destroy();
        $('#tblNotificationConfirmedLedger tbody').empty();
    }
    $('#tblNotificationConfirmedLedger').DataTable({
        bProcessing: true,
        pageLength: 50,
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        ajax: {
            url: "GetNotificationConfirmedLedgerList",
            dataSrc: ''
        },
        columns: [
            { data: "LedgerName", name: "Ledger Name" },
            { data: "Inst_ShortTitle", name: "Institute" },
            { data: "AccountGroupName", name: "Under Group" },
            {
                "title": "Delete",
                "data": "LedgerId",
                "searchable": false,
                "sortable": false,
                "className": "tr-edit",
                "render": function (data, type, full, meta) {
                    return '<a href="#" onClick="deleteAccountConfirmedLedger(' + full.Id + ')" class="btn btn-danger btn-padding">Delete</a>';
                }
            }
        ]
    });
}
function AddPendingAccountLedger(id) {
    if ($("#ddlInstitute").val() != undefined && $("#ddlInstitute").val() != '') {
        $.ajax({
            type: "GET",
            url: '/Admin/NotificationPendingLedgerUpdate',
            data: { notificationInstId: $("#ddlInstitute").val(), Id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                ShowLoading();
            },
            success: function (data) {
                alert('Record updated successfully');
                loadNotificationPendingLedgerList();
                loadNotificationConfirmedLedgerList();
            },
            error: function (error) { console.log(error); },
            complete: function () {
                HideLoading();
            }
        });
    }
    else {
        alert('Please select institute');
    }
}
function deleteAccountConfirmedLedger(id) {
    $.ajax({
        type: "GET",
        url: '/Admin/NotificationConfirmedLedgerDelete',
        data: { Id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            alert('Record deleted successfully');
            loadNotificationPendingLedgerList();
            loadNotificationConfirmedLedgerList();
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}