//
//  FormViewModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/9/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class FormViewModel {
    var pageIndex = 0
    
    func resetAppDataReport() {
        AppData.shared.resetReport()
    }
}
