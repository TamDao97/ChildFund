$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});

// create
var model =
{
    UserInfoId: '',
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
    ListPermission: [],
    RetypePassword: '',
    Password: '',

    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: false
};
var dataProfile = new FormData();

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

function GetGroupUser() {
    model.Type = $('#Type').val();
    $.post("/Combobox/GroupUserCBB?type=" + model.Type, function (result) {
        $("#groupUserId").html(result);
        GetListPermission();
    });
}

$('#Type').change(function (e) {
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
        $('#provinceId').prop('disabled', false);
        $('#districtId').prop('disabled', false);
        $('#wardId').prop('disabled', false);
    }
});

function Create(isContinue) {
    dataProfile = new FormData();

    if (ValidateUser()) {
        dataProfile.append("Model", JSON.stringify(model))
        $.ajax({
            url: "/User/AddUser",
            data: dataProfile,
            type: "POST",
            cache: false,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (data) {
                if (data.ok === true) {
                    toastr.success(GetNotifyByKey('Add_users'), { timeOut: 5000 });
                    if (isContinue === true) {
                        ResetModel();
                    } else {
                        window.location.href = '/user/index';
                    }
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
function ResetModel() {
    $('#list_data input:checked').each(function () {
        itemValue = $(this).attr('value');
        $(this).prop('checked', false);
    });
    model.UserName = $('#UserName').val('');
    model.Phone = $('#Phone').val('');
    model.FullName = $('#FullName').val('');
    model.Address = $('#Address').val('');
    model.Birthdate = $('#Birthdate').val('');
    model.Gender = $('#Gender').val('');
    model.Email = $('#Email').val('');
    model.Password = $('#Password').val('');
    model.RetypePassword = $('#RetypePassword').val('');
}
function ValidateUser() {
    var itemValue = '';
    model.ListPermission = [];
    $('#list_data input:checked').each(function () {
        itemValue = $(this).attr('value');
        model.ListPermission.push(itemValue);
    });
    model.UserName = $('#UserName').val();
    model.GroupUserId = $('#groupUserId').val();
    model.Type = $('#Type').val();
    model.Phone = $('#Phone').val();
    model.FullName = $('#FullName').val();
    model.Address = $('#Address').val();
    model.Birthdate = $('#Birthdate').val();
    model.Gender = ((document.querySelector('#male').checked) === true ? '1' : '0');
    model.Email = $('#Email').val();
    model.Password = $('#Password').val();
    model.RetypePassword = $('#RetypePassword').val();
    model.WardId = $('#wardId').val();
    model.DistrictId = $('#districtId').val();
    model.ProvinceId = $('#provinceId').val();
    var files = $("#FileCreatUserUpload").get(0).files;
    if (files.length > 0) {
        dataProfile.append("File", files[0]);
    }
    model.AvatarPath = $('#imgCreatUserPreview').attr('src');
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (model.UserName === '') {
        toastr.error(GetNotifyByKey('Enter_account'), { timeOut: 5000 }); return false;
    } else if (model.Password === '') {
        toastr.error(GetNotifyByKey('Enter_password'), { timeOut: 5000 }); return false;
    } else if (model.Password.length < 6 || model.Password.length > 50) {
        toastr.error(GetNotifyByKey('Password_validate'), { timeOut: 5000 }); return false;
    } else if (model.RetypePassword === '') {
        toastr.error(GetNotifyByKey('Enter_passwordconfirmation'), { timeOut: 5000 }); return false;
    } else if (model.Password !== model.RetypePassword) {
        toastr.error(GetNotifyByKey('Password_notvalidate'), { timeOut: 5000 }); return false;
    } else if (model.FullName === '') {
        toastr.error(GetNotifyByKey('Enter_fullname'), { timeOut: 5000 }); return false;
    } else if (model.Email === '') {
        toastr.error(GetNotifyByKey('Enter_email'), { timeOut: 5000 }); return false;
    } else if (!re.test(model.Email)) {
        toastr.error(GetNotifyByKey('Email_wrong'), { timeOut: 5000 }); return false;
    } else if (model.Gender === '') {
        toastr.error(GetNotifyByKey('Select_gender'), { timeOut: 5000 }); return false;
    } else if (model.GroupUserId === '') {
        toastr.error(GetNotifyByKey('Select_groupuser'), { timeOut: 5000 }); return false;
    } else { return true; }
}

function GetListPermission() {
    model.GroupUserId = $('#groupUserId').val();
    model.Type = $('#Type').val();

    $.post("/User/ListPermission?id=" + model.GroupUserId + "&type=" + model.Type, function (result) {
        $("#list_data").html(result);
    });
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

function SearchInit(groupUserId, Type) {
    model.GroupUserId = groupUserId;
    model.Type = Type;
    GetListPermission();
}

$('#Type').change();

function uploadCreatUserImage() {
    $('#FileCreatUserUpload').trigger('click');
    $('#FileCreatUserUpload').change(function () {
        var data = new FormData();
        let reader = new FileReader();
        var files = $("#FileCreatUserUpload").get(0).files;
        if (files.length > 0) {
            data.append("File", files[0]);
            reader.readAsDataURL(files[0]);
            reader.onload = () => {
                $('#imgCreatUserPreview').attr("src", reader.result);
            };
        }
    });
}

function clearCreatUserImage() {
    $('#imgCreatUserPreview').attr("src", "/img/avatar-34.png");
    $('#FileCreatUserUpload').val("");
}
