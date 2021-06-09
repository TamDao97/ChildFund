
var modelSearch =
{
    Id: '',
    Content: '',
    ProcessingBy:'b0afed30-4087-41c2-bcb1-fed126465e13'
};

function Search() {
    $.post("/ProfileReport/GetContent?id=" + modelSearch.Id, function (result) {
        $("#data_Content").html(result);
       
    });
}

function SendContent(id) {
    modelSearch.Id = id;
    modelSearch.Content = $('#contentValue').val();
    if (modelSearch.contentValue === '') {
        toastr.error("Nhập nội dung phản hồi!", { timeOut: 5000 }); return false;
    }
    $.ajax({
        url: "/ProfileReport/SendContent",
        type: "POST",
        data: modelSearch,
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Gửi phản hồi thành công!", { timeOut: 5000 });
                $('#contentValue').val('');
                Search();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        },
    });
}

