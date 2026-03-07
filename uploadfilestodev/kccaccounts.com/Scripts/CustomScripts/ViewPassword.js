var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        $('#btnView').on('click', function () {
            var userName = $('#txtUserName').val();
            if (userName.length == 0) {
                alert("Please enter user name");
                return false;
            }

            $.ajax({
                type: "GET",
                url: '/Account/GetPasswordDetails',
                data: { 'userName': userName },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data != undefined && data != null) {
                        $('#spnUserName').text(data.UA_AccountName);
                        $('#spnPassword').text(data.Password);
                        $('#spnCollegeName').text(data.Inst_Title);
                    }
                    else
                        alert("Failed to get password!!");
                },
                error: function (error) { alert("error while getting password"); }
            });
        });
    });
});