var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
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
        $('#dtgroupDate,#txtFromDate').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            startDate: startDate,
            endDate: endDate
        });
        var today = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
        $('#txtFromDate').datepicker('setDate', today);
        loadVocherTypeDropdown();

        $('#btnView').on('click', function () {
            var selectedVocher = $("#ddlVocherTypes option:selected").val();
            if (selectedVocher == '') {
                alert('Please select Vocher type');
                return false;
            }
            var fromDate = $('#txtFromDate').val();
            if (fromDate == '' || !ValidateDate(fromDate)) {
                alert('Please select valid date');
                return false;
            }
            bindVocherTypeGridwithList(selectedVocher, ConverttoDate(fromDate));
        });
        $('#btnSave').on('click', function () {
            var selectedVocher = $("#ddlVocherTypes option:selected").val();
            if (selectedVocher == '') {
                alert('Please select Vocher type');
                return false;
            }
            var jsonObject = makeJsonfromTable();
            if (jsonObject.length == 0) {
                alert('No vouchers avaiable');
                return false;
            }
            bindVocherTypeGridToTable(parseInt(selectedVocher), jsonObject);
        });
    });
});
function loadVocherTypeDropdown() {
    $.ajax({
        type: "GET",
        url: '/Transactions/GetVoucherTypes',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var optionhtml = '<option value="">Select Type</option>';
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
function bindVocherTypeGridwithList(vocherType, fromDate) {
    if ($.fn.DataTable.isDataTable("#tblResetVochers")) {
        $('#tblResetVochers').DataTable().draw();
        $('#tblResetVochers').DataTable().destroy();
        $('#tblResetVochers tbody').empty();
    }
    $('#tblResetVochers').DataTable({
        bProcessing: true,
        language: {
            search: "",
            searchPlaceholder: "Search records"
        },
        ajax: {
            url: "UpdateTransactionMasterByMasterID",
            type: "POST",
            data: { 'fromDate': fromDate.toISOString(), 'voucherTypeId': vocherType },
            dataSrc: ''
        },
        aaSorting: [[1, 'asc']],
        columns: [
            { data: "TransactionMasterId", name: "TransactionMasterId" },
            { data: "NewNotiNum", name: "NewNotiNum" },
            { data: "TransactionDate", name: "Transaction Date" },
            { data: "VoucherNo", name: "Old Voucher No" },
            { data: "NewVoucherNo", name: "New Voucher No" },
            { data: "ChequeNo", name: "Cheque No." },
            { data: "TotalAmount", name: "Amount", className: "align-right" },
            { data: "InsertDate", name: "Insert Date" },
        ],
        columnDefs: [
            {
                targets: [0],
                visible: false,
                searchable: false
            },
            {
                targets: [1],
                visible: false,
                searchable: false
            }
        ],
    });
}
function bindVocherTypeGridToTable(vocherType, jsonObject) {
    var resetVochers = { VocherTypeId: vocherType, NewResetVochers: jsonObject };
    $.ajax({
        url: '/Transactions/UpdateTransactionMasterWithTable',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(resetVochers),
        dataType: "json",
        success: function (data) {
            if (data == true)
                alert("All Vouchers have reset now");
            else
                alert("There is a error, inform to Admin");
        },
        error: function (error) { console.log(error); }
    });
}
function makeJsonfromTable() {
    var header = ['TransactionMasterId', 'NewNotiNum', 'NewVoucherNo'];
    var table = $('#tblResetVochers').DataTable();
    var JObjectArray = [];
    table.rows().every(function () {
        var d = this.data();
        var jObject = {};
        for (var x = 0; x < header.length; x++) {
            jObject[header[x]] = d[header[x]];
        }
        JObjectArray.push(jObject);
    });
    return JObjectArray;
}