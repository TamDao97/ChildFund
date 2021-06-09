//
//  ComboboxItemModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/19/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ComboboxItemModel: Codable {
    var id: String
    var value: String
    var pid: String
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case value = "Name"
        case pid = "PId"
    }
    
    init(id: String, value: String, pid: String = "") {
        self.id = id
        self.value = value
        self.pid = pid
    }
    
    init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.id = try container.decodeIfPresent(String.self, forKey: .id) ?? ""
        self.value = try container.decodeIfPresent(String.self, forKey: .value) ?? ""
        self.pid = try container.decodeIfPresent(String.self, forKey: .pid) ?? ""
    }
    
    static func getSessionData(from string: String) -> [ComboboxItemModel] {
        do {
            guard let data = string.data(using: .utf8) else {
                return []
            }
            
            let values = try JSONDecoder().decode([ComboboxItemModel].self, from: data)
            return values
        } catch {
            print(error)
        }
        return []
    }
}
