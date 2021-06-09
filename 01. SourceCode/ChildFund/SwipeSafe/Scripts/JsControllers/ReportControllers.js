// danh sách hồ sơ mới cấp trung ương
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy',
});
var modelSearch =
{
    Name: '',
    Phone: '',
    Email: '',
    Status: '',
    Type: '',
    ProvinceId: '',
    DistrictId: '',
    WardId: '',
    DateFrom: '',
    DateTo: '',

    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: false
};
function Refresh() {
    $('#nameSearch').val('');
    $('#phoneSearch').val('');
    $('#emailSearch').val('');
    $('#typeSearch').val('');
    $('#statusSearch').val('');
    $('#provinceIdSearch').val('');
    $('#districtIdSearch').val('');
    $('#wardIdSearch').val('');
    $('#dateFromSearch').val('');
    $('#dateToSearch').val('');

    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}
function GetModelSearch() {
    modelSearch.Name = $('#nameSearch').val();
    modelSearch.Phone = $('#phoneSearch').val();
    modelSearch.Email = $('#emailSearch').val();
    modelSearch.Type = $('#typeSearch').val();
    modelSearch.Status = $('#statusSearch').val();
    modelSearch.ProvinceId = $('#provinceIdSearch').val();
    modelSearch.DistrictId = $('#districtIdSearch').val();
    modelSearch.WardId = $('#wardIdSearch').val();
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
}

function Search() {
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/Report/ListReport", modelSearch, function (result) {
        $("#list_data").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}
$("#nameSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search();
        return false;
    }
});
$("#phoneSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search();
        return false;
    }
});
$("#emailSearch").keydown(function (event) {
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
    modelSearch.PageNumber = PageNumber;
    Search();
}
function Detail(id)
{
    window.location = '/Report/Detail/'+id;
}
function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn xóa ca báo cáo này?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/Report/Delete",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Xóa ca báo cáo thành công!", { timeOut: 5000 });
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

function CreateProfileConfim(id) {
    $('#valueConfirm').val(id);
    $('#labelConfirm').html('Bạn có muốn chuyển báo cáo này thành thành ca sự vụ không?');
    $('#modamConfirm').modal({
        show: 'true'
    });
}

function Confirm() {
    var id = $('#valueConfirm').val();
    $.ajax({
        url: "/ProfileReport/MoveReportToProfile",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Tạo ca thành công!", { timeOut: 5000 });
                $('#modamConfirm').modal('hide');
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
    $.post("/Combobox/GetDistrict?id=" + $('#provinceIdSearch').val(), function (result) {
        $("#districtIdSearch").html('<option value="">Tất cả</option>' + result);
        Search(0);
    });
}
function ChangeDistrict() {
    $.post("/Combobox/GetWard?id=" + $('#districtIdSearch').val(), function (result) {
        $("#wardIdSearch").html('<option value="">Tất cả</option>' + result);
    });
}