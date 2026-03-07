var userData = {};
var now = new Date();
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
            if (fromDate == '' || !ValidateDate(fromDate)) {
                alert('Please select valid start date');
                return false;
            }
            var toDate = $('#txtEndingDate').val();
            if (toDate == '' || !ValidateDate(toDate)) {
                alert('Please select valid end date');
                return false;
            }
            var selectedVocher = $("#ddlInstitute option:selected").val();
            if (selectedVocher == '') {
                alert('Please select valid institute');
                return false;
            }
            var forInstId = $("#ddlInstitute option:selected").val();
            bindBalanceSheetstoGrid(ConverttoDate(fromDate), ConverttoDate(toDate), forInstId);
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
function bindBalanceSheetstoGrid(fromDate, toDate, forInstId) {
    if ($.fn.DataTable.isDataTable("#tblBalanceSheetReport")) {
        $('#tblBalanceSheetReport').DataTable().draw();
        $('#tblBalanceSheetReport').DataTable().destroy();
        $('#tblBalanceSheetReport tbody').empty();
    }
    $('#tblBalanceSheetReport').DataTable({
        bProcessing: true,
        pageLength: 100,
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
        buttons: [
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6]
                },
                footer: true
            },
            {
                title: '',
                pageSize: 'A4',
                extend: 'pdfHtml5',
                filename: 'Common Balance Sheet_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
                title: 'Export',
                header: true,
                customize: function (doc) {
                    doc.content.splice(0, 1, {
                        text: [{
                            text: 'Khalsa College Charitable Society, Amritsar \n',
                            bold: true,
                            fontSize: 14
                        }, {
                            text: userData.InstName + ' \n',
                            bold: true,
                            fontSize: 11
                        }, {
                            text: 'Common Balance Sheet From(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });
                    doc.defaultStyle.fontSize = 7;
                    doc.styles.tableHeader.fontSize = 7;
                    doc.content[1].table.widths = ['20%', '15%', '15%', '20%', '15%', '15%'];
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][2].alignment = 'right';

                        doc.content[1].table.body[i][4].alignment = 'right';
                        doc.content[1].table.body[i][5].alignment = 'right';
                    };
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                {
                                    alignment: 'right',
                                    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                }
                            ],
                            margin: 20
                        }
                    });
                    var objLayout = {};
                    objLayout['hLineWidth'] = function (i) { return .5; };
                    objLayout['vLineWidth'] = function (i) { return .5; };
                    objLayout['hLineColor'] = function (i) { return '#aaa'; };
                    objLayout['vLineColor'] = function (i) { return '#aaa'; };
                    objLayout['paddingLeft'] = function (i) { return 4; };
                    objLayout['paddingRight'] = function (i) { return 4; };
                    doc.content[1].layout = objLayout;
                },
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + ", Common Balance Sheet From (" + fromDate.getDate() + "/" + (fromDate.getMonth() + 1) + "/" + fromDate.getFullYear() + " -" + toDate.getDate() + "/" + (toDate.getMonth() + 1) + "/" + toDate.getFullYear() + ") </h4></div>",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6]
                },
                customize: function (win) {
                    $(win.document.body).find('table th td:nth-child(2),table th td:nth-child(3),table th td:nth-child(5),table th td:nth-child(6)')
                        .addClass('align-right');

                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(3),table tr td:nth-child(5),table tr td:nth-child(6)')
                        .addClass('align-right');
                    $(win.document.body).find('table tr th:nth-child(2),table tr td:nth-child(2),table tr th:nth-child(3),table tr td:nth-child(3)').css('width', '150px');
                    $(win.document.body).find('table tr th:nth-child(4),table tr td:nth-child(4),table tr th:nth-child(5),table tr td:nth-child(5)').css('width', '150px');
                },
                footer: true
            },
        ],
        ajax: {
            url: "GenerateCommonBalanceSheet",
            data: { fromDate: fromDate.toISOString(), toDate: toDate.toISOString(), instituteId: forInstId },
            dataSrc: function (model) {
                $('#spnTotalCredit').text(model.TotalCredit);
                $('#spnToalDebit').text(model.TotalDebit);
                return model.balanceSheets;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] }],
        columns: [
            {
                name: "",
                data: "SeriallId",
                visible: false
            },
            {
                name: "Funds and Liabilities",
                render: function (data, type, row) {
                    if (row.AccountGroupName1 != null && row.AccountGroupName1 !='Difference') {
                        //var link = $("<a>");
                        //link.attr("href", "#");
                        //link.attr("class", row.AccountGroup1ClassName);
                        //link.attr("title", "Click here to view more details");
                        //link.attr("onclick", "bindTransactionDetails('" + row.AccountGroupId1 + "','" + row.AccountGroupName1 + "','" + fromDate + "','" + toDate + "','" + row.AccountGroup1ClassName + "','" + forInstId + "')");
                        //link.text(row.AccountGroupName1);
                        //var html = link[0].outerHTML;
                        //return html;
                        return '<span class="' + row.ClassName + '">' + row.AccountGroupName1 + '</span>';
                    }
                    else if (row.AccountGroupName1 == 'Difference')
                        return '<span class="' + row.AccountGroup1ClassName + '">' + row.AccountGroupName1 + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Credit1",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Credit1 != null)
                        return '<span class="' + row.AccountGroup1ClassName + '">' + parseFloat(row.Credit1).toFixed(2) + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Credit2",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Credit2 != null)
                        return '<span class="' + row.AccountGroup1ClassName + '">' + parseFloat(row.Credit2).toFixed(2) + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Assets and Properties",
                render: function (data, type, row) {
                    if (row.AccountGroupName2 != null) {
                        //var link = $("<a>");
                        //link.attr("href", "#");
                        //link.attr("class", row.AccountGroup2ClassName);
                        //link.attr("title", "Click here to view more details");
                        //link.attr("onclick", "bindTransactionDetails('" + row.AccountGroupId2 + "','" + row.AccountGroupName2 + "','" + fromDate + "','" + toDate + "','" + row.AccountGroup2ClassName + "','" + forInstId + "')");
                        //link.text(row.AccountGroupName2);
                        //var html = link[0].outerHTML;
                        //return html;
                        return '<span class="' + row.ClassName + '">' + row.AccountGroupName2 + '</span>';
                    }
                    else
                        return '';
                }
            },
            {
                name: "Debit1",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Debit1 != null)
                        return '<span class="' + row.AccountGroup2ClassName + '">' + parseFloat(row.Debit1).toFixed(2) + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Debit2",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Debit2 != null)
                        return '<span class="' + row.AccountGroup2ClassName + '">' + parseFloat(row.Debit2).toFixed(2) + '</span>';
                    else
                        return '';
                }
            }
        ]

    });

}
function bindTransactionDetails(accountGroupId, accountGroupName, fromDate, toDate, className, instituteId) {
    toDate = new Date(toDate);
    fromDate = new Date(fromDate);
    $('#spnOpenBalance').text('');
    $('#spnCredit').text('');
    $('#spnDebit').text('');
    $('#spnClosingBalance').text('');
    $('#spnLedgerName').text('');
    $('#spnGrpTotalCredit').text('');
    $('#spnGrpTotalDebit').text('');
    $('#spnGroupName').text('');

    var isLedger = true;
    if (className == 'blackcolor') {
        isLedger = false;
    }
    if (isLedger) {
        bindLedgerSummaryRepory(fromDate, toDate, accountGroupId, instituteId, accountGroupName);
    }
    else {
        bindGroupSummaryRepory(accountGroupId, accountGroupName, toDate, instituteId, fromDate);
    }
}

