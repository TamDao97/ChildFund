var urlRoot = "http://localhost:50517/";
var countLength = 85;
var langUse = 'Vi';

var dataLang =
    [
        { key: "Error_Process", valueVi: "Đã xảy ra lỗi, vui lòng thử lại sau", valueEn: "An error has occurred, please try again later" },
        { key: "Add_Location", valueVi: "Thêm địa bàn thành công", valueEn: "Add a successful location" },
        { key: "Enter_Location", valueVi: "Nhập tên địa bàn", valueEn: "Enter the location name" },
        { key: "Must_District", valueVi: "Phải chọn Quận Huyện", valueEn: "Must choose District" },
        { key: "Must_Ward", valueVi: "Phải chọn Phường Xã", valueEn: "Must choose Ward" },
        { key: "Update_Location", valueVi: "Cập nhật địa bàn thành công", valueEn: "Update the location successfully" },
        { key: "Delete_Location", valueVi: "Xóa địa bàn thành công", valueEn: "Delete the location successfully" },
        { key: "Delete_Confim_Location", valueVi: "Bạn có chắc chắn muốn xóa địa bàn này", valueEn: "Are you sure you want to delete this location?" },
        { key: "Change_password", valueVi: "Đổi mật khẩu thành công", valueEn: "Change password successfully" },
        { key: "Update_UserInfo", valueVi: "Cập nhật thông tin cá nhân thành công", valueEn: "Update your personal information successfully" },
        { key: "Enter_Groupname", valueVi: "Nhập tên nhóm quyền", valueEn: "Enter the rights group name" },
        { key: "Enter_Groupcode", valueVi: "Nhập mã nhóm quyền", valueEn: "Enter the rights group code" },
        { key: "Add_Group", valueVi: "Thêm nhóm quyền thành công", valueEn: "Add a group of rights successfully" },
        { key: "Enter_Dateincident", valueVi: "Nhập ngày xảy ra sự vụ", valueEn: "Enter the date of the incident" },
        { key: "Enter_ChildName", valueVi: "Nhập họ tên trẻ", valueEn: "Enter your child's full name" },
        { key: "Enter_locationoccurred", valueVi: "Nhập địa điểm xảy ra", valueEn: "Enter the location that occurred" },
        { key: "Choose_Abuse", valueVi: "Chọn loại hình", valueEn: "Choose the type of infringement" },
        { key: "Age_Mustnumber", valueVi: "Tuổi của trẻ phải là số", valueEn: "Child's age must be number" },
        { key: "Age_Mustbetween", valueVi: "Tuổi của trẻ phải từ 0-18", valueEn: "The child's age must be between 0-18" },
        { key: "Age_DadMustnumber", valueVi: "Năm sinh của bố phải là số", valueEn: "The birth year of the father must be a number" },
        { key: "Age_MotherMustnumber", valueVi: "Năm sinh của mẹ phải là số", valueEn: "The birth year of the mother must be a number" },
        { key: "Add_Cases", valueVi: "Thêm mới trường hợp thành công", valueEn: "Add new success cases" },
        { key: "Waring_File", valueVi: "Tệp dung lượng lớn hơn 10mb", valueEn: "File larger than 10mb" },
        { key: "Waring_File2", valueVi: "quá dung lượng 10mb", valueEn: "File larger than 10mb" },
        { key: "Select_Groupdocument", valueVi: "Chọn nhóm tài liệu", valueEn: "Select the document group" },
        { key: "Enter_Documentname", valueVi: "Nhập tên tài liệu", valueEn: "Enter the document name" },
        { key: "Select_Attacheddocument", valueVi: "Chọn tài liệu đính kèm", valueEn: "Select the attached document" },
        { key: "Add_Document", valueVi: "Thêm mới tài liệu thành công", valueEn: "Add new documents successfully" },
        { key: "Delete_Confim_Group", valueVi: "Bạn có chắc chắn muốn xóa nhóm tài liệu này?", valueEn: "Are you sure you want to delete this document group?" },
        { key: "Delete_GroupDocument", valueVi: "Xóa tài liệu thành công", valueEn: "Delete document successfully" },
        { key: "Enter_GroupDocumentname", valueVi: "Nhập tên nhóm tài liệu", valueEn: "Enter the document group name" },
        { key: "Add_GroupDocumentname", valueVi: "Thêm nhóm tài liệu thành công", valueEn: "Add a successful document group" },
        { key: "Update_GroupDocumentname", valueVi: "Cập nhật nhóm tài liệu thành công", valueEn: "Update a successful document group" },
        { key: "Delete_Confim_Document", valueVi: "Bạn có chắc chắn muốn xóa tài liệu này?", valueEn: "Are you sure you want to delete this document?" },
        { key: "Delete_Document", valueVi: "Xóa tài liệu thành công", valueEn: "Delete document successfully" },
        { key: "Update_Document", valueVi: "Cập nhật tài liệu thành công", valueEn: "Document update successful" },
        { key: "Lock_GroupUser", valueVi: "Khóa nhóm quyền thành công", valueEn: "Lock rights group success" },
        { key: "UnLock_GroupUser", valueVi: "Mở khóa nhóm quyền thành công", valueEn: "Unlock rights group success" },
        { key: "Update_passwordNew", valueVi: "Cập nhật mật khẩu mới thành công", valueEn: "Update your password successfully" },
        { key: "Delete_Confim_notification", valueVi: "Bạn có chắc chắn muốn xóa thông báo này?", valueEn: "Are you sure you want to delete this notification?" },
        { key: "Delete_notification", valueVi: "Xóa thông báo thành công", valueEn: "Delete success message" },
        { key: "Mark_notification", valueVi: "Đánh dấu đã đọc thành công", valueEn: "Mark has read successfully" },
        { key: "Enter_NoteClose", valueVi: "Nhập ghi chú ", valueEn: "Enter closed case note" },
        { key: "Forward_Case", valueVi: "Chuyển trường hợp thành công", valueEn: "Forward case successfully" },
        { key: "Select_Dateclosing", valueVi: "Chọn ngày đóng trường hợp ", valueEn: "Select the closing date of the case" },
        { key: "Close_case", valueVi: "Đóng trường hợp thành công ", valueEn: "Close the case successfully" },
        { key: "Update_case", valueVi: "Cập nhật trường hợp thành công ", valueEn: "Update the case successfully" },
        { key: "Number_case", valueVi: "Số trường hợp", valueEn: "Number case" },
        { key: "All_Title", valueVi: "Tất cả", valueEn: "All" },
        { key: "Age_Title", valueVi: "Độ tuổi", valueEn: "Age" },
        { key: "Update_groupUser", valueVi: "Cập nhật nhóm quyền thành công", valueEn: "Update the rights group successfully" },
        { key: "Update_verification", valueVi: "Cập nhật thu thập thông tin xác minh thành công", valueEn: "Update to collect successful verification information" },
        { key: "Update_plan", valueVi: "Cập nhật hoàn thành kế hoạch thành công", valueEn: "Update the successful plan completion" },
        { key: "Enter_reviewer", valueVi: "Nhập người đánh giá", valueEn: "Enter the reviewer" },
        { key: "Select_reviewDate", valueVi: "Chọn ngày đánh giá", valueEn: "Select the review date" },
        { key: "Update_monitoring", valueVi: "Cập nhật giám sát và lượng giá thành công", valueEn: "Update monitoring and evaluation successfully" },
        { key: "Delete_tracking", valueVi: "Xóa theo dõi thành công", valueEn: "Delete tracking successfully" },
        { key: "Add_users", valueVi: "Thêm người dùng thành công", valueEn: "Add users successfully" },
        { key: "Enter_account", valueVi: "Nhập tên tài khoản", valueEn: "Enter the account name" },
        { key: "Enter_password", valueVi: "Nhập mật khẩu", valueEn: "Enter password" },
        { key: "Password_validate", valueVi: "Mật khẩu phải nhiều hơn 6 ký tự và ít hơn 50 ký tự", valueEn: "Password must be more than 6 characters and less than 50 characters" },
        { key: "Enter_passwordconfirmation", valueVi: "Nhập xác nhận mật khẩu", valueEn: "Enter password confirmation" },
        { key: "Password_notvalidate", valueVi: "Mật khẩu xác nhận không trùng khớp", valueEn: "The confirmation password does not match" },
        { key: "Enter_fullname", valueVi: "Nhập họ tên người dùng", valueEn: "Enter fullname" },
        { key: "Enter_email", valueVi: "Nhập email", valueEn: "Enter email" },
        { key: "Email_wrong", valueVi: "Email sai định dạng", valueEn: "Email wrong format" },
        { key: "Select_gender", valueVi: "Chọn giới tính", valueEn: "Select gender" },
        { key: "Select_groupuser", valueVi: "Chọn nhóm quyền", valueEn: "Select group user" },
        { key: "Update_user", valueVi: "Cập nhật nguời dùng thành công", valueEn: "Update user successfully" },
        { key: "Lock_user", valueVi: "Khóa người dùng thành công", valueEn: "Lock user successfully" },
        { key: "Unlock_user", valueVi: "Mở khóa người dùng thành công", valueEn: "Unlock user successfully" },
        { key: "Password_reset", valueVi: "Reset mật khẩu thành công", valueEn: "Password reset successful" },
        { key: "Statistic_gender", valueVi: "Thống kê theo giới tính", valueEn: "Statistic by gender" },
        { key: "Statistic_location", valueVi: "Thống kê theo vị trí địa lý", valueEn: "Statistic by location" },
        { key: "Statistic_process", valueVi: "Thống kê theo trạng thái xử lý", valueEn: "Statistic by processing status" },
        { key: "Statistic_abuse", valueVi: "Thống kê theo loại hình xâm hại", valueEn: "Statistic by abuse type" },
        { key: "Statistic_age", valueVi: "Thống kê theo độ tuổi", valueEn: "Statistic by age" },
        { key: "Delete_Confim_ReportOk", valueVi: "Đóng trường hợp thành công", valueEn: "Close the case successfully" },
        { key: "Validate_AgeChild", valueVi: "Tuổi trẻ sai so với ngày sinh", valueEn: "Youth is wrong compared to birth date" },
        { key: "Validate_AgeFather", valueVi: "Năm sinh của bố phải từ ", valueEn: "The birth year of the father must be from " },
        { key: "Validate_AgeMother", valueVi: "Năm sinh của mẹ phải từ ", valueEn: "Mother's birth year must be from " },
        { key: "Delete_Confim_SupportAfter", valueVi: "Bạn chắc chắn muốn xóa rà soát đánh giá này?", valueEn: "Are you sure you want to delete this review?" },
        { key: "Delete_Ok_SupportAfter", valueVi: "Xóa rà soát đánh giá thành công", valueEn: "Delete review of success assessment" },
        { key: "Enter_PlantDate", valueVi: "Chọn ngày lên kế hoạch", valueEn: "Choose a planning date" },
        { key: "Enter_TitlePlant", valueVi: "Nhập tên kế hoạch", valueEn: "Enter the plan name" },
        { key: "Enter_SupportAfterTitle", valueVi: "Nhập tên đánh giá", valueEn: "Enter the review name" },
        { key: "Publish_Confim", valueVi: "Bạn chắc chắn muốn chia sẻ thông tin trường hợp lên tổng đài?", valueEn: "You definitely want to share case information with your superiors?" },
        { key: "Publish_ConfimOk", valueVi: "Chia sẻ thông tin trường hợp lên tổng đài thành công", valueEn: "Share case information to superiors successfully" },
        { key: "Delete_Confim_SupportPlant", valueVi: "Bạn có chắc chắn muốn xóa kế hoạch hỗ trợ này?", valueEn: "Are you sure you want to delete this support plan?" },
        { key: "Delete_Ok_SupportPlant", valueVi: "Xóa kế hoạch hỗ trợ thành công", valueEn: "Delete successful support plan" },
        { key: "UpdatePlant_Ok", valueVi: "Cập nhật kế hoạch hỗ trợ thành công", valueEn: "Update the support plan successfully" },
        { key: "UpdateEvFirst_Ok", valueVi: "Cập nhật đánh giá nguy cơ ban đầu thành công", valueEn: "Successful initial risk assessment update" },
        { key: "CheckAddReport_Erros", valueVi: "Bạn phải nhập thông tin trường hợp", valueEn: "You must enter case information" },
        { key: "Percentage", valueVi: "Tỷ lệ", valueEn: "Percentage" },
        { key: "DeleteAction_Confim", valueVi: "Bạn có chắc chắn muốn xóa tổ chức thực hiện?", valueEn: "Are you sure you want to delete the organization?" },
        { key: "Select_SupportAfterTitle", valueVi: "Nhập tên đánh giá", valueEn: "Enter the review name" },
        { key: "StatisticByYear", valueVi: "Thống kê theo năm", valueEn: "Statistic during" },
        { key: "StatisticByTime", valueVi: "Thống kê trong khoảng thời gian", valueEn: "Statistic during" },
        { key: "TypeOtherKey", valueVi: "Nhập loại hình", valueEn: "Enter type" },
        { key: "keyYear", valueVi: "Năm", valueEn: "year" },
        { key: "keySelectExcel", valueVi: "Bạn chưa chọn file excel", valueEn: "You have not selected the excel file" },
        { key: "keyImportExcel", valueVi: "Nhập tài khoản thành công", valueEn: "Import accounts from successful excel" },
        { key: "ReOpenSuccessfully", valueVi: "Mở lại thông tin trường hợp thành công", valueEn: "ReOpen Case successfully" },
        { key: "DeleteCaseSuccessfully", valueVi: "Thực hiện xóa trường hợp thành công", valueEn: "Delete successfully" },
        { key: "DeleteCase_Confim_Report", valueVi: "Bạn có chắc chắn muốn xóa trường hợp này?", valueEn: "Are you sure you want to close this case?" },
        { key: "ReOpen_Confim_Report", valueVi: "Bạn có chắc chắn muốn mở lại trường hợp này?", valueEn: "Are you sure you want to reopen this case?" },
        { key: "Delete_Confim_Report", valueVi: "Bạn có chắc chắn muốn đóng trường hợp này?", valueEn: "Are you sure you want to close this case?" },
               
    ];
