var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadLedgersDropdown();

        $('#btnMoveTransactions').on('click', function () {
            var fromLedgerId = $("#ddlFromLedgers option:selected").val();
            if (fromLedgerId == '' || fromLedgerId == '0') {
                alert('Please select from ledger');
                return false;
            }
            var toLedgerId = $("#ddlToLedgers option:selected").val();
            if (toLedgerId == '' || toLedgerId == '0') {
                alert('Please select to Ledger');
                return false;
            }
            moveTransactionsFromLedgertoToLedger(fromLedgerId, toLedgerId);
        });

    });
});
function loadLedgersDropdown() {
    $.ajax({
        type: "GET",
        url: '/Admin/GetAccountLedgerList',
        data: { showInTransactionPage: '2' },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            var optionhtml = '<option value="0">Select Ledger</option>';
            $("#ddlFromLedgers").append(optionhtml);
            $("#ddlToLedgers").append(optionhtml);
            $.each(data, function (i) {
                var optionhtml = '<option value="' +
                    data[i].LedgerId + '">' + data[i].LedgerName + '</option>';
                $("#ddlFromLedgers").append(optionhtml);
                $("#ddlToLedgers").append(optionhtml);
            });
            $("#ddlFromLedgers").combobox();
            $("#ddlToLedgers").combobox();
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
function moveTransactionsFromLedgertoToLedger(fromLedgerId, toLedgerId) {
    $.ajax({
        type: "GET",
        url: '/Transactions/MoveTransactiontoOtherLedger',
        data: { fromLedgerId: fromLedgerId, toLedgerId: toLedgerId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            if (data == true)
                alert("Data has been updated successfully!");
            else
                alert("There is an error during Updation");
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}