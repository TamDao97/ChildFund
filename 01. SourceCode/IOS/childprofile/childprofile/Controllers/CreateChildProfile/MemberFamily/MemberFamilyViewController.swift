//
//  MemberFamilyViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/21/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class MemberFamilyViewController: BaseViewController {
    @IBOutlet weak var nameTextField: UITextField!
    @IBOutlet weak var genderSegmentedControl: UISegmentedControl!
    @IBOutlet weak var dateOfBirthDatePickerView: DatePickerView!
    @IBOutlet weak var relationShipComboboxView: ComboboxView!
    @IBOutlet weak var jobComboboxView: ComboboxView!
    @IBOutlet weak var liveWithChildSegmentedControl: UISegmentedControl!
    
    @IBOutlet weak var nameErrorLabel: UILabel!
    @IBOutlet weak var relationShipErrorLabel: UILabel!
    
    weak var callerViewController: Step3TableViewController?
    var viewModel = MemberFamilyViewModel()
    
    override func setupView() {
        configForm()
        bindFormWithViewModel()
    }
    
    @IBAction func updateButtonWasTouched(_ sender: Any) {
        viewModel.name = nameTextField.text ?? ""
        viewModel.gender = genderSegmentedControl.selectedSegmentIndex == 0 ? 1 : 0
        viewModel.dateOfBirth = dateOfBirthDatePickerView.serverDateString ?? ""
        viewModel.relationShipId = viewModel.relationShipDataSource.get(index: relationShipComboboxView.selectedIndex)?.id ?? ""
        viewModel.jobId = viewModel.jobDataSource.get(index: jobComboboxView.selectedIndex)?.id ?? ""
        viewModel.liveWithChild = liveWithChildSegmentedControl.selectedSegmentIndex == 0 ? 1 : 0
        
        nameErrorLabel.text = viewModel.nameErrorTitle
        relationShipErrorLabel.text = viewModel.relationShipErrorTitle
        
        guard viewModel.isFormValid else {
            return
        }
        
        if viewModel.isEditMode {
            viewModel.updateEditModel()
            callerViewController?.updateFamilyMember(viewModel: viewModel)
            dismiss(animated: true, completion: nil)
        } else {
            callerViewController?.updateFamilyMember(viewModel: viewModel)
            viewModel = MemberFamilyViewModel()
            bindFormWithViewModel()
        }
        showMessage(title: Strings.updateSuccess)
    }
    
    private func configForm() {
        genderSegmentedControl.isUserInteractionEnabled = false
    }
    
    private func bindFormWithViewModel() {
        nameTextField.text = viewModel.name
        genderSegmentedControl.selectedSegmentIndex = viewModel.gender == 0 ? 1 : 0
        dateOfBirthDatePickerView.config(with: DateHelper.dateFromShortDate(viewModel.dateOfBirth))
        relationShipComboboxView.config(with: viewModel.relationShipDataSource.map { $0.value },
                                        selectedIndex: Utilities.getIndex(from: viewModel.relationShipDataSource,
                                                                          with: viewModel.relationShipId))
        relationShipComboboxView.completion = { [weak self] selectedIndex in
            guard
                let self = self,
                let genderValue = Int(self.viewModel.relationShipDataSource[selectedIndex].pid)
            else { return }
            self.genderSegmentedControl.selectedSegmentIndex = genderValue == 0 ? 1 : 0
        }
        
        jobComboboxView.config(with: viewModel.jobDataSource.map { $0.value },
                               selectedIndex: Utilities.getIndex(from: viewModel.jobDataSource,
                                                                 with: viewModel.jobId))
        liveWithChildSegmentedControl.selectedSegmentIndex = viewModel.liveWithChild == 0 ? 1 : 0
    }
    
    @IBAction func closeButtonWasTouched(_ sender: Any) {
        dismiss(animated: true, completion: nil)
    }
}

extension MemberFamilyViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        textField.resignFirstResponder()
        return true
    }
}
