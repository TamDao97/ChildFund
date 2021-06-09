//
//  Strings.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

struct ScreenTitle {
    static let login = "Child Profile"
    static let listChildProfile = "Tìm kiếm hồ sơ trẻ"
    static let createChildProfile = "Tạo mới hồ sơ"
    static let updateChildProfile = "Cập nhật hồ sơ"
    static let shareImage = "Đăng ảnh hoạt động cộng đồng"
    static let userInfo = "Thông tin cá nhân"
    static let changePassword = "Đổi mật khẩu"
    static let logout = "Đăng xuất"
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
    
    // Login view
    static let loginButton = "Đăng nhập"
    static let usernameEmptyError = "Tên đăng nhập không được để trống."
    static let passwordEmptyError = "Mật khẩu không được để trống."
    static let passwordLengthError = "Mật khẩu phải lớn hơn \(Constants.maxPasswordLength) kí tự."
    
    // Change password view
    static let twoNewPasswordNotMatchError = "Hai mật khẩu mới không khớp nhau."
    static let successChangePasswordTitle = "Thay đổi mật khẩu thành công!"
    
    // Forward password view
    static let emailEmptyError = "Email không được để trống."
    static let emailWrongFormat = "Không đúng định dạng."
    
    // Confirm forward password view
    static let confirmKeyEmptyError = "Mã xác nhận không được để trống"
    
    // Update user profile view
    static let userSuccessUpdate = "Cập nhật thông tin cá nhân thành công!"
    static let nameEmptyError = "Họ tên không được để trống."
    
    // Create child profile
    static let step1Title = "1. Thông tin chung về trẻ và chương trình"
    static let step2Title = "2. Thông tin cụ thể về trẻ"
    static let step3Title = "3. Thông tin về các thành viên trong gia đình trẻ"
    static let step4Title = "4. Thông tin về điều kiện sống của gia đình"
    static let insertChildProfileSuccess = "Thêm mới hồ sơ trẻ thành công!"
    static let updateChildProfileSuccess = "Cập nhật hồ sơ trẻ thành công!"
    static let deleteFamilyMember = "Bạn có muốn xoá thành viên này không?"
    
    // Add report child profile
    static let reportSuccessAdd = "Báo cáo thay đổi tình trạng của trẻ thành công."
    static let reportContentTitleError = "Nội dung báo cáo không được để trống."
    
    // Share images
    static let shareImageSuccess = "Tải ảnh thành công"
    
    // Menu title
    static let menuTitle = "Hồ sơ cá nhân"
}

struct ImageNames {
    static let menu = "menu"
    static let close = "close"
    static let listChildProfile = "search_profile"
    static let createChildProfile = "add_profile"
    static let userInfo = "user_info"
    static let changePassword = "change_password"
    static let logout = "logout"
    static let search = "search"
    static let albumShare = "album_share"
}
