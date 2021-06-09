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
    $.post("/Homes/GetNotify" , modelSearch, function (result) {
        $("#list_data_notify").html(result);
    });
}

function phantrang(PageNumber) {
    modelSearch.PageNumber = PageNumber;
    SearchNotify();
}

function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn xóa thông báo này?/Are you sure to delete this notification?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/Homes/DeleteNotify",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Xóa thông báo thành công!/Delete notification successfully!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                SearchNotify();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}
function Tick(id) {
    $.ajax({
        url: "/Homes/TickNotify?id=" + id,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Đánh dấu đã đọc thành công!/Marked as read!", { timeOut: 5000 });
                SearchNotify();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}

