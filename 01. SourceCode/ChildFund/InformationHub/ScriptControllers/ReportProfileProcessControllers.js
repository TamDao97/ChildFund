$('.datepicker').datepicker({
    format: 'dd/mm/yyyy',
    endDate: '+0d',
    autoclose: true
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
var dateNow = new Date();
var numberYear = dateNow.getFullYear();
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
    SummaryCase: '',
    TypeOther:'',
    SeverityLevel: '',
    ListProfileAttachment: [],
    ListProfileAttachmentUpdate: [],
    ListAbuseType: [],
    IsExport: false
};
function InitModel(Id, StatusStep1, StatusStep2, StatusStep3, StatusStep4, StatusStep5) {
    model.Id = Id;
    var dataFile = $('#idlistFile').val();
    model.ListProfileAttachmentUpdate = JSON.parse(dataFile);
    GetContentEdit(Id);
}
//hàm này get sữ liệu cho các trình soạn thảo
function GetContentEdit(Id) {
    $.ajax({
        url: '/ReportProfile/GetContentEdit?id=' + Id,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            if (data.Ok === true) {
                model.CurrentHealth = data.obj.CurrentHealth;
                model.SequelGuess = data.obj.SequelGuess;
                model.FamilySituation = data.obj.FamilySituation;
                model.Support = data.obj.Support;
                model.SummaryCase = data.obj.SummaryCase;
            }
        },
        error: function (reponse) {
            CloseWaiting();
        }
    });
}
function InitTab() {

    document.getElementById('step-1').style.display = 'block';
    document.getElementById('step-2').style.display = 'none';
    document.getElementById('step-3').style.display = 'none';
    document.getElementById('step-4').style.display = 'none';
    document.getElementById('step-5').style.display = 'none';

    //if (ProcessingStatus === 1) {
    //    document.getElementById('step-2').style.display = 'block';
    //    document.getElementById('step-1').style.display = 'none';
    //    document.getElementById('step-3').style.display = 'none';
    //    document.getElementById('step-4').style.display = 'none';
    //    document.getElementById('step-5').style.display = 'none';
    //} else if (ProcessingStatus === 2) {
    //    document.getElementById('step-3').style.display = 'block';
    //    document.getElementById('step-1').style.display = 'none';
    //    document.getElementById('step-2').style.display = 'none';
    //    document.getElementById('step-4').style.display = 'none';
    //    document.getElementById('step-5').style.display = 'none';
    //} else if (ProcessingStatus === 3) {
    //    document.getElementById('step-4').style.display = 'block';
    //    document.getElementById('step-1').style.display = 'none';
    //    document.getElementById('step-2').style.display = 'none';
    //    document.getElementById('step-3').style.display = 'none';
    //    document.getElementById('step-5').style.display = 'none';
    //}
    //else if (ProcessingStatus === 4) {
    //    document.getElementById('step-5').style.display = 'block';
    //    document.getElementById('step-1').style.display = 'none';
    //    document.getElementById('step-2').style.display = 'none';
    //    document.getElementById('step-3').style.display = 'none';
    //    document.getElementById('step-4').style.display = 'none';
    //}
}
var _shortDateFormat = "dd/MM/yyyy";
var _listFileAttachment = [];
var _listProfileAttachment = [];
function ValidateCate() {
    var isCheckOther = false;
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
        if (itemValue === 'AT05') {
            isCheckOther = true;
        }
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
    //  model.CurrentHealth = $('#CurrentHealth').val();
    //  model.SequelGuess = $('#SequelGuess').val();
    model.FatherName = $('#FatherName').val();
    model.FatherAge = $('#FatherAge').val();
    model.FatherJob = $('#FatherJob').val();
    model.MotherName = $('#MotherName').val();
    model.MotherAge = $('#MotherAge').val();
    model.MotherJob = $('#MotherJob').val();
    //   model.FamilySituation = $('#FamilySituation').val();
    model.PeopleCare = $('#PeopleCare').val();
    //  model.Support = $('#Support').val();
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
        }
        else if (model.TypeOther === '' && isCheckOther === true) {
            toastr.error(GetNotifyByKey('TypeOtherKey'), { timeOut: 5000 }); return false;
        }
        else if (model.ListAbuseType.length === 0) {
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
        else if (parseInt(model.FatherAge) < 1900 || parseInt(model.FatherAge) > (numberYear - 15)) {
            toastr.error(GetNotifyByKey('Validate_AgeFather') + '1900 - ' + (numberYear - 15), { timeOut: 5000 }); return false;
        }
        else if (parseInt(model.MotherAge) < 1900 || parseInt(model.FatherAge) > (numberYear - 15)) {
            toastr.error(GetNotifyByKey('Validate_AgeMother') + '1900 - ' + (numberYear - 15), { timeOut: 5000 }); return false;
        }
        else { return true; }
}

//Lư thông tin hồ sơ
function UpdateTab1(IsContinue, isExport) {
    var result = ValidateCate();
    if (result === true) {
        var fd = new FormData();
        for (var i = 0; i < _listProfileAttachment.length; i++) {
            fd.append(_listProfileAttachment[i].Id, _listFileAttachment[i]);
        }
        model.ListProfileAttachment = _listProfileAttachment;
        model.IsExport = isExport;
        fd.append("model", JSON.stringify(model));
        OpenWaiting();
        $.ajax({
            url: '/ReportProfile/UpdateProProfile',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                CloseWaiting();
                if (data.Ok === false) {
                    toastr.error(data.Message, { timeOut: 5000 }); return false;
                } else {
                    if (isExport) {
                        var link = document.getElementById('linkDowload');
                        $(link).attr("href", data.Path);
                        link.focus();
                        link.click();
                    } else {
                        if (data.Ok) {
                            if (IsContinue) {
                                NextStep(1);
                            }
                            toastr.success(GetNotifyByKey('Update_case'), { timeOut: 5000 });
                        }
                        else {
                            toastr.error(data.Message, { timeOut: 5000 });
                        }
                    }
                }
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
                toastr.error("File " + files[i].name + ' ' + GetNotifyByKey('Waring_File2'), { timeOut: 5000 }); return false;
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
GetDistrict(); GetProvince();
function NextStep(id) {
    // var curStep = $(this).closest(".setup-content"),
    curStepBtn = 'step-' + id,
        nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
        isValid = true;
    if (isValid) {
        nextStepWizard.removeClass('disabled').trigger('click');
    }
}
$(document).ready(function () {
    InitTab();
});

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