// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    PageSize: 10,
    PageNumber: 1
};
function ChangeSize() {
    modelSearch.PageNumber = 1;
    modelSearch.PageSize = $('#pageSize').val();
    SearchNotify();
}
function SearchNotify() {
    $.post("/Home/GetNotify" , modelSearch, function (result) {
        $("#list_data_notify").html(result);
    });
}

function phantrang(PageNumber) {
    modelSearch.PageNumber = PageNumber;
    SearchNotify();
}

function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html(GetNotifyByKey('Delete_Confim_notification'));
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/Home/DeleteNotify",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('Delete_notification'), { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                SearchNotify();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}
function Tick(id) {
    $.ajax({
        url: "/Home/TickNotify?id=" + id,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('Mark_notification'), { timeOut: 5000 });
                SearchNotify();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}

