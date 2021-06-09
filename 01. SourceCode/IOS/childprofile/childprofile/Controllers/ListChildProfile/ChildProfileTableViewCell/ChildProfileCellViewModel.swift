//
//  ChildProfileCellViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ChildProfileCellViewModel {
    var id: String
    var imageUrl: String
    var name: String
    var code: String
    var status: String
    var address: String
    
    init(model: ChildProfileSearchResult) {
        id = model.id
        name = model.name
        code = model.childCode
        address = model.childCode
        status = model.status == "0" ? "Chưa duyệt" : ""
        imageUrl = model.avatar
    }
}
