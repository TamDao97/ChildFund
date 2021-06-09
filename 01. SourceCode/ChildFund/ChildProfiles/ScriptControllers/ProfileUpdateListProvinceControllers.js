﻿$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    ProgramCode: '',
    Name: '',
    ProvinceId: '',
    DistrictId: '',
    WardId: '',
    DateFrom: '',
    DateTo: '',
    CreateBy: '',
    Export: 0,

    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: false,
    ListCheck: []
};
function Refresh() {
    modelSearchListCheck = [];
    $('#programCodeSearch').val('');
    $('#createBySearch').val('');
    $('#provinceIdSearch').val('');
    $('#districtIdSearch').val('');
    $('#wardIdSearch').val('');
    $('#nameSearch').val('');
    $('#dateFromSearch').val('');
    $('#dateToSearch').val('');
    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search(0);
}
function GetModelSearch() {
    modelSearch.Name = $('#nameSearch').val();
    modelSearch.ProvinceId = $('#provinceIdSearch').val();
    modelSearch.DistrictId = $('#districtIdSearch').val();
    modelSearch.WardId = $('#wardIdSearch').val();
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
    modelSearch.CreateBy = $('#createBySearch').val();
    modelSearch.ProgramCode = $('#programCodeSearch').val();
}

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

function Search(Export) {
    GetModelSearch();
    modelSearch.Export = Export;
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/ProfilesUpdate/GetProfileProvince", modelSearch, function (result) {
        $("#list_data").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
        var href = $('#linkDowload').attr('href');
        if (href !== '') {
            var link = document.getElementById('linkDowload');
            link.click();
        }
    });
}
$("#programCodeSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
$("#nameSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
$("#createBySearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
function ChangeSize() {
    modelSearch.PageNumber = 1;
    modelSearch.PageSize = $('#pageSize').val();
    Search(0);
}
function phantrang(PageNumber) {
    modelSearch.PageNumber = PageNumber;
    Search(0);
}

function ConfimProfile(id, avata) {
    //if (avata === null || avata === '' || avata === ' ') {
    //    toastr.error("Hồ sơ chưa có ảnh không thể phê duyệt!/Profile that lacks child photo can not be approved!", { timeOut: 5000 });
    //    return false;
    //}
    $('#valueConfim').val(id);
    $('#labelConfim').html('Bạn có chắc chắn duyệt hồ sơ này?/Are you sure to approve this profile?');
    $('#modamConfim').modal({
        show: 'true'
    });
}
function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn khóa hồ sơ này?/Are you sure to lock this profile?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({

        url: "/ProfilesUpdate/Delete",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Khóa hồ sơ thành công!/Lock profile successfully!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                Search(0);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}
function Confim() {
    var id = $('#valueConfim').val();
    $.ajax({

        url: "/ProfilesUpdate/ConfimProfile",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Duyệt hồ sơ thành công!/Approve profile successfully!", { timeOut: 5000 });
                $('#modamConfim').modal('hide');
                Search(0);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}

function CheckAllData() {
    var checkItem = document.querySelector('#checkAllData').checked;
    if (checkItem) {
        modelSearch.ListCheck = $('#dataListId').val().split(';');
        $('tbody input:checkbox[class=itemp]').each(function () {
            $(this).prop('checked', true);
        });
    } else {
        modelSearch.ListCheck = [];
        $('tbody input:checkbox[class=itemp]').each(function () {
            $(this).prop('checked', false);

        });
    }
}
function CheckData(id) {
    var checkItem = document.querySelector('#ck_' + id).checked;
    if (checkItem) {
        modelSearch.ListCheck.push(id);
        CheckSearch();
    } else {
        $('#checkAllData').prop('checked', false);
        index = modelSearch.ListCheck.indexOf(id);
        if (index !== -1) { modelSearch.ListCheck.splice(index, 1); }
    }
}
function CheckSearch() {
    var itemValue = '';
    var index = 0;
    $('tbody input:checkbox[class=itemp]').each(function () {
        itemValue = $(this).attr('value');
        index = modelSearch.ListCheck.indexOf(itemValue);
        if (index !== -1) { $(this).prop('checked', true); }
    });
    var count = parseInt($('#count_data').html());
    if (modelSearch.ListCheck.length === count && count !== 0) {
        $('#checkAllData').prop('checked', true);
    }
}
function ConfimSelect() {
    if (modelSearch.ListCheck.length === 0) {
        toastr.error("Chưa có hồ sơ nào được chọn!/None profile has been chosen!", { timeOut: 5000 });
        return;
    }
    $('#valueConfim').val('-1');
    $('#labelConfim').html('Bạn có chắc chắn duyệt hồ sơ này?/Are you sure to approve this profile?');
    $('#modamConfim').modal({
        show: 'true'
    });
}
function Confim() {
    var id = $('#valueConfim').val();
    if (id !== '-1') {//duyet 1
        $.ajax({
            url: "/ProfilesUpdate/ConfimProfile",
            type: "POST",
            data: { Id: id },
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Duyệt hồ sơ thành công!/Approve profile successfully!", { timeOut: 5000 });
                    $('#modamConfim').modal('hide');
                    Search(0);
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
            },
            error: function (response) {
                toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
            }
        });

    } else {//duyet nhieu
        $.ajax({
            url: "/ProfilesUpdate/ConfimProfile",
            type: "POST",
            data: { Id: '-1', SelectId: modelSearch.ListCheck },
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Duyệt hồ sơ thành công!/Approve profile successfully!", { timeOut: 5000 });
                    $('#modamConfim').modal('hide');
                    modelSearch.ListCheck = [];
                    Search(0);
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
            },
            error: function (response) {
                toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
            }
        });
    }
}

function ExportStorySelect() {

    if (modelSearch.ListCheck.length === 0) {
        toastr.error("Chưa có hồ sơ nào được chọn!/None profile has been chosen!", { timeOut: 5000 });
        return;
    }
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.ajax({
        url: "/ProfilesUpdate/ExportStorySelect",
        type: "POST",
        data: modelSearch,
        success: function (data) {
            if (data.ok === true) {
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
                $('#linkDowload').attr('href', data.mess);
                var link = document.getElementById('linkDowload');
                link.click();
            } else {
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
            toastr.error("Đã xảy ra lỗi!/Error", { timeOut: 5000 });
        },
    });
}

function ShowModal(Id, programCode) {
    $.ajax({
        url: "/Shared/EditCode?Id=" + Id + "&programCode=" + programCode,
        type: "POST",
        success: function (data) {
            $("#modalEditCode").html(data);
            $("#editCodeModal").modal('show');
        }
    });
}

function SaveChange() {
    var Id = $("#Id").val();
    var programCode = $("#programCode").val();

    $.ajax({
        url: "/ProfilesUpdate/SaveChangeCode?Id=" + Id + "&programCode=" + programCode,
        type: "POST",
        success: function (data) {
            if (data.ok) {
                toastr.success("Cập nhập thành công!/Update successfully", { timeOut: 5000 });
                $("#editCodeModal").modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        }
    });
}

function ExportProfileSelect() {

    if (modelSearch.ListCheck.length === 0) {
        toastr.error("Chưa có hồ sơ nào được chọn!/None profile has been chosen!", { timeOut: 5000 });
        return;
    }
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.ajax({
        url: "/ProfilesUpdate/ExportProfileSelect",
        type: "POST",
        data: modelSearch,
        success: function (data) {
            if (data.ok === true) {
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
                $('#linkDowload').attr('href', data.mess);
                var link = document.getElementById('linkDowload');
                link.click();
            } else {
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
            toastr.error("Đã xảy ra lỗi!/Error", { timeOut: 5000 });
        },
    });
}