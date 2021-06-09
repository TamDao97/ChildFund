$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});

$('.itemType1').on('click', function () {
    var vl = $(this).val();
    var check = $(this).is(":checked");
    if (vl === 'AT05' && check === true) {
        document.getElementById('divTypeOther').style.display = 'block';
    }
    if (vl === 'AT05' && check === false) {
        document.getElementById('divTypeOther').style.display = 'none';
    }
});
var model =
{
    Id: '',
    InformationSources: '',
    ReceptionTime: '',
    ReceptionDate: '',
    ChildName: '',
    ChildBirthdate: '',
    Gender: '',
    Age: '',
    CaseLocation: '',
    WardId: '',
    DistrictId: '',
    ProvinceId: '',
    CurrentHealth: '',
    SequelGuess: '',
    FatherName: '',
    FatherAge: '',
    FatherJob: '',
    MotherName: '',
    MotherAge: '',
    MotherJob: '',
    FamilySituation: '',
    PeopleCare: '',
    Support: '',
    ProviderName: '',
    ProviderPhone: '',
    ProviderAddress: '',
    ProviderNote: '',
    WordTitle: '',
    SourceNote: '',
    SeverityLevel: '',
    TypeOther:'',
    ListProfileAttachment: [],
    ListProfileAttachmentUpdate: [],
    ListAbuseType: []
};
function InitModel( Id) {
    model.Id = Id;
  
    var dataFile = $('#idlistFile').val();
    model.ListProfileAttachmentUpdate = JSON.parse(dataFile);
}
var _shortDateFormat = "dd/MM/yyyy";
var _listFileAttachment = [];
var _listProfileAttachment = [];
function ValidateCate() {
    model.ListAbuseType = [];
    var itemValue = '';
    var itemName = '';
    model.InformationSources = $('#InformationSources').val();
    model.ReceptionTime = $('#ReceptionTime').val();
    model.ReceptionDate = $('#ReceptionDate').val();
    $('#ListAbuseType_Div input:checked').each(function () {
        //$(this).attr('checked', "checked");
        itemValue = $(this).attr('value');
        itemName = $(this).attr('name');
        model.ListAbuseType.push({ Id: itemValue, Name: itemName });
    });
    model.ChildName = $('#ChildName').val();
    model.Gender = $('#Gender').val();
    model.ChildBirthdate = $('#ChildBirthdate').val();
    model.Age = $('#Age').val();
    model.SeverityLevel = $('#SeverityLevel').val();
    model.ProvinceId = $('#ProvinceId').val();
    model.DistrictId = $('#DistrictId').val();
    model.WardId = $('#WardId').val();
    model.CaseLocation = $('#CaseLocation').val();
    model.TypeOther = $('#TypeOther').val();
   // model.CurrentHealth = $('#CurrentHealth').val();
  //  model.SequelGuess = $('#SequelGuess').val();
    model.FatherName = $('#FatherName').val();
    model.FatherAge = $('#FatherAge').val();
    model.FatherJob = $('#FatherJob').val();
    model.MotherName = $('#MotherName').val();
    model.MotherAge = $('#MotherAge').val();
    model.MotherJob = $('#MotherJob').val();
  //  model.FamilySituation = $('#FamilySituation').val();
    model.PeopleCare = $('#PeopleCare').val();
    //model.Support = $('#Support').val();
    model.ProviderName = $('#ProviderName').val();
    model.ProviderPhone = $('#ProviderPhone').val();
    model.ProviderAddress = $('#ProviderAddress').val();
    model.ProviderNote = $('#ProviderNote').val();
    model.SourceNote = $('#SourceNote').val();
    model.WordTitle = $('#WordTitle').val();
    if (model.ReceptionDate === '') {
        toastr.error(GetNotifyByKey('Enter_Dateincident'), { timeOut: 5000 }); return false;
    } else
    if (model.ChildName === '') {
        toastr.error(GetNotifyByKey('Enter_ChildName'), { timeOut: 5000 }); return false;
    } else if (model.CaseLocation === '') {
        toastr.error(GetNotifyByKey('Enter_locationoccurred'), { timeOut: 5000 }); return false;
    } else if (model.ListAbuseType.length === 0) {
        toastr.error(GetNotifyByKey('Choose_Abuse'), { timeOut: 5000 }); return false;
    }
    else if (isNaN(model.Age)) {
        toastr.error(GetNotifyByKey('Age_Mustnumber'), { timeOut: 5000 }); return false;
    }
    else if (parseInt(model.Age) > 18 || parseInt(model.Age) < 0) {
        toastr.error(GetNotifyByKey('Age_Mustbetween'), { timeOut: 5000 }); return false;
    }
    else if (isNaN(model.FatherAge)) {
        toastr.error(GetNotifyByKey('Age_DadMustnumber'), { timeOut: 5000 }); return false;
    }
    else if (isNaN(model.MotherAge)) {
        toastr.error(GetNotifyByKey('Age_MotherMustnumber'), { timeOut: 5000 }); return false;
    }
    else { return true; }
}
//Lư thông tin hồ sơ
function Update() {
    var result = ValidateCate();
    if (result === true) {
        var fd = new FormData();
        for (var i = 0; i < _listProfileAttachment.length; i++) {
            fd.append(_listProfileAttachment[i].Id, _listFileAttachment[i]);
        }
        model.ListProfileAttachment = _listProfileAttachment;
        OpenWaiting();
        fd.append("model", JSON.stringify(model));
        $.ajax({
            url: '/ReportProfile/UpdateProProfile',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                if (data.Ok) {
                    sessionStorage.setItem("keyMess", GetNotifyByKey('Update_case'));
                    window.location = '/ReportProfile/Index';
                }
                else {
                    toastr.error(data.Message, { timeOut: 5000 });
                }
                CloseWaiting();
            },
            error: function (reponse) {
                CloseWaiting();
                toastr.error("Lỗi phát sinh trong quá trình xử lý.", { timeOut: 5000 });
            }
        });
    }

}

