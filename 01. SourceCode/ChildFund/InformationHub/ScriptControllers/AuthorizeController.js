$(function () {
    var profileUserModel = {};

    $("#changePassword").click(function () {
        $("#modalChangePassword").modal('show');
    });

    $("#profileUser").click(function () {
        $.post("/Authorize/GetProfileUser", function (result) {
            if (result.Ok) {
                profileUserModel = result.Data;
                SetModelFormFix('ProfileUser_', profileUserModel);
                if (profileUserModel.ImagePath != null && profileUserModel.ImagePath != undefined && profileUserModel.ImagePath != "") {
                    $('#imgPreviewProfile').attr("src", profileUserModel.ImagePath);
                } else {
                    $("#imgPreviewProfile").attr("src", "/img/avatar-34.png");
                }
                $("#modalProfileUser").modal('show');
            }
            else {
                toastr.error(result.Message);
            }
        });
    });

    $("#btnSaveProfile").click(function () {
        GetModelFormFix('ProfileUser_', profileUserModel);
        if (ValidateFormFix('ProfileUser_', profileUserModel)) {
            return;
        }

        var formDataProfile = new FormData();
        var files = $("#FileUpload").get(0).files;
        if (files.length > 0) {
            formDataProfile.append("File", files[0]);
        }

        formDataProfile.append("Model", JSON.stringify(profileUserModel));
        $.ajax({
            url: '/Authorize/UpdateProfileUser',
            data: formDataProfile,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                if (data.Ok) {
                    $("#modalProfileUser").modal('hide');
                    toastr.success(GetNotifyByKey('Update_UserInfo'), { timeOut: 5000 });
                }
                else
                    toastr.error(data.Message, { timeOut: 5000 });
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
            }
        });
    });

    $("#btnChangePassword").click(function () {
        var changePasswordUserModel = {
            Id: '',
            PasswordOld: '',
            PasswordNew: '',
            ConfirmPasswordNew: ''
        };
        GetModelFormFix('ChangePassword_', changePasswordUserModel);
        if (ValidateFormFix('ChangePassword_', changePasswordUserModel)) {
            return;
        }

        $.ajax({
            url: '/Authorize/ChangePasswordUser',
            data: changePasswordUserModel,
            type: 'POST',
            success: function (data) {
                if (data.Ok) {
                    $("#modalChangePassword").modal('hide');
                    toastr.success(GetNotifyByKey('Change_password'), { timeOut: 5000 });
                    setTimeout(function () { window.location.href = "/Authorize/Logout"; }, 500);
                }
                else
                    toastr.error(data.Message, { timeOut: 500 });
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
            }
        });
    });
});