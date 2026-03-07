var userData = {};
var now = new Date();
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
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
        $('#dtEndingDate,#txtEndingDate').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            startDate: startDate
        });
        var today = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
        $('#txtEndingDate').datepicker('setDate', today);
        if (userData.InstituteId == 300010) {
            $("#ddlInstitute").attr("disabled", false);
        }
        else {
            $("#ddlInstitute").prop("disabled", true);
        }
        $('#btnView').on('click', function () {
            var fromDate = $('#txtEndingDate').val();
            if (fromDate == '' || !ValidateDate(fromDate)) {
                alert('Please select valid date');
                return false;
            }
            var forInstId = $("#ddlInstitute option:selected").val();
            if (forInstId == '') {
                alert('Please select valid institute');
                return false;
            }
            ReconciliationReportToGrid(ConverttoDate(fromDate), forInstId);
        });
    });
});
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

function ReconciliationReportToGrid(toDate, instituteId) {
    if ($.fn.DataTable.isDataTable("#tblReconciliationReport")) {
        $('#tblReconciliationReport').DataTable().draw();
        $('#tblReconciliationReport').DataTable().destroy();
        $('#tblReconciliationReport tbody').empty();
    }
    $('#tblReconciliationReport').DataTable({
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
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                footer: true
            },
            {
                extend: 'pdfHtml5',
                pageSize: 'A4',
                title: 'Export',
                header: true,
                filename: 'Reconciliation Report_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Reconciliation Report',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.content[1].table.widths = ['15%', '15%', '15%', '10%', '15%', '15%', '15%'];
                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][2].alignment = 'right';
                        doc.content[1].table.body[i][4].alignment = 'right';
                        doc.content[1].table.body[i][5].alignment = 'right';
                        doc.content[1].table.body[i][6].alignment = 'right';
                    };
                    var objLayout = {};
                    objLayout['hLineWidth'] = function (i) { return .5; };
                    objLayout['vLineWidth'] = function (i) { return .5; };
                    objLayout['hLineColor'] = function (i) { return '#aaa'; };
                    objLayout['vLineColor'] = function (i) { return '#aaa'; };
                    objLayout['paddingLeft'] = function (i) { return 4; };
                    objLayout['paddingRight'] = function (i) { return 4; };
                    doc.content[1].layout = objLayout;
                },
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                footer: true
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Reconciliation Report</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(3)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(5)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(6)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(7)').css('text-align', 'right');


                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(3),table tr td:nth-child(5),table tr td:nth-child(6),table tr td:nth-child(7)')
                        .addClass('align-right');
                },
                footer: true
            },
        ],
        ajax: {
            type: "POST",
            url: "ReconciliationSelectAllLedgers",
            data: { toDate: toDate.toISOString(), instituteId: instituteId },
            dataSrc: function (model) {
                return model;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] }],
        columns: [
            {
                name: "My Ledger Name",
                render: function (data, type, row) {
                    if (row.MyLedgerName != null) {
                        var link = $("<a>");
                        link.attr("href", "#");
                        link.attr("title", "Click here to view reconciliation details");
                        link.attr("onclick", "getReconciliationDatails('" + toDate + "','" + row.MyLederId + "','" + row.MyInstId + "','" + row.ToLederId + "','" + row.ToInstId + "','" + row.MyLedgerName + "','" + row.InstLedgerName + "')");
                        link.text(row.MyLedgerName);
                        var html = link[0].outerHTML;
                        return html;
                    }
                    else
                        return '';
                }
            },
            {
                name: "My Debit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.MyDebit != null) {
                        return parseFloat(row.MyDebit).toFixed(2);
                    }
                    else
                        return '';
                }
            },
            {
                name: "My Credit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.MyCredit != null)
                        return parseFloat(row.MyCredit).toFixed(2);
                    else
                        return '';
                }
            },
            {
                name: "Inst Ledger Name",
                render: function (data, type, row) {
                    if (row.InstLedgerName != null) {
                        var link = $("<a>");
                        link.attr("href", "#");
                        link.attr("title", "Click here to view reconciliation details");
                        link.attr("onclick", "getReconciliationDatails('" + toDate + "','" + row.MyLederId + "','" + row.MyInstId + "','" + row.ToLederId + "','" + row.ToInstId + "','" + row.MyLedgerName + "','" + row.InstLedgerName + "')");
                        link.text(row.InstLedgerName);
                        var html = link[0].outerHTML;
                        return html;
                    }
                    else
                        return '';
                }
            }
            ,
            {
                name: "Inst Debit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.InstDebit != null)
                        return parseFloat(row.InstDebit).toFixed(2);
                    else
                        return '';
                }
            }
            ,
            {
                name: "Inst Credit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.InstCredit != null)
                        return parseFloat(row.InstCredit).toFixed(2);
                    else
                        return '';
                }
            },
            {
                name: "Difference",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Difference != null)
                        return parseFloat(row.Difference).toFixed(2);
                    else
                        return '';
                }
            }
        ]

    });
}