//Upload file
var modelFile =
{
    Name: '',
    ListProfileAttachment: []
};
//Upload file
var sizeFile = 0;
$('#uploadFileProfile').change(function (e) {
    var files = e.target.files;
    var kq = false;
    for (var i = 0; i < files.length; i++) {
        kq = CheckFile(files[i]);
        if (kq === true) {
            sizeFile = parseInt(files[i].size / 1024);
            if (sizeFile <= 10024) {
                _listFileAttachment.push(files[i]);
                _listProfileAttachment.push({ Id: Guid(), Name: files[i].name, Size: sizeFile, UploadDate: jQuery.format.date(Date.now(), _shortDateFormat) });
            } else {
                toastr.error("File " + files[i].name + GetNotifyByKey('Waring_File2'), { timeOut: 5000 }); return false;
            }
        }
    }
    ViewProfileAttachmentUpdate();
});
function CheckFile(file) {
    var kq = true;
    for (var i = 0; i < _listProfileAttachment.length; i++) {
        if (_listProfileAttachment[i].Name === file.name && _listProfileAttachment[i].Size === file.size) {
            i = 1000; kq = false;
        }
    }
    return kq;
}
//Xóa file
function DeleteFileUpdate(indexRemove) {
    model.ListProfileAttachmentUpdate.splice(indexRemove, 1);
    ViewProfileAttachmentUpdate();
}
function DeleteFile(indexRemove) {
    $('#uploadFileProfile').val('');
    _listProfileAttachment.splice(indexRemove, 1);
    _listFileAttachment.splice(indexRemove, 1);
    ViewProfileAttachmentUpdate();

}
//Hiển thị danh sách tài liệu liên quan
function ViewProfileAttachment() {
    modelFile.Name = "File";
    modelFile.ListProfileAttachment = _listProfileAttachment;
    $.post("/ReportProfile/GenViewFile", modelFile, function (result) {
        $("#bodyAttachment").append(result);
    });
}
function ViewProfileAttachmentUpdate() {
    modelFile.Name = "File";
    modelFile.ListProfileAttachment = model.ListProfileAttachmentUpdate;
    $.post("/ReportProfile/GenViewFileUpdate", modelFile, function (result) {
        $("#bodyAttachment").html(result);
        ViewProfileAttachment();
    });
}
function GetDistrict() {
    $.post("/Combobox/DistrictByWardId?wardId=" + $('#WardId').val(), function (result) {
        $("#DistrictId").html(result);
    });
}
function GetProvince() {
    $.post("/Combobox/ProvinceByWardId?wardId=" + $('#WardId').val(), function (result) {
        $("#ProvinceId").html(result);
    });
}
GetDistrict(); GetProvince();
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

function GetPopupContent(idInput) {
    $.post("/ReportProfile/ViewContentPopup", function (result) {
        $("#contentView").html(result);
        $('#idInput').val(idInput);
        CKEDITOR.instances["contentNote"].setData(model[idInput]);
        $('#modalViewContent').modal({
            show: 'true'
        });
    });
}
function SavePopup() {
    var idInput = $('#idInput').val();
    var contentNote = CKEDITOR.instances["contentNote"].getData();
    model[idInput] = contentNote;
    var modelContent = {
        Note: contentNote,
        Count: countLength
    };
    $.post("/ReportProfile/ViewContent", modelContent, function (result) {
        $("#" + idInput).val(result.ViewContent);
    });

}

function closePopupContent() {
    $('#modalViewContent').modal('hide');
    // $("#contentView").html('');
}