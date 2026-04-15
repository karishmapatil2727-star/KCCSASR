var userData = {};
window.isLoadingEdit = false;  // GLOBAL on window object
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadAccountLedgerList();
        loadddlAccountGroup();
        $('#btnDeleteRec').on('click', function () {
            deleteLedgerRecord();
        });
        $('#OpeningBalance').val('0');
    });
});
function loadAccountLedgerList() {
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
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'pdfHtml5',
                title: 'Khalsa College Charitable Society, Amritsar',
                message: userData.InstName,
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
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
                    columns: [0, 1, 2, 3, 4, 5]
                },
                customize: function (win) {
                    $(win.document.body).find('table th td:nth-child(4)')
                        .addClass('align-right');
                    $(win.document.body).find('table tr td:nth-child(4)')
                        .addClass('align-right');
                }
            },
        ],
        ajax: {
            url: "GetAccountLedgerList",
            dataSrc: function (accountLedgerList) {
                return accountLedgerList;
            }
        },
        columns: [
            { data: "LedgerId", name: "LedgerId" },
            { data: "LedgerName", name: "Ledger Name" },
            { data: "AccountGroupName", name: "Group" },
            {
                name: "Opening Balance",
                className: "align-right",
                render: function (data, type, full, meta) {
                    return parseFloat(full.OpeningBalance).toFixed(2);
                }
            },
            { data: "CrOrDr", name: "Cr/Dr" },
            { data: "Inst_ShortTitle", name: "Org Name" },
            {
                "title": "Edit",
                "data": "AssetID",
                "searchable": false,
                "sortable": false,
                className: "tr-edit",
                "render": function (data, type, full, meta) {
                    if (userData.IsOpeningBalanceEditAllow) {
                        return '<a href="#" onClick="editAccountLedger(' + full.LedgerId + ')" class="btn btn-primary btn-padding">Edit</a>';
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                "title": "Delete",
                "data": "AssetID",
                "searchable": false,
                "sortable": false,
                className: "tr-edit",
                "render": function (data, type, full, meta) {
                    if (userData.IsOpeningBalanceEditAllow) {
                        return '<a href="#" onClick="deleteAccountLedger(' + full.LedgerId + ')" class="btn btn-danger btn-padding">Delete</a>';
                    } else {
                        return '';
                    }
                }
            }
        ]
    });
}
function loadddlAccountGroup() {
    debugger;
    $.ajax({
        type: "GET",
        url: '/Admin/GetAccountGroupsList',
        data: { showInLedger: '1', financialYearId: userData.FinancialYearId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            var optionhtml = '<option value=""></option>';
            $.each(data, function (i) {
                var optionhtml = '<option value="' +
                    data[i].AccountGroupId + '">' + data[i].AccountGroupName + '</option>';
                $("#AccountGroupId").append(optionhtml);
            });
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
function accountLedgerOnSuccess(successResult) {
    if (successResult == true) {
        $("#form0")[0].reset();
        alert("Ledger saved successfully");
        loadAccountLedgerList();
    }
    else {
        alert(successResult);
    }

}
function accountLedgeronFailure() {
    alert('error occured while saving the data');
}
function editAccountLedger(ledgerId) {
    debugger;
    $.ajax({
        type: "GET",
        url: '/Admin/GetAccountLedger',
        data: { ledgerId: ledgerId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            debugger;
            // SET FLAG — prevent toggleGSTFields clearing GSTNo
            isLoadingEdit = true;
            $('#LedgerId').val(data.LedgerId);
            $('#LedgerName').val(data.LedgerName);
            $('#AccountGroupId').val(data.AccountGroupId);
            $('#OpeningBalance').val(data.OpeningBalance);
            $('#CrOrDr').val(data.CrOrDr);
            $('#Mobile').val(data.Mobile);
            $('#TIN').val(data.TIN);
            $('#CST').val(data.CST);
            $('#PAN').val(data.PAN);
            $('#Address').val(data.Address);
            $('#Narration').val(data.Narration);
            // Check both bool and int
            var isGST = (data.IsGSTSales === true || data.IsGSTSales == 1);

            if (isGST) {
                // GST Sales ON
                // 1. Clear and disable TIN CST FIRST
                $('#TIN').val('').prop('disabled', true);
                $('#CST').val('').prop('disabled', true);

                // 2. Check checkbox
                $('#chkIsGSTSales').prop('checked', true);

                // 3. Show GST section
                $('#gstPanSection').show();

                // 4. Set GSTNo
                $('#GSTNo').val(data.GSTNo != null ? data.GSTNo : '');

            } else {
                // GST Sales OFF
                // 1. Enable and set TIN CST
                $('#TIN').prop('disabled', false).val(data.TIN != null ? data.TIN : '');
                $('#CST').prop('disabled', false).val(data.CST != null ? data.CST : '');

                // 2. Uncheck checkbox
                $('#chkIsGSTSales').prop('checked', false);

                // 3. Hide GST section
                $('#gstPanSection').hide();

                // 4. Clear GSTNo
                $('#GSTNo').val('');
            }

            var isItem = (data.IsItem === true || data.IsItem == 1);
            if (isItem) {
                $('#chkIsItem').prop('checked', true);
                $('#divHSNCode').show();
                $('#HSNCode').val(data.HSNCode != null ? data.HSNCode : '');
            } else {
                $('#chkIsItem').prop('checked', false);
                $('#divHSNCode').hide();
                $('#HSNCode').val('');
            }
            // RESET FLAG
            isLoadingEdit = false;

            // Scroll to top
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
function deleteAccountLedger(ledgerId) {
    $('#hdnGroupId').val(ledgerId);
    $('#deleteModel').modal('show');
}
function deleteLedgerRecord() {
    var ledgerId = $('#hdnGroupId').val();
    $.ajax({
        type: "GET",
        url: '/Admin/DeleteAccountLedger',
        data: { ledgerId: ledgerId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            $('#deleteModel').modal('hide');
            loadAccountLedgerList();
            alert(data);
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
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

    // --- NEW GST VALIDATION LOGIC ---

    // Check if the GST checkbox is checked
    if ($('#chkIsGSTSales').is(':checked')) {
        debugger;
        var gstNo = $('#GSTNo').val().trim().toUpperCase();

        // 3. Check if GST Number is empty
        if (gstNo === "") {
            alert("Please enter GST Number.");
            $('#GSTNo').focus();
            return false;
        }

        // 4. GST Format Regex Validation
        var gstRegex = /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/;
        if (!gstRegex.test(gstNo)) {
            alert("Invalid GST Number format. Please enter a valid 15-digit GSTIN.");
            $('#divHSNCode').focus();
            return false;
        }
    }

    // HSN VALIDATION
    if ($('#chkIsItem').is(':checked')) {
        debugger;
        var hsnCode = $('#HSNCode').val();

        if (hsnCode === undefined || hsnCode === null) {
            alert('HSN/SAC Code field not found.');
            return false;
        }

        hsnCode = hsnCode.trim();

        if (hsnCode === '') {
            alert('Please enter HSN/SAC Code.');
            $('#HSNCode').focus();
            return false;
        }

        if (!/^\d+$/.test(hsnCode)) {
            alert('HSN Code must contain digits only.');
            $('#HSNCode').focus();
            return false;
        }

        if (hsnCode.length !== 4 && hsnCode.length !== 6 && hsnCode.length !== 8) {
            alert('HSN Code must be 4, 6 or 8 digits.');
            $('#HSNCode').focus();
            return false;
        }
    }

    return true;  // ← THIS was the missing line causing the bug
}



