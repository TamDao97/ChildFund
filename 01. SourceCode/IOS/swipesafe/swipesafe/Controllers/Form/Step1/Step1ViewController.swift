
//
//  Step1ViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import CoreLocation

class Step1ViewController: StepViewController {
    weak var contentViewController: Step1TableViewController?
    var updateIndex: Int?
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        guard segue.identifier == Step1TableViewController.className else { return }
        contentViewController = segue.destination as? Step1TableViewController
        contentViewController?.updateIndex = updateIndex
    }
    
    override func updateFormToViewModel() -> Bool {
        return contentViewController?.updateFormToViewModel() ?? false
    }
}

class Step1TableViewController: StepTableViewController {
    // MARK: - IBOutlet
    @IBOutlet weak var listChildView: UIView!
    
    @IBOutlet weak var nameTextField: UITextField!
    @IBOutlet weak var birthdayDatePickerView: DatePickerView!
    @IBOutlet weak var ageTextField: UITextField!
    @IBOutlet weak var addressTextField: UITextField!
    @IBOutlet weak var dateActionDatePickerView: DatePickerView!
    
    @IBOutlet weak var maleGenderRadioButtonView: RadioButtonView!
    @IBOutlet weak var femaleGenderRadioButtonView: RadioButtonView!
    @IBOutlet weak var unknownGenderRadioButtonView: RadioButtonView!
    
    @IBOutlet weak var firstLevelRadioButtonView: RadioButtonView!
    @IBOutlet weak var secondLevelRadioButtonView: RadioButtonView!
    @IBOutlet weak var thirdLevelRadioButtonView: RadioButtonView!
    
    @IBOutlet weak var provinceComboboxView: ComboboxView!
    @IBOutlet weak var districtComboboxView: ComboboxView!
    @IBOutlet weak var wardComboboxView: ComboboxView!
    
    @IBOutlet weak var childAbuseView: UIView!
    @IBOutlet weak var childAbuseTableViewHeightConstraint: NSLayoutConstraint!
    @IBOutlet weak var childAbuseTableViewCell: UITableViewCell!
    
    var childAbuseCellIndexPath = IndexPath(row: 11, section: 0)
    var listAbuseCheckboxTableViewController: CheckboxTableViewController!
    
    var listChildCellIndexPath = IndexPath(row: 0, section: 0)
    var infoUpdateTableViewController: InfoUpdateTableViewController!
    
    var insertCellIndexPath = IndexPath(row: 12, section: 0)
    
    var genderRadioButtonViews: [RadioButtonView] = []
    var currentGenderRadioButtonView: RadioButtonView?
    
    var levelRadioButtonViews: [RadioButtonView] = []
    var currentLevelRadioButtonView: RadioButtonView?
    
    let locationManager = CLLocationManager()
    
    // Edit mode
    var updateIndex: Int?
    var isEditMode: Bool {
        return updateIndex != nil
    }
    
    var viewModel = Step1ViewModel()
    
    override func setupView() {
        initGenderRadioButtonViews()
        initLevelRadioButtonViews()
        setupComboboxView()
        setupChildAbuseView()
        setupListChildView()
        setupLocationManager()
        setupDatePickerView()
    }
    
    override func refreshView() {
        refreshViewModelValue()
    }
    
    override func updateFormToViewModel() -> Bool {
        viewModel.name = nameTextField.text ?? ""
        viewModel.gender = currentGenderRadioButtonView?.value ?? ""
        viewModel.birthday = birthdayDatePickerView.serverDateString ?? ""
        viewModel.age = Int(ageTextField.text ?? "")
        viewModel.level = currentLevelRadioButtonView?.value ?? ""
        viewModel.address = addressTextField.text ?? ""
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
        viewModel.dateAction = dateActionDatePickerView.serverDateString ?? ""
        
        let errorFormMessage = viewModel.errorFormMessage
        let isFormValid = errorFormMessage.isEmpty
        
        if isFormValid {
            viewModel.updateChildReportModel()
            reloadChildInfoView()
        } else {
            showMessage(title: errorFormMessage)
        }
        
        return isFormValid
    }
    
