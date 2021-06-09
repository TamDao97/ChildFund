$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
//bat dau tab so 2
$('#table1_tab1 input[type="radio"]').on('change', function (e) {
    CounTable1Tab1();
});
$('#table2_tab1 input[type="radio"]').on('change', function (e) {
    CounTable2Tab1();
});
$('#table1_tab2 input[type="radio"]').on('change', function (e) {
    CounTable1Tab2();
});
$('#table2_tab2 input[type="radio"]').on('change', function (e) {
    CounTable2Tab2();
});

//het dem
function UpdateTab2(IsContinue, isExport) {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        $('#stepFirst').click();
        return false;
    }
    var result2 = ValidateEvaluationFirstModel();
    modelEv.ReportProfileId = model.Id;
    modelEv.IsExport = isExport;
    if (result2 === true) {
        var fd = new FormData();
        fd.append("modelEv", JSON.stringify(modelEv));
        OpenWaiting();
        $.ajax({
            url: '/ReportProfile/UpdateProcessTab2',
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
                    }
                    if (data.Ok) {
                        if (IsContinue) {
                            NextStep(2);
                        }
                        toastr.success(GetNotifyByKey('UpdateEvFirst_Ok'), { timeOut: 5000 });
                    }
                    else {
                        toastr.error(data.Message, { timeOut: 5000 });
                    }
                }
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
            }
        });
    }

}

function InitModelSup() {
    //ko xử lý
    modelSup.ListProfileAttachment = [];
    var dataFileSup = $('#idlistFileSup').val();
    modelSup.ListProfileAttachmentUpdate = JSON.parse(dataFileSup);
}
var _shortDateFormatSup = "dd/MM/yyyy";
var _listFileAttachmentSup = [];
var _listProfileAttachmentSup = [];




//Upload file
var modelFileSup =
{
    Name: '',
    ListProfileAttachment: []
};
//Upload file
//$('#uploadFileProfileSup').change(function (e) {
//    var files = e.target.files;
//    var kq = false;
//    for (var i = 0; i < files.length; i++) {
//        kq = CheckFileSup(files[i]);
//        if (kq === true) {
//            _listFileAttachmentSup.push(files[i]);
//            _listProfileAttachmentSup.push({ Id: Guid(), Name: files[i].name, Size: files[i].size, UploadDate: jQuery.format.date(Date.now(), _shortDateFormatSup) });
//        }
//    }
//    ViewProfileAttachmentUpdateSup();
//});
function CheckFileSup(file) {
    var kq = true;
    for (var i = 0; i < _listProfileAttachmentSup.length; i++) {
        if (_listProfileAttachmentSup[i].Name === file.name && _listProfileAttachmentSup[i].Size === file.size) {
            i = 1000; kq = false;
        }
    }
    return kq;
}
//Xóa file
function DeleteFileUpdateSup(indexRemove) {
    modelSup.ListProfileAttachmentUpdateSup.splice(indexRemove, 1);
    ViewProfileAttachmentUpdateSup();
}
function DeleteFileSup(indexRemove) {
    $('#uploadFileProfileSup').val('');
    _listProfileAttachmentSup.splice(indexRemove, 1);
    _listFileAttachmentSup.splice(indexRemove, 1);
    ViewProfileAttachmentUpdateSup();

}
//Hiển thị danh sách tài liệu liên quan
function ViewProfileAttachmentSup() {
    modelFileSup.Name = "File";
    modelFileSup.ListProfileAttachment = _listProfileAttachmentSup;
    $.post("/ReportProfile/GenViewFileSup", modelFileSup, function (result) {
        $("#bodyAttachmentSup").append(result);
    });
}
function ViewProfileAttachmentUpdateSup() {
    modelFileSup.Name = "File";
    modelFileSup.ListProfileAttachment = modelSup.ListProfileAttachmentUpdate;
    $.post("/ReportProfile/GenViewFileUpdateSub", modelFileSup, function (result) {
        $("#bodyAttachmentSup").html(result);
        ViewProfileAttachmentSup();
    });
}
//add dong table
function AddActionTable() {
    $.post("/ReportProfile/GenActionTable", function (result) {
        $("#tbodyItem").append(result);
    });
}
function DeleteTable(id) {
    $('#valueDelete').val(id);
    $('#typeDelete').val('100');

    $('#labelDelete').html(GetNotifyByKey('DeleteAction_Confim'));
    $('#modamDelete').modal({
        show: 'true'
    });

}

