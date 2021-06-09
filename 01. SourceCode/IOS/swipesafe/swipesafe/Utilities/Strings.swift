//
//  Strings.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

struct ScreenTitle {
    
}

struct Strings {
    // Common
    static let empty = ""
    static let ok = "Hoàn thành"
    static let cancel = "Huỷ"
    static let emptyError = "Không được để trống."
    static let updateSuccess = "Cập nhật thành công"
    static let agree = "Đồng ý"
    static let close = "Đóng"
    static let yes = "Có"
    static let no = "Không"
    
    // Show enable location
    static let locationEnableTitle = "Bạn cần cho phép ứng dụng truy xuất vị trí của bạn!"
    
    // Form
    static let confirmFormExitTitle = "Bạn sắp thoát khỏi báo cáo này. Nó sẽ xóa tất cả dữ liệu báo cáo. Bạn có muốn thoát ra không?"
    static let nextButtonTitle = "tiếp tục".uppercased()
    static let saveButtonTitle = "lưu".uppercased()
    
    // Step 1
    struct Step1 {
        static let unknownChildName = "Không biết tên"
        static let nameEmptyErrorTitle = "Họ và tên không được để trống."
        static let genderEmptyErrorTitle = "Chưa chọn giới tính cho trẻ."
        static let levelEmptyErrorTitle = "Mức độ nghiêm trọng không được để trống."
        static let provinceEmptyErrorTitle = "Chưa chọn Tỉnh/Thành nơi xảy ra."
        static let districtEmptyErrorTitle = "Chưa chọn Quận/Huyện nơi xảy ra."
        static let wardEmptyErrorTitle = "Chưa chọn Xã/Phường nơi xảy ra."
        static let dateEmptyErrorTitle = "Chưa nhập thời gian xảy ra."
        static let abuseEmptyErrorTitle = "Chưa chọn hình thức xâm hại."
        static let emptyErrorTitle = "Hãy nhập thông tin để tiếp tục."
        static let ageOver18ErrorTitle = "Số tuổi ước lượng không được nhập quá 18 tuổi"
    }
    
    // Step 2
    struct Step2 {
        static let emptyErrorTitle = "Bạn cần nhập ít nhất 1 thông tin để tiếp tục"
    }
    
    // Step 3
    struct Step3 {
        static let descriptionEmptyErrorTitle = "Bạn chưa nhập mô tả hành vi chi tiết của trẻ bị xâm hại."
        static let updateErrorTitle = "Lỗi phát sinh trong quá trình xử lý."
        static let overSizeErrorTitle = "Độ lớn của tệp lớn hơn 100MB. Vui lòng chọn tệp khác!"
    }
    
    // Step 4
    struct Step4 {
        static let emailInValidErrorTitle = "Địa chỉ email không hợp lệ."
    }
    
    static let dummyDate = "02-03-2018"
    
    static let videoTitle = "Giáo dục trẻ em"
    
    static let skillTitle = "Kỹ năng cho trẻ"
    static let skill1Title = "'Quy tắc 5 ngón tay' dạy trẻ tránh bị xâm hại"
    static let skill1Content = "Cha mẹ có thể dạy con xác định được 5 nhóm người thường gặp trong cuộc sống hàng ngày, từ đó đưa ra định hướng giao tiếp phù hợp, giúp trẻ tránh bị lạm dụng, mua chuộc hay xâm hại tình dục..."
    static let skill1HtmlFileName = "file_quytac5ngon"
    static let skill2Title = "6 kỹ năng phòng chống xâm hại trẻ em"
    static let skill2Content = "Hàng loạt các vụ cáo buộc xâm hại trẻ em gần đây là hồi chuông cảnh tỉnh cho các bậc cha mẹ và xã hội về việc bảo vệ trẻ em khỏi vấn nạn này. Sau đây là những kỹ năng phòng chống xâm hại trẻ em được các chuyên gia về giới tính và trẻ em khuyến nghị cha mẹ và nhà trường nên giáo dục cho trẻ..."
    static let skill2HtmlFileName = "file_6quytac"
    static let skill3Title = "7 kỹ năng phòng tránh xâm hại trẻ em"
    static let skill3Content = "Những kỹ năng này có thể đơn giản nhưng cũng hiệu quả trong việc giúp trẻ tránh xa nguy hiểm khi cần thiết..."
    static let skill3HtmlFileName = "file_7kynang"
    static let skill4Title = "Kỹ năng sống cần thiết cho trẻ"
    static let skill4Content = "Cha mẹ nào sinh con ra cũng mong con mình khỏe mạnh, thông minh và cố gắng tạo điều kiện tốt nhất để con phát triển. Tuy nhiên bên cạnh việc chăm sóc để con phát triển thể chất, dạy dỗ con để con học hành giỏi giang, nhiệm vụ giúp con định hướng, xây dựng và phát triển các kỹ năng sống cơ bản là không thể thiếu được..."
    static let skill4HtmlFileName = "file_kynangsong"
    
    static let rightTitle = "Quyền trẻ em"
    static let right1 = "Quyền được khai sinh và có quốc tịch"
    static let right1Url = "http://bocongan.gov.vn/KND/vb/vbqp/Lists/VBQP/Attachments/2242/QH13.pdf"
    static let right2 = "Quyền được chăm sóc, nuôi dưỡng"
    static let right2Url = "http://bocongan.gov.vn/KND/vb/vbqp/Lists/VBQP/Attachments/2242/QH13.pdf"
    static let right3 = "Quyền sống chung với cha mẹ"
    static let right3Url = "http://bocongan.gov.vn/KND/vb/vbqp/Lists/VBQP/Attachments/2242/QH13.pdf"
    static let right4 = "Quyền được tôn trọng, bảo vệ tính mạng, thân thể, nhân phẩm và danh dự"
    static let right4Url = "http://bocongan.gov.vn/KND/vb/vbqp/Lists/VBQP/Attachments/2242/QH13.pdf"
    static let right5 = "Quyền được chăm sóc sức khoẻ"
    static let right5Url = "http://bocongan.gov.vn/KND/vb/vbqp/Lists/VBQP/Attachments/2242/QH13.pdf"
}

struct ImageNames {
    static let next = "next"
    static let save = "save"
    
    static let skill1 = "lib_5ngon"
    static let skill2 = "lib_6kynang"
    static let skill3 = "lib_7kynang"
    static let skill4 = "lib_kynangsong"
    
    static let homeBackground = ""
}
