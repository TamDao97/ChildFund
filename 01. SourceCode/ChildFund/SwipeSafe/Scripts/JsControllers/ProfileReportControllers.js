// danh sách hồ sơ mới cấp trung ương
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy',
});
var modelSearch =
    {
        Name: '',
        Phone: '',
        Email: '',
    ProcessingStatus: '',
        Type: '',
        ProvinceId: '',
        DistrictId: '',
        WardId: '',
        DateFrom: '',
        DateTo: '',

        PageSize: 10,
        PageNumber: 1,
        OrderBy: 'ReceptionDate',
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
    modelSearch.ProcessingStatus = $('#statusSearch').val();
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
    $.post("/ProfileReport/ListProfileReport", modelSearch, function (result) {
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
function Detail(id) {
    window.location = '/ProfileReport/Detail/' + id;
}
function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn xóa ca này?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/ProfileReport/DeleteReport",
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