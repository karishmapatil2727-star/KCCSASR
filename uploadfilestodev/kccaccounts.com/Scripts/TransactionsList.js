var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadVocherTypeDropdown();
        loadOrgNamesDropdown();
        var startDate = new Date(userData.FinancialYearStartDate);
        startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate(), 0, 0, 0, 0);
        var endDate = new Date(userData.FinancialYearEndDate);
        var CurrentDate = new Date();
        if (endDate > CurrentDate) {
            endDate = CurrentDate;
        }
        else {
            endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate(), 0, 0, 0, 0);
        }
        $('#dtStaringDate,#txtStaringDate').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            startDate: startDate,
            endDate: endDate
        });
        $('#dtEndingDate,#txtEndingDate').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            startDate: startDate,
            endDate: endDate
        });
        var today = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
        $('#txtStaringDate').datepicker('setDate', startDate);
        $('#txtEndingDate').datepicker('setDate', today);
        if (userData.InstituteId == 300010) {
            $("#ddlInstitute").attr("disabled", false);
        }
        else {
            $("#ddlInstitute").prop("disabled", true);
        }

        $('#btnView').on('click', function () {
            var fromDate = $('#txtStaringDate').val();
            if (fromDate == '' && !ValidateDate(fromDate)) {
                alert('Please select valid start date');
                return false;
            }
            var toDate = $('#txtEndingDate').val();
            if (toDate == '' && !ValidateDate(fromDate)) {
                alert('Please select valid end date');
                return false;
            }
            var selectedVocher = $("#ddlVocherTypes option:selected").val();
            if (selectedVocher == '') {
                selectedVocher = 0;
            }
            var forInstId = $("#ddlInstitute option:selected").val();
            var instituteTitle = $("#ddlInstitute option:selected").text();


            bindLedgerVocherstoGrid(ConverttoDate(fromDate), ConverttoDate(toDate), selectedVocher, forInstId, instituteTitle);
        });
    });
});
function loadVocherTypeDropdown() {
    $.ajax({
        type: "GET",
        url: '/Transactions/GetVoucherTypes',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            var optionhtml = '<option value="0">All</option>';
            $("#ddlVocherTypes").append(optionhtml);
            $.each(data, function (i) {
                var optionhtml = '<option value="' +
                    data[i].VoucherTypeId + '">' + data[i].VoucherTypeName + '</option>';
                $("#ddlVocherTypes").append(optionhtml);
            });
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
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
function bindLedgerVocherstoGrid(fromDate, toDate, VoucherTypeId, ForInstId, instituteTitle) {
    var requestModel = { FromDate: fromDate.toISOString(), ToDate: toDate.toISOString(), VoucherTypeId: VoucherTypeId, ForInstituteId: ForInstId, ToInstituteId: ForInstId };
    if ($.fn.DataTable.isDataTable("#tblListVocherList")) {
        $('#tblListVocherList').DataTable().draw();
        $('#tblListVocherList').DataTable().destroy();
        $('#tblListVocherList tbody').empty();
    }
    $('#tblListVocherList').DataTable({
        bProcessing: true,
        pageLength: 50,
        autoWidth: true,
        dom:
            "<'row'<'col-sm-3'l><'col-sm-4 text-center'f><'col-sm-5'B>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        columnDefs: [{
            className: "dt-right",
            targets: [0]
        }],
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7] }],
        buttons: [
            {
                extend: 'excelHtml5',
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: 'pdfHtml5',
                title: '',
                orientation: 'landscape',
                pageSize: 'LEGAL',
                message: userData.InstName + ', From (' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + ' - ' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
                customize: function (doc) {
                    //Remove the title created by datatTables
                    //  doc.content.splice(0, 1);
                    doc['header'] = (function () {
                        return {
                            columns: [
                                {
                                    alignment: 'center',
                                    fontSize: 14,
                                    text: 'Khalsa College Charitable Society, Amritsar',
                                }

                            ],

                            margin: 20
                        }
                    });
                    doc.pageMargins = [20, 60, 20, 30];
                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    var objLayout = {};
                    objLayout['hLineWidth'] = function (i) { return .5; };
                    objLayout['vLineWidth'] = function (i) { return .5; };
                    objLayout['hLineColor'] = function (i) { return '#aaa'; };
                    objLayout['vLineColor'] = function (i) { return '#aaa'; };
                    objLayout['paddingLeft'] = function (i) { return 4; };
                    objLayout['paddingRight'] = function (i) { return 4; };
                    doc.content[0].layout = objLayout;
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][5].alignment = 'right';
                        doc.content[1].table.body[i][6].alignment = 'right';
                    };
                },
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                },
                footer: true
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(6),table tr th:nth-child(7)')
                        .addClass('align-right');
                    $(win.document.body).find('table tr td:nth-child(6),table tr td:nth-child(7)')
                        .addClass('align-right');
                },
                footer: true
            },
        ],
        aaSorting: [[0, 'asc']],
        ajax: {
            type: "POST",
            url: "GetLedgerVoucherList",
            data: requestModel,
            dataSrc: function (json) {
                $('#spnTitleText').text('          From (' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + ' - ' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ') for "' + instituteTitle + '"');
                $('#spnDebitAmount').text(parseFloat(json.transactionAmount.TotalDebitAmount).toFixed(2));
                $('#spnCreditAmount').text(parseFloat(json.transactionAmount.TotalCreditAmount).toFixed(2));
                return json.transactionSearchLedgers;
            }
        },
        columns: [
            {
                name: '',
                data: 'SerialId',
                visible: false
            },
            {
                name: "Date",
                render: function (data, type, row) {
                    return '<span class="' + row.className + '">' + row.TransactionDate + '</span>';
                },
                width: "100px"
            },
            {
                name: "V.Type",
                render: function (data, type, row) {
                    return '<span class="' + row.className + '">' + row.VoucherTypeName + '</span>';
                }
            },
            {
                name: "V. No",
                render: function (data, type, row) {
                    return '<span class="' + row.className + '">' + row.VoucherNo + '</span>';
                }
            },
            {
                name: "Cheque No.",
                render: function (data, type, row) {
                    return '<span class="' + row.className + '">' + row.ChequeNo + '</span>';
                }
            },
            {
                name: "Particular",
                render: function (data, type, row) {
                    return '<span class="' + row.className + '">' + row.LedgerName + '</span>';
                }
            },
            {
                name: "Debit",
                className: "align-right",
                render: function (data, type, row) {
                    return '<span class="' + row.className + '">' + row.Debit + '</span>';
                }
            },
            {
                name: "Credit",
                className: "align-right",
                render: function (data, type, row) {
                    return '<span class="' + row.className + '">' + row.Credit + '</span>';
                }
            }
        ]
    });
}