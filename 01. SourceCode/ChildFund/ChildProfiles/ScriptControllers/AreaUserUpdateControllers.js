// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    Id: '',
    Name: '',
    IsActivate: true,
    Manager: '',
    Description: '',
    ProvinceId: '',
    DistrictId: '',
    WardId: '',
    ListVillage: [],
    ListWard: [],
    ListDistrict: []
};
function InitModel(obj) {
    var data = JSON.parse(obj);
    modelSearch.Id = data.Id;
    document.querySelector('#areaUserStatus').checked = false;
    if (data.IsActivate === true) {
        document.querySelector('#areaUserStatus').checked = true;
    }
    modelSearch.ListDistrict = data.ListDistrict;
    modelSearch.ListWard = data.ListWard;
    $('#areaUserProvinceId').val(data.ProvinceId);

    modelSearch.ProvinceId = data.ProvinceId;
    $.post("/Combobox/DistrictArea", modelSearch, function (result) {
        $("#data_District").html(result);
        GetWard(modelSearch.ListDistrict[0]);
    });
}
function GetDistrict() {
    $("#data_Ward").html('');
    modelSearch.ListWard = [];
    modelSearch.ListDistrict = [];
    modelSearch.ProvinceId = $('#areaUserProvinceId').val();
    $.post("/Combobox/DistrictArea", modelSearch, function (result) {
        $("#data_District").html(result);
    });
}
function GetWard(DistrictId) {
    $("#data_Village").html('');
    modelSearch.DistrictId = DistrictId;
    $.post("/Combobox/WardArea/", modelSearch, function (result) {
        $("#data_Ward").html(result);
    });
}
function GetVillage(WardId) {
    $.post("/Combobox/GetListVillage?id=" + WardId, function (result) {
        $("#data_Village").html(result);
    });
}
function CheckDistrict(DistrictId) {
    modelSearch.DistrictId = DistrictId;
    $.post("/Combobox/WardArea/", modelSearch, function (result) {
        $("#data_Ward").html(result);
        //xử lý
        //check quyen
        var itemValue = "";
        var index = 0;
        var checkItem = document.querySelector('#ck_District_' + DistrictId).checked;
        if (checkItem) {
            $('#data_Ward input:checkbox[class=itemp]').each(function () {
                $(this).attr('checked', "checked");
                itemValue = $(this).attr('value');
                modelSearch.ListWard.push(itemValue);
            });
        } else {
            $('#data_Ward input:checkbox[class=itemp]').each(function () {
                $(this).removeAttr('checked');
                itemValue = $(this).attr('value');
                index = modelSearch.ListWard.indexOf(itemValue);
                if (index !== -1) { modelSearch.ListWard.splice(index, 1); }
            });
        }

    });
}
function CheckWard(PId, WardId) {
    var checkDistrict = false;
    var checkItem = document.querySelector('#ck_Ward_' + WardId).checked;
    if (checkItem) {
        modelSearch.ListWard.push(WardId);
    } else {
        var index = modelSearch.ListWard.indexOf(WardId);
        if (index !== -1) { modelSearch.ListWard.splice(index, 1); }
        //xóa khỏi list
    }
    $('#data_Ward input:checked').each(function () {
        checkDistrict = true;
    });
    document.querySelector('#ck_District_' + PId).checked = checkDistrict;

}

//thêm mới
function Update() {
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    var result = ValidateCate();
    if (result === true) {
        $.ajax({
            url: "/AreaUser/Update",
            data: modelSearch,
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.ok === true) {
                    sessionStorage.setItem("keyMess", "Cập nhật địa bàn thành công!/Update area successfully!");
                    window.location = '/AreaUser/Index';
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
            }

        });

    }
}
function ValidateCate() {
    var itemValue = '';
    modelSearch.ListDistrict = [];
    $('#data_District input:checked').each(function () {
        itemValue = $(this).attr('value');
        modelSearch.ListDistrict.push(itemValue);
    });
    modelSearch.Name = $('#areaUserName').val();
    modelSearch.Manager = $('#areaUserManagers').val();
    modelSearch.ProvinceId = $('#areaUserProvinceId').val();
    modelSearch.Description = $('#descriptionStatus').val();
    modelSearch.IsActivate = document.querySelector('#areaUserStatus').checked;
    if (modelSearch.Name === '') {
        toastr.error("Nhập tên địa bàn!", { timeOut: 5000 }); return false;
    } else if (modelSearch.ListDistrict.length === 0) {
        toastr.error("Phải chọn Huyện!", { timeOut: 5000 }); return false;
    } else if (modelSearch.ListWard.length === 0) {
        toastr.error("Phải chọn Xã!", { timeOut: 5000 }); return false;
    }
    else { return true; }
}
function ResetModel() {
    $('#areaUserName').val('');
    $('#areaUserManagers').val('');
    $('#descriptionStatus').val('');
    modelSearch.ListWard = [];
    modelSearch.ListDistrict = [];
    GetDistrict();
    $('#data_Ward').html('');
}

function ShowManageVillage(WardId) {
    GetVillage(WardId);
    $("#txtVillageName").val("");
    $("#txtVillageId").val("");
    $("#txtWardId").val(WardId);
}
function EditVillage(item) {
    $("#txtVillageName").val(item.Name);
    $("#txtVillageId").val(item.Id);
}
function DeleteVillage(id) {
    var dataModel = {
        WardId: $("#txtWardId").val(),
    };
    $.ajax({
        url: "/AreaUser/DeleteVillage?id=" + id,
        type: "POST",
        dataType: 'json',
        success: function (data) {
            $("#txtVillageName").val("");
            $("#txtVillageId").val("");
            $.post("/Combobox/GetListVillage?id=" + dataModel.WardId, function (result) {
                $("#data_Village").html(result);
            });
        }
    });
}
function SaveVillage() {
    var dataModel = {
        Id: $("#txtVillageId").val(),
        WardId: $("#txtWardId").val(),
        Name: $("#txtVillageName").val()
    };
    $.ajax({
        url: "/AreaUser/SaveVillage",
        data: dataModel,
        type: "POST",
        dataType: 'json',
        success: function (data) {
            $("#txtVillageName").val("");
            $("#txtVillageId").val("");
            $.post("/Combobox/GetListVillage?id=" + dataModel.WardId, function (result) {
                $("#data_Village").html(result);
            });
        }
    });
}
