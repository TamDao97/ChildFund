$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var dataProfile = new FormData();
// danh sách
var modelSearch =
{
    Name: '',
    FullName: '',
    UserLevel: '2',
    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true
};

function Refresh() {
    $('#userNameSearch').val('');
    $('#fullNameSearch').val('');
    $("#areaUserIdSearch").val('');
    $("#areaDistrictIdSearch").val('');
    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}

function GetModelSearch() {
    modelSearch.Name = $('#userNameSearch').val();
    modelSearch.FullName = $('#fullNameSearch').val();
    modelSearch.AreaUserId = $("#areaUserIdSearch").val();
    modelSearch.AreaDistrictId = $("#areaDistrictIdSearch").val();
}
function Search() {
    GetModelSearch();
    
    $.post("/Combobox/GetArea", function (rsAreaUser) {
        var html = "<option value = ''>Chọn/Pick</option>";
        $.each(rsAreaUser, function (key, value) {
            html +="<option value=" + value.Id + ">" + value.Name + "</option>";
        });
        $("#areaUserIdSearch").html(html);
        $("#areaUserIdSearch").val(modelSearch.AreaUserId);
        $("#areaDistrictIdSearch").val(modelSearch.AreaDistrictId);
    });
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
    AreaDistrictId: '',
    UserName: '',
    Type: '',
    AreaUserId: '',
    UserLevel: '2',
    HomeURL: '',
    FullName: '',
    Gender: '',
    Birthdate: '',
    Email: '',
    Phone: '',
    Address: '',
    AvatarPath: '',
    IsDisable: '',
    CreateBy: '',
    UpdateBy: ''
};

function AddUser() {
    clearImage();
    $('#password').prop('disabled', false);
    $('#retypepassword').prop('disabled', false);
    $("#divPass").show();
    $('#areaDistrictId').prop('disabled', true);
    document.getElementById('btn-update').style.display = 'none';
    document.getElementById('btn-create').style.display = 'inline-block';
    document.getElementById('btn-createContinue').style.display = 'inline-block';
    $('#titleModal').html('Thêm mới người dùng/Add user');
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
    $('#birthDate').val('');
    $('#password').prop('disabled', true);
    $('#retypepassword').prop('disabled', true);
    $('#titleModal').html('Cập nhật người dùng/Update user');
    model.Id = id;
    $.ajax({
        url: "/NguoiDung/GetUserInfo?id=" + id,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                $('#areaUserId').val(data.data.AreaUserId);
                $.post("/Combobox/AreaDistrict?Id="+$('#areaUserId').val(), function (result) {
                    $('#areaDistrictId').html(' <option value="">Chọn/Pick</option>' + result);
                    if (data.data.AreaDistrictId ==='') {
                        $('#areaDistrictId').val('');
                        $('#areaDistrictId').prop('disabled', true);
                        $("#rdArea").prop("checked", true);
                    }
                    else {
                        $('#areaDistrictId').prop('disabled', false);
                        $("#rdDistrict").prop("checked", true);
                        $('#areaDistrictId').val(data.data.AreaDistrictId);
                    }
                });
               
                $('#userName').val(data.data.UserName);
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
                    var birthDate =   day.toString()+ "/" + month.toString() + "/" +year.toString();
                    $('#birthDate').val(birthDate);
                }
                $('#email').val(data.data.Email);
                $('#phone').val(data.data.Phone);
                $('#address').val(data.data.Address);
                //$('#avatarPath').val(data.data.AvatarPath);
                $('#imgPreview').attr("src", data.data.AvatarPath);
                $('#modamUser').modal({
                    show: 'true'
                });
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (reponse) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
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
                    toastr.success("Cập nhật nguời dùng thành công!/Update user successfully!", { timeOut: 5000 });
                    CloseModel();
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
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
                    toastr.success("Thêm người dùng thành công!/Add user successfully!", { timeOut: 5000 });
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
                toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
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
    model.UserName = $('#userName').val();
    model.AreaUserId = $('#areaUserId').val();
    model.AreaDistrictId = $('#areaDistrictId').val();
    model.FullName = $('#fullName').val();
    model.Password = $('#password').val();
    model.RetypePassword = $('#retypepassword').val();
    model.Birthdate = $('#birthDate').val();
    if (model.Birthdate !== '') {
        model.Birthdate = ProcessDate($('#birthDate').val());
    }
    model.Type = $('#type').val();
    model.Gender = $('#gender').val();
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
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (model.UserName === '') {
        toastr.error("Nhập tên tài khoản!/Enter account!", { timeOut: 5000 }); return false;
    } else if (model.FullName === '') {
        toastr.error("Nhập tên nguời dùng!/Enter user full name!", { timeOut: 5000 }); return false;
    } else if (model.Password === '') {
        toastr.error("Nhập mật khẩu!/Enter password!", { timeOut: 5000 }); return false;
    } else if (model.Password.length < 6 || model.Password.length > 50) {
        toastr.error("Mật khẩu phải nhiều hơn 6 ký tự và ít hơn 50 ký tự!/Password must contain more than 6 and lower than 50 characters!", { timeOut: 5000 }); return false;
    } else if (model.Password !== model.RetypePassword) {
        toastr.error("Mật khẩu nhập lại không trùng khớp!/Re-enter password is not match with password!", { timeOut: 5000 }); return false;
    } else if (model.Gender === '') {
        toastr.error("Chọn giới tính!/Select gender!", { timeOut: 5000 }); return false;
    } else if (model.AreaUserId === '') {
        toastr.error("Chọn khu vực!/Select area!", { timeOut: 5000 }); return false;
    } else if (model.Phone.length > 50) {
        toastr.error("Độ dài số điện thoại quá quy định!/Phone number length is too long", { timeOut: 5000 }); return false;
    } else { return true; }
}
function ResetModel() {
    $('#areaDistrictId').val('');
    $('#retypepassword').val('');
    $('#birthDate').val('');
    $('#userInfoId').val('');
    $('#groupUserId').val('');
    $('#userName').val('');
    $('#type').val('');
    $('#password').val('');
    $('#fullName').val('');
    //$('#gender').val('1');
    $('#male').prop('checked', true);
    $('#rdArea').prop('checked', true);
    $('#birthdate').val('');
    $('#email').val('');
    $('#phone').val('');
    $('#address').val('');
    $('#avatarPath').val('');
    $('#imgPreview').attr('src', '');
}
function DeleteConfirmUser(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn xóa người dùng này?/Are you sure to delete this user?');
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
                toastr.success("Xóa người dùng thành công!/Delete user successfully!", { timeOut: 5000 });
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

function GetAreaDistrict() {
    var type = $('input[name=typeUser]:checked').val();
    var comboboxid = $('#areaUserId').val();
    if (type == 3) {
        $('#areaDistrictId').load('/Combobox/AreaDistrict?Id=' + comboboxid);
    }
}

function GetAreaDistrictSearch() {
    var comboboxid = $('#areaUserIdSearch').val();
    $.post("/Combobox/GetDistrictByAreaId/" + comboboxid, function (rsAreaUser) {
        var html = "<option value = ''>Chọn/Pick</option>";
        $.each(rsAreaUser, function (key, value) {
            html += "<option value=" + value.Id + ">" + value.Name + "</option>";
        });
        $("#areaDistrictIdSearch").html(html);
    });
}

function ChangeDistrict() {
    var comboboxid = $('#districtId').val();
    $('#wardId').load('/Combobox/WardCBB?Id=' + comboboxid);
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
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        }
    });
    $('#modalResetPassword').modal('show');
}

