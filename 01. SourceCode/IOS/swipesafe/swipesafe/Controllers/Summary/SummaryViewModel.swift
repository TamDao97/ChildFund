//
//  SummaryViewModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/25/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

enum SummaryContentType: String {
    case child
    case prisoner
    case description
    case reporter
    
    var title: String {
        switch self {
        case .child:
            return "Trẻ bị xâm hại"
        case .prisoner:
            return "Nghi phạm"
        case .description:
            return "Mô tả hành vi xâm hại"
        case .reporter:
            return "Thông tin người báo cáo"
        }
    }
}

class SummaryCellViewModel {
    var contentType: SummaryContentType = .child
    var order: Int = 0
    
    init(contentType: SummaryContentType, order: Int) {
        self.contentType = contentType
        self.order = order
    }
}

class SummaryViewModel {
    var summaryCellViewModels: [SummaryCellViewModel] = []
    var errorResponseContent: String?
    var successMessage = "Gửi báo cáo thành công"
    
    var reportDescription: String {
        return AppData.shared.getDescription()
    }
    
    var reporterInfo: ReporterModel {
        return AppData.shared.getReporterInfo()
    }
    
    init() {
        refreshData()
    }
    
    func refreshData() {
        let childDetailCellViewModels = (0..<AppData.shared.getChilds().count).map {
            SummaryCellViewModel(contentType: .child, order: $0)
        }
        let prisonerDetailCellViewModels = (0..<AppData.shared.getPrisoner().count).map {
            SummaryCellViewModel(contentType: .prisoner, order: $0)
        }
        let descriptionCellViewModel = SummaryCellViewModel(contentType: .description, order: 0)
        let reporterCellViewModel = SummaryCellViewModel(contentType: .reporter, order: 0)
        
        summaryCellViewModels = childDetailCellViewModels + prisonerDetailCellViewModels + [descriptionCellViewModel, reporterCellViewModel]
    }
    
    func createReport(completion: @escaping (ResultStatus) -> Void) {
        AppData.shared.updateAssetDatas() {
            ReportService.create(assetListData: AppData.shared.assetDatas, model: AppData.shared.reportModel) { [weak self] result in
                guard let self = self else { return }
                
                switch result {
                case .success:
                    completion(.success)
                case .failure(let error):
                    self.errorResponseContent = error
                    completion(.failed)
                }
            }
        }
    }
}