function getReconciliationDatails(toDate, myLederId, myInstId, toLederId, toInstId, myLedgerName, toLedgerName) {
    var endDate = new Date(toDate);
    $('#headerMyinstName').text(myLedgerName);
    $('#headerInstinstName').text(toLedgerName);
    $('#reconciliattionModel').modal('show');
    $.ajax({
        type: "POST",
        url: '/Reports/ReconciliationDetailedGrid',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'toDate': endDate.toISOString(), myLedgerId: myLederId, 'MyInstId': myInstId, 'toLedgerId': toLederId, 'toInstId': toInstId }),
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            bindMyLedgerlist(data.MyledgerDetails, myLedgerName);
            bindInstLedgerlist(data.InstLedgerDetails, toLedgerName)
            $('#spnMyBalance').text(data.MyLedgerCreditDebitBalance);
            $('#spnToBalance').text(data.InstLedgerCreditDebitBalance);
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}

function bindMyLedgerlist(responseData, myLedgerName) {
    if ($.fn.DataTable.isDataTable("#tblMyLedgerlist")) {
        $('#tblMyLedgerlist').DataTable().draw();
        $('#tblMyLedgerlist').DataTable().destroy();
        $('#tblMyLedgerlist tbody').empty();
    }
    $('#tblMyLedgerlist').DataTable({
        bProcessing: true,
        pageLength: 50,
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
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                footer: true
            },
            {
                extend: 'pdfHtml5',
                title: '',
                message: userData.InstName + ', Ledger Titile:' + myLedgerName,
                customize: function (doc) {
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
                    doc.defaultStyle.fontSize = 7;
                    doc.styles.tableHeader.fontSize = 7;
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
                        doc.content[1].table.body[i][4].alignment = 'right';
                        doc.content[1].table.body[i][5].alignment = 'right';
                    };
                },
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                footer: true
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Ledger Titile:" + myLedgerName + "</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(5)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(6)').css('text-align', 'right');

                    $(win.document.body).find('table tr td:nth-child(5),table tr td:nth-child(6)')
                        .addClass('align-right');
                },
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                footer: true
            },
        ],
        data: responseData,
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] }],
        columns: [
            {
                name: "Date",
                render: function (data, type, row) {
                    if (row.TransactionDate != null) {
                        return row.TransactionDate;
                    }
                    else
                        return '';
                }
            },
            {
                name: "V.Type",
                render: function (data, type, row) {
                    if (row.VoucherTypeName != null) {
                        return row.VoucherTypeName;
                    }
                    else
                        return '';
                }
            },
            {
                name: "V.No",
                render: function (data, type, row) {
                    if (row.VoucherNo != null)
                        return row.VoucherNo;
                    else
                        return '';
                }
            },
            {
                name: "Cheque No",
                render: function (data, type, row) {
                    if (row.ChequeNo != null) {
                        return row.ChequeNo;
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
                        return row.Debit;
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
                        return parseFloat(row.Credit).toFixed(2);
                    else
                        return '';
                }
            },
            {
                name: "Narration",
                render: function (data, type, row) {
                    if (row.MasterNarration != null)
                        return row.MasterNarration;
                    else
                        return '';
                }
            }
        ]

    });
}

function bindInstLedgerlist(responseData, toLedgerName) {
    if ($.fn.DataTable.isDataTable("#tblToLedgerlist")) {
        $('#tblToLedgerlist').DataTable().draw();
        $('#tblToLedgerlist').DataTable().destroy();
        $('#tblToLedgerlist tbody').empty();
    }
    $('#tblToLedgerlist').DataTable({
        bProcessing: true,
        pageLength: 50,
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
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                footer: true
            },
            {
                extend: 'pdfHtml5',
                title: '',
                message: userData.InstName + ', Ledger Titile:' + toLedgerName,
                customize: function (doc) {
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
                    doc.defaultStyle.fontSize = 7;
                    doc.styles.tableHeader.fontSize = 7;
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
                        doc.content[1].table.body[i][4].alignment = 'right';
                        doc.content[1].table.body[i][5].alignment = 'right';
                    };
                },
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                footer: true
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Ledger Titile:" + toLedgerName + "</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr td:nth-child(5),table tr td:nth-child(6)')
                        .addClass('align-right');
                },
                footer: true
            },
        ],
        data: responseData,
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] }],
        columns: [
            {
                name: "Date",
                render: function (data, type, row) {
                    if (row.TransactionDate != null) {
                        return row.TransactionDate;
                    }
                    else
                        return '';
                }
            },
            {
                name: "V.Type",
                render: function (data, type, row) {
                    if (row.VoucherTypeName != null) {
                        return row.VoucherTypeName;
                    }
                    else
                        return '';
                }
            },
            {
                name: "V.No",
                render: function (data, type, row) {
                    if (row.VoucherNo != null)
                        return row.VoucherNo;
                    else
                        return '';
                }
            },
            {
                name: "Cheque No",
                render: function (data, type, row) {
                    if (row.ChequeNo != null) {
                        return row.ChequeNo;
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
                        return parseFloat(row.Debit).toFixed(2);
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
                        return parseFloat(row.Credit).toFixed(2);
                    else
                        return '';
                }
            },
            {
                name: "Narration",
                render: function (data, type, row) {
                    if (row.MasterNarration != null)
                        return row.MasterNarration;
                    else
                        return '';
                }
            }
        ]

    });
}