function bindGroupSummaryRepory(accountGroupId, accountGroupName, toDate, instituteId, fromDate) {
    if ($.fn.DataTable.isDataTable("#tblGroupSummaryReport")) {
        $('#tblGroupSummaryReport').DataTable().draw();
        $('#tblGroupSummaryReport').DataTable().destroy();
        $('#tblGroupSummaryReport tbody').empty();
    }
    $('#tblGroupSummaryReport').DataTable({
        bProcessing: true,
        pageLength: 100,
        autoWidth: false,
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
        buttons: [
            {
                extend: 'excelHtml5',
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3]
                }
            },
            {
                title: '',
                pageSize: 'A4',
                extend: 'pdfHtml5',
                filename: 'Account Group Summary_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
                title: 'Export',
                header: true,
                customize: function (doc) {
                    doc.content.splice(0, 1, {
                        text: [{
                            text: 'Khalsa College Charitable Society, Amritsar \n',
                            bold: true,
                            fontSize: 14
                        }, {
                            text: userData.InstName + ' \n',
                            bold: true,
                            fontSize: 11
                        }, {
                            text: 'Account Group Name: ' + accountGroupName + '(' + fromDate.getDate() + ' / ' + (fromDate.getMonth() + 1) + ' / ' + fromDate.getFullYear() + ' - ' + toDate.getDate() + ' / ' + (toDate.getMonth() + 1) + ' / ' + toDate.getFullYear() + ')',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });
                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    doc.content[1].table.widths = ['60%', '20%', '20%'];
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][2].alignment = 'right';
                    };
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                {
                                    alignment: 'right',
                                    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                }
                            ],
                            margin: 20
                        }
                    });
                    var objLayout = {};
                    objLayout['hLineWidth'] = function (i) { return .5; };
                    objLayout['vLineWidth'] = function (i) { return .5; };
                    objLayout['hLineColor'] = function (i) { return '#aaa'; };
                    objLayout['vLineColor'] = function (i) { return '#aaa'; };
                    objLayout['paddingLeft'] = function (i) { return 4; };
                    objLayout['paddingRight'] = function (i) { return 4; };
                    doc.content[1].layout = objLayout;
                },
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3]
                },
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>Group Name:" + accountGroupName + " From (" + fromDate.getDate() + " / " + (fromDate.getMonth() + 1) + " / " + fromDate.getFullYear() + " - " + toDate.getDate() + " / " + (toDate.getMonth() + 1) + " / " + toDate.getFullYear() + ")</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2),table tr th:nth-child(3)')
                        .addClass('align-right');

                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(3)')
                        .addClass('align-right');
                }
            },
        ],
        ajax: {
            type: "POST",
            url: "BindGroupSummaryReport",
            data: { accountGroupId: accountGroupId, accountGroupName: accountGroupName, toDate: toDate.toISOString() },
            dataSrc: function (model) {
                $('#spnGrpTotalCredit').text(model.TotalCredit);
                $('#spnGrpTotalDebit').text(model.TotalDebit);
                $('#spnGroupName').text(accountGroupName);
                return model.finamncialReportViews;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3] }],
        columns: [
            {
                name: "SerialId",
                data: "SerialId",
                visible: false
            },
            {
                name: "Particulars",
                render: function (data, type, row) {
                    if (row.AccountGroupName != null && row.IsLedger == false) {
                        var link = $("<a>");
                        link.attr("href", "#");
                        link.attr("class", row.ClassName);
                        link.attr("title", "Click here to view more details");
                        link.attr("onclick", "bindSchGroupSummary('" + row.AccountGroupId + "','" + row.AccountGroupName + "','" + toDate + "','" + instituteId + "','" + fromDate + "')");
                        link.text(row.AccountGroupName);
                        var html = link[0].outerHTML;
                        return html;
                    }
                    else if (row.AccountGroupName != null && row.IsLedger == true) {
                        var link = $("<a>");
                        link.attr("href", "#");
                        link.attr("class", row.ClassName);
                        link.attr("title", "Click here to view more details");
                        link.attr("onclick", "bindLedgerSummaryRepory('" + fromDate + "','" + toDate + "','" + row.AccountGroupId + "','" + instituteId + "','" + row.AccountGroupName + "')");
                        link.text(row.AccountGroupName);
                        var html = link[0].outerHTML;
                        return html;
                    }
                    else
                        return '';
                }
            },
            {
                name: "Debit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Debit != null)
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Debit).toFixed(2) + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Credit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Credit != null)
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Credit).toFixed(2) + '</span>';
                    else
                        return '';
                }
            }
        ]

    });
    $('#dvGroupSummaryRpt').modal('show');
}
function bindLedgerSummaryRepory(fromDate, toDate, ledgerId, instituteId, accountGroupName) {
    toDate = new Date(toDate);
    fromDate = new Date(fromDate);
    if ($.fn.DataTable.isDataTable("#tblSingleLedgerReport")) {
        $('#tblSingleLedgerReport').DataTable().draw();
        $('#tblSingleLedgerReport').DataTable().destroy();
        $('#tblSingleLedgerReport tbody').empty();
    }
    $('#tblSingleLedgerReport').DataTable({
        bProcessing: true,
        pageLength: 50,
        autoWidth: false,
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
        buttons: [
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                footer: true
            },
            {
                title: '',
                pageSize: 'A4',
                orientation: 'landscape',
                extend: 'pdfHtml5',
                filename: 'Ledger_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
                title: 'Export',
                header: true,
                customize: function (doc) {
                    doc.content.splice(0, 1, {
                        text: [{
                            text: 'Khalsa College Charitable Society, Amritsar \n',
                            bold: true,
                            fontSize: 14
                        }, {
                            text: userData.InstName + ' \n',
                            bold: true,
                            fontSize: 11
                        }, {
                            text: 'Ledger Title:' + accountGroupName + '(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    doc.content[1].table.widths = ['10%', '8%', '13%', '15%', '10%', '10%', '10%', '10%', '8%'];
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][5].alignment = 'right';
                        doc.content[1].table.body[i][6].alignment = 'right';
                        doc.content[1].table.body[i][7].alignment = 'right';
                    };
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                {
                                    alignment: 'right',
                                    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                }
                            ],
                            margin: 20
                        }
                    });
                    var objLayout = {};
                    objLayout['hLineWidth'] = function (i) { return .5; };
                    objLayout['vLineWidth'] = function (i) { return .5; };
                    objLayout['hLineColor'] = function (i) { return '#aaa'; };
                    objLayout['vLineColor'] = function (i) { return '#aaa'; };
                    objLayout['paddingLeft'] = function (i) { return 4; };
                    objLayout['paddingRight'] = function (i) { return 4; };
                    doc.content[1].layout = objLayout;
                },
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>Ledger Title:" + accountGroupName + " From (" + fromDate.getDate() + " / " + (fromDate.getMonth() + 1) + " / " + fromDate.getFullYear() + " - " + toDate.getDate() + " / " + (toDate.getMonth() + 1) + " / " + toDate.getFullYear() + ")</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(9),table tr th:nth-child(10)')
                        .addClass('align-right');

                    $(win.document.body).find('table tr td:nth-child(9),table tr td:nth-child(10)')
                        .addClass('align-right');
                },
                footer: true
            },
        ],
        ajax: {
            type: "POST",
            url: "SingleLedgerAccountStatement",
            data: { fromDate: fromDate.toISOString(), toDate: toDate.toISOString(), ledgerId: ledgerId, instituteId: instituteId },
            dataSrc: function (model) {
                $('#spnOpenBalance').text(model.OpeningBalance);
                $('#spnCredit').text(model.TotalCredit);
                $('#spnDebit').text(model.TotalDebit);
                $('#spnClosingBalance').text(model.ClosingBalance);
                $('#spnLedgerName').text(accountGroupName.trim());
                return model.accountBooksReports;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }],
        columns: [
            {
                name: "",
                data: "SerialNo",
                visible: false
            },
            {
                name: "Date",
                render: function (data, type, row) {
                    if (row.TransactionDate != null)
                        return '<span class="' + row.ClassName + '">' + row.TransactionDate + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "V.Type",
                render: function (data, type, row) {
                    if (row.VoucherTypeName != null)
                        return '<span class="' + row.ClassName + '">' + row.VoucherTypeName + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "V.No",
                render: function (data, type, row) {
                    if (row.VoucherNo != null) {
                        return '<span class="' + row.ClassName + '">' + row.VoucherNo + '</span>';
                    }
                    else
                        return '';
                }
            },
            {
                name: "Cheque No",
                render: function (data, type, row) {
                    if (row.ChequeNo != null)
                        return '<span class="' + row.ClassName + '">' + row.ChequeNo + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Account",
                className: "ledgerAccountName",
                render: function (data, type, row) {
                    if (row.ChildLedgerName != null && row.ChildLedgerName != '') {
                        var link = $("<a>");
                        link.attr("href", "#");
                        link.attr("class", row.ClassName);
                        link.attr("title", "Click here to view transaction details");
                        link.attr("onclick", "VocherNoClick('" + row.TransactionMasterId + "')");
                        link.text(row.ChildLedgerName);
                        var html = link[0].outerHTML;
                        return html;
                    }
                    else
                        return '';
                }
                
            }
            ,
            {
                name: "Debit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Debit != null)
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Debit).toFixed(2) + '</span>';
                    else
                        return '';
                }
            }
            ,
            {
                name: "Credit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Credit != null)
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Credit).toFixed(2) + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Balance",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Balance != null)
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Balance).toFixed(2) + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Narration",
                render: function (data, type, row) {
                    if (row.MasterNarration != null)
                        return '<span class="' + row.ClassName + '">' + row.MasterNarration + '</span>';
                    else
                        return '';
                }
            }
        ]

    });
    $('#dvLedgerSummaryRpt').modal('show');
}

