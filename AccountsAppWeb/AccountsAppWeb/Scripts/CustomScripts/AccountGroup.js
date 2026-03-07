var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        loadAccountGroupList();
        loaddropdownsOnLoad();
        $('#AccountGroupName').on('input', function () {
            $('#AccountGroupNameAlias').val($('#AccountGroupName').val());
        });
        $('#btnDeleteRec').on('click', function () {
            deleteGroupRecord();
        });

    });
});

function loaddropdownsOnLoad() {
    $("#GroupUnder").empty();
    $("#Nature").empty()
    $.ajax({
        type: "GET",
        url: '/Admin/GetAccountGroupsList',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            $('#GroupUnder').empty();
            $.each(data, function (i) {
                optionhtml = '<option value="' +
                    data[i].AccountGroupId + '">' + data[i].AccountGroupName + '</option>';
                $("#GroupUnder").append(optionhtml);
            });
            $('#GroupUnder').combobox();
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });

    $.ajax({
        type: "GET",
        url: '/Admin/GetNatureGroupByAll',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            $.each(data, function (i) {
                var optionhtml = '<option value="' +
                    data[i].NatureShortTitle + '">' + data[i].NatureShortTitle + '</option>';
                $("#Nature").append(optionhtml);
            });
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
function loadAccountGroupList() {
    if ($.fn.DataTable.isDataTable("#tblGroupList")) {
        $('#tblGroupList').DataTable().draw();
        $('#tblGroupList').DataTable().destroy();
        $('#tblGroupList tbody').empty();
    }
    $('#tblGroupList').DataTable({
        bProcessing: true,
        pageLength: 50,
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
            "targets": [0]
        }],
        buttons: [
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'pdfHtml5',
                title: 'Khalsa College Charitable Society, Amritsar',
                message: userData.InstName,
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'print',
                title: '',
                message: "<div class='row'><h3 class='text-center'>Khalsa College Charitable Society, Amritsar</h3></div>" +
                    "<div class='row exportoption'><h4 class='text-center'>" + userData.InstName + "</h4></div>",
                orientation: 'landscape',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
        ],
        ajax: {
            url: "GetAccountGroupsList",
            dataSrc: ''
        },
        columns: [
            { data: "AccountGroupId", name: "Group Id" },
            { data: "AccountGroupName", name: "Group Name" },
            { data: "UnderGroupTitle", name: "Under Group" },
            { data: "Nature", name: "Nature" },
            {
                "title": "Edit",
                "data": "AccountGroupId",
                "searchable": false,
                "sortable": false,
                "className": "tr-edit",
                "render": function (data, type, full, meta) {
                    if (userData.IsEnableGroupPage) {
                        return '<a href="#" onClick="editAccountGroup(' + full.AccountGroupId + ')" class="btn btn-primary btn-padding">Edit</a>';
                    }
                    else
                        return '';
                }
            },
            {
                "title": "Delete",
                "data": "AssetID",
                "searchable": false,
                "sortable": false,
                "className": "tr-edit",
                "render": function (data, type, full, meta) {
                    if (userData.IsEnableGroupPage) {
                        return '<a href="#" onClick="deleteAccountGroup(' + full.AccountGroupId + ')" class="btn btn-danger btn-padding">Delete</a>';
                    }
                    else
                        return '';
                }
            }
        ]
    });

}
function accountGroupOnSuccess(data) {
    if (data == true) {
        $("#form0")[0].reset();
        loadAccountGroupList();
        alert("Data saved successfully");
    }
    else {
        alert(data);
    }

}
function accountGroupOnFailure(error) {
    alert('error occured while saving the data');
}
function editAccountGroup(groupId) {

    $.ajax({
        type: "GET",
        url: '/Admin/GetAccountGroup',
        data: { groupId: groupId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#AccountGroupId').val(data.AccountGroupId);
            $('#AccountGroupName').val(data.AccountGroupName);
            $('#AccountGroupNameAlias').val(data.AccountGroupNameAlias);
            $("#GroupUnder option:contains(" + data.UnderGroupTitle + ")").attr('selected', true);
            $('#Nature').val(data.Nature);
            if (data.IsAdminGroup) {
                $('#AccountGroupName').attr('readonly', true);
                $('#AccountGroupNameAlias').attr('readonly', true);
            } else {
                $('#AccountGroupName').attr('readonly', false);
                $('#AccountGroupNameAlias').attr('readonly', false)
            }
        },
        error: function (error) { console.log(error); }
    });
}
function deleteAccountGroup(groupId) {
    $('#hdnGroupId').val(groupId);
    $('#deleteModel').modal('show');
}
function deleteGroupRecord() {
    var groupId = $('#hdnGroupId').val();
    $.ajax({
        type: "GET",
        url: '/Admin/DeleteAccountGroup',
        data: { groupId: groupId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            ShowLoading();
        },
        success: function (data) {
            $('#deleteModel').modal('hide');
            loadAccountGroupList();
            alert(data);
        },
        error: function (error) { console.log(error); },
        complete: function () {
            HideLoading();
        }
    });
}
function validateGroupfrom() {
    if (!userData.IsEnableGroupPage) {
        var AccountGroupId = $('#AccountGroupId').val();
        if (AccountGroupId == 0 || AccountGroupId == undefined) {
            alert("You don't have permissions to add new group.. Please contact adminstrator");
            return false;
        }
    }
    if (!userData.IsEnableGroupPage) {
        var AccountGroupId = $('#AccountGroupId').val();
        if (AccountGroupId > 0) {
            alert("You don't have permissions to modify the group.. Please contact adminstrator");
            return false;
        }
    }
}