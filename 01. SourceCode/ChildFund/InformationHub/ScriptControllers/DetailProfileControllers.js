document.getElementById('step-1').style.display = 'block';
document.getElementById('step-2').style.display = 'none';
document.getElementById('step-3').style.display = 'none';
document.getElementById('step-4').style.display = 'none';
document.getElementById('step-5').style.display = 'none';
var model = {
    Id: '',
    CurrentHealth: '',
    SequelGuess: '',
    FamilySituation: '',
    Support: '',
    SummaryCase: ''
};
function InitModel(id) {
    model.Id = id;
    GetContentEdit(id);
}
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

function GetPopupContent(idInput) {
    $.post("/ReportProfile/ViewContentPopupDetail", function (result) {
        $("#contentView").html(result);
        $("#contentViewDetail").html(model[idInput]);
        $('#modalViewContent').modal({
            show: 'true'
        });
    });
}

function closePopupContent() {
    $('#modalViewContent').modal('hide');
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
        } else if (ListTable1Tab1[i] === 2) {
            EvTotalLevelHigh++;
        }
    }
    $('#EvTotalLevelHigh').html(EvTotalLevelHigh);
    $('#EvTotalLevelAverage').html(EvTotalLevelAverage);
    $('#EvTotalLevelLow').html(EvTotalLevelLow);
}
function CounTable1Tab2() {
    var CaTotalLevelHigh = 0;
    var CaTotalLevelAverage = 0;
    var CaTotalLevelLow = 0;
    var ListTable1Tab2 = [];
    ListTable1Tab2.push(parseInt($('input[name=CaLevelHarm]:checked').val()));
    ListTable1Tab2.push(parseInt($('input[name=CaLevelApproach]:checked').val()));
    ListTable1Tab2.push(parseInt($('input[name=CaLevelDevelopmentEffect]:checked').val()));
    ListTable1Tab2.push(parseInt($('input[name=CaLevelCareObstacle]:checked').val()));
    ListTable1Tab2.push(parseInt($('input[name=CaLevelNoGuardian]:checked').val()));
    for (var i = 0; i < ListTable1Tab2.length; i++) {
        if (ListTable1Tab2[i] === 0) {
            CaTotalLevelLow++;
        } else if (ListTable1Tab2[i] === 1) {
            CaTotalLevelAverage++;
        } else {
            CaTotalLevelHigh++;
        }
    }
    $('#CaTotalLevelHigh').html(CaTotalLevelHigh);
    $('#CaTotalLevelAverage').html(CaTotalLevelAverage);
    $('#CaTotalLevelLow').html(CaTotalLevelLow);
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
function CounTable2Tab2() {
    var CaTotalAbilityHigh = 0;
    var CaTotalAbilityAverage = 0;
    var CaTotalAbilityLow = 0;
    var ListTable2Tab2 = [];
    ListTable2Tab2.push(parseInt($('input[name=CaAbilityProtectYourself]:checked').val()));
    ListTable2Tab2.push(parseInt($('input[name=CaAbilityKnowGuard]:checked').val()));
    ListTable2Tab2.push(parseInt($('input[name=CaAbilityEstablishRelationship]:checked').val()));
    ListTable2Tab2.push(parseInt($('input[name=CaAbilityRelyGuard]:checked').val()));
    ListTable2Tab2.push(parseInt($('input[name=CaAbilityHelpOthers]:checked').val()));
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
        } else if (ListTable2Tab4[i] === 2) {
            SupAfTotalAbilityHigh++;
        }
    }
    $('#SupAfTotalAbilityHigh').html(SupAfTotalAbilityHigh);
    $('#SupAfTotalAbilityAverage').html(SupAfTotalAbilityAverage);
    $('#SupAfTotalAbilityLow').html(SupAfTotalAbilityLow);
}
function GenStep5View(id) {
    $.post("/ReportProfile/GenStep5DetailView?id=" + id + "&reportProfileId=" + model.Id, function (result) {
        $("#data-step5").html(result);
    });
}
function GenStep4View(id) {
    $.post("/ReportProfile/GenStep4DetailView?id=" + id + "&reportProfileId=" + model.Id, function (result) {
        $("#data-step4").html(result);
        // var dataFileSup = $('#idlistFileSup').val();
        // modelSup.ListProfileAttachmentUpdate = JSON.parse(dataFileSup);
    });
}
//het dem
function ExportWordForm1() {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        return false;
    }
    OpenWaiting();
    $.ajax({
        url: '/ReportProfile/ExportWordForm1?profileId=' + model.Id,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            CloseWaiting();
            if (data.Ok === false) {
                toastr.error(data.Message, { timeOut: 5000 }); return false;
            } else {
                var link = document.getElementById('linkDowload');
                $(link).attr("href", data.Path);
                link.focus();
                link.click();
            }
        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
        }
    });
}
function ExportWordForm2() {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        return false;
    }
    OpenWaiting();
    $.ajax({
        url: '/ReportProfile/ExportWordForm2?profileId=' + model.Id,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            CloseWaiting();
            if (data.Ok === false) {
                toastr.error(data.Message, { timeOut: 5000 }); return false;
            } else {
                var link = document.getElementById('linkDowload');
                $(link).attr("href", data.Path);
                link.focus();
                link.click();
            }
        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
        }
    });
}
function ExportWordForm3() {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        return false;
    }
    OpenWaiting();
    $.ajax({
        url: '/ReportProfile/ExportWordForm3?profileId=' + model.Id,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            CloseWaiting();
            if (data.Ok === false) {
                toastr.error(data.Message, { timeOut: 5000 }); return false;
            } else {
                var link = document.getElementById('linkDowload');
                $(link).attr("href", data.Path);
                link.focus();
                link.click();
            }
        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
        }
    });
}
function ExportWordForm4() {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        return false;
    }
    var id = $('#idSupportPlant').val();
    OpenWaiting();
    $.ajax({
        url: '/ReportProfile/ExportWordForm4?profileId=' + model.Id+'&id='+id,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            CloseWaiting();
            if (data.Ok === false) {
                toastr.error(data.Message, { timeOut: 5000 }); return false;
            } else {
                var link = document.getElementById('linkDowload');
                $(link).attr("href", data.Path);
                link.focus();
                link.click();
            }
        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
        }
    });
}
function ExportWordForm5() {
    if (model.Id === '') {
        toastr.error(GetNotifyByKey('CheckAddReport_Erros'), { timeOut: 5000 });
        return false;
    }
    var id = $('#idSupportAfter').val();
    OpenWaiting();
    $.ajax({
        url: '/ReportProfile/ExportWordForm5?profileId=' + model.Id + '&id=' + id,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            CloseWaiting();
            if (data.Ok === false) {
                toastr.error(data.Message, { timeOut: 5000 }); return false;
            } else {
                var link = document.getElementById('linkDowload');
                $(link).attr("href", data.Path);
                link.focus();
                link.click();
            }
        },
        error: function (reponse) {
            toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
        }
    });
}

CounTable1Tab1();
CounTable1Tab2();
CounTable2Tab1();
CounTable2Tab2();
CounTable1Tab4();
CounTable2Tab4();
