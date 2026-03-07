var userData = {};
$.getJSON('/Base/GetUserData', function (data) {
    userData = JSON.parse(data);
    $(document).ready(function () {
        setPermissions(userData.InstituteId);
        $('#btnSubmit').on('click', function () {
            var currentPassword = $('#txtUserName').val();
            var newPassword = $('#txtNewPassword').val();
            var confirmPassword = $('#txtConfirmPassword').val();
            if (currentPassword.length == 0) {
                alert("Please enter current password");
                return false;
            }
            else if (currentPassword.length < 3) {
                alert("Please enter minimum three Characters");
                return false;
            }
            else if (newPassword.length < 6) {
                alert("Minimum six characters required for new password");
                return false;
            }
            else if (confirmPassword.length < 6) {
                alert("Please enter new password again");
                return false;
            }
            else if (newPassword.trim() != confirmPassword.trim()) {
                alert("New Password And Confirm Password Does Not Match");
                return false;
            }
            $.ajax({
                type: "POST",
                url: '/Account/ChangeCurrentPassword',
                data: JSON.stringify({ 'userName': userData.UserName, 'currentPassword': currentPassword, 'newPassword': newPassword }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data == true) {
                        alert("Password has been changed successfully!!");
                        $('#txtCurrentPassword').val('');
                        $('#txtNewPassword').val('');
                        $('#txtConfirmPassword').val('');
                    }
                    else
                        alert("Failed to change password!!");
                },
                error: function (error) { alert("error while password change"); }
            });
        });
    });
});