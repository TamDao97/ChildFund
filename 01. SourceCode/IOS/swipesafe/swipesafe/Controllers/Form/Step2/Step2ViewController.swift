
//
//  Step1ViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import CoreLocation

class Step2ViewController: StepViewController {
    weak var contentViewController: Step2TableViewController?
    var updateIndex: Int?
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        guard segue.identifier == Step2TableViewController.className else { return }
        contentViewController = segue.destination as? Step2TableViewController
        contentViewController?.updateIndex = updateIndex
    }
    
    override func updateFormToViewModel() -> Bool {
        return contentViewController?.updateFormToViewModel() ?? false
    }
}

class Step2TableViewController: StepTableViewController {
    // MARK: - IBOutlet
    @IBOutlet weak var listPrisonerView: UIView!
    
    @IBOutlet weak var nameTextField: UITextField!
    @IBOutlet weak var birthdayDatePickerView: DatePickerView!
    @IBOutlet weak var ageTextField: UITextField!
    @IBOutlet weak var phoneTextField: UITextField!
    @IBOutlet weak var addressTextField: UITextField!
    
    @IBOutlet weak var relationShipComboboxView: ComboboxView!
    @IBOutlet weak var provinceComboboxView: ComboboxView!
    @IBOutlet weak var districtComboboxView: ComboboxView!
    @IBOutlet weak var wardComboboxView: ComboboxView!
    
    @IBOutlet weak var maleGenderRadioButtonView: RadioButtonView!
    @IBOutlet weak var femaleGenderRadioButtonView: RadioButtonView!
    @IBOutlet weak var unknownGenderRadioButtonView: RadioButtonView!
    
    var listPrisonerCellIndexPath = IndexPath(row: 0, section: 0)
    var insertCellIndexPath = IndexPath(row: 12, section: 0)
    var infoUpdateTableViewController: InfoUpdateTableViewController!
    
    var genderRadioButtonViews: [RadioButtonView] = []
    var currentGenderRadioButtonView: RadioButtonView?
    
    let locationManager = CLLocationManager()
    
    var childComboboxView = ComboboxView()
    var viewModel = Step2ViewModel()
    
    // Edit mode
    var updateIndex: Int?
    var isEditMode: Bool {
        return updateIndex != nil
    }
    
    override func setupView() {
        initGenderRadioButtonViews()
        setupComboboxView()
        setupListPrisonerView()
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
        viewModel.phone = phoneTextField.text ?? ""
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
            viewModel.updateReportPrisonerModel()
            reloadPrisonerInfoView()
        } else {
            showMessage(title: errorFormMessage)
        }
        
