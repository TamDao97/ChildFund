
function ChangeProvince() {
    $.post("/Combobox/GetAreaDistrictHome?Id=" + $('#provinceIdSearch').val(), function (result) {
        $("#districtIdSearch").html('<option value="">Tất cả/All</option>' + result);
    });
}

function ChangeDistrict() {
    $.post("/Combobox/WardByUser?Id=" + $('#districtIdSearch').val(), function (result) {
        $("#wardIdSearch").html('<option value="">Tất cả/All</option>' + result);

    });
}

// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    ProvinceId: '',
    DistrictId: '',
    WardId: '',
    PageSize: 20,
    PageNumber: 1
};
function SearchInit() {
    Search();
}

function GetModelSearch() {
    modelSearch.ProvinceId = $('#provinceIdSearch').val();
    modelSearch.DistrictId = $('#districtIdSearch').val();
    modelSearch.WardId = $('#wardIdSearch').val();
}

function Search() {
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/ChildDataByWard/Search", modelSearch, function (result) {
        $("#list_data").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}

function ChangeSize() {
    modelSearch.PageNumber = 1;
    modelSearch.PageSize = $('#pageSize').val();
    Search();
}

function phantrang(PageNumber) {
    modelSearch.PageNumber = PageNumber;
    Search();
}