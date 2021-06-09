// danh sách báo cáo survey
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var modelSearch =
{
    SurveyId:'',
    SurveyName: '',
    UserName: '',
    DateFrom: '',
    DateTo: '',
    ListCheck: [],

    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'SurveyName',
    OrderType: true
};
var surveyRefresh = '';
function InitModel(id) {
    //modelSearch.SurveyId = id;
    $('#surveyIdSearch').val(id);
    surveyRefresh = id;

    Search();
}
function Refresh() {
    modelSearch.ListCheck = [];
    $('#userNameSearch').val('');
    $('#dateFromSearch').val('');
    $('#dateToSearch').val('');
    $('#surveyIdSearch').val(surveyRefresh);

    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;

    Search();
}
function GetModelSearch() {
    modelSearch.UserName = $('#userNameSearch').val();
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
    modelSearch.SurveyId = $('#surveyIdSearch').val();
}

function Search() {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/SurveyResult/ListSurveyResult", modelSearch, function (result) {
        $("#list_data").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}
$("#userNameSearch").keydown(function (event) {
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
    Search();
}

function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn xóa báo cáo này?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/SurveyResult/Delete",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Xóa báo cáo thành công!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        }
    });
}
