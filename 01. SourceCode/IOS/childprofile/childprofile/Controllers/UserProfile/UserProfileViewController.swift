//
//  UserProfileViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class UserProfileViewController: BaseTableViewController {
    @IBOutlet weak var userImageView: UIImageView!
    
    @IBOutlet weak var cameraImageView: UIImageView!
    @IBOutlet weak var albumSelectImageView: UIImageView!
    
    @IBOutlet weak var nameTextField: UITextField!
    @IBOutlet weak var nameErrorLabel: UILabel!
    
    @IBOutlet weak var genderSegmentedControl: UISegmentedControl!
    
    @IBOutlet weak var dateOfBirthTextField: UITextField!
    @IBOutlet weak var dateOfBirthErrorLabel: UILabel!
    
    @IBOutlet weak var phoneNumberTextField: UITextField!
    @IBOutlet weak var phoneNumberErrorLabel: UILabel!
    
    @IBOutlet weak var emailTextField: UITextField!
    @IBOutlet weak var emailErrorLabel: UILabel!
    
    @IBOutlet weak var identifyNumberTextField: UITextField!
    @IBOutlet weak var identifyNumberErrorLabel: UILabel!
    
    @IBOutlet weak var addressNumberTextField: UITextField!
    @IBOutlet weak var addressNumberErrorLabel: UILabel!
    
    var viewModel = UserProfileViewModel()
    
    override func setupTitle() {
        title = ScreenTitle.userInfo
    }
    
    override func setupView() {
        getUserInfo()
        setupImageButton()
    }
    
    override func refreshView() {
        parent?.title = title
    }
    
    
    @IBAction func updateUserProfileButtonWasTouched(_ sender: Any) {
        viewModel.name = nameTextField.text ?? ""
        if let dateOfBirth = dateOfBirthTextField.text {
            viewModel.dateOfBirth = DateHelper.stringFromLocalDateToShortDate(dateOfBirth)
        }
        viewModel.phoneNumber = phoneNumberTextField.text ?? ""
        viewModel.email = emailTextField.text ?? ""
        viewModel.gender = genderSegmentedControl.selectedSegmentIndex == 0 ? 1 : 0
        viewModel.identifyNumber = identifyNumberTextField.text ?? ""
        viewModel.address = addressNumberTextField.text ?? ""
        
        nameErrorLabel.text = viewModel.usernameErrorTitle
        emailErrorLabel.text = viewModel.emailErrorTitle
        
        guard viewModel.isFormValid else {
            return
        }
        
        showHUD()
        viewModel.update() { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            guard let mainViewController = self.parent as? MainViewController else {
                return
            }
            mainViewController.reloadProfileImage()
            mainViewController.showMessage(title: self.viewModel.successMessage)
        }
    }
    
    @IBAction func cameraButtonWasTouched(_ sender: Any) {
        guard let imagePicker = getImagePickerController(type: .camera) else {
            return
        }
        imagePicker.delegate = self
        present(imagePicker, animated: true, completion: nil)
        imagePicker.popoverPresentationController?.barButtonItem = navigationItem.leftBarButtonItem
    }
    
    @IBAction func albumButtonWasTouched(_ sender: Any) {
        guard let imagePicker = getImagePickerController(type: .photoLibrary) else {
            return
        }
        imagePicker.delegate = self
        present(imagePicker, animated: true, completion: nil)
        imagePicker.popoverPresentationController?.barButtonItem = navigationItem.leftBarButtonItem
    }
}

// MARK: - Setup
extension UserProfileViewController {
    private func setupImageButton() {
        cameraImageView.image = cameraImageView.image?.withRenderingMode(.alwaysTemplate)
        cameraImageView.tintColor = .white
        albumSelectImageView.image = albumSelectImageView.image?.withRenderingMode(.alwaysTemplate)
        albumSelectImageView.tintColor = .white
    }
    
    private func getUserInfo() {
        showHUD()
        viewModel.get() { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            self.bindToViewModel()
        }
    }
}

// MARK: - Helpers
extension UserProfileViewController {
    private func bindToViewModel() {
        nameTextField.text = viewModel.name
        userImageView.setImage(urlString: viewModel.imagePath)
        genderSegmentedControl.selectedSegmentIndex = viewModel.genderSegmentIndex
        dateOfBirthTextField.text = viewModel.dateOfBirth
        identifyNumberTextField.text = viewModel.identifyNumber
        phoneNumberTextField.text = viewModel.phoneNumber
        emailTextField.text = viewModel.email
        addressNumberTextField.text = viewModel.address
    }
}

// MARK: - Image picker view controller delegate
extension UserProfileViewController: UIImagePickerControllerDelegate, UINavigationControllerDelegate {
    func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [UIImagePickerController.InfoKey : Any]) {
        guard let chosenImage = info[UIImagePickerController.InfoKey.originalImage] as? UIImage else {
            dismissImagePicker(picker)
            return
        }
        userImageView.image = chosenImage
        viewModel.setImageData(image: chosenImage)
        dismissImagePicker(picker)
    }
    
    func imagePickerControllerDidCancel(_ picker: UIImagePickerController) {
        dismissImagePicker(picker)
    }
    
    private func dismissImagePicker(_ picker: UIImagePickerController) {
        picker.dismiss(animated: true, completion: nil)
    }
}
