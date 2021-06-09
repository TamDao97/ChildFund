// delete
var model =
    {
        UserInfoId: '',
        GroupUserId: '',
        UserName: '',
        Type: '',
        WardId: '',
        DistrictId: '',
        ProvinceId: '',
        SecurityStamp: '',
        PasswordHash: '',
        HomeURL: '',
        FullName: '',
        Gender: '',
        Birthdate: '',
        Email: '',
        Phone: '',
        Address: '',
        AvatarPath: '',
        IsDisable: ''
    };

function CloseModel() {
    $('#modamCate').modal('hide');
    Search();
}

function ValidateUser() {
    model.UserName = $('#userName').val();
    model.UserInfoId = $('#userInfoId').val();
    model.GroupUserId = $('#groupUserId').val();
    model.Type = $('#type').val();
    model.SecurityStamp = $('#securityStamp').val();
    model.PasswordHash = $('#passwordHash').val();
    model.HomeURL = $('#homeURL').val();
    model.Gender = $('#gender').val();
    model.Email = $('#email').val();
    model.Phone = $('#phone').val();
    model.IsViewHome = ((document.querySelector('#cateIsViewHome').checked) === true ? '1' : '0');
    model.IsDisable = ((document.querySelector('#isDisable').checked) === true ? '1' : '0');
    if (model.UserName === '') {
        toastr.error("Nhập tên nguời dùng!", { timeOut: 5000 }); return false;
    } else { return true; }
}

function ResetModel() {
    $('#userInfoId').val('');
    $('#groupUserId').val('');
    $('#userName').val('');
    $('#wardId').val('');
    $('#districtId').val('');
    $('#provinceId').val('');
    $('#securityStamp').val('');
    $('#type').val('');
    $('#passwordHash').val('');
    $('#homeURL').val('');
    $('#fullName').val('');
    $('#gender').val('1');
    $('#birthdate').val('');
    $('#email').val('');
    $('#phone').val('');
    $('#address').val('');
    $('#avatarPath').val('');
    document.querySelector('#cateStatus').checked = true;
    document.querySelector('#cateIsViewHome').checked = false;
}

function DeleteConfirmUser(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('bạn có chắc chắn muốn xóa người dùng này?');
    $('#modamDelete').modal({
        show: 'true'
    });
}

function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({

        url: "/User/DeleteUser/",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Xóa người dùng thành công!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        },
    });
}