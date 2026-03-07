var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        var todayDate = new Date();
        $('#txtDate').val(todayDate.getDate() + "." + todayDate.getMonth() + "." + todayDate.getFullYear());
        loadOrgNamesDropdown();
        loadVocherTypeDropdown();
        $('#ddlVocherTypes').on('change', function () {
            var selectedVocher = $("#ddlVocherTypes option:selected").val();
            if (selectedVocher == '') {
                $('#txtVoucherNo').removeAttr('disabled');
                $('#txtVoucherNo').val('');
                alert('Please select Vocher type');
                return false;
            }
            if (userData.FinancialYearId == 4) {
                $('#txtVoucherNo').removeAttr('disabled');
                $('#txtVoucherNo').val('');
            }
            else {
                $.ajax({
                    type: "GET",
                    url: '/Transactions/SelectMaximumNewVoucherNoAutoGenerate',
                    data: { VoucherTypeId: selectedVocher },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var newVoucherNo = data + "/" + $('#txtDate').val() + "/" + userData.InstituteId;
                        $('#txtVoucherNo').attr('disabled', 'disabled');
                        $('#txtVoucherNo').val(newVoucherNo);
                    },
                    error: function (error) { console.log(error); }
                });
            }
        });
        $('#btnProceed').on('click', function () {
            var selectedVocher = $("#ddlVocherTypes option:selected").val();
            if (selectedVocher == '') {            
                alert('Please select Vocher type');
                return false;
            }
            var voucherNo = $("#txtVoucherNo").val();
            if (voucherNo == '') {
                alert('Please select Voucher No');
                return false;
            }
            var chequeOrCash = $("#txtChequeOrCash").val();
            if (chequeOrCash == '') {
                alert('Please select cheque no./cash');
                return false;
            }
        });
    });
});

function loadOrgNamesDropdown() {
    $.ajax({
        type: "GET",
        url: '/Admin/GetDepartmentsList',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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
            $("#ddlInstitute").prop("disabled", true);
        },
        error: function (error) { console.log(error); }
    });
}
function loadVocherTypeDropdown() {
    $.ajax({
        type: "GET",
        url: '/Transactions/GetVoucherTypes',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var optionhtml = '<option value=""></option>';
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