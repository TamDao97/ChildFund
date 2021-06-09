// danh sách hồ sơ mới cấp trung ương
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var modelSearch =
{
    InformationSources: '',
    ChildName: '',
    ProviderName: '',
    CaseLocation: '',
    CurrentHealth: '',
    DateFrom: '',
    DateTo: '',
    ProcessingStatus: '',
    SeverityLevel: '',
    Export: 0,

    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: false
};
function Refresh() {
    $('#InformationSources').val('');
    $('#ChildName').val('');
    $('#ProviderName').val('');
    $('#CaseLocation').val('');
    $('#CurrentHealth').val('');
    $('#DateFrom').val('');
    $('#DateTo').val('');
    $('#ProcessingStatus').val('');
    $('#SeverityLevel').val('');

    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}
function GetModelSearch() {
    modelSearch.InformationSources = $('#InformationSources').val();
    modelSearch.ChildName = $('#ChildName').val();
    modelSearch.ProviderName = $('#ProviderName').val();
    modelSearch.CaseLocation = $('#CaseLocation').val();
    modelSearch.CurrentHealth = $('#CurrentHealth').val();
    modelSearch.DateFrom = $('#DateFrom').val();
    modelSearch.DateTo = $('#DateTo').val();
    modelSearch.ProcessingStatus = $('#ProcessingStatus').val();
    modelSearch.SeverityLevel = $('#SeverityLevel').val();
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
    $.post("/ReportProfile/GetList", modelSearch, function (result) {
        $("#list_data").html(result);
        CloseWaiting();
        if (modelSearch.Export === 1) {
            var link = document.getElementById('linkDowload');
            link.focus();
            link.click();
        }

        modelSearch.Export = 0;
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
    Search();
}
function phantrang(PageNumber) {
    modelSearch.PageNumber = PageNumber;
    Search(0);
}

function CloseReportView(id) {
    $('#ReportProfileId').val(id);
    $('#ClosedNote').val('');
    $('#modalCloseReport').modal({
        show: 'true'
    });
}
function CloseReport() {
    var Id = $('#ReportProfileId').val();
    var ClosedNote = $('#ClosedNote').val();
    var ClosedDate = $('#ClosedDate').val();
    if (ClosedNote === '') {
        toastr.error(GetNotifyByKey('Enter_NoteClose'), { timeOut: 5000 }); return;
    }
    if (ClosedDate === '') {
        toastr.error(GetNotifyByKey('Select_Dateclosing'), { timeOut: 5000 }); return;
    }
    var model = { Id: Id, ClosedNote: ClosedNote, ClosedDate: ClosedDate, ProcessingStatus: 4 };
    var fd = new FormData();
    fd.append("model", JSON.stringify(model));
    OpenWaiting();
    $.ajax({
        url: "/ReportProfile/CloseReport",
        processData: false,
        contentType: false,
        type: "POST",
        data: fd,
        success: function (data) {
            CloseWaiting();
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('Close_case'), { timeOut: 5000 });
                $('#modalCloseReport').modal('hide');
                Search(0);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
        },
    });
}