//lay du lieu tab 1- bang EvaluationFirstModel
var modelEv =
{
    Id: '',
    ReportProfileId: '',
    PerformingDate: '',
    LevelHarm: '',
    LevelHarmContinue: '',
    TotalLevelHigh: '',
    TotalLevelAverage: '',
    TotalLevelLow: '',
    AbilityProtectYourself: '',
    AbilityReceiveSupport: '',
    TotalAbilityHigh: '',
    TotalAbilityAverage: '',
    TotalAbilityLow: '',
    Result: '',
    UnitProvideLiving: '',
    UnitProvideCare: '',
    ServiceProvideLiving: '',
    ServiceProvideCare: '',
    LevelHarmNote: '',
    LevelHarmContinueNote: '',
    AbilityProtectYourselfNote: '',
    AbilityReceiveSupportNote: '',
    IsExport: false
};
function ValidateEvaluationFirstModel() {
    modelEv.ReportProfileId = model.Id;
    modelEv.PerformingDate = $('#EvPerformingDate').val();
    modelEv.LevelHarm = $('input[name=EvLevelHarm]:checked').val();
    modelEv.LevelHarmContinue = $('input[name=EvLevelHarmContinue]:checked').val();
    modelEv.TotalLevelHigh = $('#EvTotalLevelHigh').html();
    modelEv.TotalLevelAverage = $('#EvTotalLevelAverage').html();
    modelEv.TotalLevelLow = $('#EvTotalLevelLow').html();

    modelEv.AbilityProtectYourself = $('input[name=EvAbilityProtectYourself]:checked').val();
    modelEv.AbilityReceiveSupport = $('input[name=EvAbilityReceiveSupport]:checked').val();
    modelEv.TotalAbilityHigh = $('#EvTotalAbilityHigh').html();
    modelEv.TotalAbilityAverage = $('#EvTotalAbilityAverage').html();
    modelEv.TotalAbilityLow = $('#EvTotalAbilityLow').html();

    modelEv.Result = $('#EvResult').val();
    modelEv.UnitProvideLiving = $('#EvUnitProvideLiving').val();
    modelEv.UnitProvideCare = $('#EvUnitProvideCare').val();
    modelEv.AbilityProtectYourselfNote = $('#EvAbilityProtectYourselfNote').val();
    modelEv.AbilityReceiveSupportNote = $('#EvAbilityReceiveSupportNote').val();
    modelEv.LevelHarmContinueNote = $('#EvLevelHarmContinueNote').val();
    modelEv.LevelHarmNote = $('#EvLevelHarmNote').val();
    modelEv.ServiceProvideCare = $('#EvServiceProvideCare').val();
    modelEv.ServiceProvideLiving = $('#EvServiceProvideLiving').val();
    return true;
}
//lay du lieu tab 2- bang EvaluationFirstModel
var modelCa =
{
    Id: '',
    ReportProfileId: model.Id,
    PerformingDate: '',
    PerformingBy: '',
    Condition: '',
    FamilySituation: '',
    CurrentQualityCareOK: '',
    CurrentQualityCareNG: '',
    PeopleCareFuture: '',
    FutureQualityCareOK: '',
    FutureQualityCareNG: '',
    LevelHarm: '',
    LevelApproach: '',
    LevelDevelopmentEffect: '',
    LevelCareObstacle: '',
    LevelNoGuardian: '',
    TotalLevelHigh: '',
    TotalLevelAverage: '',
    TotalLevelLow: '',
    AbilityProtectYourself: '',
    AbilityKnowGuard: '',
    AbilityEstablishRelationship: '',
    AbilityRelyGuard: '',
    AbilityHelpOthers: '',
    TotalAbilityHigh: '',
    TotalAbilityAverage: '',
    TotalAbilityLow: '',
    Result: '',
    ProblemIdentify: '',
    ChildAspiration: '',
    FamilyAspiration: '',
    ServiceNeeds: '',
    Extend: '',
    IsExport: false
};
function ValidateCaseVerificationModel() {
    modelCa.ReportProfileId = model.Id;
    modelCa.PerformingDate = $('#CaPerformingDate').val();
    modelCa.PerformingBy = $('#CaPerformingBy').val();
    modelCa.Condition = $('#CaCondition').val();
    modelCa.FamilySituation = $('#CaFamilySituation').val();
    modelCa.CurrentQualityCareOK = $('#CaCurrentQualityCareOK').val();
    modelCa.CurrentQualityCareNG = $('#CaCurrentQualityCareNG').val();
    modelCa.PeopleCareFuture = $('#CaPeopleCareFuture').val();
    modelCa.FutureQualityCareOK = $('#CaFutureQualityCareOK').val();
    modelCa.FutureQualityCareNG = $('#CaFutureQualityCareNG').val();

    modelCa.LevelHarm = ($('input[name=CaLevelHarm]:checked').val() !== undefined) ? $('input[name=CaLevelHarm]:checked').val() : '-1';
    modelCa.LevelApproach = ($('input[name=CaLevelApproach]:checked').val() !== undefined) ? $('input[name=CaLevelApproach]:checked').val() : '-1';
    modelCa.LevelDevelopmentEffect = ($('input[name=CaLevelDevelopmentEffect]:checked').val() !== undefined) ? $('input[name=CaLevelDevelopmentEffect]:checked').val() : '-1';
    modelCa.LevelCareObstacle = ($('input[name=CaLevelCareObstacle]:checked').val() !== undefined) ? $('input[name=CaLevelCareObstacle]:checked').val() : '-1';
    modelCa.LevelNoGuardian = ($('input[name=CaLevelNoGuardian]:checked').val() !== undefined) ? $('input[name=CaLevelNoGuardian]:checked').val() : '-1';

    modelCa.TotalLevelHigh = $('#CaTotalLevelHigh').html();
    modelCa.TotalLevelAverage = $('#CaTotalLevelAverage').html();
    modelCa.TotalLevelLow = $('#CaTotalLevelLow').html();

    modelCa.AbilityProtectYourself = ($('input[name=CaAbilityProtectYourself]:checked').val() !== undefined) ? $('input[name=CaAbilityProtectYourself]:checked').val() : '-1';
    modelCa.AbilityKnowGuard = ($('input[name=CaAbilityKnowGuard]:checked').val() !== undefined) ? $('input[name=CaAbilityKnowGuard]:checked').val() : '-1';
    modelCa.AbilityEstablishRelationship = ($('input[name=CaAbilityEstablishRelationship]:checked').val() !== undefined) ? $('input[name=CaAbilityEstablishRelationship]:checked').val() : '-1';
    modelCa.AbilityRelyGuard = ($('input[name=CaAbilityRelyGuard]:checked').val() !== undefined) ? $('input[name=CaAbilityRelyGuard]:checked').val() : '-1';
    modelCa.AbilityHelpOthers = ($('input[name=CaAbilityHelpOthers]:checked').val() !== undefined) ? $('input[name=CaAbilityHelpOthers]:checked').val() : '-1';

    modelCa.TotalAbilityHigh = $('#CaTotalAbilityHigh').html();
    modelCa.TotalAbilityAverage = $('#CaTotalAbilityAverage').html();
    modelCa.TotalAbilityLow = $('#CaTotalAbilityLow').html();
    modelCa.Result = $('#CaResult').val();
    modelCa.Extend = $('#CaExtend').val();
    modelCa.ProblemIdentify = $('#CaProblemIdentify').val();
    modelCa.ChildAspiration = $('#CaChildAspiration').val();
    modelCa.FamilyAspiration = $('#CaFamilyAspiration').val();
    modelCa.ServiceNeeds = $('#CaServiceNeeds').val();

    modelCa.LevelHarmNote = $('#CaLevelHarmNote').val();
    modelCa.LevelApproachNote = $('#CaLevelApproachNote').val();
    modelCa.LevelDevelopmentEffectNote = $('#CaLevelDevelopmentEffectNote').val();
    modelCa.LevelCareObstacleNote = $('#CaLevelCareObstacleNote').val();
    modelCa.LevelNoGuardianNote = $('#CaLevelNoGuardianNote').val();
    modelCa.AbilityProtectYourselfNote = $('#CaAbilityProtectYourselfNote').val();
    modelCa.AbilityKnowGuardNote = $('#CaAbilityKnowGuardNote').val();
    modelCa.AbilityEstablishRelationshipNote = $('#CaAbilityEstablishRelationshipNote').val();
    modelCa.AbilityRelyGuardNote = $('#CaAbilityRelyGuardNote').val();
    modelCa.AbilityHelpOthersNote = $('#CaAbilityHelpOthersNote').val();
    return true;
}

