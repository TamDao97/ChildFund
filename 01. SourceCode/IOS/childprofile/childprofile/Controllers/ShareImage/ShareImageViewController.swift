//
//  ShareImageViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/26/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import DKImagePickerController

class ShareImageViewController: BaseViewController {
    let limitSelected = 10
    var viewModel = ShareImageViewModel()
    
    override func setupTitle() {
        title = ScreenTitle.shareImage
    }
    
    override func refreshView() {
        parent?.title = title
    }
    
    @IBAction func selectImageButtonWasTouched(_ sender: Any) {
        let pickerController = DKImagePickerController()
        pickerController.showsCancelButton = true
        pickerController.sourceType = .photo
        pickerController.maxSelectableCount = limitSelected
        pickerController.didSelectAssets = { [weak self] (assets: [DKAsset]) in
            guard let self = self else { return }
            var images: [UIImage] = []
            self.showHUD()
            for (index, asset) in assets.enumerated() {
                asset.fetchOriginalImage { imageSelected, _ in
                    if let image = imageSelected {
                        images.append(image)
                    }
                    
                    guard index == assets.count - 1 else { return }
                    self.viewModel.imageDatas = images.compactMap { $0.jpeg(.medium) }
                    self.uploadImage()
                }
            }
        }
        
        present(pickerController, animated: true, completion: nil)
    }
    
    private func uploadImage() {
        viewModel.upload() { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            self.showMessage(title: self.viewModel.successMessage)
        }
    }
}
