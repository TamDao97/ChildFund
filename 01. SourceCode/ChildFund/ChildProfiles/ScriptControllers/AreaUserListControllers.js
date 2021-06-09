﻿// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    Name: '',
    IsActivate: null,
    Manager: '',

    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true
};
function Refresh() {
    $('#nameSearch').val('');
    $('#managerSearch').val('');
    $('#statusSearch').val('');

    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}
function GetModelSearch() {
    modelSearch.Name = $('#nameSearch').val();
    modelSearch.Manager = $('#managerSearch').val();
    var IsActivate = $('#statusSearch').val();
    modelSearch.IsActivate = null;
    if (IsActivate !== '') {
        if (IsActivate === '1') {
            modelSearch.IsActivate = true;
        } else {
            modelSearch.IsActivate = false;
        }
    }

}

function Search() {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/AreaUser/ListAreaUser", modelSearch, function (result) {
        $("#list_data").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}
$("#nameSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
$("#managerSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
function ChangeSize() {
    modelSearch.PageNumber = 1;
    modelSearch.PageSize = $('#pageSize').val();
    Search();
}
function phantrang(PageNumber) {
    modelSearch.PageNumber = PageNumber;
    Search();
}

function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn xóa địa bàn này?/Are you sure to delete this area?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/AreaUser/Delete",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Xóa địa bàn thành công!/Delete area successfully!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}
