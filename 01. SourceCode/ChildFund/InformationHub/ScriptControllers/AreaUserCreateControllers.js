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
    ListWard: [],
    ListDistrict: []
};

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
    modelSearch.DistrictId = DistrictId;
    $.post("/Combobox/WardArea/", modelSearch, function (result) {
        $("#data_Ward").html(result);
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
function Create(isContinue) {
    var result = ValidateCate();
    if (result === true) {
        OpenWaiting();
        $.ajax({
            url: "/AreaUser/Create",
            data: modelSearch,
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.ok === true) {
                    if (isContinue === true) {
                        toastr.success(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
                        CloseWaiting();
                        ResetModel();
                    } else {
                        sessionStorage.setItem("keyMess", GetNotifyByKey('Add_Location'));
                        window.location = '/AreaUser/Index';
                    }
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                CloseWaiting();
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
                CloseWaiting();
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
        toastr.error(GetNotifyByKey('Enter_Location'), { timeOut: 5000 }); return false;
    } else if (modelSearch.ListDistrict.length === 0) {
        toastr.error(GetNotifyByKey('Must_District'), { timeOut: 5000 }); return false;
    } else if (modelSearch.ListWard.length === 0) {
        toastr.error(GetNotifyByKey('Must_Ward'), { timeOut: 5000 }); return false;
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