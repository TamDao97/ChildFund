
//
//  Step1ViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import CoreLocation

class Step4ViewController: StepViewController {
    @IBOutlet weak var isInvisibleCheckboxView: CheckboxView!
    @IBOutlet weak var containerView: UIView!
    weak var contentViewController: Step4TableViewController?
    
    var isEditMode: Bool = false
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        guard segue.identifier == Step4TableViewController.className else { return }
        contentViewController = segue.destination as? Step4TableViewController
        contentViewController?.containerViewController = self
        contentViewController?.isEditMode = isEditMode
    }
    
    override func updateFormToViewModel() -> Bool {
        return contentViewController?.updateFormToViewModel() ?? false
    }
    
    override func setupView() {
        setupIsInvisibleCheckboxView()
    }
    
    private func setupIsInvisibleCheckboxView() {
        isInvisibleCheckboxView.selectedCompletion = { [weak self] in
            guard let self = self else { return }
            self.containerView.isHidden = self.isInvisibleCheckboxView.isSelected
        }
    }
    
    func configInvisibleCheckboxView(isSelected: Bool) {
        isInvisibleCheckboxView.isSelected = isSelected
        containerView.isHidden = isInvisibleCheckboxView.isSelected
    }
}

class Step4TableViewController: StepTableViewController {
    @IBOutlet weak var nameTextField: UITextField!
    @IBOutlet weak var phoneTextField: UITextField!
    @IBOutlet weak var mailTextField: UITextField!
    @IBOutlet weak var addressTextField: UITextField!
    
    @IBOutlet weak var relationShipComboboxView: ComboboxView!
    @IBOutlet weak var provinceComboboxView: ComboboxView!
    @IBOutlet weak var districtComboboxView: ComboboxView!
    @IBOutlet weak var wardComboboxView: ComboboxView!
    
    @IBOutlet weak var maleGenderRadioButtonView: RadioButtonView!
    @IBOutlet weak var femaleGenderRadioButtonView: RadioButtonView!
    
    var genderRadioButtonViews: [RadioButtonView] = []
    var currentGenderRadioButtonView: RadioButtonView?
    
    let locationManager = CLLocationManager()
    
    weak var containerViewController: Step4ViewController?
    var viewModel = Step4ViewModel()
    
    var isEditMode: Bool = false
    
    override func setupView() {
        initGenderRadioButtonViews()
        setupComboboxView()
        setupEditModeIfNeeded()
        setupLocationManager()
    }
    
    override func updateFormToViewModel() -> Bool {
        guard let containerViewController = self.containerViewController else {
            return false
        }
        viewModel.type = containerViewController.isInvisibleCheckboxView.isSelected ? ReporterType.invisible : ReporterType.open
        viewModel.name = nameTextField.text ?? ""
        viewModel.gender = currentGenderRadioButtonView?.value ?? ""
        viewModel.phone = phoneTextField.text ?? ""
        viewModel.mail = mailTextField.text ?? ""
        viewModel.address = addressTextField.text ?? ""
        if let relationShip = viewModel.relationShipDataSource.get(index: relationShipComboboxView.selectedIndex) {
            viewModel.relationShip = relationShip.id
            viewModel.relationShipName = relationShip.value
        }
        if let province = viewModel.provinceDataSource.get(index: provinceComboboxView.selectedIndex) {
            viewModel.provinceId = province.id
            viewModel.provinceName = province.value
        }
        if let district = viewModel.districtDataSource.get(index: districtComboboxView.selectedIndex) {
            viewModel.districtId = district.id
            viewModel.districtName = district.value
        }
        if let ward = viewModel.wardDataSource.get(index: wardComboboxView.selectedIndex) {
            viewModel.wardId = ward.id
            viewModel.wardName = ward.value
        }
        
        let errorFormMessage = viewModel.errorFormMessage
        let isFormValid = errorFormMessage.isEmpty
        
        if isFormValid {
            viewModel.updateReporterModel()
        } else {
            showMessage(title: errorFormMessage)
        }
        
        return isFormValid
    }
    
    @IBAction func useCurrentLocation(_ sender: Any) {
        locationManager.requestWhenInUseAuthorization()
        if let isLocationPermissionGranted = LocationHelper.isLocationPermissionGranted(), !isLocationPermissionGranted {
            AlertController.shared.showConfirmMessage(message: Strings.locationEnableTitle, confirm: Strings.yes, cancel: Strings.no) { isConfirm in
                guard isConfirm else { return }
                LocationHelper.goToLocationSetting()
            }
        }
        locationManager.startUpdatingLocation()
    }
}

// MARK: - First setup
extension Step4TableViewController {
    private func setupEditModeIfNeeded() {
        guard isEditMode else { return }
        refreshFromReporterData()
    }
    
    private func setupLocationManager() {
        locationManager.delegate = self
        locationManager.desiredAccuracy = kCLLocationAccuracyBest
    }
    
    func refreshFromReporterData() {
        viewModel.updateValueFromAppData()
        bindFormFromViewModel()
    }
    
