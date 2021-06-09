// danh sách hồ sơ mới cấp trung ương
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy',

});


var modelSearch =
{
    AddressByUser: '',
    InformationSources: '',
    ChildName: '',
    ProviderName: '',
    CaseLocation: '',
    CurrentHealth: '',
    AbuseId: '',
    DateFrom: '',
    DateTo: '',
    StatusStep1: '',
    StatusStep2: '',
    StatusStep3: '',
    StatusStep4: '',
    StatusStep5: '',
    StatusStep6: '',
    SeverityLevel: '',
    WardId: '',
    DistrictId: '',
    ProvinceId: '',
    Age: '',
    Gender: '',
    Export: 0,

    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: false
};
function Refresh() {
    var type_User = $('#type_User').val();
    if (type_User === '0') {
        $('#sProvince').val('');
        $('#sDistrict').val('');
        $('#sWard').val('');
    } else if (type_User === '1') {
        $('#sDistrict').val('');
        $('#sWard').val('');
    }
    else if (type_User === '2') {
        $('#sWard').val('');
    }
    $('#InformationSources').val('');
    $('#AbuseId').val('');
    $('#ChildName').val('');
    $('#ProviderName').val('');
    $('#CaseLocation').val('');
    $('#CurrentHealth').val('');
    $('#DateFrom').val('');
    $('#DateTo').val('');
    $('#ProcessingStatus').val('');
    $('#SeverityLevel').val('');
    $('#Age').val('');
    $('#Gender').val('');
    $('#DateFrom').val($('#DateFrom_hid').val());
    $('#DateTo').val($('#DateTo_hid').val());
    modelSearch.PageSize = 10;
    modelSearch.PageNumber = 1;
    for (var i = 1; i < 7; i++) {
        modelSearch['StatusStep' + i] = false;
    }
    Search(0);
}
function GetModelSearch() {
    var type_User = $('#type_User').val();
    //địa chỉ
    var keyAddressByUser = sessionStorage.getItem("keyAddressByUser");
    if (keyAddressByUser !== null && keyAddressByUser !== '') {
        var arrayAddress = keyAddressByUser.split(';');
        if (arrayAddress[1] === '1') {
            $('#sProvince').val(arrayAddress[0]);
            modelSearch.ProvinceId = arrayAddress[0];
            modelSearch.DistrictId = '';
            modelSearch.WardId = '';
            $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
                $("#sDistrict").html('<option value="">Tất cả</option>' + result);
            });
        } else if (arrayAddress[1] === '2') {
            // $('#sDistrict').val(arrayAddress[0]);
            GetDistrictByDistrictId(arrayAddress[0]);
            modelSearch.DistrictId = arrayAddress[0];
            modelSearch.ProvinceId = '';
            modelSearch.WardId = '';
            $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
                $("#sWard").html('<option value="">Tất cả</option>' + result);
            });
        } else if (arrayAddress[1] === '3') {
            // $('#sWard').val(arrayAddress[0]);
            modelSearch.WardId = arrayAddress[0];
            modelSearch.ProvinceId = '';
            modelSearch.DistrictId = '';
            if (type_User === '0') {
                GetWardByWardId(arrayAddress[0]);
                GetDistrictByWardId(arrayAddress[0]);
                //  console.log(arrayAddress[0]);
            } else if (type_User === '1') {
                GetWardByWardId(arrayAddress[0]);
                // GetDistrictByWardId(arrayAddress[0]);
            } else if (type_User === '2') {
                GetWardByWardId(arrayAddress[0]);
            }
        }
        sessionStorage.removeItem("keyAddressByUser");
    } else {
        modelSearch.ProvinceId = $('#sProvince').val();
        modelSearch.DistrictId = $('#sDistrict').val();
        modelSearch.WardId = $('#sWard').val();
    }

    modelSearch.Age = $('#Age').val();
    modelSearch.Gender = $('#Gender').val();
    modelSearch.AddressByUser = $('#addressByUser').val();
    modelSearch.AbuseId = $('#AbuseId').val();
    modelSearch.InformationSources = $('#InformationSources').val();
    modelSearch.ChildName = $('#ChildName').val();
    modelSearch.ProviderName = $('#ProviderName').val();
    modelSearch.CaseLocation = $('#CaseLocation').val();
    modelSearch.CurrentHealth = $('#CurrentHealth').val();
    modelSearch.DateFrom = $('#DateFrom').val();
    modelSearch.DateTo = $('#DateTo').val();
    modelSearch.SeverityLevel = $('#SeverityLevel').val();
    var ProcessingStatus = $('#ProcessingStatus').val();
    for (var i = 1; i < 7; i++) {
        modelSearch['StatusStep' + i] = false;
    }
    modelSearch['StatusStep' + ProcessingStatus] = true;
}
function GetCacheSearch() {
    //giới tính
    var keyGender = sessionStorage.getItem("keyGender");
    if (keyGender !== null && keyGender !== '') {
        $('#Gender').val(keyGender);
        sessionStorage.removeItem("keyGender");
    }
    //giới tính
    var keyAge = sessionStorage.getItem("keyAge");
    if (keyAge !== null && keyAge !== '') {
        $('#Age').val(keyAge);
        sessionStorage.removeItem("keyAge");
    }
    //loại hình xp
    var keyAbuse = sessionStorage.getItem("keyAbuse");
    if (keyAbuse !== null && keyAbuse !== '') {
        $('#AbuseId').val(keyAbuse);
        sessionStorage.removeItem("keyAbuse");
    }
    //trạng thái
    //var keyStatus = sessionStorage.getItem("keyStatus");
    //if (keyStatus !== null && keyStatus !== '') {
    //    $('#ProcessingStatus').val(keyStatus);
    //    sessionStorage.removeItem("keyStatus");
    //}
    //thoi gian
    var keyDate = sessionStorage.getItem("keyDate");
    if (keyDate !== null && keyDate !== '') {
        var dateSearch = keyDate.split(';');
        $('#DateFrom').val(dateSearch[0]);
        $('#DateTo').val(dateSearch[1]);
        sessionStorage.removeItem("keyDate");
    }
}
function Search(Export) {
    modelSearch.Export = Export;
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetCacheSearch();
    GetModelSearch();
    OpenWaiting();
    $.post("/ReportProfile/GetList", modelSearch, function (result) {
        if (result.Ok === false) {
            CloseWaiting();
            toastr.error(result.Message, { timeOut: 5000 }); return false;
        } else {
            modelSearch.PageNumber = 1;
            $("#list_data").html(result);
            CloseWaiting();
            if (modelSearch.Export === 1) {
                var link = document.getElementById('linkDowload');
                link.focus();
                link.click();
            }
            modelSearch.Export = 0;
        }
    });
}
$("#CurrentHealth").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
$("#CaseLocation").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
$("#ProviderName").keydown(function (event) {
    if (event.keyCode === 13) {
        Search(0);
        return false;
    }
});
$("#ChildName").keydown(function (event) {
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

function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html(GetNotifyByKey('Delete_Confim_Report'));
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/ReportProfile/CloseReport?id=" + id,
        type: "POST",
        success: function (data) {
            if (data.Ok === true) {
                toastr.success(GetNotifyByKey('Delete_Confim_ReportOk'), { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}

function ForwardConfim(id) {
    $('#ReportProfileId').val(id);
    $('#ForwardNote').val('');
    $('#modalForwardReport').modal({
        show: 'true'
    });
}

function DeleteCaseConfirm(id) {
    $('#valueDeleteCase').val(id);
    $('#labelDeleteCase').html(GetNotifyByKey('DeleteCase_Confim_Report'));
    $('#modalDeleteCase').modal({
        show: 'true'
    });
}

function ReOpenCaseConfirm(id) {
    $('#valueReOpen').val(id);
    $('#labelReOpen').html(GetNotifyByKey('ReOpen_Confim_Report'));
    $('#modalReOpen').modal({
        show: 'true'
    });
}

function SendForward() {
    var Id = $('#ReportProfileId').val();
    var ForwardNote = $('#ForwardNote').val();
    if (ForwardNote === '') {
        toastr.error(GetNotifyByKey('Enter_NoteClose'), { timeOut: 5000 }); return;
    }
    OpenWaiting();
    var model = { Id: Id, ForwardNote: ForwardNote };
    $.ajax({
        url: "/ReportProfile/SendForward",
        data: model,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.Ok === true) {
                $('#modalForwardReport').modal('hide');
                toastr.success(GetNotifyByKey('Forward_Case'), { timeOut: 5000 });
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

//viet mơi tk theo tỉnh thành
function GetDistrict() {
    modelSearch.ProvinceId = $('#sProvince').val();
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#sDistrict").html('<option value="">Tất cả</option>' + result);
    });
}
function GetWard() {
    modelSearch.DistrictId = $('#sDistrict').val();
    $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
        $("#sWard").html('<option value="">Tất cả</option>' + result);
    });
}
//get combobox theo dk tìm kiếm truyền sang
function GetDistrictByDistrictId(districtId) {
    $.post("/Combobox/GetDistrictByDistrictId?districtId=" + districtId, function (result) {
        $("#sDistrict").html('<option value="">Tất cả</option>' + result);
        $("#sDistrict").val(districtId);
    });
}
function GetDistrictByWardId(wardId) {
    $.post("/Combobox/GetDistrictByWardId?wardId=" + wardId, function (result) {
        $("#sDistrict").html('<option value="">Tất cả</option>' + result);
    });
}
function GetWardByWardId(wardId) {
    $.post("/Combobox/GetWardByWardId?wardId=" + wardId, function (result) {
        $("#sWard").html('<option value="">Tất cả</option>' + result);
        $("#sWard").val(wardId);
    });
}

function PublishConfim(id) {
    $('#valuePublish').val(id);
    $('#labelPublish').html(GetNotifyByKey('Publish_Confim'));
    $('#modamPublish').modal({
        show: 'true'
    });
}
function Publish() {
    var id = $('#valuePublish').val();
    $.ajax({
        url: "/ReportProfile/PublishReport?id=" + id,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('Publish_ConfimOk'), { timeOut: 5000 });
                $('#modamPublish').modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}

function ReOpen() {
    var id = $('#valueReOpen').val();
    $.ajax({
        url: "/ReportProfile/ReOpenCase?id=" + id,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('ReOpenSuccessfully'), { timeOut: 5000 });
                $('#modalReOpen').modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}

function DeleteCase() {
    var id = $('#valueDeleteCase').val();
    $.ajax({
        url: "/ReportProfile/DeleteCase?id=" + id,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('DeleteCaseSuccessfully'), { timeOut: 5000 });
                $('#modalDeleteCase').modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}