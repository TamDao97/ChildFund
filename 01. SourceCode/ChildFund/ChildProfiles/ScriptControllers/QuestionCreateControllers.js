// danh sách hồ sơ mới cấp trung ương
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var modelSearch =
{
    Id: '',
    Name: '',
    IsPublish: '0',
    OrderNumber: '',
    StartDate: '',
    EndDate: '',
    ListGroupQuestion: []
};

//thêm mới
function Create(isContinue) {
    var result = ValidateCate();
    if (result === true) {
        $('#loader_id').addClass("loader");
        document.getElementById("overlay").style.display = "block";
        $.ajax({
            url: "/Question/Create",
            data: modelSearch,
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.ok === true) {
                    if (isContinue === true) {
                        toastr.success("Thêm khảo sát thành công!", { timeOut: 5000 });
                        $('#loader_id').removeClass("loader");
                        document.getElementById("overlay").style.display = "none";
                        ResetModel();
                    } else {
                        sessionStorage.setItem("keyMess", "Thêm Thêm khảo sát thành công!");
                        window.location = '/Question/Index';
                    }
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
            }
        });
    }
}
function ValidateCate() {
    modelSearch.ListGroupQuestion = [];
    modelSearch.Name = $('#nameSurvey').val();
    modelSearch.StartDate = $('#startDate').val();
    modelSearch.EndDate = $('#endDate').val();
    modelSearch.OrderNumber = $('#orderNumber').val();
    if (modelSearch.Name === '') {
        toastr.error("Nhập tên khảo sát!", { timeOut: 5000 }); return false;
    } else if (modelSearch.StartDate === '') {
        toastr.error("Chọn ngày bắt đầu!", { timeOut: 5000 }); return false;
    } else if (modelSearch.EndDate === '') {
        toastr.error("Chọn ngày kết thúc!", { timeOut: 5000 }); return false;
    }
    else if (modelSearch.OrderNumber === '') {
        toastr.error("Nhập thứ tự!", { timeOut: 5000 }); return false;
    }
    else if (isNaN(modelSearch.OrderNumber)) {
        toastr.error("Thứ tự phải là số!", { timeOut: 5000 }); return false;
    }
    else {
        var ListQuestion = [];
        var surveyIndex = 0;
        var surveyName = '';
        var Contents = '';
        var Type = '';
        var typeofItem = '';
        var idsurvey = '';
        var idquestion = '';
        var vlItem = '';
        var ListAnswer = [];
        $('#data-all .row-survey').each(function () {
            $(this).find("input.survey-item").each(function () {
                surveyName = $(this).val();
            });
            surveyIndex++;
            ListQuestion = [];
            //cap 2 tìm theo câu hỏi
            idsurvey = "#" + $(this).attr('id');

            $(idsurvey + " .row-question").each(function () {
                idquestion = "#" + $(this).attr('id');
                ListAnswer = [];
                $(this).find("input.question-item").each(function () {
                    Contents = $(this).val();
                });
                $(this).find("select.question-item-type").each(function () {
                    Type = $(this).val();
                });
                //cap 3 tìm theo câu trả loi
                if (Type === '0' || Type === '1') {
                    //loai này thì có câu trả lời
                    $(idquestion + " .list-answer").each(function () {
                        $(this).find("input.form-control").each(function () {
                            typeofItem = $(this).attr('typeof');
                            vlItem = $(this).val();
                            if (typeofItem === '0') {
                                ListAnswer.push({ Type: typeofItem, Answer: vlItem, Other: '', Check: false });
                            } else {
                                ListAnswer.push({ Type: typeofItem, Other: vlItem, Answer: '', Check: false });
                            }
                        });
                    });
                    ListQuestion.push({ Contents: Contents, Type: Type, ListAnswer: ListAnswer });
                } else {
                    //  loại này ko có câu trả lời mà chỉ có câu hỏi
                    ListQuestion.push({ Contents: Contents, Type: Type });
                }

            });

            modelSearch.ListGroupQuestion.push({ OrderNumber: surveyIndex, Name: surveyName, ListQuestion: ListQuestion });

        });
        return true;
    }
}
function ResetModel() {
    $('#nameSurvey').val('');
    $('#orderNumber').val('');
    $('#startDate').val($('#hid_dateFrom').val());
    $('#endDate').val($('#hid_dateTo').val());
}

//cac ham them xoa 
function DeleteAnswer(id, idq, type) {
    $('#answer_' + id).remove();
    $('#hid_' + id).val(type);
    if (type === 1) {
        document.getElementById('sp_' + idq).style.display = "inline-block";
        document.getElementById('taga_' + idq).style.display = "inline-block";
    }
}
function AddAnswer(id) {
    var type = $('#type_' + id).val();
    $.post("/Question/GenAnser?type=" + type, function (result) {
        $('#row-question-child_' + id).append(result);
    });
}
function AddAnswerPart(id) {
    var type = $('#type_' + id).val();
    $.post("/Question/GenPartHtml?type=" + type, function (result) {
        $('#row-question-child_' + id).append(result);
    });
}
function DeleteQuestion(id) {
    $('#row-question_' + id).remove();
}
function AddQuestion(id) {
    $.post("/Question/GenQuestion?nameGroup=" + id, function (result) {
        $('#row-survey_' + id).append(result);
    });
}

function AddSurvey() {
    $.post("/Question/GenGroupQuestion", function (result) {
        $('#data-all').append(result);
    });
}
function AddOtherAnswer(id) {
    var type = $('#type_' + id).val();
    document.getElementById('sp_' + id).style.display = "none";
    document.getElementById('taga_' + id).style.display = "none";
    $.post("/Question/GenAnserOther?type=" + type + "&idq=" + id, function (result) {
        $('#row-question-child_' + id).append(result);
    });
}

function ChangeType(id) {
    var type = $('#type_' + id).val();
    var lstReult = [];
    var typeofItem = '';
    var vlItem = '';
    if (type === '0' || type === '1') {
        $('#row-question-child_' + id + ' .list-answer').each(function () {
            $(this).find("input.form-control").each(function () {
                typeofItem = $(this).attr('typeof');
                vlItem = $(this).val();
                lstReult.push({ Type: typeofItem, Answer: vlItem });
            });
            $(this).remove();

        });
        var model = {
            lstReult: lstReult,
            type: type,
            idq: id
        };

        $.post("/Question/GenListAnser", model, function (result) {
            $('#row-question-child_' + id).append(result);
        });

        document.getElementById('add-item_' + id).style.display = "block";
    } else {
        $('#row-question-child_' + id + ' .list-answer').each(function () {
            $(this).remove();
        });
        AddAnswerPart(id);
        document.getElementById('add-item_' + id).style.display = "none";
    }

}
function CopyQuestion(id) {

}