// bat dau tab so 3
function UpdateTab3(IsContinue, isExport) {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        $('#stepFirst').click();
        return false;
    }
    var result3 = ValidateCaseVerificationModel();
    modelCa.ReportProfileId = model.Id;
    modelCa.IsExport = isExport;
    if (result3 === true) {
        var fd = new FormData();
        fd.append("modelCa", JSON.stringify(modelCa));
        OpenWaiting();
        $.ajax({
            url: '/ReportProfile/UpdateProcessTab3',
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
                    }
                    if (data.Ok) {
                        if (IsContinue) {
                            NextStep(3);
                        }
                        toastr.success(GetNotifyByKey('Update_verification'), { timeOut: 5000 });
                    }
                    else {
                        toastr.error(data.Message, { timeOut: 5000 });
                    }
                }
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
            }
        });
    }

}

//bat đâu tab so 5
var modelSupAf =
{
    Id: '',
    ReportProfileId: model.Id,
    PerformingDate: '',
    PerformingBy: '',
    LevelHarm: '',
    LevelApproach: '',
    LevelCareObstacle: '',
    TotalLevelHigh: '',
    TotalLevelAverage: '',
    TotalLevelLow: '',
    AbilityProtectYourself: '',
    AbilityKnowGuard: '',
    AbilityHelpOthers: '',
    TotalAbilityHigh: '',
    TotalAbilityAverage: '',
    TotalAbilityLow: '',
    Result: '',
    SupportAfterTitle: '',
    LevelHarmNote: '',
    LevelApproachNote: '',
    LevelCareObstacleNote: '',
    AbilityProtectYourselfNote: '',
    AbilityKnowGuardNote: '',
    AbilityHelpOthersNote: '',
    IsExport: false
};
function ValidateSupportAfterStatusModel() {
    modelSupAf.ReportProfileId = model.Id;
    modelSupAf.Id = $('#idSupportAfter').val();
    modelSupAf.PerformingDate = $('#SupAfPerformingDate').val();
    modelSupAf.PerformingBy = $('#SupAfPerformingBy').val();
    modelSupAf.LevelHarm = $('input[name=SupAfLevelHarm]:checked').val();
    modelSupAf.LevelApproach = $('input[name=SupAfLevelApproach]:checked').val();
    modelSupAf.LevelCareObstacle = $('input[name=SupAfLevelCareObstacle]:checked').val();
    modelSupAf.TotalLevelHigh = $('#SupAfTotalLevelHigh').html();
    modelSupAf.TotalLevelAverage = $('#SupAfTotalLevelAverage').html();
    modelSupAf.TotalLevelLow = $('#SupAfTotalLevelLow').html();

    modelSupAf.AbilityProtectYourself = $('input[name=SupAfAbilityProtectYourself]:checked').val();
    modelSupAf.AbilityKnowGuard = $('input[name=SupAfAbilityKnowGuard]:checked').val();
    modelSupAf.AbilityHelpOthers = $('input[name=SupAfAbilityHelpOthers]:checked').val();
    modelSupAf.TotalAbilityHigh = $('#SupAfTotalAbilityHigh').html();
    modelSupAf.TotalAbilityAverage = $('#SupAfTotalAbilityAverage').html();
    modelSupAf.TotalAbilityLow = $('#SupAfTotalAbilityLow').html();

    modelSupAf.Result = $('#SupAfResult').val();
    modelSupAf.SupportAfterTitle = $('#SupAfSupportAfterTitle').val();
    modelSupAf.LevelHarmNote = $('#SupAfLevelHarmNote').val();
    modelSupAf.LevelApproachNote = $('#SupAfLevelApproachNote').val();
    modelSupAf.LevelCareObstacleNote = $('#SupAfLevelCareObstacleNote').val();
    modelSupAf.AbilityProtectYourselfNote = $('#SupAfAbilityProtectYourselfNote').val();
    modelSupAf.AbilityKnowGuardNote = $('#SupAfAbilityKnowGuardNote').val();
    modelSupAf.AbilityHelpOthersNote = $('#SupAfAbilityHelpOthersNote').val();

    if (modelSupAf.PerformingBy === '') {
        toastr.error(GetNotifyByKey('Enter_reviewer'), { timeOut: 5000 }); return;
    }
    if (modelSupAf.PerformingDate === '') {
        toastr.error(GetNotifyByKey('Select_reviewDate'), { timeOut: 5000 }); return;
    }
    if (modelSupAf.SupportAfterTitle === '') {
        toastr.error(GetNotifyByKey('Select_SupportAfterTitle'), { timeOut: 5000 }); return;
    }
    return true;
}
function UpdateTab5(isExport) {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        $('#stepFirst').click();
        return false;
    }
    var result5 = ValidateSupportAfterStatusModel();
    modelSupAf.ReportProfileId = model.Id;
    modelSupAf.IsExport = isExport;
    if (result5 === true) {
        var fd = new FormData();
        fd.append("modelSupAf", JSON.stringify(modelSupAf));
        OpenWaiting();
        $.ajax({
            url: '/ReportProfile/UpdateProcessTab5',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                CloseWaiting();
                if (data.Ok === false) {
                    toastr.error(data.Message, { timeOut: 5000 }); return false;
                } else {
                    $('#idSupportAfter').val(data.Id);
                    GenTitleStep5View();
                    if (isExport) {
                        var link = document.getElementById('linkDowload');
                        $(link).attr("href", data.Path);
                        link.focus();
                        link.click();
                    }
                    if (data.Ok) {
                        toastr.success(GetNotifyByKey('Update_monitoring'), { timeOut: 5000 });
                    }
                }
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
            }
        });
    }

}
//dùng chung đóng truong hơp, xoa ke hoach, xoa danh gia
function FinishReport() {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        $('#stepFirst').click();
        return false;
    }
    $('#valueDelete').val(model.Id);
    $('#typeDelete').val('6');
    $('#labelDelete').html(GetNotifyByKey('Delete_Confim_Report'));
    $('#modamDelete').modal({
        show: 'true'
    });
}
function DeleteSupportAfter(id) {
    $('#valueDelete').val(id);
    $('#typeDelete').val('5');

    $('#labelDelete').html(GetNotifyByKey('Delete_Confim_SupportAfter'));
    $('#modamDelete').modal({
        show: 'true'
    });
}
function DeleteSupportPlant(id) {
    $('#valueDelete').val(id);
    $('#typeDelete').val('4');

    $('#labelDelete').html(GetNotifyByKey('Delete_Confim_SupportPlant'));
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    var typeDelete = $('#typeDelete').val();
    var urlDelete = '';
    if (typeDelete === '100') {
        //xóa table tổ chức thực hiện trên giao diện
        $('#trItem_' + id).remove();
        $('#modamDelete').modal('hide');
        return;
    } else
        if (typeDelete === '5') {
            //xóa bảng support after tab 5
            urlDelete = '/ReportProfile/DeleteSupportAfter?id=' + id;
        } else if (typeDelete === '4') {
            //xóa bảng support plant  tab 4
            urlDelete = '/ReportProfile/DeleteSupportPlant?id=' + id;
        } else {
            urlDelete = '/ReportProfile/FinishReport?id=' + id;
        }
    $.ajax({
        url: urlDelete,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            if (data.Ok) {
                if (typeDelete === '5') {
                    //bảng support after tab 5
                    toastr.success(GetNotifyByKey('Delete_Ok_SupportAfter'), { timeOut: 5000 });
                    GenTitleStep5View();
                    GenStep5View('');
                } else if (typeDelete === '4') {
                    //bảng support plant  tab 4
                    toastr.success(GetNotifyByKey('Delete_Ok_SupportPlant'), { timeOut: 5000 });
                    GenTitleStep4View();
                    GenStep4View('');
                } else {
                    toastr.success(GetNotifyByKey('Close_case'), { timeOut: 5000 });
                    window.location = '/ReportProfile/Index';
                }
                $('#modamDelete').modal('hide');
            }
            else {
                $('#modamDelete').modal('hide');
                toastr.error(data.Message, { timeOut: 5000 });
            }

        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
        }
    });
}
//het dung chung
function GenTitleStep4View() {
    $.post("/ReportProfile/GenTitleStep4View?reportProfileId=" + model.Id, function (result) {
        $("#title-data-step4").html(result);
    });
}
function GenStep4View(id) {
    modelSup.ListProfileAttachment = [];
    _listProfileAttachmentSup = [];
    _listFileAttachmentSup = [];
    $('#uploadFileProfileSup').val('');
    $.post("/ReportProfile/GenStep4View?id=" + id + "&reportProfileId=" + model.Id, function (result) {
        $("#data-step4").html(result);
        var dataFileSup = $('#idlistFileSup').val();
        modelSup.ListProfileAttachmentUpdate = JSON.parse(dataFileSup);
    });
}

