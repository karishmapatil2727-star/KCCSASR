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
            startDate: startDate,
            endDate: endDate
        });
        $('#dtEndingDate,#txtEndingDate').datepicker({
            format: "dd/mm/yyyy",
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
            var toDate = $('#txtEndingDate').val();
            if (toDate == '') {
                toDate = endDate.getDate() + "/" + (endDate.getMonth() + 1) + "/" + endDate.getFullYear();
            }

            var endDate = ConverttoDate(toDate);

            loadTableContent(endDate.toISOString());
        });

        $('#btnSave').on('click', function () {
            saveTableContentData();
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
function loadTableContent(endDate) {
    if ($.fn.DataTable.isDataTable("#tblTableContent")) {
        $('#tblTableContent').DataTable().draw();
        $('#tblTableContent').DataTable().destroy();
        $('#tblTableContent tbody').empty();
    }
    var dataTable = $('#tblTableContent').DataTable({
        bProcessing: true,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        language: {
            search: "",
            searchPlaceholder: "Search records",
        },
        ajax: {
            url: "LoadTableContent",
            data: { toDate: endDate },
            dataSrc: ''
        },
        aoColumnDefs: [{ "bSortable": false, "aTargets": [0, 1, 2] }],
        columns: [
            { data: "AccountId", name: "AccountId" },
            { data: "AccountGroupName", name: "Particulars" },
            { data: "PageNumber", name: "Page Number" }
        ]
    });

    $('#tblTableContent').on('click', 'tbody td:nth-child(3)', function () {
        var OriginalContent = $(this).text();
        $(this).addClass("cellEditing");
        $(this).html("<input type='text' value='" + OriginalContent + "' />");
        $(this).children().first().focus();
        $(this).children().first().keypress(function (e) {
            if (e.which == 13) {
                var newContent = $(this).val();
                $(this).parent().text(newContent);
                $(this).parent().removeClass("cellEditing");
            }
        });

        $(this).children().first().blur(function () {
            var newContent = $(this).val();
            $(this).parent().text(newContent);
            $(this).parent().removeClass("cellEditing");
        });
    });
}
function saveTableContentData() {
    var ContentModels = [];
    $('#tblTableContent > tbody  > tr').each(function (rowIndex) {
        var accountId = $('#tblTableContent > tbody  > tr').eq(rowIndex).find('td').eq(0).html();
        var AccountGroupName = $('#tblTableContent > tbody  > tr').eq(rowIndex).find('td').eq(1).html();
        var pageNumber = $('#tblTableContent > tbody  > tr').eq(rowIndex).find('td').eq(2).html();
        var content = { "AccountId": accountId, "AccountGroupName": AccountGroupName, "PageNumber": pageNumber };
        ContentModels.push(content);
    });
    if (ContentModels.length > 1) {
        $.ajax({
            type: "POST",
            url: '/Admin/SaveableContent',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ tableContentModels: ContentModels }),
            dataType: "json",
            beforeSend: function () {
                ShowLoading();
            },
            success: function (data) {
                if (data == true) {
                    alert("Data save successfully");                   
                    var toDate = $('#txtEndingDate').val();
                    if (toDate == '') {
                        var endDate = new Date();
                        endDate.setDate(endDate.getDate());
                        toDate = endDate.getDate() + "/" + (endDate.getMonth() + 1) + "/" + endDate.getFullYear();
                    }
                    var endDate = ConverttoDate(toDate);
                    loadTableContent(endDate.toISOString());
                }
                else
                    alert("Data failed to save");
            },
            error: function (error) { console.log(error); },
            complete: function () {
                HideLoading();
            }
        });
    }
}