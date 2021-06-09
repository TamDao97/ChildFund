//
//  AppData.swift
//  childprofile
//
//  Created by Thanh Luu on 1/28/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import DKImagePickerController

class AppData {
    static var shared = AppData()
    private init() {}
    
    var reportModel = ReportModel()
    var assets: [DKAsset] = []
    var assetDatas: [(type: DKAssetType, data: Data)] = []
}
