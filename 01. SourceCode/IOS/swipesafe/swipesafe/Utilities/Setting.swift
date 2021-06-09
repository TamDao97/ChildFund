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
    
    init(userDefaultKey: SettingUserDefaultKey, defaultValue: T) {
        self.key = userDefaultKey.rawValue
        self.defaultValue = defaultValue
    }
}

struct Setting {
    static let formAbuse = WrappedUserDefault<String>(userDefaultKey: .formAbuse, defaultValue: "")
    static let district = WrappedUserDefault<String>(userDefaultKey: .district, defaultValue: "")
    static let province = WrappedUserDefault<String>(userDefaultKey: .province, defaultValue: "")
    static let ward = WrappedUserDefault<String>(userDefaultKey: .ward, defaultValue: "")
    static let relationShip = WrappedUserDefault<String>(userDefaultKey: .relationShip, defaultValue: "")
}

extension Setting {
    static func clear() {
        Setting.province.value = ""
        Setting.district.value = ""
    }
}

enum SettingUserDefaultKey: String {
    case formAbuse
    case district
    case province
    case ward
    case relationShip
}