    @IBAction func addNewChild(_ sender: Any) {
        guard updateFormToViewModel() else { return }
        resetForm()
        tableView.scrollToTop()
    }
    
    @IBAction func useCurrentLocation(_ sender: Any) {
        locationManager.startUpdatingLocation()
        guard let isLocationPermissonGrandted = LocationHelper.isLocationPermissionGranted() else {
            locationManager.requestWhenInUseAuthorization()
            return
        }
        guard isLocationPermissonGrandted else {
            AlertController.shared.showConfirmMessage(message: Strings.locationEnableTitle, confirm: Strings.yes, cancel: Strings.no) { isConfirm in
                guard isConfirm else { return }
                LocationHelper.goToLocationSetting()
            }
            return
        }
    }
    
    @IBAction func ageTextFieldChanged(_ sender: Any) {
        if let ageString = ageTextField.text,
            let age = Int(ageString),
            age > Constants.maximumChildAge {
            showMessage(title: Strings.Step1.ageOver18ErrorTitle)
            ageTextField.text = "\(Constants.maximumChildAge)"
        }
    }
}

// MARK: - Location delegate
extension Step1TableViewController: CLLocationManagerDelegate {
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

// MARK: - First setup
extension Step1TableViewController {
    private func refreshViewModelValue() {
        if let index = updateIndex  {
            viewModel.updateValue(by: index)
        } else {
            viewModel.resetToFirstChild()
        }
        bindFormFromViewModel()
    }
    
    private func setupLocationManager() {
        locationManager.delegate = self
        locationManager.desiredAccuracy = kCLLocationAccuracyBest
    }
    
    private func initGenderRadioButtonViews() {
        genderRadioButtonViews = [maleGenderRadioButtonView,
                                  femaleGenderRadioButtonView,
                                  unknownGenderRadioButtonView]
        genderRadioButtonViews.forEach { [weak self] genderRadioButtonView in
            guard let self = self else { return }
            genderRadioButtonView.selectedCompletion = {
                guard self.currentGenderRadioButtonView != genderRadioButtonView else {
                    self.currentGenderRadioButtonView?.isSelected = true
                    return
                }
                self.currentGenderRadioButtonView?.isSelected = false
                self.currentGenderRadioButtonView = genderRadioButtonView
            }
        }
    }
    
    private func initLevelRadioButtonViews() {
        levelRadioButtonViews = [firstLevelRadioButtonView,
                                 secondLevelRadioButtonView,
                                 thirdLevelRadioButtonView]
        levelRadioButtonViews.forEach { [weak self] levelRadioButtonView in
            guard let self = self else { return }
            levelRadioButtonView.selectedCompletion = {
                guard self.currentLevelRadioButtonView != levelRadioButtonView else {
                    self.currentLevelRadioButtonView?.isSelected = true
                    return
                }
                self.currentLevelRadioButtonView?.isSelected = false
                self.currentLevelRadioButtonView = levelRadioButtonView
            }
        }
    }
    
