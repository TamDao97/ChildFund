// danh sách
var modelSearch =
{
    Name: '',
    Description: '',
    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true
};

function Refresh() {
    $('#nameSearch').val('');
    $('#descriptionSearch').val('');
    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}

function GetModelSearch() {
    modelSearch.Name = $('#nameSearch').val();
    modelSearch.Description = $('#descriptionSearch').val();
}
function Search() {
    GetModelSearch();
    $.post("/GroupUser/ListGroupUser", modelSearch, function (result) {
        $("#list_group_user").html(result);
    });
}
function GetPermission() {
    $.post("/GroupUser/ListPermission", function (result) {
        $("#list_permission").html(result);
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
    $.post("/GroupUser/ListGroupUser", modelSearch, function (result) {
        $("#list_group_user").html(result);
    });
}

// them moi
var model =
    {
        Id: '',
        Name: '',
        Description: '',
        IsDisable: '',
        CreateBy: '',
        UpdateBy: ''
    };

function AddGroupUser() {
    document.getElementById('btn-update').style.display = 'none';
    document.getElementById('btn-create').style.display = 'inline-block';
    document.getElementById('btn-createContinue').style.display = 'inline-block';
    $('#h4Title').html('Thêm mới nhóm quyền');
    ResetModel();
    GetPermission();
    $('#modamGroupUser').modal({
        show: 'true'
    });
}
function EditGroupUser(id) {
    document.getElementById('btn-update').style.display = 'inline-block';
    document.getElementById('btn-create').style.display = 'none';
    document.getElementById('btn-createContinue').style.display = 'none';
    $('#h4Title').html('Cập nhật nhóm quyền');
    model.Id = id;
    $.ajax({
        url: "/GroupUser/GetGroupUserInfo?id=" + id,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                $('#name').val(data.data.Name);
                $('#departmentId').val(data.data.DepartmentId);
                $('#description').val(data.data.Description);
                if (data.data.IsDisable === '1') {
                    document.querySelector('#isDisable').checked = true;
                }
                
                $.post("/GroupUser/ListPermission", function (result) {
                    $("#list_permission").html(result);
                    $('#list_permission input:checkbox[class=itemp]').each(function () {
                        itemValue = $(this).attr('value');
                        var index = data.data.ListPermission.indexOf(itemValue);
                        if (index > -1) {
                            $(this).attr('checked', "checked");
                        }
                    });
                });
            
                $('#modamGroupUser').modal({
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
    $('#modamGroupUser').modal('hide');
    Search();
}
function Update() {
    if (ValidateUser()) {
        $.ajax({
            url: "/GroupUser/UpdateGroupUser",
            data: model,
            type: "POST",
            cache: false,
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Cập nhật nhóm quyền thành công!", { timeOut: 5000 });
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
    if (ValidateUser()) {
        $.ajax({
            url: "/GroupUser/CreateGroupUser",
            data: model,
            type: "POST",
            cache: false,
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Thêm nhóm quyền thành công!", { timeOut: 5000 });
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

function ValidateUser() {
    var itemValue = '';
    model.ListPermission = [];
    $('#list_permission input:checked').each(function () {
        itemValue = $(this).attr('value');
        model.ListPermission.push(itemValue);
    });
    model.Name = $('#name').val();
    model.Description = $('#description').val();
    model.IsDisable = ((document.querySelector('#isDisable').checked) === true ? '1' : '0');
    if (model.Name === '') {
        toastr.error("Nhập tên nhóm quyền!", { timeOut: 5000 }); return false;
    } else { return true; }
}
function ResetModel() {
    $('#name').val('');
    $('#departmentId').val('');
    $('#description').val('');
    document.querySelector('#isDisable').checked = true;
}
function DeleteConfirmGroupUser(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('bạn có chắc chắn muốn xóa nhóm quyền này?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/GroupUser/DeleteGroupUser/",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Xóa nhóm quyền thành công!", { timeOut: 5000 });
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