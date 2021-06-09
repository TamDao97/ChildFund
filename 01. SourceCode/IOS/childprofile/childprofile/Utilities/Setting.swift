//
//  Setting.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class WrappedUserDefault<T> {
    let key: String
    let defaultValue: T
    
    var value: T {
        get {
            if let value = UserDefaults.standard.object(forKey: key) as? T {
                return value
            } else {
                return defaultValue
            }
        }
        set {
            UserDefaults.standard.set(newValue, forKey: key)
            UserDefaults.standard.synchronize()
        }
    }
    
    init(key: String, defaultValue: T) {
        self.key = key
        self.defaultValue = defaultValue
    }
}

struct Setting {
    static let isLogin = WrappedUserDefault<Bool>(key: SettingUserDefaultKey.isLogin, defaultValue: false)
    
    static let userId = WrappedUserDefault<String>(key: SettingUserDefaultKey.userId, defaultValue: "")
    static let areaId = WrappedUserDefault<String>(key: SettingUserDefaultKey.areaId, defaultValue: "")
    static let areaDistrictId = WrappedUserDefault<String>(key: SettingUserDefaultKey.areaDistrictId, defaultValue: "")
    static let userLevel = WrappedUserDefault<String>(key: SettingUserDefaultKey.userLevel, defaultValue: "")
    static let userFullName = WrappedUserDefault<String>(key: SettingUserDefaultKey.userFullName, defaultValue: "")
    static let imagePath = WrappedUserDefault<String>(key: SettingUserDefaultKey.imagePath, defaultValue: "")
    
    static let relationShip = WrappedUserDefault<String>(key: SettingUserDefaultKey.relationShip, defaultValue: "")
    static let job = WrappedUserDefault<String>(key: SettingUserDefaultKey.job, defaultValue: "")
    static let religion = WrappedUserDefault<String>(key: SettingUserDefaultKey.religion, defaultValue: "")
    static let nation = WrappedUserDefault<String>(key: SettingUserDefaultKey.nation, defaultValue: "")
    static let province = WrappedUserDefault<String>(key: SettingUserDefaultKey.province, defaultValue: "")
    static let district = WrappedUserDefault<String>(key: SettingUserDefaultKey.district, defaultValue: "")
    static let ward = WrappedUserDefault<String>(key: SettingUserDefaultKey.ward, defaultValue: "")
}

extension Setting {
    static func clear() {
        Setting.userId.value = ""
        Setting.areaId.value = ""
        Setting.areaDistrictId.value = ""
        Setting.userLevel.value = ""
        Setting.userFullName.value = ""
        Setting.imagePath.value = ""
        
        Setting.relationShip.value = ""
        Setting.job.value = ""
        Setting.religion.value = ""
        Setting.nation.value = ""
        Setting.province.value = ""
        Setting.district.value = ""
        Setting.ward.value = ""
    }
}

enum SettingUserDefaultKey {
    static let isLogin = "isLogin"
    
    static let userId = "userId"
    static let areaId = "areaId"
    static let areaDistrictId = "areaDistrictId"
    static let userLevel = "userLevel"
    static let userFullName = "userFullName"
    static let imagePath = "imagePath"
    
    static let relationShip = "relationShip"
    static let job = "job"
    static let religion = "religion"
    static let nation = "nation"
    static let province = "province"
    static let district = "district"
    static let ward = "ward"
}


