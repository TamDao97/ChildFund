$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var modelDocument =
{
    Id: '',
    DocumentTyeid: '',
    Name: '',
    Path: '',
    Size: '',
    Extension: '',
    Description: '',
    IsDisplay: null,
    UploadBy: '',
    UploadDateFrom: '',
    UploadDateTo: '',
    UpdateBy: '',
    UpdateDate: '',

    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true
};

function GetDocumentModelSearch() {
    modelDocument.Name = $('#nameSearch').val();
    modelDocument.Description = $('#description').val();
    modelDocument.UploadBy = $('#uploadby').val();
    modelDocument.UploadDateFrom = $('#uploadFrom').val();
    modelDocument.UploadDateTo = $('#uploadTo').val();
    modelDocument.DocumentTyeid = $('#documentTyeid').val();
    var IsDisplay = $('#statusSearch').val();
    modelDocument.IsDisplay = null;
    if (IsDisplay !== '') {
        if (IsDisplay === '1') {
            modelDocument.IsDisplay = true;
        } else {
            modelDocument.IsDisplay = false;
        }
    }
}

function SearchDocument() {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetDocumentModelSearch();
    $.post("/Document/ListDocument", modelDocument, function (result) {
        modelDocument.PageNumber = 1;
        $("#list_data").html(result);
    });
}

$("#nameSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        SearchDocument();
        return false;
    }
});

function ChangeSize() {
    modelDocument.PageNumber = 1;
    modelDocument.PageSize = $('#pageSize').val();
    SearchDocument();
}
function phantrang(PageNumber) {
    modelDocument.PageNumber = PageNumber;
    SearchDocument();
}

function RefreshListDocument() {
    $('#nameSearch').val('');
    $('#statusSearch').val('');
    $('#description').val('');
    $('#uploadby').val('');
    $('#uploadFrom').val('');
    $('#uploadTo').val('');
    $('#documentTyeid').val('').change();

    modelDocument.PageSize = 10;
    modelDocument.PageNumber = 1;
    SearchDocument();
}

function DeleteConfim(id) {
    $('#valueDelete').val(id);
    //$('#labelDelete').html(GetNotifyByKey('Delete_Confim_Document'));
    $('#labelDelete').html('Bạn có chắc chắn muốn xóa tài liệu này?/Are you sure to delete this document?');
    $('#modamDelete').modal({
        show: 'true'
    });
}

function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/Document/Delete",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                //toastr.success(GetNotifyByKey('Delete_Document'), { timeOut: 5000 });
                toastr.success("Xóa tài liệu thành công!/Delete document successfully!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                SearchDocument();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}

function DownloadFile(id) {
    $.ajax({
        url: "/Document/DownloadFile?id=" + id,
        type: "POST",
        success: function (data) {
            if (data.Ok === true) {
                var link = document.getElementById('linkDowload');
                link.download  = 'Download.rar';
                link.href = data.path;
                link.focus();
                link.click();
            } else {
                toastr.error("Error", { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}
