var modelDocumentUpdate =
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


    PageSize: 20,
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
        modelDocumentUpdate.Size = files[i].size;
    }
    $('#nameFile').html(fileName);
});

function ValidateCate() {
    modelDocumentUpdate.DocumentTyeid = $("#documentTyeid").val();
    modelDocumentUpdate.Id = $("#documentId").val();
    if (modelDocumentUpdate.Size > 10240000) {
        toastr.error(GetNotifyByKey('Waring_File'), { timeOut: 5000 }); return false;
    }
    var IsDisplay = $("#statusSearch").val();
    if (IsDisplay === '0') {
        modelDocumentUpdate.IsDisplay = false;
    }

    modelDocumentUpdate.Name = $("#nameDocument").val();
    modelDocumentUpdate.Description = $("#desciptionDocument").val();


    if (modelDocumentUpdate.DocumentTyeid === '') {
        toastr.error(GetNotifyByKey('Select_Groupdocument'), { timeOut: 5000 }); return false;
    } else if (modelDocumentUpdate.Name === 0) {
        toastr.error(GetNotifyByKey('Enter_Documentname'), { timeOut: 5000 }); return false;
    }
    else { return true; }
}

function UpdateDocument(isContinue) {
    var result = ValidateCate();
    if (result === true) {
        //OpenWaiting();
        var fd = new FormData();
        for (var i = 0; i < _listFileAttachment.length; i++) {
            fd.append("update", _listFileAttachment[i]);
        }

        fd.append("modelDocumentUpdate", JSON.stringify(modelDocumentUpdate));
        $.ajax({
            url: '/Document/Update',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                if (data.Ok) {
                    sessionStorage.setItem("keyMess", GetNotifyByKey('Update_Document'));
                    window.location = '/Document/Index';
                }
                else {
                    toastr.error(data.Message, { timeOut: 5000 });
                }
                CloseWaiting();
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
            }
        });
    }

}

function DeleteFile() {
    _listFileAttachment = [];
    $('#nameFile').html('');
    $('#uploadFileDocument').val('');
}