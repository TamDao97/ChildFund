$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    Name: '',
    DateFrom: '',
    DateTo: '',
    CreateBy: '',
    Status: '',

    PageSize: 20,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: false
};
function Refresh() {
    $('#createBySearch').val('');
    $('#statusSearch').val('');
    $('#nameSearch').val('');
    $('#dateFromSearch').val('');
    $('#dateToSearch').val('');
    modelSearch.PageSize = 20;
    modelSearch.PageNumber = 1;
    Search();
}
function GetModelSearch() {
    $('input').attr('autocomplete', 'off');
    modelSearch.Name = $('#nameSearch').val();
    modelSearch.Status = $('#statusSearch').val();
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
    modelSearch.CreateBy = $('#createBySearch').val();
}

function Search() {
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/ReportProfile/GetReportProvince", modelSearch, function (result) {
        $("#list_data").html(result);
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
    });
}
$("#nameSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search();
        return false;
    }
});
$("#createBySearch").keydown(function (event) {
    if (event.keyCode === 13) {
        Search();
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
function ConfimProvince(id) {
    $('#valueConfim').val(id);
    $('#labelConfim').html('Bạn có chắc chắn duyệt báo cáo tình trạng trẻ này?/Are you sure to approve this status report?');
    $('#modamConfim').modal({
        show: 'true'
    });
}
function DeleteConfim(id) {
    $('#valueDelete').val(id);
    $('#labelDelete').html('Bạn có chắc chắn muốn khóa báo cáo tình trạng trẻ này?/Are you sure to lock this status report?');
    $('#modamDelete').modal({
        show: 'true'
    });
}
function Delete() {
    var id = $('#valueDelete').val();
    $.ajax({
        url: "/ReportProfile/Delete",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Khóa báo cáo tình trạng trẻ thành công!/Lock report successfully!", { timeOut: 5000 });
                $('#modamDelete').modal('hide');
                Search(0);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        },
    });
}
function Confim() {
    var id = $('#valueConfim').val();
    $.ajax({
        url: "/ReportProfile/ConfimProvince",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                toastr.success("Duyệt báo cáo tình trạng trẻ thành công!/Approve report successfully!", { timeOut: 5000 });
                $('#modamConfim').modal('hide');
                Search(0);
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}
function Download(id) {
    $.ajax({
        url: "/ReportProfile/Download",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {

                for (i = 0; i < data.mess.length; i++) {
                    var link = document.createElement('a');
                    link.setAttribute("type", "hidden");
                    window.open(data.mess[i], "_blank");
                }               
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        }
    });
}

function DetailProvince(id) {
    $.ajax({
        url: "/ReportProfile/GetReportProfile",
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                $('#txtTenTre').html(data.data.Name);
                $('#txtTenKhac').html(data.data.NickName);
                if (data.data.Gender == 1) {
                    $('#txtGioiTinh').html('Nam/Male');
                }
                else {
                    $('#txtGioiTinh').html('Nữ/Female');
                }
                if (data.data.DateOfBirth != null) {
                    var date = new Date(parseInt(data.data.DateOfBirth.replace('/Date(', '')));
                    var month = date.getMonth() + 1;
                    if (month < 10) {
                        month = "0" + month.toString();
                    }
                    var day = date.getDate();
                    if (day < 10) {
                        day = "0" + day.toString();
                    }
                    var year = date.getFullYear();
                    var birthDate = day.toString() + "/" + month.toString() + "/" + year.toString();
                    $('#txtNgaySinh').html(birthDate);
                }
                $('#noteDetail').html(data.data.Content);
                $('#descriptionDetail').html(data.data.Description);
                $('#txtMaSoTre').html(data.data.ChildCode);
                $('#txtMaChuongTrinh').html(data.data.ProgramCode);
                $('#txtSchool').html(data.data.SchoolId);
                $('#txtVillageName').html(data.data.VillageName);
                $('#txtWardId').html(data.data.WardId + ', ' + data.data.ProvinceId);
                $('#imgPreview').attr("src", data.data.ImagePath);
                if (data.familyMember.length > 0) {
                    $('#txtFa').html(data.familyMember[0].Name);
                    $('#txtMo').html(data.familyMember[1].Name);
                }
                $('#modalDetail').modal();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        },
    });
}

function CloseModalDetail() {
    $('#modalDetail').modal('hide');
    Search();
}