    private func setupComboboxView() {
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
    
    private func setupChildAbuseView() {
        listAbuseCheckboxTableViewController = CheckboxTableViewController(childAbuseModels: viewModel.childAbuseDataSource)
        add(childViewController: listAbuseCheckboxTableViewController, containerView: childAbuseView)
        childAbuseTableViewHeightConstraint.constant = listAbuseCheckboxTableViewController.contentHeight + 24
    }
    
    private func setupListChildView() {
        infoUpdateTableViewController = InfoUpdateTableViewController(infoCellViewModels: viewModel.childInfoCellViewModels)
        infoUpdateTableViewController.updateHandler = { [weak self] index in
            guard
                let self = self,
                self.updateFormToViewModel()
            else { return }
            self.viewModel.updateValue(by: index)
            self.bindFormFromViewModel()
        }
        
        infoUpdateTableViewController.removeHandler = { [weak self] index in
            guard let self = self else { return }
            self.viewModel.removeChildModelFromAppData(at: index)
            self.resetForm()
            self.reloadChildInfoView()
        }
        
        add(childViewController: infoUpdateTableViewController, containerView: listChildView)
    }
    
    private func setupDatePickerView() {
        birthdayDatePickerView.completion = { [weak self] in
            guard let self = self else { return }
            
            let currentDate = Date()
            
            if let actionDate = self.dateActionDatePickerView.date,
                let birthdate = self.birthdayDatePickerView.date,
                birthdate >= actionDate {
                self.birthdayDatePickerView.updateDate(nil)
                self.ageTextField.isEnabled = true
                self.ageTextField.text = ""
                self.showMessage(title: "Ngày sinh không đúng.")
            }
            
            if let birthdate = self.birthdayDatePickerView.date {
                self.ageTextField.text = "\(currentDate.year - birthdate.year)"
                self.ageTextField.isEnabled = false
            }
        }
        
        dateActionDatePickerView.completion = { [weak self] in
            guard let self = self else { return }
            
            if let actionDate = self.dateActionDatePickerView.date,
                let birthdate = self.birthdayDatePickerView.date,
                birthdate >= actionDate {
                self.dateActionDatePickerView.updateDate(nil)
                self.showMessage(title: "Thời gian xảy ra không đúng.")
            }
        }
    }
}

// MARK: - Helpers
extension Step1TableViewController {
    private func bindFormFromViewModel() {
        nameTextField.text = viewModel.name
        birthdayDatePickerView.config(with: DateHelper.dateFromShortDate(viewModel.birthday),
                                      minimumDate: Date().subtract(years: Constants.maximumChildAge).startOfDay,
                                      maximumDate: Date())
        ageTextField.text = viewModel.age != nil ? "\(viewModel.age!)" : ""
        addressTextField.text = viewModel.address
        dateActionDatePickerView.config(with: DateHelper.dateFromString(viewModel.dateAction, with: DateFormatString.serverDateFormat),
                                        maximumDate: Date())
        setCurrentRadioButtonView(&currentGenderRadioButtonView,
                                  radioButtonViews: genderRadioButtonViews,
                                  value: viewModel.gender)
        setCurrentRadioButtonView(&currentLevelRadioButtonView,
                                  radioButtonViews: levelRadioButtonViews,
                                  value: viewModel.level)
        provinceComboboxView.config(with: viewModel.provinceTitles,
                                  selectedIndex: Utilities.getIndex(from: viewModel.provinceDataSource, with: viewModel.provinceId))
        viewModel.setDistrictDataSource(provinceId: viewModel.provinceId)
        districtComboboxView.config(with: viewModel.districtTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.districtDataSource, with: viewModel.districtId))
        viewModel.setWarDataSource(districtId: viewModel.districtId)
        wardComboboxView.config(with: viewModel.wardTitles,
                                selectedIndex: Utilities.getIndex(from: viewModel.wardDataSource, with: viewModel.wardId))
        listAbuseCheckboxTableViewController.reload(with: viewModel.childAbuseDataSource)
    }
    
    private func setCurrentRadioButtonView(_ currentRadioButtonView: inout RadioButtonView?, radioButtonViews: [RadioButtonView], value: String) {
        radioButtonViews.forEach { view in
            view.isSelected = false
        }
        
        currentRadioButtonView = radioButtonViews.filter { $0.value == value }.first
        currentRadioButtonView?.isSelected = true
    }
    
    private func reloadChildInfoView() {
        infoUpdateTableViewController.reloadData(with: viewModel.childInfoCellViewModels)
        tableView.reloadRows(at: [listChildCellIndexPath], with: UITableView.RowAnimation.none)
    }
    
    private func resetForm() {
        viewModel = Step1ViewModel()
        viewModel.updateNextChildIndex()
        bindFormFromViewModel()
    }
}

// MARK: - Tableview Delegates
extension Step1TableViewController {
    override func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        switch indexPath {
        case listChildCellIndexPath:
            if isEditMode {
                return 0
            }
            return viewModel.childInfoViewIsHidden ? 0 : infoUpdateTableViewController.contentHeight + 4.5
        case childAbuseCellIndexPath:
            return 232
        case insertCellIndexPath:
            return isEditMode ? 0 : super.tableView(tableView, heightForRowAt: indexPath)
        default:
            return super.tableView(tableView, heightForRowAt: indexPath)
        }
    }
}
