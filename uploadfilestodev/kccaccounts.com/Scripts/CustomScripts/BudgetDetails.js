var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    //userData.FinancialYearId = 10;
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadOrgNamesDropdown();// added this for budget
        $('#btnView').on('click', function () {
            var forInstId = $("#ddlInstitute option:selected").val();
            if (forInstId == '') {
                alert('Please select valid institute');
                return false;
            }
            loadBudgetDetailsList(forInstId);
        });
        $('#divModifyBudget').css("display", "none");
        $('#btnCancel').on('click', function () {
            $('#divModifyBudget').css("display", "none");
            $('#BD_BudgetAmount').val(data.BD_BudgetAmount);
            $('#BD_BudgetAmount_4_Months').val('');
            $('#PBD_Financial0BudgetAmount').val('');
            $('#PBD_Financial1BudgetAmount').val('');
            $('#PBD_Financial2BudgetAmount').val('');
            $('#PBD_Financial3BudgetAmount').val('');
            $('#PBD_Financial4BudgetAmount').val('');
        });
    });
});
function loadBudgetDetailsList(instituteId) {
    if ($.fn.DataTable.isDataTable("#tblLedgerList")) {
        $('#tblLedgerList').DataTable().draw();
        $('#tblLedgerList').DataTable().destroy();
        $('#tblLedgerList tbody').empty();
    }
    $('#tblLedgerList').DataTable({
        processing: true,
        pageLength: 50,
        dom:
            "<'row'<'col-sm-3'l><'col-sm-4 text-center'f><'col-sm-5'B>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        language: {
            search: "",
            searchPlaceholder: "Search records",
            sEmptyTable: "No ledgers available"
        },
        columnDefs: [
            { className: "dt-right", targets: [0] }
        ],
        buttons: [
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'pdfHtml5',
                title: 'Khalsa College Charitable Society, Amritsar',
                message: userData.InstName,
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                },
                customize: function (doc) {
                    var rowCount = doc.content[2].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[2].table.body[i][3].alignment = 'right';
                    };
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                },
                customize: function (win) {
                    $(win.document.body).find('table th td:nth-child(4)')
                        .addClass('align-right');
                    $(win.document.body).find('table tr td:nth-child(4)')
                        .addClass('align-right');
                    $(win.document.body).find('table tr th:nth-child(2),table tr td:nth-child(2)').css('width', '200px');

                }
            },
        ],
        ajax: {
        
            type: "GET",
            url: '/Admin/GetAccountBudgetDetails',
            data: { instituteId: instituteId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            dataSrc: function (accountLedgerList) {
                return accountLedgerList;
            }
        },
        columns: [
            { data: "LedgerId", name: "LedgerId" },
            { data: "LedgerName", name: "Ledger Name" },
            { data: "AccountGroupName", name: "Group" },
            { data: "Inst_ShortTitle", name: "Org Name" },
            {
                "title": "Edit",
                "data": "AssetID",
                "searchable": false,
                "sortable": false,
                className: "tr-edit",
                "render": function (data, type, full, meta) {
                    if (userData.IsOpeningBalanceEditAllow) {
                        return '<a href="#" onClick="editAccountBudget(' + instituteId + ',' + full.LedgerId + ')" class="btn btn-primary btn-padding">Edit</a>';
                    }
                    else {
                        return '';
                    }
                }
            },
        ]
    });
}
function loadOrgNamesDropdown() {
    $.ajax({
        type: "GET",
        url: '/Admin/GetDepartmentsList',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            var optionhtml = '<option value=""></option>';
            $("#ddlInstitute").append(optionhtml);
            $.each(data, function (i) {
                if (userData.InstituteId == data[i].Inst_Id) {
                    var optionhtml = '<option selected="selected" value="' +
                        data[i].Inst_Id + '">' + data[i].Inst_ShortTitle + '</option>';
                }
                else {
                    var optionhtml = '<option value="' +
                        data[i].Inst_Id + '">' + data[i].Inst_ShortTitle + '</option>';
                }
                $("#ddlInstitute").append(optionhtml);
            });
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
function accountBudgetOnSuccess(successResult) {
    if (successResult == true) {
        $("#form0")[0].reset();
        var forInstId = $("#ddlInstitute option:selected").val();
        alert("Budget Details saved successfully");
        loadBudgetDetailsList(forInstId);
        $('#divModifyBudget').css("display", "none");
    }
    else {
        alert(successResult);
    }

}
function accountBudgetonFailure() {
    alert('error occured while saving the data');
}
function editAccountBudget(instId,ledgerId) {
    $.ajax({
        type: "GET",
        url: '/Admin/GetAccountBudgetDetailsForUpdate',
        data: { instituteId:instId,ledgerId: ledgerId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            if (userData.FinancialYearId == data.FinalFinancialYearId) {
                $('#InstId').val(data.InstId);
                $('#LedgerId').val(data.LedgerId);
                $('#LedgerName').val(data.LedgerName);
                $('#BD_BudgetAmount').val(data.BD_BudgetAmount);
                $('#BD_BudgetAmount_4_Months').val(data.BD_BudgetAmount_4_Months);
                $('#PBD_Financial0BudgetAmount').val(data.PBD_Financial0BudgetAmount);
                $('#PBD_Financial1BudgetAmount').val(data.PBD_Financial1BudgetAmount);
                $('#PBD_Financial2BudgetAmount').val(data.PBD_Financial2BudgetAmount);
                $('#PBD_Financial3BudgetAmount').val(data.PBD_Financial3BudgetAmount);
                $('#PBD_Financial4BudgetAmount').val(data.PBD_Financial4BudgetAmount);
            }
            else  {                  // This will work to fetch data for  previous financial id(with -1)
                $('#InstId').val(data.InstId);
                $('#LedgerId').val(data.LedgerId);
                $('#LedgerName').val(data.LedgerName);
                $('#BD_BudgetAmount').val(''); //data.BD_BudgetAmount_4_Months contains previous year budget, so empty it so that can be enter again
                $('#BD_BudgetAmount_4_Months').val('');////data.BD_BudgetAmount contains previous year budget, so empty it so that can be enter again
                if (data.BD_BudgetAmount!=0)
                    $('#PBD_Financial0BudgetAmount').val(data.BD_BudgetAmount);
                else
                    $('#PBD_Financial0BudgetAmount').val('');
                if (data.PBD_Financial0BudgetAmount != 0)
                    $('#PBD_Financial1BudgetAmount').val(data.PBD_Financial0BudgetAmount);
                else
                    $('#PBD_Financial1BudgetAmount').val('');
                if (data.PBD_Financial1BudgetAmount != 0)
                    $('#PBD_Financial2BudgetAmount').val(data.PBD_Financial1BudgetAmount);
                else
                    $('#PBD_Financial2BudgetAmount').val('');
                if (data.PBD_Financial2BudgetAmount != 0)
                    $('#PBD_Financial3BudgetAmount').val(data.PBD_Financial2BudgetAmount);
                else
                    $('#PBD_Financial3BudgetAmount').val('');
                if (data.PBD_Financial3BudgetAmount != 0)
                    $('#PBD_Financial4BudgetAmount').val(data.PBD_Financial3BudgetAmount);
                else
                    $('#PBD_Financial4BudgetAmount').val('');
            }
            
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
            $('#divModifyBudget').css("display", "block");
        }
    });
}
function validateLedgerfrom() {
    if (!userData.IsNewLedgerAddAllow) {
        var lederId = $('#LedgerId').val();
        if (lederId == 0 || lederId == undefined) {
            alert("You don't have permissions to add new ledger.. Please contact adminstrator");
            return false;
        }
    }
    if (!userData.IsOpeningBalanceEditAllow) {
        var lederId = $('#LedgerId').val();
        if (lederId > 0) {
            alert("You don't have permissions to modify the ledger.. Please contact adminstrator");
            return false;
        }
    }
}




