//
//  AppData+Report.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/17/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import DKImagePickerController

extension AppData {
    // MARK: - Reset
    func resetReport() {
        reportModel = ReportModel()
        assetDatas = []
    }
    
    // MARK: - Child Info
    func updateReportChildModel(_ childModel: ChildModel, at index: Int? = nil) {
        guard
            let index = index,
            index < reportModel.childs.count
        else {
            reportModel.childs.append(childModel)
            return
        }
        
        reportModel.childs[index] = childModel
    }
    
    func removeReportChildModel(at index: Int) {
        guard index < reportModel.childs.count else { return }
        reportModel.childs.remove(at: index)
    }
    
    func getChilds() -> [ChildModel] {
        return reportModel.childs
    }
    
    func getChild(at index: Int) -> ChildModel? {
        guard index < reportModel.childs.count else { return nil }
        return reportModel.childs[index]
    }
    
    // MARK: - Prisoner Info
    func updateReportPrisonelModel(_ prisonerModel: PrisonerModel, at index: Int? = nil) {
        guard
            let index = index,
            index < reportModel.prisoners.count
        else {
            reportModel.prisoners.append(prisonerModel)
            return
        }
        
        reportModel.prisoners[index] = prisonerModel
    }
    
    func removeReportPrisonerModel(at index: Int) {
        guard index < reportModel.prisoners.count else { return }
        reportModel.prisoners.remove(at: index)
    }
    
    func getPrisoner() -> [PrisonerModel] {
        return reportModel.prisoners
    }
    
    func getPrisoner(at index: Int) -> PrisonerModel? {
        guard index < reportModel.prisoners.count else { return nil }
        return reportModel.prisoners[index]
    }
    
    // MARK: - Description
    func updateAbuseDescription(_ content: String) {
        reportModel.description = content
    }
    
    func getDescription() -> String {
        return reportModel.description
    }
    
    // MARK: - Reporter
    func updateInfoReporter(from reporterModel: ReporterModel) {
        reportModel.updateValue(from: reporterModel)
    }
    
    func getReporterInfo() -> ReporterModel {
        return reportModel.getReporterInfo()
    }
    
    // MARK: - Photos
    func updateAssets(_ assets: [DKAsset]) {
        self.assets = assets
    }
    
    func getAssets() -> [DKAsset] {
        return assets
    }
    
    func updateAssetDatas(completion: @escaping () -> Void) {
        assetDatas = []
        DKImageAssetExporter.sharedInstance.exportAssetsAsynchronously(assets: assets) { [unowned self] (info) in
            self.assets.forEach({ (asset) in
                if let fileUrl = asset.localTemporaryPath, let data = NSData(contentsOf: fileUrl) as Data? {
                    var assetData = (type: asset.type, data: data)
                    if assetData.type == .photo, let image = UIImage(data: assetData.data), let compressData = image.jpeg(.medium) {
                        assetData.data = compressData
                    }
                    self.assetDatas.append(assetData)
                }
            })
            completion()
        }
    }
}
