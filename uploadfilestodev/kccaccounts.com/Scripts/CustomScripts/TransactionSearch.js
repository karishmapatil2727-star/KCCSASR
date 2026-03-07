var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);

        $('#btnViewtransaction').on('click', function () {
            searchTransaction($('#txtSeachValue').val());
        });
    });
});
function searchTransaction(searchValue) {
    if ($.fn.DataTable.isDataTable("#tblVocherList")) {
        $('#tblVocherList').DataTable().draw();
        $('#tblVocherList').DataTable().destroy();
        $('#tblVocherList tbody').empty();
    }
    $('#tblVocherList').DataTable({
        bProcessing: true,
        pageLength: 50,
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        dom:
            "<'row'<'col-sm-3'l><'col-sm-4 text-center'f><'col-sm-5'B>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
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
                message:userData.InstName,            
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
                    doc.defaultStyle.fontSize = 9;
                    doc.styles.tableHeader.fontSize = 9;
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
                    columns: [0, 1, 2, 3, 4, 5, 6]
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
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                customize: function (win) {
                    $(win.document.body).find('table th td:nth-child(6),table th td:nth-child(7)')
                        .addClass('align-right');

                    $(win.document.body).find('table tr td:nth-child(6),table tr td:nth-child(7)')
                        .addClass('align-right');
                },
                footer: true
            },
        ],
        ajax: {
            url: "GetLedgerVoucherBykeywordorList",
            data: { searchValue: searchValue },
            dataSrc: function (json) {
                $('#spnDebitAmount').text(parseFloat(json.transactionAmount.TotalDebitAmount).toFixed(2));
                $('#spnCreditAmount').text(parseFloat(json.transactionAmount.TotalCreditAmount).toFixed(2));
                return json.transactionSearchLedgers;
            }
        },
        aaSorting: [[6, 'asc']],
        columns: [
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