function bindSchGroupSummary(accountGroupId, accountGroupName, toDate, instituteId, fromDate) {
    toDate = new Date(toDate);
    fromDate = new Date(fromDate);
    if ($.fn.DataTable.isDataTable("#tblGroupSCHSummaryReport")) {
        $('#tblGroupSCHSummaryReport').DataTable().draw();
        $('#tblGroupSCHSummaryReport').DataTable().destroy();
        $('#tblGroupSCHSummaryReport tbody').empty();
    }
    $('#tblGroupSCHSummaryReport').DataTable({
        bProcessing: true,
        pageLength: 100,
        autoWidth: false,
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
        buttons: [
            {
                extend: 'excelHtml5',
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3]
                }
            },
            {
                title: '',
                pageSize: 'A4',
                extend: 'pdfHtml5',
                filename: 'Account Group_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
                title: 'Export',
                header: true,
                customize: function (doc) {
                    doc.content.splice(0, 1, {
                        text: [{
                            text: 'Khalsa College Charitable Society, Amritsar \n',
                            bold: true,
                            fontSize: 14
                        }, {
                            text: userData.InstName + ' \n',
                            bold: true,
                            fontSize: 11
                        }, {
                            text: ' Account Group Name: ' + accountGroupName + '(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    doc.content[1].table.widths = ['60%', '15%', '15%'];
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][2].alignment = 'right';
                    };
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                {
                                    alignment: 'right',
                                    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                }
                            ],
                            margin: 20
                        }
                    });
                    var objLayout = {};
                    objLayout['hLineWidth'] = function (i) { return .5; };
                    objLayout['vLineWidth'] = function (i) { return .5; };
                    objLayout['hLineColor'] = function (i) { return '#aaa'; };
                    objLayout['vLineColor'] = function (i) { return '#aaa'; };
                    objLayout['paddingLeft'] = function (i) { return 4; };
                    objLayout['paddingRight'] = function (i) { return 4; };
                    doc.content[1].layout = objLayout;
                },
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>Group Name:" + accountGroupName + "</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2),table tr th:nth-child(3)')
                        .addClass('align-right');

                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(3)')
                        .addClass('align-right');
                },
            },
        ],
        ajax: {
            type: "POST",
            url: "BindGroupSchSummaryReport",
            data: { instituteId: instituteId, accountGroupId: accountGroupId, accountGroupName: accountGroupName, toDate: toDate.toISOString() },
            dataSrc: function (model) {
                $('#spnschGrpTotalCredit').text(model.TotalCredit);
                $('#spnschGrpTotalDebit').text(model.TotalDebit);
                $('#spnschGroupName').text(accountGroupName);
                return model.finamncialReportViews;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3] }],
        columns: [
            {
                name: "SerialId",
                data: "SerialId",
                visible: false
            },
            {
                name: "Particulars",
                render: function (data, type, row) {
                    if (row.AccountGroupName != null) {
                        //   return '<span class="' + row.ClassName + '">' + row.AccountGroupName + '</span>';
                        var link = $("<a>");
                        link.attr("href", "#");
                        link.attr("class", row.ClassName);
                        link.attr("title", "Click here to view more details");
                        link.attr("onclick", "bindLedgerSummaryRepory('" + fromDate + "','" + toDate + "','" + row.AccountGroupId + "','" + instituteId + "','" + row.AccountGroupName + "')");
                        link.text(row.AccountGroupName);
                        var html = link[0].outerHTML;
                        return html;
                    }
                    else
                        return '';
                }
            },
            {
                name: "Debit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Debit != null)
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Debit).toFixed(2) + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Credit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Credit != null)
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Credit).toFixed(2) + '</span>';
                    else
                        return '';
                }
            }
        ]

    });
    $('#dvGroupSchSummaryRpt').modal('show');
}


