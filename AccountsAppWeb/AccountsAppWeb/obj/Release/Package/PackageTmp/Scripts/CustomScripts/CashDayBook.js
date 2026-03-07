var userData = {};
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

        if (userData.InstituteId == 300010) {
            $("#ddlInstitute").attr("disabled", false);
        }
        else {
            $("#ddlInstitute").prop("disabled", true);
        }
        var today = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
        $('#txtStaringDate').datepicker('setDate', startDate);
        $('#txtEndingDate').datepicker('setDate', today);

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
            bindCashDayBookReporttoGrid(ConverttoDate(fromDate), ConverttoDate(toDate), forInstId);
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
function bindCashDayBookReporttoGrid(fromDate, toDate, forInstId) {
    if ($.fn.DataTable.isDataTable("#tblCashDayBookReport")) {
        $('#tblCashDayBookReport').DataTable().draw();
        $('#tblCashDayBookReport').DataTable().destroy();
        $('#tblCashDayBookReport tbody').empty();
    }
    $('#tblCashDayBookReport').DataTable({
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
                footer: true,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                }
            },
            {
                text: 'PDF',
                action: function (e, dt, node, config) {
                    window.location.href = '/Reports/GenerateAccountDayBookReportPdf';
                }
            }
            //{
            //    extend: 'pdfHtml5',
            //    title: '',
            //    message: userData.InstName + ', Cash/Day Book From (' + fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + ' - ' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear() + ')',
            //    orientation: 'landscape',
            //    pageSize: 'LEGAL',
            //    customize: function (doc) {
            //        doc['header'] = (function () {
            //            return {
            //                columns: [
            //                    {
            //                        alignment: 'center',
            //                        fontSize: 14,
            //                        text: 'Khalsa College Charitable Society, Amritsar',
            //                    }
            //                ],
            //                margin: 20
            //            }
            //        });
            //        doc.pageMargins = [20, 60, 20, 30];
            //        doc.defaultStyle.fontSize = 7;
            //        doc.styles.tableHeader.fontSize = 7;
            //        doc.styles['td:nth-child(0),td:nth-child(5)'] = {
            //            width: '10%'
            //        }

            //        var objLayout = {};
            //        objLayout['hLineWidth'] = function (i) { return .5; };
            //        objLayout['vLineWidth'] = function (i) { return .5; };
            //        objLayout['hLineColor'] = function (i) { return '#aaa'; };
            //        objLayout['vLineColor'] = function (i) { return '#aaa'; };
            //        objLayout['paddingLeft'] = function (i) { return 4; };
            //        objLayout['paddingRight'] = function (i) { return 4; };
            //        doc.content[0].layout = objLayout;
            //        var rowCount = doc.content[1].table.body.length;
            //        for (i = 0; i < rowCount; i++) {
            //            doc.content[1].table.body[i][3].alignment = 'right';
            //            doc.content[1].table.body[i][4].alignment = 'right';
            //            doc.content[1].table.body[i][8].alignment = 'right';
            //            doc.content[1].table.body[i][9].alignment = 'right';
            //        };
            //    },
            //    footer: true,
            //    exportOptions: {
            //        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            //    }
            //},
            //{
            //    extend: 'print',
            //    title: '',
            //    message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
            //        "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + ", Cash/Day BookFrom (" + fromDate.getDate() + "/" + (fromDate.getMonth() + 1) + "/" + fromDate.getFullYear() + " -" + toDate.getDate() + "/" + (toDate.getMonth() + 1) + "/" + toDate.getFullYear() + ") </h4></div>",
            //    orientation: 'landscape',
            //    pageSize: 'LEGAL',
            //    exportOptions: {
            //        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            //    },
            //    customize: function (win) {
            //        $(win.document.body).find('table tr th:nth-child(4)').css('text-align', 'right');
            //        $(win.document.body).find('table tr th:nth-child(5)').css('text-align', 'right');
            //        $(win.document.body).find('table tr th:nth-child(9)').css('text-align', 'right');
            //        $(win.document.body).find('table tr th:nth-child(10)').css('text-align', 'right');

            //        $(win.document.body).find('table tr th:nth-child(1)').css('width', '100px');
            //        $(win.document.body).find('table tr td:nth-child(1)').css('width', '100px');
            //        $(win.document.body).find('table tr th:nth-child(6)').css('width', '100px');
            //        $(win.document.body).find('table tr td:nth-child(6)').css('width', '100px');

            //        $(win.document.body).find('table tr td:nth-child(4),table tr td:nth-child(5),table tr td:nth-child(9),table tr td:nth-child(10)')
            //            .addClass('align-right');              
            //    },
            //},
        ],
        ajax: {
            type: "POST",
            url: "GenerateAccountDayBook",
            data: { fromDate: fromDate.toISOString(), toDate: toDate.toISOString(), instituteId: forInstId },
            dataSrc: function (dayReport) {
                return dayReport;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }],
        columns: [
            {
                name: "SerialId",
                data: "SerialNo",
                visible: false
            },
            {
                name: "Particulars",
                render: function (data, type, row) {
                    if (row.LedgerName1 != null && row.LedgerName1 != '')
                        return '<span>' + row.LedgerName1 + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Type",
                render: function (data, type, row) {
                    if (row.VoucherTypeName1 != null && row.VoucherTypeName1 != '')
                        return '<span>' + row.VoucherTypeName1 + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Vocher No",
                render: function (data, type, row) {
                    if (row.VoucherNo1 != null && row.VoucherNo1 != '')
                        return '<span>' + row.VoucherNo1 + '</span>';
                    else
                        return '';
                }
            }, {
                name: "Cash Amount",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.CashCredit != null)
                        return '<span>' + row.CashCredit + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Credit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Credit != null)
                        return '<span>' + row.Credit + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Particulars",
                render: function (data, type, row) {
                    if (row.LedgerName2 != null && row.LedgerName2 != '')
                        return '<span>' + row.LedgerName2 + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Type",
                render: function (data, type, row) {
                    if (row.VoucherTypeName2 != null && row.VoucherTypeName2 != '')
                        return '<span>' + row.VoucherTypeName2 + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Vocher No",
                render: function (data, type, row) {
                    if (row.VoucherNo2 != null && row.VoucherNo2 != '')
                        return '<span>' + row.VoucherNo2 + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Cash Amount",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.CashDebit != null)
                        return '<span>' + row.CashDebit + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Debit",
                className: "align-right",
                render: function (data, type, row) {
                    if (row.Debit != null)
                        return '<span>' + row.Debit + '</span>';
                    else
                        return '';
                }
            }
        ],
        fnRowCallback: function (nRow, aData, iDisplayIndex) {
            if (aData.TransactionMasterId1 == '0' && aData.TransactionMasterId2 == '0' && aData.LedgerName1 == '' && aData.LedgerName2 == '') {
                $(nRow).addClass(aData.ClassName1);
            }
            else if (aData.TransactionMasterId1 == '0' && aData.TransactionMasterId2 == '0' && aData.LedgerName1 != '') {
                $(nRow).addClass('browncolor');
            }

        },
        createdRow: function (row, data, dataIndex) {
            if (data.TransactionMasterId1 == '0' && data.TransactionMasterId2 == '0' && data.LedgerName1 == '' && data.LedgerName2 == '' && data.VoucherTypeName1 != '' && data.VoucherTypeName2 != '') {
                $('td:eq(0)', row).css('display', 'none');
                $('td:eq(1)', row).attr('colspan', 5);
                $('td:eq(1)', row).css('text-align', 'center');
                $('td:eq(2)', row).css('display', 'none');
                $('td:eq(3)', row).css('display', 'none');
                $('td:eq(4)', row).css('display', 'none');

                $('td:eq(5)', row).css('display', 'none');
                $('td:eq(6)', row).attr('colspan', 5);
                $('td:eq(6)', row).css('text-align', 'center');
                $('td:eq(7)', row).css('display', 'none');
                $('td:eq(8)', row).css('display', 'none');
                $('td:eq(9)', row).css('display', 'none');
            }
            else if (data.TransactionMasterId1 == '0' && data.TransactionMasterId2 == '0' && data.LedgerName1 != '' && data.LedgerName1 != 'Total' && data.LedgerName1 != 'Grand Total') {
                $('td:eq(0)', row).attr('colspan', 10);
                $('td:eq(1)', row).css('display', 'none');
                $('td:eq(2)', row).css('display', 'none');
                $('td:eq(3)', row).css('display', 'none');
                $('td:eq(4)', row).css('display', 'none');
                $('td:eq(5)', row).css('display', 'none');
                $('td:eq(6)', row).css('display', 'none');
                $('td:eq(7)', row).css('display', 'none');
                $('td:eq(8)', row).css('display', 'none');
                $('td:eq(9)', row).css('display', 'none');
            }
        }

    });

}