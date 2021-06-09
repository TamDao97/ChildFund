// danh sách hồ sơ mới cấp trung ương
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
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
    Export: 0,
    Age: '',
    Gender: '',
    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'CreateDate',
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

    $('#addressByUser').val('');
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
    //modelSearch.ProcessingStatus = $('#ProcessingStatus').val();
    modelSearch.SeverityLevel = $('#SeverityLevel').val();
    modelSearch.ProvinceId = $('#sProvince').val();
    modelSearch.DistrictId = $('#sDistrict').val();
    modelSearch.WardId = $('#sWard').val();
    var ProcessingStatus = $('#ProcessingStatus').val();
    for (var i = 1; i < 7; i++) {
        modelSearch['StatusStep' + i] = false;
    }
    modelSearch['StatusStep' + ProcessingStatus] = true;
}

function Search(Export) {
    modelSearch.Export = Export;
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    OpenWaiting();
    $.post("/ReportProfile/GetListForward", modelSearch, function (result) {
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

function ForwardConfim(id) {
    $('#ReportProfileId').val(id);
    $('#ForwardNote').val('');
    $('#modalForwardReport').modal({
        show: 'true'
    });
}
function SendForward() {
    var Id = $('#ReportProfileId').val();
    var ForwardNote = $('#ForwardNote').val();
    if (ForwardNote === '') {
        toastr.error(GetNotifyByKey('Enter_NoteClose'), { timeOut: 5000 }); return;
    }
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
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
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
        }
    });
}

function DetailForward(id) {
    $.ajax({
        url: "/ReportProfile/DetailForward?id=" + id,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                $('#ForwardViewNote').val(data.note);
                $('#modalForwardViewReport').modal({
                    show: 'true'
                });
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
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
