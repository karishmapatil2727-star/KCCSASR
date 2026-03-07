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
            getCosolidatedRevenueReport(ConverttoDate(fromDate), ConverttoDate(toDate), forInstId);
        });

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $($.fn.dataTable.tables(true)).DataTable()
                .columns.adjust();
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
function getCosolidatedRevenueReport(fromDate, toDate, forInstId) {
    $.ajax({
        type: "POST",
        url: '/Reports/GetGeneralRevenueReport',
        data: JSON.stringify({ 'fromDate': fromDate.toISOString(), 'toDate': toDate.toISOString(), 'instituteId': forInstId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (model) {
            bindConsolidatedRevenue(model.consolidatedGeneralRevenues, fromDate, toDate);
            bindConsolidatedStatement(model.statementRevenues, fromDate, toDate);
            bindStatementShorttoGrid(model.statementRevenues, fromDate, toDate);
            bindScheduleDtoGrid(model.statementRevenues, fromDate, toDate);
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
function bindConsolidatedRevenue(jsonData, fromDate, toDate) {
    if ($.fn.DataTable.isDataTable("#tblIncomeExpenditure")) {
        $('#tblIncomeExpenditure').DataTable().draw();
        $('#tblIncomeExpenditure').DataTable().destroy();
        $('#tblIncomeExpenditure tbody').empty();
    }
    $('#tblIncomeExpenditure').DataTable({
        bProcessing: true,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        data: jsonData,
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
                    columns: [1, 2, 3, 4]
                }
            },
            {
                pageSize: 'A4',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                filename: 'Consolidated General Revenue Report_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Consolidated General Revenue Report From(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });
                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    doc.content[1].table.widths = ['30%', '20%', '30%', '20%'];
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][3].alignment = 'right';
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
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Consolidated General Revenue Report From (" + fromDate.getDate() + "/" + (fromDate.getMonth() + 1) + "/" + fromDate.getFullYear() + " -" + toDate.getDate() + "/" + (toDate.getMonth() + 1) + "/" + toDate.getFullYear() + ")</h4></div> ",
                exportOptions: {
                    columns: [1, 2, 3, 4]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(4)').css('text-align', 'right');

                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(4)')
                        .addClass('align-right');
                }
            },
        ],
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4] }],
        columns: [
            {
                name: "Id",
                data: "Id",
                visible: false

            },
            {
                name: "Name Of Organization",
                render: function (data, type, row) {
                    if (row.AccountGroupName1 != null)
                        return row.AccountGroupName1;
                    else
                        return '';
                }
            },
            {
                name: "Deflict",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Credit != null)
                        return row.Credit;
                    else
                        return '';
                }
            },
            {
                name: "Name Of Organization",
                render: function (data, type, row) {
                    if (row.AccountGroupName2 != null)
                        return row.AccountGroupName2;
                    else
                        return '';
                }
            },
            {
                name: "Surplus",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Debit != null)
                        return row.Debit;
                    else
                        return '';
                }
            }
        ]

    });
}
function bindConsolidatedStatement(jsonData, fromDate, toDate) {
    if ($.fn.DataTable.isDataTable("#tblStatement")) {
        $('#tblStatement').DataTable().draw();
        $('#tblStatement').DataTable().destroy();
        $('#tblStatement tbody').empty();
    }
    $('#tblStatement').DataTable({
        bProcessing: true,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        data: jsonData,
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
                    columns: [1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                pageSize: 'A4',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                filename: 'Consolidated Statement of IE Report_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Consolidated Statement of I&E Report From(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    doc.content[1].table.widths = ['12%', '12%', '12%', '12%', '20%', '12%', '20%'];
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][2].alignment = 'right';
                        doc.content[1].table.body[i][3].alignment = 'right';
                        doc.content[1].table.body[i][4].alignment = 'right';
                        doc.content[1].table.body[i][5].alignment = 'right';
                        doc.content[1].table.body[i][6].alignment = 'right';
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
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Consolidated Statement of I&E Report From (" + fromDate.getDate() + "/" + (fromDate.getMonth() + 1) + "/" + fromDate.getFullYear() + " -" + toDate.getDate() + "/" + (toDate.getMonth() + 1) + "/" + toDate.getFullYear() + ")</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(3)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(4)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(5)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(6)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(7)').css('text-align', 'right');

                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(3),table tr td:nth-child(4), table tr td:nth-child(5), table tr td:nth-child(6), table tr td:nth-child(7)')
                        .addClass('align-right');
                }
            },
        ],
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7] }],
        columns: [
            {
                name: "Id",
                data: "Id",
                visible: false

            },
            {
                name: "Name Of Organization",
                render: function (data, type, row) {
                    if (row.AccountGroupName != null)
                        return row.AccountGroupName;
                    else
                        return '';
                }
            },
            {
                name: "Sch.D",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.SchD != null)
                        return row.SchD;
                    else
                        return '';
                }
            },
            {
                name: "Receipt",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Income != null)
                        return row.Income;
                    else
                        return '';
                }
            },
            {
                name: "Expenses",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Expenses != null)
                        return row.Expenses;
                    else
                        return '';
                }
            }, {
                name: "Expenses Without Sch.D",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.ExpensesSchD != null)
                        return row.ExpensesSchD;
                    else
                        return '';
                }
            },
            {
                name: "Net Saving",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.NetSaving != null)
                        return row.NetSaving;
                    else
                        return '';
                }
            }
            ,
            {
                name: "Net Saving  Without Sch.D",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.NetSavingSchD != null)
                        return row.NetSavingSchD;
                    else
                        return '';
                }
            }
        ]

    });
}
function bindStatementShorttoGrid(jsonData, fromDate, toDate) {
    if ($.fn.DataTable.isDataTable("#tblStatementShort")) {
        $('#tblStatementShort').DataTable().draw();
        $('#tblStatementShort').DataTable().destroy();
        $('#tblStatementShort tbody').empty();
    }
    $('#tblStatementShort').DataTable({
        bProcessing: true,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
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
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                pageSize: 'A4',
                extend: 'pdfHtml5',
                filename: 'Statement of IE Short Report_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Statement of I&E Short Report From(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    doc.content[1].table.widths = ['40%', '15%', '15%', '15%', '15%'];
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][1].alignment = 'right';
                        doc.content[1].table.body[i][2].alignment = 'right';
                        doc.content[1].table.body[i][3].alignment = 'right';
                        doc.content[1].table.body[i][4].alignment = 'right';
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
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Statement of I&E Short Report From (" + fromDate.getDate() + "/" + (fromDate.getMonth() + 1) + "/" + fromDate.getFullYear() + " -" + toDate.getDate() + "/" + (toDate.getMonth() + 1) + "/" + toDate.getFullYear() + ")</h4></div> ",
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(3)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(4)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(5)').css('text-align', 'right');

                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(3),table tr td:nth-child(4),table tr td:nth-child(5)')
                        .addClass('align-right');
                }
            },
        ],
        data: jsonData,
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5] }],
        columns: [
            {
                name: "Id",
                data: "Id",
                visible: false

            },
            {
                name: "Name Of Organization",
                render: function (data, type, row) {
                    if (row.AccountGroupName != null)
                        return row.AccountGroupName;
                    else
                        return '';
                }
            },
            {
                name: "Receipt",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Income != null)
                        return row.Income;
                    else
                        return '';
                }
            },
            {
                name: "Expenses",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Expenses != null)
                        return row.Expenses;
                    else
                        return '';
                }
            },
            {
                name: "Net Saving",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.NetSaving != null)
                        return row.NetSaving;
                    else
                        return '';
                }
            },
            {
                name: "Net Loss",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.NetLoss != null)
                        return row.NetLoss;
                    else
                        return '';
                }
            }
        ]

    });
}
function bindScheduleDtoGrid(jsonData, fromDate, toDate) {
    if ($.fn.DataTable.isDataTable("#tblScheduleD")) {
        $('#tblScheduleD').DataTable().draw();
        $('#tblScheduleD').DataTable().destroy();
        $('#tblScheduleD tbody').empty();
    }
    $('#tblScheduleD').DataTable({
        bProcessing: true,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
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
                    columns: [1, 2]
                }
            },
            {
                pageSize: 'A4',
                extend: 'pdfHtml5',
                filename: 'Schedule-D Report_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Schedule-D Report From(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    doc.content[1].table.widths = ['80%', '20%']
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
                        doc.content[1].table.body[i][1].alignment = 'right';
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
                exportOptions: {
                    columns: [1, 2]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>Schedule-D Report From (" + fromDate.getDate() + "/" + (fromDate.getMonth() + 1) + "/" + fromDate.getFullYear() + " -" + toDate.getDate() + "/" + (toDate.getMonth() + 1) + "/" + toDate.getFullYear() + ")</h4></div> ",
                exportOptions: {
                    columns: [1, 2]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2)').css('text-align', 'right');

                    $(win.document.body).find('table tr td:nth-child(2)')
                        .addClass('align-right');
                }
            },
        ],
        data: jsonData,
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2] }],
        columns: [
            {
                name: "Id",
                data: "Id",
                visible: false

            },
            {
                name: "Name Of Organization",
                render: function (data, type, row) {
                    if (row.AccountGroupName != null)
                        return row.AccountGroupName;
                    else
                        return '';
                }
            },
            {
                name: "Sch.D",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.SchD != null)
                        return row.SchD;
                    else
                        return '';
                }
            }
        ]

    });
}