var IsRemoteImg = false;
function uploadImage() {
    $('#FileUpload').trigger('click');
    $('#FileUpload').change(function () {
        var data = new FormData();
        let reader = new FileReader();
        var files = $("#FileUpload").get(0).files;
        if (files.length > 0) {
            data.append("File", files[0]);
            reader.readAsDataURL(files[0]);
            reader.onload = () => {
                $('#imgPreviewProfile').attr("src", reader.result);
            };
        }
    });
}
function clearImage() {
    IsRemoteImg = true;
    $('#imgPreviewProfile').attr("src", "/img/avatar-34.png");
    $('#FileUpload').val("");
}
var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
};

function OpenWaiting() {
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
}
function CloseWaiting() {
    $('#loader_id').removeClass("loader");
    document.getElementById("overlay").style.display = "none";
}
function GetNotifyByKey(key) {
    var index = dataLang.findIndex(x => x.key === key);
    if (langUse === 'en') {
        return dataLang[index].valueEn;
    } else {
        return dataLang[index].valueVi;
    }
}

function GetLanguage() {
    $.ajax({
        url: "/Home/GetLanguage",
        cache: false,
        type: "POST",
        success: function (data) {
            if (data.ok === true) {
                langUse = data.lang;
            }
        },
        error: function (reponse) {
        }
    });
}
GetLanguage();

