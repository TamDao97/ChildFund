// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    Id: '',
    StoryContent: ''
};

function UpdateStory(id) {
    modelSearch.StoryContent = $('#mainContentStory').val();
    modelSearch.Id = id;
    if (modelSearch.StoryContent === '') {
        toastr.error("Nhập nội dung câu chuyện!", { timeOut: 5000 }); return;
    }
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.ajax({
        url: "/ProfileNew/UpdateStory",
        data: modelSearch,
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Cập nhật câu chuyện thành công!", { timeOut: 5000 });
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
function ResetStory(id) {
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.ajax({
        url: "/ProfileNew/ResetStory",
        data: { Id: id },
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                toastr.success("làm mới câu chuyện thành công!", { timeOut: 5000 });
                $('#mainContentStory').val(data.mess);
                $('#counter').text(data.mess.length);
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

$('#mainContentStory').keyup(function () {
    var textlen = $(this).val();
    $('#counter').text(textlen.length);
});