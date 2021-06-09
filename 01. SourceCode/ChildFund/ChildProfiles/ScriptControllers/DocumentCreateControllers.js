var modelDocumentCreate =
{
    Id: '',
    DocumentTyeid: '',
    Name: '',
    Path: '',
    Size: '',
    Extension: '',
    Description: '',
    IsDisplay: true,
    UploadBy: '',
    UpdateBy: '',
    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true
};
var _listFileAttachment = [];
var fileName;

// upload file
$('#uploadFileDocument').change(function (e) {
    var files = e.target.files;
    _listFileAttachment = [];
    for (var i = 0; i < files.length; i++) {
        _listFileAttachment.push(files[i]);
        fileName = files[i].name;
        modelDocumentCreate.Size = files[i].size;
    }
    $('#nameFile').html(fileName);
});

function ValidateCate() {
    modelDocumentCreate.DocumentTyeid = $("#documentTyeid").val();
    var IsDisplay = $("#statusSearch").val();
    if (modelDocumentCreate.Size > 10240000) {
        toastr.error("Tệp dung lượng lớn hơn 10mb", { timeOut: 5000 }); return false;
    }
    if (IsDisplay === '0') {
        modelDocumentCreate.IsDisplay = false;
    }
    modelDocumentCreate.Name = $("#nameDocument").val();
    modelDocumentCreate.Description = $("#desciptionDocument").val();
    if (modelDocumentCreate.DocumentTyeid === '') {
        toastr.error("Chọn nhóm tài liệu/Choose document type", { timeOut: 5000 }); return false;
    } else if (modelDocumentCreate.Name === "") {
        toastr.error("Nhập tên tài liệu/enter document name", { timeOut: 5000 }); return false;
    } else if (_listFileAttachment.length === 0) {
        toastr.error("Chọn tài liệu đính kèm/Choose attachment", { timeOut: 5000 }); return false;
    }
    else { return true; }
}

function CreateDocument(isContinue) {
    var result = ValidateCate();
    if (result === true) {
        var fd = new FormData();
        for (var i = 0; i < _listFileAttachment.length; i++) {
            fd.append("dd", _listFileAttachment[i]);
        }
        fd.append("modelDocumentCreate", JSON.stringify(modelDocumentCreate));
        $.ajax({
            url: '/Document/Create',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                if (data.Ok) {
                    if (isContinue === true) {
                        toastr.success("Thêm mới thành công/Create successfully", { timeOut: 5000 });
                        window.location.href = '/Document/CreateDocument';
                    } else {
                        sessionStorage.setItem("keyMess", "Thêm mới thành công/Create successfully");
                        window.location = '/Document/Index';

                    }
                }
                else {
                    toastr.error(data.Message, { timeOut: 5000 });
                }
                CloseWaiting();
            },
            error: function (reponse) {
                toastr.error("Có lỗi trong quá trình xử lý/Error has occured while processing", { timeOut: 5000 });
            }
        });
    }

}

function DeleteFile() {
    _listFileAttachment = [];
    $('#nameFile').html('');
    $('#uploadFileDocument').val('');
}

//Nhóm tài liệu
var modelDocumentType =
{
    Id: '',
    Name: '',
    Description: '',
    IsDisplay: true,
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: '',

    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true
};

function DocumentTypeShow() {
    $('#modalDocumentType').modal({
        show: 'true'
    });

    SearchDocumentType();
    $("#nameDocumentType").val("");
}

function SearchDocumentType() {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    //GetDocumentModelSearch();
    //OpenWaiting();
    $.post("/Document/ListDocumentType", modelDocumentType, function (result) {
        modelDocumentType.PageNumber = 1;
        $("#list_data").html(result);
        //CloseWaiting();
    });
}

function ChangeSize() {
    modelDocumentType.PageNumber = 1;
    modelDocumentType.PageSize = $('#pageSize').val();
    SearchDocumentType();
}

function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html("Bạn có chắc chắn muốn xóa nhóm tài liệu này?/Are you sure to delete this document?");
    $('#modamDelete').modal({
        show: 'true'
    });
}

function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/Document/DeleteDocumentType",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Xóa tài liệu thành công/Delete document successfully", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                $("#nameDocumentType").val("");
                SearchDocumentType();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi, vui lòng thử lại sau/Error has occured, please try later", { timeOut: 5000 });
        },
    });
}

function ValidateDocumentType() {
    modelDocumentType.Name = $("#nameDocumentType").val();
    if (modelDocumentType.Name === '') {
        toastr.error("Nhập tên nhóm tài liệu/Enter document type name", { timeOut: 5000 }); return false;
    }
    else { return true; }
}

function CreateDocumentType() {
    dataProfile = new FormData();
    if (ValidateDocumentType()) {
        $.ajax({
            url: "/Document/CreateDocumentType",
            data: JSON.stringify(modelDocumentType),
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Thêm nhóm tài liệu thành công/Create document type successfully", { timeOut: 5000 });
                    $("#nameDocumentType").val("");
                    DocumentTypeShow();
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                CloseWaiting();
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi, vui lòng thử lại sau/Error has occured, please try later", { timeOut: 5000 });
            }
        });
    }
}

function UpdateDocumentTypeShow(id, name) {
    modelDocumentType.Id = id;
    $("#nameDocumentType").val(name);
}

function UpdateDocumentType() {
    var result = ValidateDocumentType();
    if (result === true) {
        $.ajax({
            url: "/Document/UpdateDocumentType",
            data: modelDocumentType,
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.ok === true) {
                    sessionStorage.setItem("keyMess", "Cập nhật nhóm tài liệu thành công/Update document type succseefully"); 
                    $("#nameDocumentType").val("");
                    DocumentTypeShow();
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                CloseWaiting();
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi, vui lòng thử lại sau/Error has occured, please try later", { timeOut: 5000 });
                CloseWaiting();
            }

        });

    }
}

function phantrang(PageNumber) {
    modelDocumentType.PageNumber = PageNumber;
    SearchDocumentType();
}

