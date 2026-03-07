var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadNextYearsList();

        $('#btnTransfer').on('click', function () {
            transferBalanceToNextyear();
        });
    });
});

function loadNextYearsList() {
    $('#spnCurrentYear').text(userData.FinTitle);
    $.ajax({
        type: "GET",
        url: '/Admin/GetNextYear',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            if (data == undefined || data == '') {
                $('#spnNextYear').text("Next year not found");
                $('#btnTransfer').addClass('novisibility');
            }
            else
                $('#spnNextYear').text(data);
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}

function transferBalanceToNextyear() {
    $.ajax({
        type: "GET",
        url: '/Admin/BalanceTransferEndYear',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            if (data == true)
                alert("Balances have been transfered successfully");
            else
                alert("There is a error, contact Administration");
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}