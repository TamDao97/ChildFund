$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var dataProfile = new FormData();
// danh sách
var modelSearch =
    {
        Name: '',
        Status: '',
        PageSize: 20,
        PageNumber: 1,
        OrderBy: 'Name',
        OrderType: true
    };

function Refresh() {
    $('#userNameSearch').val('');
    $('#fullNameSearch').val('');
    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}

function GetModelSearch() {
    modelSearch.Name = $('#userNameSearch').val();
    modelSearch.FullName = $('#fullNameSearch').val();
}
function Search() {
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/NguoiDung/ListUser", modelSearch, function (result) {
        $("#list_user").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}
$("#userNameSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search();
        return false;
    }
});
function ChangeSize() {
    modelSearch.PageNumber = 1;
    modelSearch.PageSize = $('#pageSize').val();
    Search();
}
function phantrang(PageNumber) {
    GetModelSearch();
    modelSearch.PageNumber = PageNumber;
    $.post("/NguoiDung/ListUser", modelSearch, function (result) {
        $("#list_user").html(result);
    });
}

// them moi
var model =
    {
        Id: '',
        GroupUserId: '',
        UserName: '',
        Type: '1',
        AreaUserId: '',
        Password: '',
        RetypePassword: '',
        HomeURL: '',
        FullName: '',
        Gender: '0',
        Birthdate: '',
        Email: '',
        Phone: '',
        Address: '',
        AvatarPath: '',
        UserLevel: '',
        IsDisable: '',
        CreateBy: '',
        UpdateBy: ''
    };

