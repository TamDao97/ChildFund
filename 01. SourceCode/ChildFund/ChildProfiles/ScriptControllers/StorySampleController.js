// danh sách
var modelSearch =
{
    Title: '',
    Type: '',
    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'Title',
    OrderType: true
};
function Refresh() {
    $('#titleSample').val('');
    $('#typeSample').val('');
    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}
function GetModelSearch() {
    modelSearch.Title = $('#titleSample').val();
    modelSearch.Type = $('#typeSample').val();
}
function Search() {
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/StorySample/ListStorySample", modelSearch, function (result) {
        $("#listStorySample").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}
function InitModel() {
    $("#titleSample").keydown(function (event) {
        if (event.keyCode === 13) {
            Search();
            return false;
        }
    });
}
function ChangeSize() {
    modelSearch.PageNumber = 1;
    modelSearch.PageSize = $('#pageSize').val();
    Search();
}
function phantrang(PageNumber) {
    modelSearch.PageNumber = PageNumber;
    Search();
}
//function pageOnclick(PageNumber) {
//    GetModelSearch();
//    modelSearch.PageNumber = PageNumber;
//    $.post("/StorySample/ListStorySample", modelSearch, function (result) {
//        $("#listStorySample").html(result);
//    });
//}

function ChangeStatus(id, status) {
    $.ajax({
        url: "/StorySample/ChangeStatus/",
        type: "POST",
        data: { Id: id, Status: status },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Đổi trạng thái thành công!/Change status successfully!", { timeOut: 5000 });
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}
function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn khóa mẫu câu chuyện này?/Are you sure to lock this sample story?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/StorySample/DeleteSampleStory/",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Khóa mẫu câu chuyện thành công!/Lock sample story succcessfully!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}


// them moi
var model = {
    Id: '',
    Title: '',
    Content: '',
    Status: '0',
    Type: '',
    UpdateDate: new Date(),
    CreateDate: new Date()
};

function AddStoryTeplate() {
    document.getElementById('btn-update').style.display = 'none';
    document.getElementById('btn-create').style.display = 'inline-block';
    document.getElementById('btn-createContinue').style.display = 'inline-block';
    $('#h4Title').html('Thêm mới mẫu câu chuyện/Add new sample story');
    ResetModel();
    $('#modalStoryTeplate').modal({
        show: 'true'
    });
}

function EditStoryTemplate(id) {
    model.Id = id;
    $.post("/StorySample/FormStoryTemplate?id=" + id, function (result) {
        $("#divFormStoryTemplate").html(result);
        document.getElementById('btn-update').style.display = 'inline-block';
        document.getElementById('btn-create').style.display = 'none';
        document.getElementById('btn-createContinue').style.display = 'none';
        $('#h4Title').html('Cập nhật mẫu câu chuyện/Update sample story');
        $('#modalStoryTeplate').modal({
            show: 'true'
        });
    });
}



function CloseModel() {
    $('#modalStoryTeplate').modal('hide');
    Search();
}

function Update() {
    var result = ValidateCate();
    if (result === true) {
        $.ajax({
            url: "/StorySample/UpdateTemplate",
            data: model,
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Cập nhật mẫu câu truyện thành công!/Update sample story succcessfully!", { timeOut: 5000 });
                    CloseModel();
                    Search();
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
            }
        });
    }
}

function Create(isContinue) {
    var result = ValidateCate();
    if (result === true) {
        $.ajax({
            url: "/StorySample/AddTemplate",
            data: model,
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.ok === true) {
                    toastr.success("Thêm mẫu câu truyện thành công!/Create sample story succcessfully!", { timeOut: 5000 });
                    if (isContinue === true) {
                        ResetModel();
                    } else {
                        CloseModel();
                    }
                    Search();
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
            }
        });
    }
}

function ValidateCate() {
    model.Title = $('#title').val();
    model.Content = $('#content').val();
    model.Type = $('#type').val();
    if (model.Title === '') {
        toastr.error("Nhập tiêu đề!/Enter title!", { timeOut: 5000 }); return false;
    } else if (model.Type === '') {
        toastr.error("Nhập loại mẫu!/Enter type!", { timeOut: 5000 }); return false;
    }
    else if (model.Content === '') {
        toastr.error("Nhập nội dung!/Enter content!", { timeOut: 5000 }); return false;
    }
    return true;
}

function ResetModel() {
    $('#title').val('');
    $('#type').val('1');
    $('#content').val('');
}


function SelectCategory(text) {
    text = text + ' ';
    var areaId = 'content';
    var txtarea = document.getElementById(areaId);
    if (!txtarea) {
        return;
    }

    var scrollPos = txtarea.scrollTop;
    var strPos = 0;
    var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?
        "ff" : (document.selection ? "ie" : false));
    if (br == "ie") {
        txtarea.focus();
        var range = document.selection.createRange();
        range.moveStart('character', -txtarea.value.length);
        strPos = range.text.length;
    } else if (br == "ff") {
        strPos = txtarea.selectionStart;
    }

    var front = (txtarea.value).substring(0, strPos);
    var back = (txtarea.value).substring(strPos, txtarea.value.length);
    txtarea.value = front + text + back;
    strPos = strPos + text.length;
    if (br == "ie") {
        txtarea.focus();
        var ieRange = document.selection.createRange();
        ieRange.moveStart('character', -txtarea.value.length);
        ieRange.moveStart('character', strPos);
        ieRange.moveEnd('character', 0);
        ieRange.select();
    } else if (br == "ff") {
        txtarea.selectionStart = strPos;
        txtarea.selectionEnd = strPos;
        txtarea.focus();
    }

    txtarea.scrollTop = scrollPos;
}