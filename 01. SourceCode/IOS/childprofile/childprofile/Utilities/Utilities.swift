//
//  Utilities.swift
//  childprofile
//
//  Created by Thanh Luu on 1/22/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct Utilities {
    static func getName(from dataSource: [ComboboxItemModel], with id: String) -> String {
        if let item = dataSource.first(where: { $0.id == id }) {
            return item.value
        }
        return ""
    }
    
    static func getIndex(from dataSource: [ComboboxItemModel], with id: String) -> Int {
        return (dataSource.map { $0.id }).firstIndex(of: id) ?? -1
    }
}
