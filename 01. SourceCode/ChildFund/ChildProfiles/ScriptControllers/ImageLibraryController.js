$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
// danh sách
var modelSearch =
{
    CreateBy: '',
    Description: '',
    DateFrom: '',
    DateTo: '',
    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'UploadDate',
    OrderType: true
};
function Refresh() {
    $('#description').val('');
    $('#createby').val('');
    $('#datefrom').val('');
    $('#dateto').val('');
    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}
function GetModelSearch() {
    modelSearch.Description = $('#description').val();
    modelSearch.CreateBy = $('#createby').val();
    modelSearch.DateFrom = $('#datefrom').val();
    modelSearch.DateTo = $('#dateto').val();
}
function Search() {
    GetModelSearch();
    document.getElementById("datefrom").disabled = true;
    document.getElementById("dateto").disabled = true;
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/ImageLibrary/ListUploadImage", modelSearch, function (result) {
        $("#divListUploadImage").html(result);
        document.getElementById("datefrom").disabled = false;
        document.getElementById("dateto").disabled = false;
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
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
//    $.post("/ImageLibrary/ListUploadImage", modelSearch, function (result) {
//        $("#divListUploadImage").html(result);
//    });
//}
function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn khóa thư viện ảnh này?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val(); 
    $.ajax({
        url: "/ImageLibrary/DeleteItemUpload/",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Khóa 1 mục ảnh thành công!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
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


function DownLoadAllImage(id) { 
    $.ajax({
        url: "/ImageLibrary/DownloadAllImage/",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                var link = document.createElement('a');
                link.setAttribute("type", "hidden");
                link.href = urlRoot + data.mess;
                link.download = 'Download.zip';
                document.body.appendChild(link);
                link.focus();
                link.click();
                toastr.success("Tải xuống 1 mục ảnh thành công!", { timeOut: 5000 });
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        },
    });
     
}


function InitModel() {
    $("#description").keydown(function (event) {
        if (event.keyCode === 13) {
            Search();
            return false;
        }
    });
    $("#createby").keydown(function (event) {
        if (event.keyCode === 13) {
            Search();
            return false;
        }
    });
}

function ShowViewImage(Id) {
    window.location.href = '/ImageLibrary/ViewListUpload?id=' + Id;
}
