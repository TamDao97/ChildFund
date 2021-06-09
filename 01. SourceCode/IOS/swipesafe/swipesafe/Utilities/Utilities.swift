//
//  Utilities.swift
//  childprofile
//
//  Created by Thanh Luu on 1/22/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation
import DKImagePickerController

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
    
    static func getDataSize(from assets: [DKAsset], completion: @escaping (String) -> Void) {
        var size = 0
        DKImageAssetExporter.sharedInstance.exportAssetsAsynchronously(assets: assets) { (info) in
            assets.forEach({ (asset) in
                guard let fileUrl = asset.localTemporaryPath, let data = NSData(contentsOf: fileUrl) as Data? else { return }
                var tempData = data
                if asset.type == .photo, let image = UIImage(data: tempData), let compressData = image.jpeg(.medium) {
                    tempData = compressData
                }
                
                size += tempData.count
            })
            if size == 0 {
                completion("0 MB")
                return
            }
            completion(sizeString(bytes: Int64(size)))
        }
    }
    
    static func sizeString(bytes: Int64) -> String {
        let byteCountFormatter = ByteCountFormatter()
        byteCountFormatter.allowedUnits = [.useMB]
        byteCountFormatter.countStyle = .file
        
        return byteCountFormatter.string(fromByteCount: bytes)
    }
}