function AddUser() {
    clearImage();
    $('#password').prop('disabled', false);
    $('#retypepassword').prop('disabled', false);
    $("#divPass").show();
    $('#retypepassword').val('');
    document.getElementById('btn-update').style.display = 'none';
    document.getElementById('btn-create').style.display = 'inline-block';
    document.getElementById('btn-createContinue').style.display = 'inline-block';
    $('#titleModal').html('Thêm mới người dùng');
    ResetModel();
    $('#modamUser').modal({
        show: 'true'
    });
}
function EditUser(id) {
    $("#divPass").hide();
    document.getElementById('btn-update').style.display = 'inline-block';
    document.getElementById('btn-create').style.display = 'none';
    document.getElementById('btn-createContinue').style.display = 'none';
    $('#titleModal').html('Cập nhật người dùng');
    $('#birthDate').val('');
    $('#password').prop('disabled', true);
    $('#retypepassword').prop('disabled', true);
    model.Id = id;
    $.ajax({
        url: "/NguoiDung/GetUserInfo?id=" + id,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                $('#groupUserId').val(data.data.GroupUserId);
                $('#userName').val(data.data.UserName);
                $('#areaUserId').val(data.data.AreaUserId);
                $('#fullName').val(data.data.FullName);
                if (data.data.Gender == 1) {
                    document.querySelector('#male').checked = true;
                } else {
                    document.querySelector('#female').checked = true;
                }
                $('#password').val(data.data.Password);
                $('#retypepassword').val(data.data.Password);
                if (data.data.Birthdate != null) {
                    var date = new Date(parseInt(data.data.Birthdate.replace('/Date(', '')));
                    var month = date.getMonth() + 1;
                    if (month < 10) {
                        month = "0" + month.toString();
                    }
                    var day = date.getDate();
                    if (day < 10) {
                        day = "0" + day.toString();
                    }
                    var year = date.getFullYear();
                    var birthDate = day.toString() + "/" + month.toString() + "/" + year.toString();
                    $('#birthDate').val(birthDate);
                }
                $('#email').val(data.data.Email);
                $('#phone').val(data.data.Phone);
                $('#address').val(data.data.Address);
                //$('#avatarPath').val(data.data.AvatarPath);
                $('#imgPreview').attr("src", data.data.AvatarPath);
                if (data.data.IsDisable) {
                    document.querySelector('#isDisable').checked = false;
                } else {
                    document.querySelector('#isDisable').checked = true;
                }
                $("input[name=typeUser][value=" + data.data.Type + "]").prop('checked', true);
                if (data.data.Type === 1) {
                    $("#rdWard").click();
                  //  document.querySelector('#rdWard').checked = true;
                    $("#slProvince").val(data.data.ProvinceId);
                    $("#slDistrict").val(data.data.DistrictId);
                    $("#slWard").val(data.data.WardId);
                } else if (data.data.Type === 2) {
                    $("#rdDistrict").click();
                    //document.querySelector('#rdDistrict').checked = false;
                    $("#slProvince").val(data.data.ProvinceId);
                    $("#slDistrict").val(data.data.DistrictId);
                    $("#slWard").val('');
                } else if (data.data.Type === 3) {
                    $("#rdProvince").click();
                   // document.querySelector('#rdProvince').checked = false;
                    $("#slProvince").val(data.data.ProvinceId);
                    $("#slDistrict").val('');
                    $("#slWard").val('');
                }

                $.post("/Combobox/GetDistrict?id=" + data.data.ProvinceId, function (result) {
                    $("#slDistrict").html('<option value="">Tất cả</option>' + result);
                    $("#slDistrict").val(data.data.DistrictId);
                });

                $.post("/Combobox/GetWard?id=" + data.data.DistrictId, function (result) {
                    $("#slWard").html('<option value="">Tất cả</option>' + result);
                    $("#slWard").val(data.data.WardId);
                });

                $('#modamUser').modal({
                    show: 'true'
                });
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (reponse) {
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        }
    });

}
function CloseModel() {
    $('#modamUser').modal('hide');
    Search();
}
function Update() {
    dataProfile = new FormData();
    if (ValidateUser()) {
        dataProfile.append("Model", JSON.stringify(model))
        $.ajax({
            url: "/NguoiDung/UpdateUser",
            data: dataProfile,
            type: "POST",
            cache: false,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Cập nhật nguời dùng thành công!", { timeOut: 5000 });
                    CloseModel();
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
            }
        });
    }
}
function Create(isContinue) {
    dataProfile = new FormData();

    if (ValidateUser()) {
        dataProfile.append("Model", JSON.stringify(model))
        $.ajax({
            url: "/NguoiDung/CreateUser",
            data: dataProfile,
            type: "POST",
            cache: false,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Thêm người dùng thành công!", { timeOut: 5000 });
                    if (isContinue === true) {
                        ResetModel();
                    } else {
                        // window.location.href = '/hosomoi/qlhosomoi';
                        CloseModel();
                    }
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
            }
        });
    }
}

function ProcessDate(date) {
    //var dateTemp = date.split('-');
    //var rs = '';
    //if (dateTemp[0].length === 4) {
    //    rs = dateTemp[2] + '/' + dateTemp[1] + '/' + dateTemp[0];
    //} else {
    //    rs = dateTemp[0] + '/' + dateTemp[1] + '/' + dateTemp[2];
    //}
    return date;
}