function GenTitleStep5View() {
    $.post("/ReportProfile/GenTitleStep5View?reportProfileId=" + model.Id, function (result) {
        $("#title-data-step5").html(result);
    });
}
function GenStep5View(id) {
    $.post("/ReportProfile/GenStep5View?id=" + id + "&reportProfileId=" + model.Id, function (result) {
        $("#data-step5").html(result);
    });
}

//bat dau tab so 4
$('#SupIsEstimateCost').on('change', function () {
    var kqCheck = document.querySelector('#SupIsEstimateCost').checked;
    if (kqCheck) {
        $("#divEstimateCost").hide(600);
        // document.getElementById('divEstimateCost').style.display = 'none';
    } else {
        $("#divEstimateCost").show(600);
        // document.getElementById('divEstimateCost').style.display = 'block';
    }
});
//tab 4 - SupportPlantModel
function ValidateSupportPlantModel() {
    modelSup.ReportProfileId = model.Id;
    modelSup.ListOrganizationActivities = [];
    modelSup.Id = $('#idSupportPlant').val();
    modelSup.PlantDate = $('#SupPlantDate').val();

    // modelSup.TargetOtherNote = $('#SupTargetOtherNote').val();
    // modelSup.ActionOtherNote = $('#SupActionOtherNote').val();

    modelSup.TargetNote = $('#SupTargetNote').val();
    modelSup.TitlePlant = $('#SupTitlePlant').val();
    modelSup.IsEstimateCost = !document.querySelector('#SupIsEstimateCost').checked;
    modelSup.ActionNote = CKEDITOR.instances["supportPlantModel_ActionNote"].getData();

    if (modelSup.PlantDate === '') {
        toastr.error(GetNotifyByKey('Enter_PlantDate'), { timeOut: 5000 }); return false;
    }
    if (modelSup.TitlePlant === '') {
        toastr.error(GetNotifyByKey('Enter_TitlePlant'), { timeOut: 5000 }); return false;
    }
    var id = ''; var obj = {};
    var vlItem = '';
    $('#tableActivityTab3 tbody tr').each(function () {
        obj = {};
        $(this).find("input[type='text'], textarea").each(function () {
            id = $(this).attr('name');
            vlItem = $(this).val();
            if (id.includes('ActionName_')) {
                obj.Name = vlItem;
            }
            if (id.includes('UserNameAction_')) {
                obj.UserName = vlItem;
            }
            if (id.includes('UserOtherAction_')) {
                obj.UserOther = vlItem;
            }
            if (id.includes('DateActionAction_')) {
                obj.DateAction = vlItem;
            }
        });
        modelSup.ListOrganizationActivities.push(obj);
    });
    return true;
}

