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
            bindCloseTrailBalanceSheetstoGrid(ConverttoDate(fromDate), ConverttoDate(toDate), forInstId);
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
function bindCloseTrailBalanceSheetstoGrid(fromDate, toDate, forInstId) {
    if ($.fn.DataTable.isDataTable("#tblOpenTrailBalanceScheduleSheetReport")) {
        $('#tblOpenTrailBalanceScheduleSheetReport').DataTable().draw();
        $('#tblOpenTrailBalanceScheduleSheetReport').DataTable().destroy();
        $('#tblOpenTrailBalanceScheduleSheetReport tbody').empty();
    }
    $('#tblOpenTrailBalanceScheduleSheetReport').DataTable({
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
                    columns: [1, 2, 3]
                },
                footer: true
            },
            {
                extend: 'pdfHtml5',
                title: '',
                extend: 'pdfHtml5',
                pageSize: 'A4',
                filename: 'Open Trail Balance Schedule Wise_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Open Trail Balance (With Schedule Wise) From(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
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
                }
            },          
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Open Trail Balance (Schedule Wise) From (" + fromDate.getDate() + "/" + (fromDate.getMonth() + 1) + "/" + fromDate.getFullYear() + " -" + toDate.getDate() + "/" + (toDate.getMonth() + 1) + "/" + toDate.getFullYear() + ")</h4></div> ",
                orientation: 'landscape',          
                titleAttr: 'Print',
                exportOptions: {
                    columns: [1, 2, 3]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(3)').css('text-align', 'right');

                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(3)')
                        .addClass('align-right');
                },
                footer: true
            },
        ],
        ajax: {
            url: "OpenTrailBalanceScheduleJsonReport",
            data: { fromDate: fromDate.toISOString(), toDate: toDate.toISOString(), instituteId: forInstId },
            dataSrc: function (model) {
                $('#spnTotalCredit').text(model.TotalCredit);
                $('#spnTotalDebit').text(model.TotalDebit);
                return model.finamncialReportViews;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3] }],
        columns: [
            {
                name: "",
                data: "SerialId",
                visible: false
            },
            {
                name: "Particulars",
                render: function (data, type, row) {
                    if (row.AccountGroupName != null) {
                        var link = $("<a>");
                        link.attr("href", "#");
                        link.attr("class", row.ClassName);
                        link.attr("title", "Click here to view more details");
                        link.attr("onclick", "bindTransactionDetails('" + row.AccountGroupId + "','" + row.AccountGroupName + "','" + fromDate.toISOString() + "','" + toDate.toISOString() + "','" + row.ClassName + "','" + forInstId + "')");
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
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Debit).toFixed(2)+ '</span>';
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


    bindGroupSummaryRepory(accountGroupId, accountGroupName, fromDate, instituteId, fromDate);

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
                extend: 'pdfHtml5',
                title: '',
                message: userData.InstName + ', Account Group Name: ' + accountGroupName + "From (" + fromDate.getDate() + " / " + (fromDate.getMonth() + 1) + " / " + fromDate.getFullYear() + " - " + toDate.getDate() + " / " + (toDate.getMonth() + 1) + " / " + toDate.getFullYear() + ")",
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
                    doc.defaultStyle.fontSize = 10;
                    doc.styles.tableHeader.fontSize = 10;
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
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][2].alignment = 'right';
                    };
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
                    else if (row.AccountGroupName != null)
                        return '<span class="' + row.ClassName + '">' + row.AccountGroupName + '</span>';
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
                extend: 'pdfHtml5',
                title: '',
                message: userData.InstName + ', Account Group Name: ' + accountGroupName + " From (" + fromDate.getDate() + " / " + (fromDate.getMonth() + 1) + " / " + fromDate.getFullYear() + " - " + toDate.getDate() + " / " + (toDate.getMonth() + 1) + " / " + toDate.getFullYear() + ")",
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
                    doc.defaultStyle.fontSize = 10;
                    doc.styles.tableHeader.fontSize = 10;
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
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][2].alignment = 'right';
                    };
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
                    if (row.AccountGroupName != null)
                        return '<span class="' + row.ClassName + '">' + row.AccountGroupName + '</span>';
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