function ValidateUser() {
    model.Type = $('input[name=typeUser]:checked').val();
   
    model.UserName = $('#userName').val();
    model.FullName = $('#fullName').val();
    model.Password = $('#password').val();
    model.RetypePassword = $('#retypepassword').val();
    model.Phone = $('#phone').val();
    model.Birthdate = $('#birthDate').val();
    if (model.Birthdate !== '') {
        model.Birthdate = ProcessDate($('#birthDate').val());
    }
    model.Email = $('#email').val();
    model.Phone = $('#phone').val();
    model.Address = $('#address').val();
    var files = $("#FileUpload").get(0).files;
    if (files.length > 0) {
        dataProfile.append("File", files[0]);
    }
    model.AvatarPath = $('#imgPreview').attr('src');
    model.Gender = ((document.querySelector('#male').checked) === true ? '1' : '0');
    model.IsDisable = false;

    if (model.Type == "1") {
        model.WardId = $("#slWard").val();
        model.DistrictId = $("#slDistrict").val();
        model.ProvinceId = $("#slProvince").val();

        if (model.ProvinceId === '') {
            toastr.error("Chưa chọn tỉnh/thành phố quản lý!", { timeOut: 5000 }); return false;
        } else if (model.DistrictId === '') {
            toastr.error("Chưa chọn quận/huyện quản lý!", { timeOut: 5000 }); return false;
        } else if (model.WardId === '') {
            toastr.error("Chưa chọn xã/phường quản lý!", { timeOut: 5000 }); return false;
        }
    } else if (model.Type == "2") {
        model.WardId = "";
        model.DistrictId = $("#slDistrict").val();
        model.ProvinceId = $("#slProvince").val();

        if (model.ProvinceId === '') {
            toastr.error("Chưa chọn tỉnh/thành phố quản lý!", { timeOut: 5000 }); return false;
        } else if (model.DistrictId === '') {
            toastr.error("Chưa chọn quận/huyện quản lý!", { timeOut: 5000 }); return false;
        }
    } else if (model.Type == "3") {
        model.WardId = "";
        model.DistrictId = "";
        model.ProvinceId = $("#slProvince").val();

        if (model.ProvinceId === '') {
            toastr.error("Chưa chọn tỉnh/thành phố quản lý!", { timeOut: 5000 }); return false;
        }
    }

    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (model.UserName === '') {
        toastr.error("Nhập tên tài khoản!", { timeOut: 5000 }); return false;
    } else if (model.UserName.length < 3 || model.UserName.length > 50) {
        toastr.error("Tên tài khoản phải nhiều hơn 3 ký tự và ít hơn 50 ký tự!", { timeOut: 5000 }); return false;
    } else if (model.FullName === '') {
        toastr.error("Nhập tên nguời dùng!", { timeOut: 5000 }); return false;
    } else if (model.Id === '' && model.Password === '') {
        toastr.error("Nhập mật khẩu!", { timeOut: 5000 }); return false;
    } else if (model.Id === '' && (model.Password.length < 6 || model.Password.length > 50)) {
        toastr.error("Mật khẩu phải nhiều hơn 6 ký tự và ít hơn 50 ký tự!", { timeOut: 5000 }); return false;
    } else if (model.Id === '' && model.Password !== model.RetypePassword) {
        toastr.error("Mật khẩu nhập lại không trùng khớp!", { timeOut: 5000 }); return false;
    } else if (model.Gender === '') {
        toastr.error("Chọn giới tính!", { timeOut: 5000 }); return false;
    } else if (model.Phone.length > 50) {
        toastr.error("Độ dài số điện thoại quá quy định!", { timeOut: 5000 }); return false;
    } else { return true; }
}
function ResetModel() {
    $('#retypepassword').val('');
    $('#birthDate').val('');
    $('#userInfoId').val('');
    $('#groupUserId').val('');
    $('#userName').val('');
    $('#type').val('');
    $('#password').val('');
    $('#fullName').val('');
    $('#male').prop('checked', true);
    $('#birthdate').val('');
    $('#email').val('');
    $('#phone').val('');
    $('#address').val('');
    $('#avatarPath').val('');
    $('#imgPreview').attr('src', '');
    $("#slProvince").val('');
    $("#slDistrict").val('');
    $("#slWard").val('');
}
function DeleteConfirmUser(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn xóa người dùng này?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({

        url: "/NguoiDung/DeleteUser/",
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

function ChangeProvince() {
    $.post("/Combobox/GetDistrict?id=" + $('#slProvince').val(), function (result) {
        $("#slDistrict").html('<option value="">Tất cả</option>' + result);
    });
}
function ChangeDistrict() {
    $.post("/Combobox/GetWard?id=" + $('#slDistrict').val(), function (result) {
        $("#slWard").html('<option value="">Tất cả</option>' + result);
    });
}

function uploadImage() {
    $('#FileUpload').trigger('click');
    $('#FileUpload').change(function () {
        var data = new FormData();
        let reader = new FileReader();
        var files = $("#FileUpload").get(0).files;
        if (files.length > 0) {
            data.append("File", files[0]);
            reader.readAsDataURL(files[0]);
            reader.onload = () => {
                $('#imgPreview').attr("src", reader.result);
            };
        }
    });
}

function clearImage() {
    $('#imgPreview').attr("src", "/img/avatar.png");
}

function ResetPassword(id) {
    $('#rsPassword').val('');
    $('#rsRetypepassword').val('');
    $.ajax({
        url: "/NguoiDung/GetUserInfo?id=" + id,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                $('#rsUserId').val(id);
                $('#rsUserName').val(data.data.UserName);
                $('#rsFullName').val(data.data.FullName);
                $('#rsUserName').prop('disabled', true);
                $('#rsFullName').prop('disabled', true);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (reponse) {
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        }
    });
    $('#modalResetPassword').modal('show');
}

function Reset() {
    var id = $('#rsUserId').val();
    var rsPassword = $('#rsPassword').val();
    var rsRetypepassword = $('#rsRetypepassword').val();
    if (rsPassword.length < 6 || rsPassword.length > 50) {
        toastr.error("Mật khẩu phải nhiều hơn 6 ký tự và ít hơn 50 ký tự!", { timeOut: 5000 }); return false;
    } else {
        if (rsPassword == rsRetypepassword) {
            $.ajax({
                url: "/NguoiDung/ResetPassword",
                data: { id: id, password: rsPassword },
                cache: false,
                type: "POST",
                success: function (data) {
                    if (data.ok === true) {
                        toastr.success("Reset mật khẩu thành công!", { timeOut: 5000 });
                        CloseModelRS();
                    } else {
                        toastr.error(data.mess, { timeOut: 5000 });
                    }
                },
                error: function (reponse) {
                    toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
                }
            });
        } else {
            toastr.error("Mật khẩu nhập lại không trùng khớp!", { timeOut: 5000 }); return false;
        }
    }
}

function CloseModelRS() {
    $('#modalResetPassword').modal('hide');
    Search();
}

function Change(id, isDisable) {
    $('#csUserId').val(id);
    if (isDisable) {
        $('#titleChangeStatus').html('Kích hoạt tài khoản');
        document.getElementById("content").innerHTML = "Bạn có chắc chắn muốn kích hoạt tài khoản này?";
    }
    else {
        $('#titleChangeStatus').html('Khóa tài khoản');
        document.getElementById("content").innerHTML = "Bạn có chắc chắn muốn khóa tài khoản này?";
    }
    $('#modalChangeStatus').modal('show');
}

function ChangeStatus() {
    var id = $('#csUserId').val();
    $.ajax({
        url: "/NguoiDung/ChangeStatus",
        data: { id: id },
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Thay đổi trạng thái thành công!", { timeOut: 5000 });
                CloseModelCS();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (reponse) {
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        }
    });
}

function CloseModelCS() {
    $('#modalChangeStatus').modal('hide');
    Search();
}

//function ChangeProvince() {
//    $.post("/Combobox/GetDistrict?id=" + $('#slProvince').val(), function (result) {
//        $("#slDistrict").html('<option value="">Tất cả</option>' + result);
//    });
//}
//function ChangeDistrict() {
//    $.post("/Combobox/GetWard?id=" + $('#slDistrict').val(), function (result) {
//        $("#slWard").html('<option value="">Tất cả</option>' + result);
//    });
//}

$("#rdProvince").click(function () {
    $("#divProvince").show();
    $("#divDistrict").hide();
    $("#divWard").hide();
    model.Type = "3";
})

$("#rdDistrict").click(function () {
    $("#divProvince").show();
    $("#divDistrict").show();
    $("#divWard").hide();
    model.Type = "2";
})

$("#rdWard").click(function () {
    $("#divProvince").show();
    $("#divDistrict").show();
    $("#divWard").show();
    model.Type = "1";
})