var modelSup =
{
    Id: '',
    ReportProfileId: model.Id,
    PlantDate: '',
    TargetRecovery: '',
    TargetOvercome: '',
    TargetIntegration: '',
    TargetOther: '',
    TargetOtherNote: '',
    ActionHealth: '',
    ActionSociety: '',
    ActionEducate: '',
    ActionJuridical: '',
    ActionOther: '',
    ActionOtherNote: '',
    OrganizationActivities: '',
    ListProfileAttachment: [],
    ListProfileAttachmentUpdate: [],
    ListOrganizationActivities: [],
    IsEstimateCost: true,
    TitlePlant: '',
    TargetNote: '',
    ActionNote: '',
    IsExport: false
};
function UpdateTab4(IsContinue, isExport) {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        $('#stepFirst').click();
        return false;
    }
    var result4 = ValidateSupportPlantModel();
    modelSup.ReportProfileId = model.Id;
    modelSup.IsExport = isExport;
    if (result4 === true) {
        var fd = new FormData();
        for (var i = 0; i < _listProfileAttachmentSup.length; i++) {
            fd.append(_listProfileAttachmentSup[i].Id, _listFileAttachmentSup[i]);
        }
        modelSup.ListProfileAttachment = _listProfileAttachmentSup;
        fd.append("modelSup", JSON.stringify(modelSup));
        OpenWaiting();
        $.ajax({
            url: '/ReportProfile/UpdateProcessTab4',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                CloseWaiting();
                if (data.Ok === false) {
                    toastr.error(data.Message, { timeOut: 5000 }); return false;
                } else {
                    $('#idSupportPlant').val(data.Id);
                    GenTitleStep4View();
                    if (isExport) {
                        var link = document.getElementById('linkDowload');
                        $(link).attr("href", data.Path);
                        link.focus();
                        link.click();
                    }
                    if (data.Ok) {
                        if (IsContinue) {
                            NextStep(4);
                        }
                        toastr.success(GetNotifyByKey('UpdatePlant_Ok'), { timeOut: 5000 });
                    }
                }
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
            }
        });
    }
}
function GoBackTab4() {
    NextStep(3);
}