    private func initGenderRadioButtonViews() {
        genderRadioButtonViews = [maleGenderRadioButtonView,
                                  femaleGenderRadioButtonView]
        genderRadioButtonViews.forEach { [weak self] genderRadioButtonView in
            guard let self = self else { return }
            genderRadioButtonView.selectedCompletion = {
                guard self.currentGenderRadioButtonView != genderRadioButtonView else {
                    self.currentGenderRadioButtonView?.isSelected = true
                    return
                }
                self.currentGenderRadioButtonView?.isSelected = false
                self.currentGenderRadioButtonView = genderRadioButtonView
                log(self.currentGenderRadioButtonView?.value)
            }
        }
    }
    
    private func setupComboboxView() {
        relationShipComboboxView.config(with: viewModel.relationTitles)
        provinceComboboxView.config(with: viewModel.provinceTitles)
        provinceComboboxView.completion = { [weak self] selectedIndex in
            guard let self = self else { return }
            self.viewModel.setDistrictDataSource(provinceId: self.viewModel.provinceDataSource[selectedIndex].id)
            self.districtComboboxView.config(with: self.viewModel.districtTitles)
            self.viewModel.setWarDataSource(districtId: nil)
            self.wardComboboxView.config(with: self.viewModel.wardTitles)
        }
        districtComboboxView.config(with: viewModel.districtTitles)
        districtComboboxView.completion = { [weak self] selectedIndex in
            guard let self = self else { return }
            let selectedDistrictId = self.viewModel.districtDataSource[selectedIndex].id
            self.viewModel.setWarDataSource(districtId: selectedDistrictId)
            self.wardComboboxView.config(with: self.viewModel.wardTitles)
        }
    }
}

// MARK: - Location delegate
extension Step4TableViewController: CLLocationManagerDelegate {
    func locationManager(_ manager: CLLocationManager, didUpdateLocations locations: [CLLocation]) {
        guard let currentLocation = manager.location?.coordinate else { return }
        LocationHelper.geocode(latitude: currentLocation.latitude, longitude: currentLocation.longitude) { [weak self] (address, error) in
            guard let address = address, let self = self else {
                log(error?.localizedDescription)
                return
            }
            
            let (provinceId, districtId, wardId) = self.viewModel.getAreaIdFromName(provinceName: address.provinceName,
                                                                                    districtName: address.districtName,
                                                                                    wardName: address.wardName)
            
            self.setAddressFromLocation(address: address.name,
                                        provinceId: provinceId,
                                        districtId: districtId,
                                        wardId: wardId)
        }
        locationManager.stopUpdatingLocation()
    }
    
    private func setAddressFromLocation(address: String, provinceId: String, districtId: String, wardId: String) {
        addressTextField.text = address
        provinceComboboxView.config(with: viewModel.provinceTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.provinceDataSource, with: provinceId))
        viewModel.setDistrictDataSource(provinceId: provinceId)
        districtComboboxView.config(with: viewModel.districtTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.districtDataSource, with: districtId))
        viewModel.setWarDataSource(districtId: districtId)
        wardComboboxView.config(with: viewModel.wardTitles,
                                selectedIndex: Utilities.getIndex(from: viewModel.wardDataSource, with: wardId))
    }
}

// MARK: - Helpers
extension Step4TableViewController {
    private func bindFormFromViewModel() {
        containerViewController?.configInvisibleCheckboxView(isSelected: viewModel.type == ReporterType.invisible)
        nameTextField.text = viewModel.name
        addressTextField.text = viewModel.address
        phoneTextField.text = viewModel.phone
        mailTextField.text = viewModel.mail
        setCurrentRadioButtonView(&currentGenderRadioButtonView,
                                  radioButtonViews: genderRadioButtonViews,
                                  value: viewModel.gender)
        relationShipComboboxView.config(with: viewModel.relationTitles,
                                        selectedIndex: Utilities.getIndex(from: viewModel.relationShipDataSource,
                                                                          with: viewModel.relationShip))
        provinceComboboxView.config(with: viewModel.provinceTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.provinceDataSource,
                                                                      with: viewModel.provinceId))
        viewModel.setDistrictDataSource(provinceId: viewModel.provinceId)
        districtComboboxView.config(with: viewModel.districtTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.districtDataSource,
                                                                      with: viewModel.districtId))
        viewModel.setWarDataSource(districtId: viewModel.districtId)
        wardComboboxView.config(with: viewModel.wardTitles,
                                selectedIndex: Utilities.getIndex(from: viewModel.wardDataSource,
                                                                  with: viewModel.wardId))
    }
    
    private func setCurrentRadioButtonView(_ currentRadioButtonView: inout RadioButtonView?, radioButtonViews: [RadioButtonView], value: String) {
        radioButtonViews.forEach { view in
            view.isSelected = false
        }
        
        currentRadioButtonView = radioButtonViews.filter { $0.value == value }.first
        currentRadioButtonView?.isSelected = true
    }
}
