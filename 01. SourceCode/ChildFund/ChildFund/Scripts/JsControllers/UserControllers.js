// danh sách
var modelSearch =
{
    Name: '',
    Status: '',
    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true
};

function Refresh() {
    $('#userNameSearch').val('');
    $('#codeSearch').val('');
    modelSearch.PageSize = 10;
    modelSearch.PageNumber = 1;
    Search();
}

function GetModelSearch() {
    modelSearch.Name = $('#userNameSearch').val();
    modelSearch.Status = $('#codeSearch').val();
}
function Search() {
    GetModelSearch();
    $.post("/User/ListUser", modelSearch, function (result) {
        $("#list_user").html(result);
    });
}
$("#nameSearch").keydown(function (event) {
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
    $.post("/User/ListUser", modelSearch, function (result) {
        $("#list_category").html(result);
    });
}

// them moi
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

function AddUser() {
    document.getElementById('btn-update').style.display = 'none';
    document.getElementById('btn-create').style.display = 'inline-block';
    document.getElementById('btn-createContinue').style.display = 'inline-block';
    $('#h4Title').html('Thêm mới người dùng');
    ResetModel();
    $('#modamCate').modal({
        show: 'true'
    });
}
function EditUser(id) {
    document.getElementById('btn-update').style.display = 'inline-block';
    document.getElementById('btn-create').style.display = 'none';
    document.getElementById('btn-createContinue').style.display = 'none';
    $('#h4Title').html('Cập nhật người dùng');
    model.Id = id;
    $.ajax({
        url: "/User/ListUser?id=" + id,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                $('#userInfoId').val(data.data.UserInfoId);
                $('#groupUserId').val(data.data.GroupUserId);
                $('#userName').val(data.data.UserName);
                $('#wardId').val(data.data.WardId);
                $('#districtId').val(data.data.DistrictId);
                $('#provinceId').val(data.data.ProvinceId);
                $('#securityStamp').val(data.data.SecurityStamp);
                $('#type').val(data.data.Type);
                $('#passwordHash').val(data.data.PasswordHash);
                $('#homeURL').val(data.data.HomeURL);
                $('#fullName').val(data.data.FullName);
                $('#gender').val(data.data.Gender);
                $('#birthdate').val(data.data.Birthdate);
                $('#email').val(data.data.Email);
                $('#phone').val(data.data.Phone);
                $('#address').val(data.data.Address);
                $('#avatarPath').val(data.data.AvatarPath);
                document.querySelector('#isDisable').checked = false;
                document.querySelector('#cateIsViewHome').checked = false;
                if (data.data.IsDisable === '1') {
                    document.querySelector('#isDisable').checked = true;
                }
                if (data.data.IsViewHome === '1') {
                    document.querySelector('#cateIsViewHome').checked = true;
                }
                $('#modamCate').modal({
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
    $('#modamCate').modal('hide');
    Search();
}
function Update() {
    var result = ValidateCate();
    if (result === true) {
        $.ajax({
            url: "/User/UpdateUser",
            data: model,
            cache: false,
            type: "POST",
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
    var result = ValidateCate();
    if (result === true) {
        $.ajax({
            url: "/User/CreateUser",
            data: model,
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Thêm người dùng thành công!", { timeOut: 5000 });
                    if (isContinue === true) {
                        ResetModel();
                    } else {
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
function ValidateCate() {
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
function DeleteConfimCategory(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('bạn có chắc chắn muốn xóa nhóm tin này?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({

        url: "/Category/DeleteCategory/",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Xóa nhóm tin thành công!", { timeOut: 5000 });
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