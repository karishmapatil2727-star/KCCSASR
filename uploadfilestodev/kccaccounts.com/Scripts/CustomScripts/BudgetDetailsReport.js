var userData = {};
var now = new Date();
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadOrgNamesDropdown();
        $('#btnView').on('click', function () {
            var forInstId = $("#ddlInstitute option:selected").val();
            if (forInstId == '') {
                alert('Please select valid institute');
                return false;
            }
            
            BudgetDetailsReportForIncome(forInstId);
            BudgetDetailsReportForExpenditure(forInstId);
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
function BudgetDetailsReportForIncome(instituteId) {
    if ($.fn.DataTable.isDataTable("#tblBudgetDetailsReportInc")) {
        $('tblBudgetDetailsReportInc').DataTable().draw();
        $('#tblBudgetDetailsReportInc').DataTable().destroy();
        $('#tblBudgetDetailsReportInc tbody').empty();
    }
    $('#tblBudgetDetailsReportInc').DataTable({
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
                    columns: [0, 1, 2, 3, 4, 5, 6,7,8,9]
                },
                footer: true
            },
            {
                extend: 'pdfHtml5',
                pageSize: 'A4',
                title: 'Export',
                header: true,
                filename: 'Budget Details Income Report_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Budget Details Income Report',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.content[1].table.widths = ['15%', '15%', '13%', '10%', '9%', '9%', '9%', '9%', '9%', '9%'];
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
                    columns: [0, 1, 2, 3, 4, 5, 6,7,8,9]
                },
                footer: true
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Budget Details Income Report</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6,7,8,9]
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
            type: "GET",
            url: "GetBudgetDetailsForIncome",
            data: {instituteId: instituteId },
            dataSrc: function (budgetDetailsIncList) {
                return budgetDetailsIncList;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6,7,8,9] }],
        columns: [
            { data: "LedgerName", name: "Ledger Name" },
            { data: "Inst_ShortTitle", name: "Org.Name" },
            { data: "BD_BudgetAmount", name: "Total Budget(Current Financial Year)" },
            { data: "BD_BudgetAmount_8_Months", name: "8 Months Transaction" },
            { data: "BD_BudgetAmount_4_Months", name: "4 Months Total Budget" },
            { data: "PBD_Financial1BudgetAmount", name: "2016-17 Total Budget" },
            { data: "PBD_Financial2BudgetAmount", name: "2017-18 Total Budget" },
            { data: "PBD_Financial3BudgetAmount", name: "2018-19 Total Budget" },
            { data: "PBD_Financial4BudgetAmount", name: "2019-20 Total Budget" },
            { data: "BD_NetTotal", name: "Net.Total" },
        ]

    });
}
function BudgetDetailsReportForExpenditure(instituteId) {
    if ($.fn.DataTable.isDataTable("#tblBudgetDetailsReportExp")) {
        $('tblBudgetDetailsReportExp').DataTable().draw();
        $('#tblBudgetDetailsReportExp').DataTable().destroy();
        $('#tblBudgetDetailsReportExp tbody').empty();
    }
    $('#tblBudgetDetailsReportExp').DataTable({
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
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                footer: true
            },
            {
                extend: 'pdfHtml5',
                pageSize: 'A4',
                title: 'Export',
                header: true,
                filename: 'Budget Details Expenditure Report_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Budget Details Expenditure Report',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.content[1].table.widths = ['15%', '15%', '13%', '10%', '9%', '9%', '9%', '9%', '9%', '9%'];
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
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                footer: true
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>" +
                    "<div class='row exportoption'><h4 class='text-center'> Budget Details Expenditure Report</h4></div> ",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
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
            type: "GET",
            url: "GetBudgetDetailsForExpenditure",
            data: { instituteId: instituteId },
            dataSrc: function (budgetDetailsIncList) {
                return budgetDetailsIncList;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }],
        columns: [
            { data: "LedgerName", name: "Ledger Name" },
            { data: "Inst_ShortTitle", name: "Org.Name" },
            { data: "BD_BudgetAmount", name: "Total Budget(Current Financial Year)" },
            { data: "BD_BudgetAmount_8_Months", name: "8 Months Transaction" },
            { data: "BD_BudgetAmount_4_Months", name: "4 Months Total Budget" },
            { data: "PBD_Financial1BudgetAmount", name: "2016-17 Total Budget" },
            { data: "PBD_Financial2BudgetAmount", name: "2017-18 Total Budget" },
            { data: "PBD_Financial3BudgetAmount", name: "2018-19 Total Budget" },
            { data: "PBD_Financial4BudgetAmount", name: "2019-20 Total Budget" },
            { data: "BD_NetTotal", name: "Net.Total" },
        ]

    });
}



