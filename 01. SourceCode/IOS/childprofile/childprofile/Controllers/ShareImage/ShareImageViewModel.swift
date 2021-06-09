//
//  ShareImageViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/27/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ShareImageViewModel: BaseViewModel {
    var userUploadImageModel: UserUploadImageModel?
    var successMessage = Strings.shareImageSuccess
    
    var imageDatas: [Data] = []
    
    func upload(completion: @escaping (ResultStatus) -> Void) {
        userUploadImageModel = UserUploadImageModel(content: "\(Setting.userFullName.value) tải ảnh lên",
                                                    userId: Setting.userId.value)
        ShareImageService.uploadImage(images: imageDatas, model: userUploadImageModel!) { [weak self] result in
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


