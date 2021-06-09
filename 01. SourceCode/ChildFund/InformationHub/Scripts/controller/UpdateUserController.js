// update
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var model =
{
    Id: '',
    GroupUserId: '',
    UserName: '',
    Type: '',
    WardId: '',
    DistrictId: '',
    ProvinceId: '',
    FullName: '',
    Gender: '',
    Birthdate: '',
    Email: '',
    Phone: '',
    Address: '',
    AvatarPath: '',
    ListPermission: []
};

var dataProfile = new FormData();

function InitModel(Type, GroupUserId, ProvinceId, DistrictId, WardId, Id) {
    model.Id = Id;
    $('#Type').val(Type);
    $('#provinceId').val(ProvinceId);
    SetEnableCombobox();
    if (Type === '1') {
        $('#provinceId').val(ProvinceId);

    } else
        if (Type === '2') {
            GetDistrictInit(ProvinceId, DistrictId);
        }
        else if (Type === '3') {
            GetDistrictInit(ProvinceId, DistrictId);
            GetWardInit(DistrictId, WardId);
        }
    GetGroupUserInit(Type, GroupUserId, Id);
}

function Update() {
    dataProfile = new FormData();
    if (ValidateUser()) {
        dataProfile.append("Model", JSON.stringify(model))
        $.ajax({
            url: "/User/UpdateUser",
            data: dataProfile,
            type: "POST",
            cache: false,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (data) {
                if (data.ok === true) {
                    toastr.success(GetNotifyByKey('Update_user'), { timeOut: 5000 });
                    window.location.href = '/user/index';
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
            }
        });
    }
}

function ValidateUser() {
    var itemValue = '';
    model.ListPermission = [];
    $('#list_data input:checked').each(function () {
        itemValue = $(this).attr('value');
        model.ListPermission.push(itemValue);
    });
    model.UserName = $('#UserName').val();
    model.FullName = $('#FullName').val();
    model.Phone = $('#Phone').val();
    model.Birthdate = $('#Birthdate').val();
    model.Type = $('#Type').val();
    model.Email = $('#Email').val();
    model.Address = $('#Address').val();
    model.WardId = $('#wardId').val();
    model.DistrictId = $('#districtId').val();
    model.ProvinceId = $('#provinceId').val();
    model.GroupUserId = $('#groupUserId').val();
    var files = $("#FileUpdateUserUpload").get(0).files;
    if (files.length > 0) {
        dataProfile.append("File", files[0]);
    }
    model.AvatarPath = $('#imgUpdateUserPreview').attr('src');
    model.Gender = ((document.querySelector('#male').checked) === true ? '1' : '0');
    model.IsDisable = false;
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (model.UserName === '') {
        toastr.error(GetNotifyByKey('Enter_account'), { timeOut: 5000 }); return false;
    } else if (model.FullName === '') {
        toastr.error(GetNotifyByKey('Enter_fullname'), { timeOut: 5000 }); return false;
    } else if (model.Email === '') {
        toastr.error(GetNotifyByKey('Enter_email'), { timeOut: 5000 }); return false;
    }  else if (!re.test(model.Email)) {
        toastr.error(GetNotifyByKey('Email_wrong'), { timeOut: 5000 }); return false;
    } else if (model.Gender === '') {
        toastr.error(GetNotifyByKey('Select_gender'), { timeOut: 5000 }); return false;
    }  else { return true; }
}

function CheckAllData() {
    var checkItem = document.querySelector('#checkAllData').checked;
    if (checkItem) {
        $('tbody input:checkbox[class=itemp]').each(function () {
            $(this).prop('checked', true);
        });
    } else {
        $('tbody input:checkbox[class=itemp]').each(function () {
            $(this).prop('checked', false);

        });
    }
}
function CheckData(id) {
    var checkItem = document.querySelector('#ck_' + id).checked;
    if (!checkItem) {
        $('#checkAllData').prop('checked', false);
    }
}

function SetEnableCombobox() {
    var type = $('#Type').val();
    if (type == 0) {
        //disabled dropdown
        document.getElementById("provinceCBB").style.display = 'none';
        document.getElementById("districtCBB").style.display = 'none';
        document.getElementById("wardCBB").style.display = 'none';
        //reset dropdown value
        $('#provinceId option').prop('selected', function () {
            return this.defaultSelected;
        });
        $('#districtId option').prop('selected', function () {
            return this.defaultSelected;
        });
        $('#wardId option').prop('selected', function () {
            return this.defaultSelected;
        });
    }
    if (type == 1) {
        document.getElementById("provinceCBB").style.display = 'block';
        document.getElementById("districtCBB").style.display = 'none';
        document.getElementById("wardCBB").style.display = 'none';
        //
        $('#districtId option').prop('selected', function () {
            return this.defaultSelected;
        });
        $('#wardId option').prop('selected', function () {
            return this.defaultSelected;
        });
    }
    if (type == 2) {
        document.getElementById("provinceCBB").style.display = 'block';
        document.getElementById("districtCBB").style.display = 'block';
        document.getElementById("wardCBB").style.display = 'none';
        //
        $('#wardId option').prop('selected', function () {
            return this.defaultSelected;
        });
    }
    if (type == 3) {
        document.getElementById("provinceCBB").style.display = 'block';
        document.getElementById("districtCBB").style.display = 'block';
        document.getElementById("wardCBB").style.display = 'block';
        //
        $('#provinceId').prop('disabled', false);
        $('#districtId').prop('disabled', false);
        $('#wardId').prop('disabled', false);
    }
}
$('#Type').change(function (e) {
    SetEnableCombobox();
    GetGroupUser();
});

function uploadUpdateUserImage() {
    $('#FileUpdateUserUpload').trigger('click');
    $('#FileUpdateUserUpload').change(function () {
        var data = new FormData();
        let reader = new FileReader();
        var files = $("#FileUpdateUserUpload").get(0).files;
        if (files.length > 0) {
            data.append("File", files[0]);
            reader.readAsDataURL(files[0]);
            reader.onload = () => {
                $('#imgUpdateUserPreview').attr("src", reader.result);
            };
        }
    });
}

function clearUpdateUserImage() {
    $('#imgUpdateUserPreview').attr("src", "/img/avatar-34.png");
    $('#FileUpdateUserUpload').val("");
}

function GetDistrict() {
    model.ProvinceId = $('#provinceId').val();
    $.post("/Combobox/DistrictCBB?Id=" + model.ProvinceId, function (result) {
        $("#districtId").html('<option selected value=""></option>' + result);
    });
}
function GetWard(DistrictId) {
    model.DistrictId = $('#districtId').val();
    $.post("/Combobox/WardCBB?Id=" + model.DistrictId, function (result) {
        $("#wardId").html('<option selected value=""></option>' + result);
    });
}

function GetListPermission() {
    model.GroupUserId = $('#groupUserId').val();
    model.Type = $('#Type').val();
    $.post("/User/ListPermission?id=" + model.GroupUserId + "&type=" + model.Type, function (result) {

        $("#list_data").html(result);
    });
}

function GetGroupUser() {
    model.Type = $('#Type').val();
    $.post("/Combobox/GroupUserCBB?type=" + model.Type, function (result) {
        $("#groupUserId").html(result);
        GetListPermission();
    });
}
//cac ham khoi tao
function GetDistrictInit(ProvinceId, districtId) {
    $.post("/Combobox/DistrictCBB?Id=" + ProvinceId, function (result) {
        $("#districtId").html('<option selected value=""></option>' + result);
        $("#districtId").val(districtId);
    });
}
function GetWardInit(DistrictId, wardId) {
    $.post("/Combobox/WardCBB?Id=" + DistrictId, function (result) {
        $("#wardId").html('<option selected value=""></option>' + result);
        $("#wardId").val(wardId);
    });
}
function GetGroupUserInit(Type, groupUserId, Id) {
    $.post("/Combobox/GroupUserCBB?type=" + Type, function (result) {
        $("#groupUserId").html(result);
        $("#groupUserId").val(groupUserId);
        GetListPermissionInit(Id, groupUserId, Type);
    });
}
function GetListPermissionInit(Id, groupUserId, Type) {
    $.post("/User/ListPermissionUpdate?groupUserId=" + groupUserId + "&userId=" + Id + "&type=" + Type, function (result) {
        $("#list_data").html(result);
    });
}