function CounTable1Tab1() {
    var EvTotalLevelHigh = 0;
    var EvTotalLevelAverage = 0;
    var EvTotalLevelLow = 0;
    var ListTable1Tab1 = [];
    ListTable1Tab1.push(parseInt($('input[name=EvLevelHarm]:checked').val()));
    ListTable1Tab1.push(parseInt($('input[name=EvLevelHarmContinue]:checked').val()));
    for (var i = 0; i < ListTable1Tab1.length; i++) {
        if (ListTable1Tab1[i] === 0) {
            EvTotalLevelLow++;
        } else if (ListTable1Tab1[i] === 1) {
            EvTotalLevelAverage++;
        } else if (ListTable1Tab1[i] === 2){
            EvTotalLevelHigh++;
        }
    }
    $('#EvTotalLevelHigh').html(EvTotalLevelHigh);
    $('#EvTotalLevelAverage').html(EvTotalLevelAverage);
    $('#EvTotalLevelLow').html(EvTotalLevelLow);
}
function CounTable2Tab1() {
    var EvTotalAbilityHigh = 0;
    var EvTotalAbilityAverage = 0;
    var EvTotalAbilityLow = 0;
    var ListTable2Tab1 = [];
    ListTable2Tab1.push(parseInt($('input[name=EvAbilityProtectYourself]:checked').val()));
    ListTable2Tab1.push(parseInt($('input[name=EvAbilityReceiveSupport]:checked').val()));
    for (var i = 0; i < ListTable2Tab1.length; i++) {
        if (ListTable2Tab1[i] === 0) {
            EvTotalAbilityLow++;
        } else if (ListTable2Tab1[i] === 1) {
            EvTotalAbilityAverage++;
        } else if (ListTable2Tab1[i] === 2) {
            EvTotalAbilityHigh++;
        }
    }
    $('#EvTotalAbilityHigh').html(EvTotalAbilityHigh);
    $('#EvTotalAbilityAverage').html(EvTotalAbilityAverage);
    $('#EvTotalAbilityLow').html(EvTotalAbilityLow);
}
function CounTable1Tab2() {
    var CaTotalLevelHigh = 0;
    var CaTotalLevelAverage = 0;
    var CaTotalLevelLow = 0;
    var ListTable1Tab2 = [];
    if ($('input[name=CaLevelHarm]:checked').val() !== undefined) {
        ListTable1Tab2.push(parseInt($('input[name=CaLevelHarm]:checked').val()));
    }
    if ($('input[name=CaLevelApproach]:checked').val() !== undefined) {
        ListTable1Tab2.push(parseInt($('input[name=CaLevelApproach]:checked').val()));
    }
    if ($('input[name=CaLevelDevelopmentEffect]:checked').val() !== undefined) {
        ListTable1Tab2.push(parseInt($('input[name=CaLevelDevelopmentEffect]:checked').val()));
    }
    if ($('input[name=CaLevelCareObstacle]:checked').val() !== undefined) {
        ListTable1Tab2.push(parseInt($('input[name=CaLevelCareObstacle]:checked').val()));
    }
    if ($('input[name=CaLevelCareObstacle]:checked').val() !== undefined) {
        ListTable1Tab2.push(parseInt($('input[name=CaLevelNoGuardian]:checked').val()));
    }
    for (var i = 0; i < ListTable1Tab2.length; i++) {
        if (ListTable1Tab2[i] === 0) {
            CaTotalLevelLow++;
        } else if (ListTable1Tab2[i] === 1) {
            CaTotalLevelAverage++;
        } else if (ListTable1Tab2[i] === 2){
            CaTotalLevelHigh++;
        }
    }
    $('#CaTotalLevelHigh').html(CaTotalLevelHigh);
    $('#CaTotalLevelAverage').html(CaTotalLevelAverage);
    $('#CaTotalLevelLow').html(CaTotalLevelLow);
}
function CounTable2Tab2() {
    var CaTotalAbilityHigh = 0;
    var CaTotalAbilityAverage = 0;
    var CaTotalAbilityLow = 0;
    var ListTable2Tab2 = [];
    if ($('input[name=CaAbilityProtectYourself]:checked').val() !== undefined) {
        ListTable2Tab2.push(parseInt($('input[name=CaAbilityProtectYourself]:checked').val()));
    }
    if ($('input[name=CaAbilityKnowGuard]:checked').val() !== undefined) {
        ListTable2Tab2.push(parseInt($('input[name=CaAbilityKnowGuard]:checked').val()));
    }
    if ($('input[name=CaAbilityEstablishRelationship]:checked').val() !== undefined) {
        ListTable2Tab2.push(parseInt($('input[name=CaAbilityEstablishRelationship]:checked').val()));
    }
    if ($('input[name=CaAbilityRelyGuard]:checked').val() !== undefined) {
        ListTable2Tab2.push(parseInt($('input[name=CaAbilityRelyGuard]:checked').val()));
    }
    if ($('input[name=CaAbilityHelpOthers]:checked').val() !== undefined) {
        ListTable2Tab2.push(parseInt($('input[name=CaAbilityHelpOthers]:checked').val()));
    }
    for (var i = 0; i < ListTable2Tab2.length; i++) {
        if (ListTable2Tab2[i] === 0) {
            CaTotalAbilityLow++;
        } else if (ListTable2Tab2[i] === 1) {
            CaTotalAbilityAverage++;
        } else if (ListTable2Tab2[i] === 2){
            CaTotalAbilityHigh++;
        }
    }
    $('#CaTotalAbilityHigh').html(CaTotalAbilityHigh);
    $('#CaTotalAbilityAverage').html(CaTotalAbilityAverage);
    $('#CaTotalAbilityLow').html(CaTotalAbilityLow);
}
function CounTable1Tab4() {
    var SupAfTotalLevelHigh = 0;
    var SupAfTotalLevelAverage = 0;
    var SupAfTotalLevelLow = 0;
    var ListTable1Tab4 = [];
    ListTable1Tab4.push(parseInt($('input[name=SupAfLevelHarm]:checked').val()));
    ListTable1Tab4.push(parseInt($('input[name=SupAfLevelApproach]:checked').val()));
    ListTable1Tab4.push(parseInt($('input[name=SupAfLevelCareObstacle]:checked').val()));

    for (var i = 0; i < ListTable1Tab4.length; i++) {
        if (ListTable1Tab4[i] === 0) {
            SupAfTotalLevelLow++;
        } else if (ListTable1Tab4[i] === 1) {
            SupAfTotalLevelAverage++;
        } else if (ListTable1Tab4[i] === 2) {
            SupAfTotalLevelHigh++;
        }
    }
    $('#SupAfTotalLevelHigh').html(SupAfTotalLevelHigh);
    $('#SupAfTotalLevelAverage').html(SupAfTotalLevelAverage);
    $('#SupAfTotalLevelLow').html(SupAfTotalLevelLow);
}
function CounTable2Tab4() {
    var SupAfTotalAbilityHigh = 0;
    var SupAfTotalAbilityAverage = 0;
    var SupAfTotalAbilityLow = 0;
    var ListTable2Tab4 = [];
    ListTable2Tab4.push(parseInt($('input[name=SupAfAbilityProtectYourself]:checked').val()));
    ListTable2Tab4.push(parseInt($('input[name=SupAfAbilityKnowGuard]:checked').val()));
    ListTable2Tab4.push(parseInt($('input[name=SupAfAbilityHelpOthers]:checked').val()));
    for (var i = 0; i < ListTable2Tab4.length; i++) {
        if (ListTable2Tab4[i] === 0) {
            SupAfTotalAbilityLow++;
        } else if (ListTable2Tab4[i] === 1) {
            SupAfTotalAbilityAverage++;
        } else if (ListTable2Tab4[i] === 2){
            SupAfTotalAbilityHigh++;
        }
    }
    $('#SupAfTotalAbilityHigh').html(SupAfTotalAbilityHigh);
    $('#SupAfTotalAbilityAverage').html(SupAfTotalAbilityAverage);
    $('#SupAfTotalAbilityLow').html(SupAfTotalAbilityLow);
}

//khoi tao dem
CounTable2Tab2();
CounTable1Tab2();
CounTable2Tab1();
CounTable1Tab1();
CounTable2Tab4();
CounTable1Tab4();
InitModelSup();


function auto_grow(element) {
    element.style.height = "5px";
    element.style.height = (element.scrollHeight) + "px";
}