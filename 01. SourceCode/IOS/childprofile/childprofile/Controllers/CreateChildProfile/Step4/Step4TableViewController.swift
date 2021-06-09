//
//  Step4TableViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class Step4TableViewController: StepViewController {
    @IBOutlet weak var typeHousingCheckboxListView: CheckboxListView!
    @IBOutlet weak var typeHousingOtherTextField: UITextField!
    @IBOutlet weak var roofMaterialCheckboxListView: CheckboxListView!
    @IBOutlet weak var roofMaterialOtherTextField: UITextField!
    @IBOutlet weak var wallMaterialCheckboxListView: CheckboxListView!
    @IBOutlet weak var wallMaterialsOtherTextField: UITextField!
    @IBOutlet weak var floorMaterialCheckboxListView: CheckboxListView!
    @IBOutlet weak var floorMaterialsOtherTextField: UITextField!
    @IBOutlet weak var isElectricityCheckboxListView: CheckboxListView!
    @IBOutlet weak var schoolMetCheckboxListView: CheckboxListView!
    @IBOutlet weak var clinicMetCheckboxListView: CheckboxListView!
    @IBOutlet weak var waterSourceMetCheckboxListView: CheckboxListView!
    @IBOutlet weak var waterFromCheckboxListView: CheckboxListView!
    @IBOutlet weak var waterOtherTextField: UITextField!
    @IBOutlet weak var roadConditionsCheckboxListView: CheckboxListView!
    @IBOutlet weak var incomeFamilyTextFieldListView: TextFieldListView!
    @IBOutlet weak var harvestOutputTextFieldListView: TextFieldListView!
    @IBOutlet weak var petTextField: UITextField!
    @IBOutlet weak var totalIncomeTextField: UITextField!
    @IBOutlet weak var familyTypeTextField: UITextField!
    @IBOutlet weak var incomeOtherCheckboxListView: CheckboxListView!
    
    weak var containerViewController: CreateChildProfileViewController?
    private let viewModel = Step4ViewModel()
    
    override func setupView() {
        setDataCheckboxListView()
    }
    
    override func refreshView() {
        containerViewController?.setupTitleLabel(Strings.step4Title.uppercased())
    }
    
    override func updateFormToViewModel() -> Bool {
        viewModel.typeHousingOther = typeHousingOtherTextField.text ?? ""
        viewModel.roofMaterialOther = roofMaterialOtherTextField.text ?? ""
        viewModel.wallMaterialsOther = wallMaterialsOtherTextField.text ?? ""
        viewModel.floorMaterialsOther = floorMaterialsOtherTextField.text ?? ""
        viewModel.waterOther = waterOtherTextField.text ?? ""
        viewModel.pet = petTextField.text ?? ""
        viewModel.totalIncome = totalIncomeTextField.text ?? ""
        viewModel.familyType = familyTypeTextField.text ?? ""
        
        return true
    }
    
    func getViewModel() -> Step4ViewModel {
        return viewModel
    }
}

// MARK: - Setup
extension Step4TableViewController {
    private func setDataCheckboxListView() {
        typeHousingCheckboxListView.setDataSource(viewModel.listTypeHousing)
        roofMaterialCheckboxListView.setDataSource(viewModel.listRoofMaterial)
        wallMaterialCheckboxListView.setDataSource(viewModel.listWallMaterial)
        floorMaterialCheckboxListView.setDataSource(viewModel.listFloorMaterial)
        isElectricityCheckboxListView.setDataSource(viewModel.listIsElectricity)
        schoolMetCheckboxListView.setDataSource(viewModel.listSchoolMet)
        clinicMetCheckboxListView.setDataSource(viewModel.listClinicMet)
        waterSourceMetCheckboxListView.setDataSource(viewModel.listWaterSourceMet)
        waterFromCheckboxListView.setDataSource(viewModel.listWaterFrom)
        roadConditionsCheckboxListView.setDataSource(viewModel.listRoadCondition)
        incomeFamilyTextFieldListView.setDataSource(viewModel.listIncomeFamily)
        harvestOutputTextFieldListView.setDataSource(viewModel.listHarvestOutput)
        incomeOtherCheckboxListView.setDataSource(viewModel.listIncomeOther)
    }
}

// MARK: - Helpers
extension Step4TableViewController {
    func setupViewModelFromParent(_ parentViewModel: CreateChildProfileViewModel) {
        guard let model = parentViewModel.childProfileModel else {
            return
        }
        
        viewModel.listTypeHousing = model.houseTypeModel?.listObject ?? []
        viewModel.listRoofMaterial = model.houseRoofModel?.listObject ?? []
        viewModel.listWallMaterial = model.houseWallModel?.listObject ?? []
        viewModel.listFloorMaterial = model.houseFloorModel?.listObject ?? []
        viewModel.listIsElectricity = model.useElectricityModel?.listObject ?? []
        viewModel.listSchoolMet = model.schoolDistanceModel?.listObject ?? []
        viewModel.listClinicMet = model.clinicDistanceModel?.listObject ?? []
        viewModel.listWaterSourceMet = model.waterSourceDistanceModel?.listObject ?? []
        viewModel.listWaterFrom = model.waterSourceUseModel?.listObject ?? []
        viewModel.listRoadCondition = model.roadConditionModel?.listObject ?? []
        viewModel.listIncomeFamily = model.incomeFamilyModel?.listObject ?? []
        viewModel.listHarvestOutput = model.harvestOutputModel?.listObject ?? []
        viewModel.listIncomeOther = model.incomeOtherModel?.listObject ?? []
        
        if parentViewModel.isEditMode {
            setupEditMode(model)
        }
    }
    
    private func setupEditMode(_ model: ChildProfileModel) {
        viewModel.typeHousingOther = model.houseTypeModel?.otherValue ?? ""
        viewModel.roofMaterialOther = model.houseRoofModel?.otherValue ?? ""
        viewModel.wallMaterialsOther = model.houseWallModel?.otherValue ?? ""
        viewModel.floorMaterialsOther = model.houseFloorModel?.otherValue ?? ""
        viewModel.waterOther = model.waterSourceUseModel?.otherValue ?? ""
        viewModel.familyType = model.familyType
        viewModel.pet = model.numberPet
        viewModel.totalIncome = model.totalIncome
        
        _ = view
        typeHousingOtherTextField.text = viewModel.typeHousingOther
        roofMaterialOtherTextField.text = viewModel.roofMaterialOther
        wallMaterialsOtherTextField.text = viewModel.wallMaterialsOther
        floorMaterialsOtherTextField.text = viewModel.floorMaterialsOther
        waterOtherTextField.text = viewModel.waterOther
        petTextField.text = viewModel.pet
        totalIncomeTextField.text = viewModel.totalIncome
        familyTypeTextField.text = viewModel.familyType
    }
}
