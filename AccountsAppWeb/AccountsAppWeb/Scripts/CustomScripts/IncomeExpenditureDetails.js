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
            bindIncomeExpenditureSheetstoGrid(ConverttoDate(fromDate), ConverttoDate(toDate), forInstId);
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
function bindIncomeExpenditureSheetstoGrid(fromDate, toDate, forInstId) {
    if ($.fn.DataTable.isDataTable("#IncomeExpenditureSheet")) {
        $('#IncomeExpenditureSheet').DataTable().draw();
        $('#IncomeExpenditureSheet').DataTable().destroy();
        $('#IncomeExpenditureSheet tbody').empty();
    }
    $('#IncomeExpenditureSheet').DataTable({
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
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4] }],
        buttons: [
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4]
                },
                footer: true
            },
            {
                title: '',
                pageSize: 'A4',
                extend: 'pdfHtml5',
                filename: 'Income and Expenditure Details_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Income&Expenditure Details From(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
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
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>Income&Expenditure Details  From ( " + fromDate.getDate() + "/ " + (fromDate.getMonth() + 1) + "/ " + fromDate.getFullYear() + " -  " + toDate.getDate() + "/ " + (toDate.getMonth() + 1) + "/ " + toDate.getFullYear() + ")</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3, 4]
                },
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(4)').css('text-align', 'right');
                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(4)')
                        .addClass('align-right');
                    $(win.document.body).find('table tr th:nth-child(2),table tr td:nth-child(2),table tr th:nth-child(3),table tr td:nth-child(3)').css('width', '150px');
                    $(win.document.body).find('table tr th:nth-child(4),table tr td:nth-child(4),table tr th:nth-child(5),table tr td:nth-child(5)').css('width', '150px');
                },

            },
        ],
        ajax: {
            url: "GenerateIncomeExpenditureDetails",
            data: { fromDate: fromDate.toISOString(), toDate: toDate.toISOString(), instituteId: forInstId },
            dataSrc: function (model) {
                return model.incomeExpenditures;
            }
        },
        columns: [
            {
                name: "",
                data: "SeriallId",
                visible: false
            },
            {
                name: "Expenditure",
                render: function (data, type, row) {
                    var link = $("<a>");
                    link.attr("href", "#");
                    link.attr("title", "Click here to view more details");
                    link.attr("onclick", "bindExpenditureDetails('" + fromDate + "','" + toDate + "','" + forInstId + "')");
                    link.text(row.AccountGroupName1);
                    var html = link[0].outerHTML;
                    return html;
                }
            },
            {
                name: "Amount",
                className: "align-right",
                data: function (row, type, val, meta) {
                    if (row.Debit != null)
                        return parseFloat(row.Debit).toFixed(2);
                    else
                        return '';
                }
            },
            {
                name: "Income",
                render: function (data, type, row) {
                    var link = $("<a>");
                    link.attr("href", "#");
                    link.attr("title", "Click here to view more details");
                    link.attr("onclick", "bindExpenditureDetails('" + fromDate + "','" + toDate + "','" + forInstId + "')");
                    link.text(row.AccountGroupName2);
                    var html = link[0].outerHTML;
                    return html;
                }
            },
            {
                name: "Amount",
                className: "align-right",
                data: function (row, type, val, meta) {
                    if (row.Credit != null)
                        return parseFloat(row.Credit).toFixed(2);
                    else
                        return '';
                }
            }
        ]
    });
}

function bindExpenditureDetails(fromDate, toDate, instituteId) {
    toDate = new Date(toDate);
    fromDate = new Date(fromDate);
    $('#spnincomeTotalDebit').text('');
    $('#spnincomeTotalCredit').text('');

    $.ajax({
        type: "POST",
        url: '/Reports/CreateLedgerIncomeExpenditute',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ instituteId: instituteId, fromDate: fromDate.toISOString(), toDate: toDate.toISOString() }),
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            bindIncomeExpenditureDetails(data, fromDate, toDate);
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
    $('#dvledgerIncomeExpenditureRpt').modal('show');
}
function bindIncomeExpenditureDetails(result, fromDate, toDate) {
    if ($.fn.DataTable.isDataTable("#tblExpenditureDetails")) {
        $('#tblExpenditureDetails').DataTable().draw();
        $('#tblExpenditureDetails').DataTable().destroy();
        $('#tblExpenditureDetails tbody').empty();
    }
    $('#tblExpenditureDetails').DataTable({
        bProcessing: true,
        pageLength: 50,
        searching: false,
        lengthChange: false,
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
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4] }],
        buttons: [
            {
                extend: 'excelHtml5',
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                title: '',
                pageSize: 'A4',
                extend: 'pdfHtml5',
                filename: 'Income Expenditure_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Income Expenditure From(' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
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
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>Income Expenditure From (" + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + ' - ' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ")</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                footer: true,
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(2)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(4)').css('text-align', 'right');

                    $(win.document.body).find('table tr td:nth-child(2),table tr td:nth-child(4)')
                        .addClass('align-right');
                },
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
        ],
        data: result.ledgerIncomeExpenditures,
        columns: [
            {
                name: "",
                data: "SerialId",
                visible: false
            },
            {
                name: "Expenditure",
                render: function (data, type, row) {
                    if (row.DebitAccountGroupName != null)
                        return '<span class="' + row.ClassName + '">' + row.DebitAccountGroupName + '</span>';
                    else return '';
                }
            },
            {
                name: "Amount",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Debit != null && row.Debit != '')
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Debit).toFixed(2) + '</span>';
                    else return '';
                }
            },
            {
                name: "Income",
                render: function (data, type, row) {
                    if (row.CreditAccountGroupName != null)
                        return '<span class="' + row.ClassName + '">' + row.CreditAccountGroupName + '</span>';
                    else return '';
                }
            },
            {
                name: "Amount",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Credit != null && row.Credit != '')
                        return '<span class="' + row.ClassName + '">' + parseFloat(row.Credit).toFixed(2) + '</span>';
                    else
                        return '';
                }
            }
        ]
    });
    $('#spnincomeTotalCredit').text(result.TotalCredit);
    $('#spnincomeTotalDebit').text(result.TotalDebit);
}


