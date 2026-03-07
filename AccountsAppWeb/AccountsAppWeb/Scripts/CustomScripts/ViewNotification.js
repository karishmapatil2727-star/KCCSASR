var userData = {};
var now = new Date();
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        bindViewNotificationstoGird(0);
        $('#btnShowAll').on('click', function () {
            bindViewNotificationstoGird(1);
        });
    });
});
function bindViewNotificationstoGird(isCompTransaction) {
    if ($.fn.DataTable.isDataTable("#tblViewNotificationReport")) {
        $('#tblViewNotificationReport').DataTable().draw();
        $('#tblViewNotificationReport').DataTable().destroy();
        $('#tblViewNotificationReport tbody').empty();
    }
    $('#tblViewNotificationReport').DataTable({
        bProcessing: true,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        searching: false,
        dom: 'fBrtip<"clear">l',
        columnDefs: [{
            className: "dt-right",
            targets: [0]
        }],
        buttons: [
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8,]
                }
            },
            {
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A4',
                title: 'Export',
                header: true,
                filename: 'Notification Report_' + now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(),
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
                            text: 'Notification Report',
                            bold: true,
                            fontSize: 11
                        }],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });

                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 8;
                    doc.content[1].table.widths = ['10%', '10%', '10%', '13%', '15%', '14%', '14%', '14%'];
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 0; i < rowCount; i++) {
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
                    columns: [1, 2, 3, 4, 5, 6, 7, 8,]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + ", Notification Report </h4></div>",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                customize: function (win) {
                    $(win.document.body).find('table tr th:nth-child(7)').css('text-align', 'right');
                    $(win.document.body).find('table tr th:nth-child(8)').css('text-align', 'right');

                    $(win.document.body).find('table tr td:nth-child(7),table tr td:nth-child(8)')
                        .addClass('align-right');
                    $(win.document.body).find('table tr th:nth-child(6),table tr td:nth-child(6)').css('width', '150px');
                },
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8,]
                }
            },
        ],
        ajax: {
            type: "POST",
            url: "TransactionNotification",
            data: { isCompTransaction: isCompTransaction },
            dataSrc: function (data) {
                return data;
            }
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }],
        columns: [
            {
                name: "Id",
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
                name: "Institution",
                render: function (data, type, row) {
                    if (row.AccountShortTitle != null)
                        return '<span class="' + row.ClassName + '">' + row.AccountShortTitle + '</span>';
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
                    if (row.VoucherNo != null)
                        return '<span class="' + row.ClassName + '">' + row.VoucherNo + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Cheque No.",
                render: function (data, type, row) {
                    if (row.ChequeNo != null)
                        return '<span class="' + row.ClassName + '">' + row.ChequeNo + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Particulars",
                render: function (data, type, row) {
                    if (row.LedgerName != null)
                        return '<span class="' + row.ClassName + '">' + row.LedgerName + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Debit",
                render: function (data, type, row) {
                    if (row.Debit != null)
                        return '<span class="' + row.ClassName + '">' + row.Debit + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Credit",
                render: function (data, type, row) {
                    if (row.Credit != null)
                        return '<span class="' + row.ClassName + '">' + row.Credit + '</span>';
                    else
                        return '';
                }
            },
            {
                name: "Submit",
                render: function (data, type, row) {
                    return '<a href="#" onClick="notificationSubmit(' + row.UniqueId + ')" class="btn btn-primary btn-padding">Submit</a>';

                }
            }
        ]

    });

}
function notificationSubmit(uniqueId) {
    $.ajax({
        type: "GET",
        url: '/Reports/TransactionNotificationUpdate',
        data: { uniqueId: uniqueId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            bindViewNotificationstoGird(0);
        },
        error: function (error) { alert("Failed to update"); },
        complete: function () {
            HideLoading();
        }
    });

}
