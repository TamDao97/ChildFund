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
}

extension Setting {
    static func clear() {
        Setting.userId.value = ""
    }
}

enum SettingUserDefaultKey {
    static let isLogin = "isLogin"
    
    static let userId = "userId"
}