        return isFormValid
    }
    
    @IBAction func ageTextFieldChanged(_ sender: Any) {
        if let ageString = ageTextField.text,
            let age = Int(ageString),
            age > Constants.maximumChildAge {
            showMessage(title: Strings.Step1.ageOver18ErrorTitle)
            ageTextField.text = "\(Constants.maximumChildAge)"
        }
    }
    
    @IBAction func addNewPrisoner(_ sender: Any) {
        guard updateFormToViewModel() else { return }
        resetForm()
        tableView.scrollToTop()
    }
    
    @IBAction func configMatchChildAddressAction(_ sender: Any) {
        configMatchChildAddress()
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
extension Step2TableViewController {
    private func refreshViewModelValue() {
        if let index = updateIndex  {
            viewModel.updateValue(by: index)
        } else {
            viewModel.resetToFirstPrisoner()
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
        childComboboxView.config(with: viewModel.childTitles)
        childComboboxView.completion = { [weak self] selectedIndex in
            guard let self = self else { return }
            self.setAddressFromChild(self.viewModel.childs.get(index: selectedIndex))
        }
    }
    
    private func configMatchChildAddress() {
        let childs = self.viewModel.childs
        guard childs.count > 1 else {
            self.setAddressFromChild(self.viewModel.childs.get(index: 0))
            return
        }
        self.childComboboxView.config(with: self.viewModel.childTitles)
        self.childComboboxView.show()
    }
    
    private func setupListPrisonerView() {
        infoUpdateTableViewController = InfoUpdateTableViewController(infoCellViewModels: viewModel.prisonerInfoCellViewModels)
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
            self.viewModel.removePrisonerModelFromAppData(at: index)
            self.resetForm()
            self.reloadPrisonerInfoView()
        }
        
        add(childViewController: infoUpdateTableViewController, containerView: listPrisonerView)
    }
    
    private func setupDatePickerView() {
        birthdayDatePickerView.completion = { [weak self] in
            guard let self = self else { return }
            
            let currentDate = Date()
            
            if let birthdate = self.birthdayDatePickerView.date {
                self.ageTextField.text = "\(currentDate.year - birthdate.year)"
                self.ageTextField.isEnabled = false
            }
        }
    }
}

// MARK: - Location delegate
extension Step2TableViewController: CLLocationManagerDelegate {
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
extension Step2TableViewController {
    private func bindFormFromViewModel() {
        nameTextField.text = viewModel.name
        birthdayDatePickerView.config(with: DateHelper.dateFromShortDate(viewModel.birthday), maximumDate: Date())
        ageTextField.text = viewModel.age != nil ? "\(viewModel.age!)" : ""
        addressTextField.text = viewModel.address
        phoneTextField.text = viewModel.phone
        setCurrentRadioButtonView(&currentGenderRadioButtonView,
                                  radioButtonViews: genderRadioButtonViews,
                                  value: viewModel.gender)
        relationShipComboboxView.config(with: viewModel.relationTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.relationShipDataSource, with: viewModel.relationShip))
        provinceComboboxView.config(with: viewModel.provinceTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.provinceDataSource, with: viewModel.provinceId))
        viewModel.setDistrictDataSource(provinceId: viewModel.provinceId)
        districtComboboxView.config(with: viewModel.districtTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.districtDataSource, with: viewModel.districtId))
        viewModel.setWarDataSource(districtId: viewModel.districtId)
        wardComboboxView.config(with: viewModel.wardTitles,
                                selectedIndex: Utilities.getIndex(from: viewModel.wardDataSource, with: viewModel.wardId))
    }
    
    private func setAddressFromChild(_ childModel: ChildModel?) {
        guard let model = childModel else { return }
        addressTextField.text = model.address
        provinceComboboxView.config(with: viewModel.provinceTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.provinceDataSource, with: model.provinceId))
        viewModel.setDistrictDataSource(provinceId: model.provinceId)
        districtComboboxView.config(with: viewModel.districtTitles,
                                    selectedIndex: Utilities.getIndex(from: viewModel.districtDataSource, with: model.districtId))
        viewModel.setWarDataSource(districtId: model.districtId)
        wardComboboxView.config(with: viewModel.wardTitles,
                                selectedIndex: Utilities.getIndex(from: viewModel.wardDataSource, with: model.wardId))
    }
    
    private func setCurrentRadioButtonView(_ currentRadioButtonView: inout RadioButtonView?, radioButtonViews: [RadioButtonView], value: String) {
        radioButtonViews.forEach { view in
            view.isSelected = false
        }
        
        currentRadioButtonView = radioButtonViews.filter { $0.value == value }.first
        currentRadioButtonView?.isSelected = true
    }
    
    private func reloadPrisonerInfoView() {
        infoUpdateTableViewController.reloadData(with: viewModel.prisonerInfoCellViewModels)
        tableView.reloadRows(at: [listPrisonerCellIndexPath], with: UITableView.RowAnimation.none)
    }
    
    private func resetForm() {
        viewModel = Step2ViewModel()
        viewModel.updateNextPrisonerIndex()
        bindFormFromViewModel()
    }
}

// MARK: - Tableview Delegates
extension Step2TableViewController {
    override func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        switch indexPath {
        case listPrisonerCellIndexPath:
            if isEditMode {
                return 0
            }
            return viewModel.prisonerInfoViewIsHidden ? 0 : infoUpdateTableViewController.contentHeight + 4.5
        case insertCellIndexPath:
            return isEditMode ? 0 : super.tableView(tableView, heightForRowAt: indexPath)
        default:
            return super.tableView(tableView, heightForRowAt: indexPath)
        }
    }
}
