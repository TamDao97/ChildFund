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
    OrderType: false,
    AreaApproverNotes:''
};
var modelFile = {
    Id: '',
    ListIdRemote:[]
}
var dataOld = [];
var data = [];
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
    $.post("/ReportProfile/GetReportWard", modelSearch, function (result) {
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
function Confim() {
    var id = $('#valueConfim').val();
    var a = $('#inputText').val();
    $.ajax({
        url: "/ReportProfile/ConfimProvince",
        type: "POST",
        data: { Id: id, AreaApproverNotes : a },
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
                var link = document.createElement('a');
                link.setAttribute("type", "hidden");
                //link.href = urlRoot + data.mess;
                //link.download = 'Download.zip';
                //document.body.appendChild(link);
                //link.focus();
                //link.click();

                window.open(urlRoot + data.mess, "_blank");
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        }
    });
}
function AttachFile(id) {
    $('#divFile').html('');
    $("#FileUpload").val('');
    data = [];
    dataOld = [];
    modelFile.Id = id;
    modelFile.ListIdRemote = [];
    $.ajax({
        url: "/ReportProfile/GetAttachFile/" + id,
        type: "POST",
        data: { Id: id },
        success: function (data) {
            if (data.ok === true) {
                dataOld = data.data;
                GenHtmlOld();
            }
        },
        error: function (response) {
            toastr.error("Đã xảy ra lỗi!/Error!", { timeOut: 5000 });
        }
    });

    $('#modamAttach').modal({
        show: 'true'
    });
}
function uploadFile() {
    $('#FileUpload').trigger('click');
    $('#FileUpload').change(function () {
        var files = $("#FileUpload").get(0).files;
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                if (checkFile(files[i])) {
                    data.push(files[i]);
                }
            }
        }
        $("#FileUpload").val('');
        GenHtml();
    });
}
function checkFile(file) {
    var rs = true;
    for (var i = 0; i < data.length; i++) {
        if (data[i].name === file.name && data[i].size === file.size) {
            rs = false;
            i = 1000000;
        }
    }
    return rs;
}
function remoteFile(index) {
    data.splice(index, 1);
    GenHtml();
}
function remoteFileOld(index) {
    modelFile.ListIdRemote.push(dataOld[index].Id);
    dataOld.splice(index, 1);
    GenHtmlOld();
}
function GenHtml() {
    var html = '';
    for (var ie = 0; ie < data.length; ie++) {
        html += '<div class="col-sm-10"><span>' + data[ie].name + '</span></div>';
        html += '<div class="col-sm-2"><button type="button" title="Xóa/Delete" class="btn btn-danger funtion-icon margin-bottom-5" onclick="remoteFile(' + ie + ')"><span class="glyphicon glyphicon-trash"></span></button></div>';
    }
    $('#divFile').html(html);
}
function GenHtmlOld() {
    var html = '';
    for (var ie = 0; ie < dataOld.length; ie++) {
        html += '<div class="col-sm-10"><span>' + dataOld[ie].Name + '</span></div>';
        html += '<div class="col-sm-2"><button type="button" title="Xóa/Delete" class="btn btn-danger funtion-icon margin-bottom-5" onclick="remoteFileOld(' + ie + ')"><span class="glyphicon glyphicon-trash"></span></button></div>';
    }
    $('#divFileOld').html(html);
}
function UploadAttachFile() {
    if (data.length === 0 && modelFile.ListIdRemote.length===0) {
        toastr.error("Chưa có file được chọn!/No file has been chosen!", { timeOut: 5000 }); return false;
    }
    var id = $('#valueAttach').val();
    var fd = new FormData();
    for (var i = 0; i < data.length; i++) {
        fd.append('file' + i, data[i]);
    }
    fd.append("model", JSON.stringify(modelFile));
    $.ajax({
        url: '/ReportProfile/UploadAttachFile',
        data: fd,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            if (data.ok) {
                toastr.success("Đính kèm file thành công!/Attach file successfully!", { timeOut: 5000 });
                $('#modamAttach').modal('hide');
                Search();
            }
            else {
                toastr.error(data.Message, { timeOut: 5000 });
            }
        },
        error: function (reponse) {
            toastr.error("Lỗi phát sinh trong quá trình xử lý!/Error has occured while processing!", { timeOut: 5000 });
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
            toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
        },
    });
}

function CloseModalDetail() {
    $('#modalDetail').modal('hide');
    Search();
}