//
//  ListChildProvileViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ListChildProfileViewModel: BaseViewModel {
    var searchCondition = ChildProfileSearchCondition(userId: Setting.userId.value,
                                                      orderType: true,
                                                      orderBy: "Name",
                                                      pageNumber: 1,
                                                      pageSize: 15)
    
    var currentChildId: String = ""
    var currentReportContent: String = ""
    
    var listChildProfileCellViewModel: [ChildProfileCellViewModel] = []
    var totalItem = 0
    var recentItemCount = 0
    
    func search(completion: @escaping (ResultStatus) -> Void) {
        ChildProfileService.search(searchModel: searchCondition) { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success(let searchResult):
                let items = searchResult.listResult.map { ChildProfileCellViewModel(model: $0)}
                
                if self.searchCondition.pageNumber == 1 {
                    self.listChildProfileCellViewModel = items
                } else {
                    self.listChildProfileCellViewModel.append(contentsOf: items)
                }
                
                self.totalItem = searchResult.totalItem
                self.recentItemCount = items.count
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
    
    func addReport(completion: @escaping (ResultStatus) -> Void) {
        let reportModel = ReportProfileModel(content: currentReportContent,
                                             childProfileId: currentChildId,
                                             status: Constants.profileNew,
                                             isDelete: Constants.profilesIsUse,
                                             createBy: Setting.userId.value,
                                             updateBy: Setting.userId.value)
        
        ChildProfileService.addReport(reportProfileModel: reportModel) { [weak self] result in
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
