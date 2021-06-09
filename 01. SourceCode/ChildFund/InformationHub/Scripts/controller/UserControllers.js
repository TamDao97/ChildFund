var dataProfile = new FormData();
// danh sách
var modelSearch =
{
    Id: '',
    Name: '',
    FullName: '',
    Type: '',
    ProvinceId: '',
    DistrictId: '',
    WardId: '',
    Export: 0,

    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'Type',
    OrderType: true
};

function Refresh() {
    $('#userNameSearch').val('');
    $('#fullNameSearch').val('');
    $('#provinceId').val('');
    $('#districtId').val('');
    $('#wardId').val('');
    $('#Type').val('');
    modelSearch.PageSize = 10;
    modelSearch.PageNumber = 1;
    Search(0);
}

function GetModelSearch() {
    modelSearch.Name = $('#userNameSearch').val();
    modelSearch.FullName = $('#fullNameSearch').val();
    modelSearch.Type = $('#Type').val();
    modelSearch.ProvinceId = $('#provinceId').val();
    modelSearch.DistrictId = $('#districtId').val();
    modelSearch.WardId = $('#wardId').val();
}
function Search(Export) {
    modelSearch.Export = Export;
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    var pageSizeByUser = $('#pageSize').val();
    if (pageSizeByUser != null) {
        modelSearch.PageSize = pageSizeByUser;
    }
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/User/ListUser", modelSearch, function (result) {
        $("#list_user").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
        if (Export !== 0) {
            var link = document.getElementById('linkDonwload');
            link.focus();
            link.click();
        }
        modelSearch.Export = 0;
    });
}

$("#userNameSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
$("#fullNameSearch").keydown(function (event) {
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
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    modelSearch.PageNumber = PageNumber;
    $.post("/User/ListUser", modelSearch, function (result) {
        $("#list_user").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}

function LockUser(id) {
    $.ajax({
        url: "/User/LockUser?id=" + id,
        type: "POST",
        cache: false,
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('Lock_user'), { timeOut: 5000 });
                Search(0);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}
function UnLockUser(id) {
    $.ajax({
        url: "/User/UnLockUser?id=" + id,
        type: "POST",
        cache: false,
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('Unlock_user'), { timeOut: 5000 });
                Search(0);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}

function ResetPassword(id) {
    $('#rsPassword').val('');
    $('#rsRetypepassword').val('');
    $.ajax({
        url: "/User/GetUserInfo?id=" + id,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                $('#rsUserId').val(id);
                $('#rsUserName').val(data.data.UserName);
                $('#rsFullName').val(data.data.FullName);
                $('#rsUserName').prop('disabled', true);
                $('#rsFullName').prop('disabled', true);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        }
    });
    $('#modalResetPassword').modal('show');
}
function Reset() {
    var id = $('#rsUserId').val();
    var rsPassword = $('#rsPassword').val();
    var rsRetypepassword = $('#rsRetypepassword').val();
    if (rsPassword.length < 6 || rsPassword.length > 50) {
        toastr.error(GetNotifyByKey('Password_validate'), { timeOut: 5000 }); return false;
    } else {
        if (rsPassword == rsRetypepassword) {
            $.ajax({
                url: "/User/ResetPassword",
                data: { id: id, password: rsPassword },
                cache: false,
                type: "POST",
                success: function (data) {
                    if (data.ok === true) {
                        toastr.success(GetNotifyByKey('Password_reset'), { timeOut: 5000 });
                        CloseModelRS();
                    } else {
                        toastr.error(data.mess, { timeOut: 5000 });
                    }
                },
                error: function (reponse) {
                    toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
                }
            });
        } else {
            toastr.error(GetNotifyByKey('Password_notvalidate'), { timeOut: 5000 }); return false;
        }
    }
}

function CloseModelRS() {
    $('#modalResetPassword').modal('hide');
    Search(0);
}

function GetDistrict() {
    modelSearch.ProvinceId = $('#provinceId').val();
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#districtId").html('<option selected value=""></option>' + result);
    });
}
function GetWard(DistrictId) {
    modelSearch.DistrictId = $('#districtId').val();
    $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
        $("#wardId").html('<option selected value=""></option>' + result);
    });
}

function DownloadTemplate() {
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/User/DownloadTemplate", function (result) {
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";

        var linkTemplate = document.getElementById('linkDownloadTemplate');
        linkTemplate.href = result.PathFile;
        linkTemplate.download = 'Mau-Excel-Them-Nguoi-Dung.xlsx';
        linkTemplate.focus();
        linkTemplate.click();
    });
}


function ShowImportProfile() {
    $('#fileImport').val('');
    $('#modamImport').modal({
        show: 'true'
    });
    document.getElementById("fileSelectImport").value = "";
}
var dataFrom = null;
function ImportProfile() {
    var fileValue = $('#fileImport').val();
    if (fileValue == '') {
        toastr.error(GetNotifyByKey('keySelectExcel')); return;
    }
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.ajax({
        url: "/User/ImportProfile",
        data: dataFrom,
        type: "POST",
        cache: false,
        processData: false,
        contentType: false,
        dataType: 'json',
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('keyImportExcel'));
                $('#fileImport').val('');
                $('#modamImport').modal('hide');
                Refresh();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
        },
        error: function (reponse) {
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        }
    });
}
$('#fileSelectImport').change(function () {
    dataFrom = new FormData();
    let reader = new FileReader();
    var files = $("#fileSelectImport").get(0).files;
    if (files.length > 0) {
        dataFrom.append("Avatar", files[0]);
        $('#fileImport').val(files[0].name);

    }
});