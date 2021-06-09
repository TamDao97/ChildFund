//
//  SearchViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class SearchViewModel {
    var code: String = ""
    var name: String = ""
    var address: String = ""
    
    convenience init(code: String, name: String, address: String) {
        self.init()
        self.code = code
        self.name = name
        self.address = address
    }
}
