//
//  Step3ViewModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/24/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import DKImagePickerController

class Step3ViewModel {
    var description: String = ""
    var assets: [DKAsset] = []
    var totalSize = 0.0
    
    var errorFormMessage: String {
        if description.isEmpty {
            return Strings.Step3.descriptionEmptyErrorTitle
        }
        
        if totalSize > Constants.maxReportSize {
            return Strings.Step3.descriptionEmptyErrorTitle
        }
        
        return ""
    }
    
    func updateValueFromAppData() {
        description = AppData.shared.getDescription()
        assets = AppData.shared.getAssets()
    }
    
    func updateReportDescriptionData() {
        AppData.shared.updateAbuseDescription(description)
        AppData.shared.updateAssets(assets)
    }
    
    func updateTotalSize(size: String) {
        totalSize = Double(String(size.dropLast(3))) ?? 0
    }
}