function VocherNoClick(transactionMasterId) {
    if (transactionMasterId != '0') {
        detailsloadVocherTypeDropdown();
        detailsloadOrgNamesDropdown();
        loadTansactionsDetails(transactionMasterId);
        $($.fn.dataTable.tables(true)).DataTable()
            .columns.adjust();
        $('#transactionModel').modal('show');
    }
    else {
        alert("Vocher No not found");
    }
}
function loadTansactionsDetails(transactionId) {
    var date = '', VoucherNo = '', ChequeNo = '', VoucherType = '';
    if ($.fn.DataTable.isDataTable("#tblNewLedger")) {
        $('#tblNewLedger').DataTable().draw();
        $('#tblNewLedger').DataTable().destroy();
        $('#tblNewLedger tbody').empty();
    }
    $('#tblNewLedger').DataTable({
        bProcessing: true,
        pageLength: 50,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-3'l><'col-sm-4 text-center'f><'col-sm-5'B>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5] }],
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        buttons: [
            {
                extend: 'pdfHtml5',
                footer: true,
                title: 'Khalsa College Charitable Society, Amritsar',
                message: userData.InstName,
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                customize: function (doc) {
                    var rowCount = doc.content[2].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[2].table.body[i][7].alignment = 'right';
                        doc.content[2].table.body[i][8].alignment = 'right';
                    };
                }
            },
            {
                extend: 'print',
                footer: true,
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(8),table tr th:nth-child(9)').addClass('align-right');
                    $(win.document.body).find('table tr td:nth-child(8),table tr td:nth-child(9)')
                        .addClass('align-right');
                    // $(win.document.body).find('table tr th:nth-child(3),table tr td:nth-child(3)').css('width', '200px');

                }
            },

        ],
        ajax: {
            url: "TransactionMasterAndDetailByVoucherNo",
            type: "POST",
            data: { transactionMasterId: transactionId },
            dataSrc: function (json) {
                if (json.ledgerViewModels == null) {
                    alert("Vocher No not found. Please enter valid vocher no");
                    return false;
                }
                else {
                    $('#ddlVocherTypes').val(json.VoucherTypeId);
                    var transactionDate = new Date(parseInt(json.TransactionDate.substr(6)));
                    var todayDate = new Date();
                    transactionDate = transactionDate.getDate() + "." + (transactionDate.getMonth() + 1) + "." + transactionDate.getFullYear();
                    var splitArray = json.VoucherNo.split("/");
                    $('#txtMasterId').val(json.TransactionMasterId);
                    $('#txtDate').val(transactionDate);
                    $('#txtVoucherNo').val(splitArray[0] + "/" + $('#txtDate').val() + "/" + splitArray[2]);
                    $('#txtChequeOrCash').val(json.ChequeNo);
                    $('#txtNarration').val(json.MasterNarration);

                    date = transactionDate;
                    VoucherNo = splitArray[0] + "/" + $('#txtDate').val() + "/" + splitArray[2];
                    VoucherType = $("#ddlVocherTypes option:selected").text();
                    ChequeNo = json.ChequeNo;
                    return json.ledgerViewModels;
                }
            }
        },
        columns: [
            {
                name: "Select",
                render: function () {
                    return "<input type='checkbox' name='record'>";
                },
                visible: false
            },
            {
                name: "Date",
                render: function () {
                    return date;
                },
                visible: false
            },
            {
                name: "V.Type",
                render: function () {
                    return VoucherType;
                },
                visible: false
            },
            {
                name: "V.No",
                render: function () {
                    return VoucherNo;
                },
                visible: false
            },
            {
                name: "Cheque No./Cash",
                render: function () {
                    return ChequeNo;
                },
                visible: false
            },
            {
                name: "Cr/Dr",
                render: function (data, type, row) {
                    if (row.Debit > 0)
                        return 'D';
                    else
                        return 'C';
                },
                visible: false
            },
            { data: "LedgerId", name: "Ledger Id" },
            { data: "LedgerName", name: "LedgerName" },
            {
                name: "Debit", className: "align-right",
                data: function (row, type, val, meta) {
                    return parseFloat(row.Debit).toFixed(2);
                }
            },
            {
                name: "Credit", className: "align-right",
                data: function (row, type, val, meta) {
                    return parseFloat(row.Credit).toFixed(2);
                }
            },
        ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            // converting to interger to find total
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;
            };

            // computing column Total of the complete result 
            var debitTotal = api
                .column(8)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var creditTotal = api
                .column(9)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $('#lblDebitAmount').text(parseFloat(debitTotal).toFixed(2));
            $('#lblCreditAmount').text(parseFloat(creditTotal).toFixed(2));
        }
    });
}

function detailsloadVocherTypeDropdown() {
    $.ajax({
        type: "GET",
        url: '/Transactions/GetVoucherTypes',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            var optionhtml = '<option value=""></option>';
            $("#ddlVocherTypes").append(optionhtml);
            $.each(data, function (i) {
                var optionhtml = '<option value="' +
                    data[i].VoucherTypeId + '">' + data[i].VoucherTypeName + '</option>';
                $("#ddlVocherTypes").append(optionhtml);
            });
        },
        error: function (error) { console.log(error); }
    });
}
function detailsloadOrgNamesDropdown() {
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
            $(".ddlInstitute").append(optionhtml);
            $.each(data, function (i) {
                if (userData.InstituteId == data[i].Inst_Id) {
                    var optionhtml = '<option selected="selected" value="' +
                        data[i].Inst_Id + '">' + data[i].Inst_ShortTitle + '</option>';
                }
                else {
                    var optionhtml = '<option value="' +
                        data[i].Inst_Id + '">' + data[i].Inst_ShortTitle + '</option>';
                }
                $(".ddlInstitute").append(optionhtml);
            });
            $(".ddlInstitute").prop("disabled", true);
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
