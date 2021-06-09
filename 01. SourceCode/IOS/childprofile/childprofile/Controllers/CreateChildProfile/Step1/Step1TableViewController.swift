//
//  Step1TableViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class Step1TableViewController: StepViewController {
    @IBOutlet weak var childImageView: UIImageView!
    @IBOutlet weak var employeeNameTextField: UITextField!
    @IBOutlet weak var infoDateDatePickerView: DatePickerView!
    @IBOutlet weak var programCodeComboboxView: ComboboxView!
    @IBOutlet weak var childCodeTextField: UITextField!
    @IBOutlet weak var orderNumberTextField: UITextField!
    @IBOutlet weak var nationComboboxView: ComboboxView!
    @IBOutlet weak var religionComboboxView: ComboboxView!
    @IBOutlet weak var provinceComboboxView: ComboboxView!
    @IBOutlet weak var districtComboboxView: ComboboxView!
    @IBOutlet weak var wardComboboxView: ComboboxView!
    @IBOutlet weak var addressTextField: UITextField!
    
    @IBOutlet weak var employeeNameErrorLabel: UILabel!
    @IBOutlet weak var infoDateErrorLabel: UILabel!
    @IBOutlet weak var programCodeErrorLabel: UILabel!
    @IBOutlet weak var childCodeErrorLabel: UILabel!
    @IBOutlet weak var orderNumberErrorLabel: UILabel!
    @IBOutlet weak var nationErrorLabel: UILabel!
    @IBOutlet weak var provinceErrorLabel: UILabel!
    @IBOutlet weak var districtErrorLabel: UILabel!
    @IBOutlet weak var wardErrorLabel: UILabel!
    
    private let viewModel = Step1ViewModel()
    
    weak var containerViewController: CreateChildProfileViewController?
    
    override func setupView() {
        setupInfo()
        setupDatePickerView()
        setupComboBoxView()
    }
    
    override func refreshView() {
        containerViewController?.setupTitleLabel(Strings.step1Title.uppercased())
    }
    
    override func updateFormToViewModel() -> Bool {
        viewModel.employeeName = employeeNameTextField.text ?? ""
        viewModel.infoDate = infoDateDatePickerView.serverDateString ?? ""
        viewModel.programCode = viewModel.programCodeDataSource.get(index: programCodeComboboxView.selectedIndex)?.id ?? ""
        viewModel.childCode = childCodeTextField.text ?? ""
        viewModel.orderNumber = Int(orderNumberTextField.text ?? "")
        viewModel.nationId = viewModel.nationDataSource.get(index: nationComboboxView.selectedIndex)?.id ?? ""
        viewModel.religionId = viewModel.religionDataSource.get(index: religionComboboxView.selectedIndex)?.id ?? ""
        viewModel.provinceId = viewModel.provinceDataSource.get(index: provinceComboboxView.selectedIndex)?.id ?? ""
        viewModel.districtId = viewModel.districtDataSource.get(index: districtComboboxView.selectedIndex)?.id ?? ""
        viewModel.wardId = viewModel.wardDataSource.get(index: wardComboboxView.selectedIndex)?.id ?? ""
        viewModel.address = addressTextField.text ?? ""
        
        employeeNameErrorLabel.text = viewModel.employeeNameErrorTitle
        infoDateErrorLabel.text = viewModel.infoDateErrorTitle
        programCodeErrorLabel.text = viewModel.programCodeErrorTitle
        childCodeErrorLabel.text = viewModel.childCodeErrorTitle
        orderNumberErrorLabel.text = viewModel.orderNumberErrorTitle
        nationErrorLabel.text = viewModel.nationErrorTitle
        provinceErrorLabel.text = viewModel.provinceErrorTitle
        districtErrorLabel.text = viewModel.districtErrorTitle
        wardErrorLabel.text = viewModel.wardErrorTitle
        
        return viewModel.isFormValid
    }
    
    func getViewModel() -> Step1ViewModel {
        return viewModel
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
extension Step1TableViewController {
    private func setupInfo() {
        employeeNameTextField.text = Setting.userFullName.value
    }
    
    private func setupDatePickerView() {
        infoDateDatePickerView.config(with: Date())
    }
    
    private func setupComboBoxView() {
        nationComboboxView.config(with: viewModel.nationDataSource.map { $0.value }, selectedIndex: -1)
        programCodeComboboxView.config(with: viewModel.programCodeDataSource.map { $0.value }, selectedIndex: -1)
        religionComboboxView.config(with: viewModel.religionDataSource.map { $0.value }, selectedIndex: -1)
        provinceComboboxView.config(with: viewModel.provinceDataSource.map { $0.value }, selectedIndex: 0, isEnable: false)
        districtComboboxView.config(with: viewModel.districtDataSource.map { $0.value }, selectedIndex: 0, isEnable: false)
        wardComboboxView.config(with: viewModel.wardDataSource.map { $0.value }, selectedIndex: -1)
    }
}

// MARK: - Get info
extension Step1TableViewController {
    func setupViewModelFromParent(_ parentViewModel: CreateChildProfileViewModel) {
        guard
            parentViewModel.isEditMode,
            let model = parentViewModel.childProfileModel
        else { return }
        
        viewModel.employeeName = model.employeeName
        viewModel.imagePath = model.imagePath
        viewModel.infoDate = DateHelper.stringFromServerDateToShortDate(model.infoDate)
        viewModel.programCode = model.programCode
        viewModel.childCode = model.childCode
        viewModel.orderNumber = model.orderNumber
        viewModel.nationId = model.ethnicId
        viewModel.religionId = model.religionId
        viewModel.provinceId = model.provinceId
        viewModel.districtId = model.districtId
        viewModel.wardId = model.wardId
        viewModel.address = model.address
        
        childImageView.setImage(urlString: viewModel.imagePath)
        employeeNameTextField.text = viewModel.employeeName
        infoDateDatePickerView.config(with: DateHelper.dateFromShortDate(viewModel.infoDate))
        childCodeTextField.text = viewModel.childCode
        
        if let orderNumber = viewModel.orderNumber {
            orderNumberTextField.text = "\(orderNumber)"
        }
        nationComboboxView.config(with: viewModel.nationDataSource.map { $0.value },
                                  selectedIndex: Utilities.getIndex(from: viewModel.nationDataSource, with: viewModel.nationId))
        programCodeComboboxView.config(with: viewModel.programCodeDataSource.map { $0.value },
                                       selectedIndex: Utilities.getIndex(from: viewModel.programCodeDataSource, with: viewModel.programCode))
        religionComboboxView.config(with: viewModel.religionDataSource.map { $0.value },
                                    selectedIndex: Utilities.getIndex(from: viewModel.religionDataSource, with: viewModel.religionId))
        provinceComboboxView.config(with: viewModel.provinceDataSource.map { $0.value },
                                    selectedIndex: Utilities.getIndex(from: viewModel.provinceDataSource, with: viewModel.provinceId),
                                    isEnable: false)
        districtComboboxView.config(with: viewModel.districtDataSource.map { $0.value },
                                    selectedIndex: Utilities.getIndex(from: viewModel.districtDataSource, with: viewModel.districtId),
                                    isEnable: false)
        wardComboboxView.config(with: viewModel.wardDataSource.map { $0.value },
                                selectedIndex: Utilities.getIndex(from: viewModel.wardDataSource, with: viewModel.wardId))
        addressTextField.text = viewModel.address
    }
}

// MARK: - Image picker view controller delegate
extension Step1TableViewController: UIImagePickerControllerDelegate, UINavigationControllerDelegate {
    func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [UIImagePickerController.InfoKey : Any]) {
        guard let chosenImage = info[UIImagePickerController.InfoKey.originalImage] as? UIImage else {
            dismissImagePicker(picker)
            return
        }
        childImageView.image = chosenImage
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
