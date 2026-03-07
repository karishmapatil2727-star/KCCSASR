var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        bindTransactionPermissiontoGrid();

        $('#tblTransactionPermissions').on('click', 'tbody .edit_btn', function () {
            var table = $('#tblTransactionPermissions').DataTable();
            var data = table.row($(this).parents('tr')).data();
            $.ajax({
                type: "POST",
                url: '/Admin/UpdateTransactionPermissions',
                data: JSON.stringify({ 'instituteId': data.Inst_ID, 'isTransactionAddAllow': data.BI_IsTransactionAddAllow, 'isTransactionEditAllow': data.BI_IsTransactionEditAllow, 'isOpeningBalanceEditAllow': data.BI_IsOpeningBalanceEditAllow, 'isNewLedgerAddAllow': data.BI_IsNewLedgerAddAllow }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    ShowLoading();
                },
                success: function (data) {
                    if (data == true) {
                        alert("Data has been saved successfully!!");
                        bindTransactionPermissiontoGrid();
                    }
                    else
                        alert("Somthing went wrong, Contact Adminsitrator");
                },
                error: function (error) { console.log(error); },
                complete: function () {
                    HideLoading();
                }
            });
        });
    });
});

function bindTransactionPermissiontoGrid() {
    if ($.fn.DataTable.isDataTable("#tblTransactionPermissions")) {
        $('#tblTransactionPermissions').DataTable().draw();
        $('#tblTransactionPermissions').DataTable().destroy();
        $('#tblTransactionPermissions tbody').empty();
    }
    $('#tblTransactionPermissions').DataTable({
        bProcessing: true,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,    
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        ajax: {
            url: "LoadTransactionPermissions",
            dataSrc: function (model) {
                return model;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5] }],
        columns: [
            {
                name: "Institution",
                data: "Inst_Title"
            },
            {
                name: "Transaction Add",
                "className": "tr-edit",
                render: function (data, type, row) {
                    if (row.BI_IsTransactionAddAllow == true)
                        return '<input id="chkTransactionAddAllow_' + row.Inst_ID + '" type="checkbox" onClick="IsChecked(this)" checked />';
                    else
                        return '<input id="chkTransactionAddAllow_' + row.Inst_ID + '" type="checkbox" onClick="IsChecked(this)"/>';
                }
            },
            {
                name: "Transaction Edit",
                "className": "tr-edit",
                render: function (data, type, row) {
                    if (row.BI_IsTransactionEditAllow == true)
                        return '<input id="chkTransactionEditAllow_' + row.Inst_ID + '" type="checkbox" onClick="IsChecked(this)" checked />';
                    else
                        return '<input id="chkTransactionEditAllow_' + row.Inst_ID + '" type="checkbox" onClick="IsChecked(this)"/>';
                }
            }
            ,
            {
                name: "Open. Balance Edit",
                "className": "tr-edit",
                render: function (data, type, row) {
                    if (row.BI_IsOpeningBalanceEditAllow == true)
                        return '<input id="chkOpeningBalanceEditAllow_' + row.Inst_ID + '" type="checkbox" onClick="IsChecked(this)" checked />';
                    else
                        return '<input id="chkOpeningBalanceEditAllow_' + row.Inst_ID + '" type="checkbox" onClick="IsChecked(this)"/>';
                }
            },
            {
                name: "New Ledger Add",
                "className": "tr-edit",
                render: function (data, type, row) {
                    if (row.BI_IsNewLedgerAddAllow == true)
                        return '<input id="chkNewLedgerAddAllow_' + row.Inst_ID + '" type="checkbox" onClick="IsChecked(this)" checked />';
                    else
                        return '<input id="chkNewLedgerAddAllow_' + row.Inst_ID + '" type="checkbox" onClick="IsChecked(this)"/>';
                }
            },
            {
                name: "Update",
                "className": "tr-edit",
                render: function (data, type, row) {
                    return '<button class="btn btn-primary btn-padding" id="' + row.Inst_ID + '">Update</button>';
                }
            }
        ]
    });
}

function IsChecked(obj) {
    var table = $('#tblTransactionPermissions').DataTable();
    var datarow = table.row($(obj).parents('tr')).data();
    var elementId = obj.id;
    if (obj.checked) {
        $(obj).prop('checked', true);
        if (elementId.match("^chkTransactionAddAllow_"))
            datarow.BI_IsTransactionAddAllow = true;
        if (elementId.match("^chkTransactionEditAllow_"))
            datarow.BI_IsTransactionEditAllow = true;
        if (elementId.match("^chkOpeningBalanceEditAllow_"))
            datarow.BI_IsOpeningBalanceEditAllow = true;
        if (elementId.match("^chkNewLedgerAddAllow_"))
            datarow.BI_IsNewLedgerAddAllow = true;
    }
    else {
        $(obj).prop('checked', false);
        if (elementId.match("^chkTransactionAddAllow_"))
            datarow.BI_IsTransactionAddAllow = false;
        if (elementId.match("^chkTransactionEditAllow_"))
            datarow.BI_IsTransactionEditAllow = false;
        if (elementId.match("^chkOpeningBalanceEditAllow_"))
            datarow.BI_IsOpeningBalanceEditAllow = false;
        if (elementId.match("^chkNewLedgerAddAllow_"))
            datarow.BI_IsNewLedgerAddAllow = false;
    }
}