
var searchModel =
{
    SchoolName: '',
    PageSize: 20,
    PageNumber: 1
};

var modelCreate = {
    Id: '',
    WardId: '',
    SchoolName: ''
}

function Refresh() {
    $('#nameSearch').val('');
    searchModel.PageSize = 20;
    searchModel.PageNumber = 1;
    Search();
}

function GetModelCreate() {
    modelCreate.Id = $('#idSchool').val();
    modelCreate.WardId = $('#wardId').val();
    modelCreate.SchoolName = $('#schoolName').val();
    return modelCreate;
}

function GetModelSearch() {
    searchModel.SchoolName = $('#nameSearch').val();
}

function Search() {
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/SystemConfig/GetListSchool", searchModel, function (result) {
        $("#list_data").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}

function ChangeSize() {
    searchModel.PageNumber = 1;
    searchModel.PageSize = $('#pageSize').val();
    Search();
}

function phantrang(PageNumber) {
    searchModel.PageNumber = PageNumber;
    Search();
}

function ShowModalCreate(cases, id) {
    if (cases == 'create') {
        $.ajax({
            type: "POST",
            url: "/SystemConfig/ShowCreateForm",
            success: function (data) {
                $("#modalContent").html(data);
                $('#title').html('Thêm mới trường học');
                $('#modalCreate').modal('show');
            }
        });
    } else {
        $.ajax({
            type: "POST",
            url: "/SystemConfig/ShowEditForm/" + id,
            success: function (data) {
                $("#modalContent").html(data);
                $('#title').html('Cập nhập trường học');
                $('#modalCreate').modal('show');
            }
        });
    }
}

function ChangeProvince() {
    var provinceId = $('#provinceId').val();
    $.post("/Combobox/DistrictByUser?Id=" + provinceId, function (result) {
        $("#districtId").html(result);
        ChangeDistrict();
    });
}

function ChangeDistrict() {
    var districtId = $('#districtId').val();
    $.post("/Combobox/WardByUser?Id=" + districtId, function (result) {
        $("#wardId").html(result);
    });
}

function Save() {
    var model = GetModelCreate();
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/SystemConfig/Save",
        contentType: "application/json"
    }).done(function (res) {
        if (res) {
            Search();
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
            $('#modalCreate').modal('hide');
            toastr.success("Lưu thành công!", { timeOut: 5000 });
        }
    });
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
        url: "/SystemConfig/Delete",
        type: "POST",
        data: { Id: id },
        success: function (success) {
            if (success) {
                toastr.success("Xóa trường học thành công!/Delete school successfully!", { timeOut: 5000 });
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
