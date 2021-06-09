
//
//  Step1ViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import DKImagePickerController

class Step3ViewController: StepViewController {
    @IBOutlet weak var descriptionTextView: UITextView!
    @IBOutlet weak var galleryCollectionView: UICollectionView!
    @IBOutlet weak var currentSizeLabel: UILabel!
    
    var viewModel = Step3ViewModel()
    
    var isEditMode: Bool = false
    var filesTotalSizeString = "" {
        didSet {
            viewModel.updateTotalSize(size: filesTotalSizeString)
            currentSizeLabel.text = "(Độ lớn hiện tại: \(filesTotalSizeString))"
        }
    }
    
    override func setupView() {
        setupEditModeIfNeeded()
        configKeyboard()
        configCollectionView()
    }
    
    override func updateFormToViewModel() -> Bool {
        viewModel.description = descriptionTextView.text
        
        let errorFormMessage = viewModel.errorFormMessage
        let isFormValid = errorFormMessage.isEmpty
        
        if isFormValid {
            viewModel.updateReportDescriptionData()
        } else {
            showMessage(title: errorFormMessage)
        }
        
        return isFormValid
    }
    
    @IBAction func cameraButtonWasTouched(_ sender: Any) {
        guard UIImagePickerController.isSourceTypeAvailable(.camera) else {
            return
        }
        
        let pickerController = DKImagePickerController()
        pickerController.sourceType = .camera
        
        DKImageExtensionController.registerExtension(extensionClass: CustomCameraExtension.self, for: .inlineCamera)
        
        pickerController.didSelectAssets = { [weak self] (assets: [DKAsset]) in
            guard let self = self else { return }
            self.viewModel.assets.append(contentsOf: assets)
            self.reloadGalleryCollectionView()
        }
        
        present(pickerController, animated: true, completion: nil)
    }
    
    @IBAction func albumButtonWasTouched(_ sender: Any) {
        let pickerController = DKImagePickerController()
        pickerController.showsCancelButton = true
        pickerController.sourceType = .photo
        pickerController.select(assets: viewModel.assets)
        
        pickerController.didSelectAssets = { [weak self] (assets: [DKAsset]) in
            guard let self = self else { return }
            self.viewModel.assets = assets
            self.reloadGalleryCollectionView()
        }
        
        present(pickerController, animated: true, completion: nil)
    }
}

// MARK: - First setup
extension Step3ViewController {
    private func setupEditModeIfNeeded() {
        guard isEditMode else { return }
        refreshValueFromAppData()
    }
    
    func refreshValueFromAppData() {
        viewModel.updateValueFromAppData()
        updateTotalFileSize()
        bindFormFromViewModel()
    }
    
    private func configKeyboard() {
        hideKeyboardWhenTappedAround()
    }
    
    private func configCollectionView() {
        galleryCollectionView.dataSource = self
        galleryCollectionView.delegate = self
        
        galleryCollectionView.register(UINib(nibName: Step3PhotoCollectionViewCell.className, bundle: nil),
                                       forCellWithReuseIdentifier: Step3PhotoCollectionViewCell.className)
    }
}

// MARK: - Helpers
extension Step3ViewController {
    private func reloadGallery() {
        galleryCollectionView.reloadData()
    }
    
    private func bindFormFromViewModel() {
        descriptionTextView.text = viewModel.description
    }
    
    private func reloadGalleryCollectionView() {
        updateTotalFileSize()
        reloadGallery()
    }
    
    private func updateTotalFileSize() {
        Utilities.getDataSize(from: viewModel.assets, completion: { [weak self] sizeString in
            self?.filesTotalSizeString = sizeString
        })
    }
}

// MARK: - Collection view datasource
extension Step3ViewController: UICollectionViewDataSource {
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return viewModel.assets.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: Step3PhotoCollectionViewCell.className, for: indexPath) as! Step3PhotoCollectionViewCell
        cell.delegate = self
        cell.tag = indexPath.item
        if let layout = collectionView.collectionViewLayout as? UICollectionViewFlowLayout {
            viewModel.assets[indexPath.item].fetchImage(with: layout.itemSize.toPixel()) { [weak self] (image, info) in
                guard let self = self else { return }
                if let imageFetched = image, cell.tag == indexPath.item {
                    cell.config(image: imageFetched,
                                index: indexPath.item,
                                isVideo: self.viewModel.assets[indexPath.item].type == .video)
                }
            }
        }
        
        return cell
    }
}

// MARK: - Collection view delegate
extension Step3ViewController: UICollectionViewDelegateFlowLayout {
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let length = (collectionView.bounds.size.width - 3) / 4
        return CGSize(width: length, height: length)
    }
}

// MARK: - Collection cell delegate
extension Step3ViewController: Step3PhotoCollectionViewCellDelegate {
    func removePhoto(at index: Int) {
        viewModel.assets.remove(at: index)
        reloadGalleryCollectionView()
    }
}