function Reset() {
    var id = $('#rsUserId').val();
    var rsPassword = $('#rsPassword').val();
    var rsRetypepassword = $('#rsRetypepassword').val();
    if (rsPassword.length < 6 || rsPassword.length > 50) {
        toastr.error("Mật khẩu phải nhiều hơn 6 ký tự và ít hơn 50 ký tự!/Password must contain more than 6 and lower than 50 characters!", { timeOut: 5000 }); return false;
    } else {
        if (rsPassword == rsRetypepassword) {
            $.ajax({
                url: "/NguoiDung/ResetPassword",
                data: { id: id, password: rsPassword },
                cache: false,
                type: "POST",
                success: function (data) {
                    if (data.ok === true) {
                        toastr.success("Reset mật khẩu thành công!/Reset password successfully!", { timeOut: 5000 });
                        CloseModelRS();
                    } else {
                        toastr.error(data.mess, { timeOut: 5000 });
                    }
                },
                error: function (reponse) {
                    toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
                }
            });
        } else {
            toastr.error("Mật khẩu nhập lại không trùng khớp!/Re-enter password is not match with password!", { timeOut: 5000 }); return false;
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
        $('#titleChangeStatus').html('Kích hoạt tài khoản/Activated account');
        document.getElementById("content").innerHTML = "Bạn có chắc chắn muốn kích hoạt tài khoản này?/Are you sure to activated this account?";
    }
    else {
        $('#titleChangeStatus').html('Khóa tài khoản/Lock account');
        document.getElementById("content").innerHTML = "Bạn có chắc chắn muốn khóa tài khoản này?/Are you sure to lock this account?";
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
                toastr.success("Thay đổi trạng thái thành công!/Change status successfully!", { timeOut: 5000 });
                CloseModelCS();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (reponse) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        }
    });
}

function CloseModelCS() {
    $('#modalChangeStatus').modal('hide');
    Search();
}

$("input[name='typeUser']").change(function (e) {
    var type = $('input[name=typeUser]:checked').val();
    if (type == 2) {
        $('#areaDistrictId').val('');
        $('#areaDistrictId').prop('disabled', true);
    }
    else {
        GetAreaDistrict();
        $('#areaDistrictId').prop('disabled', false);
    }
});