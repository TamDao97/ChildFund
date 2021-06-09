//
//  Step2TableViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class Step2TableViewController: StepViewController {
    @IBOutlet weak var childNameTextField: UITextField!
    @IBOutlet weak var nickNameTextField: UITextField!
    @IBOutlet weak var genderSegmentedControl: UISegmentedControl!
    @IBOutlet weak var dateOfBirthDatePickerView: DatePickerView!
    @IBOutlet weak var childhoodCheckboxView: CheckboxView!
    @IBOutlet weak var outSchoolCheckboxView: CheckboxView!
    @IBOutlet weak var handicapCheckboxView: CheckboxView!
    @IBOutlet weak var preSchoolCheckboxView: CheckboxView!
    @IBOutlet weak var primarySchoolCheckboxView: CheckboxView!
    @IBOutlet weak var primaryClassComboboxView: ComboboxView!
    @IBOutlet weak var secondarySchoolCheckboxView: CheckboxView!
    @IBOutlet weak var secondaryClassComboboxView: ComboboxView!
    @IBOutlet weak var subjectCheckboxListView: CheckboxListView!
    @IBOutlet weak var differentSubjectTextField: UITextField!
    @IBOutlet weak var bestSubjectTextField: UITextField!
    @IBOutlet weak var leaningCapacityCheckboxListView: CheckboxListView!
    @IBOutlet weak var achievementTextField: UITextField!
    @IBOutlet weak var houseWorkCheckboxListView: CheckboxListView!
    @IBOutlet weak var workOtherTextField: UITextField!
    @IBOutlet weak var healthCheckboxListView: CheckboxListView!
    @IBOutlet weak var healthOtherTextField: UITextField!
    @IBOutlet weak var personalityCheckboxListView: CheckboxListView!
    @IBOutlet weak var personalityOtherTextField: UITextField!
    @IBOutlet weak var hobbieCheckboxListView: CheckboxListView!
    @IBOutlet weak var hobbieOtherTextField: UITextField!
    @IBOutlet weak var dreamCheckboxListView: CheckboxListView!
    @IBOutlet weak var dreamOtherTextField: UITextField!
    
    @IBOutlet weak var childNameErrorLabel: UILabel!
    @IBOutlet weak var dateOfBirthErrorLabel: UILabel!
    @IBOutlet weak var learningStatusErrorLabel: UILabel!
    
    var currentLearningStatusCheckBoxView: CheckboxView?
    var learningStatusCheckBoxViews: [CheckboxView] = []
    
    weak var containerViewController: CreateChildProfileViewController?
    private let viewModel = Step2ViewModel()
    
    override func setupView() {
        setLearningStatusCheckboxView()
        setDataCheckboxListView()
        setupComboboxView()
    }
    
    override func refreshView() {
        containerViewController?.setupTitleLabel(Strings.step2Title.uppercased())
    }
    
    override func updateFormToViewModel() -> Bool {
        viewModel.childName = childNameTextField.text ?? ""
        viewModel.nickName = nickNameTextField.text ?? ""
        viewModel.gender = genderSegmentedControl.selectedSegmentIndex == 0 ? 1 :0
        viewModel.dateOfBirth = dateOfBirthDatePickerView.serverDateString ?? ""
        viewModel.learningStatus = currentLearningStatusCheckBoxView?.result ?? ""
        if viewModel.learningStatus == "\(LearningStatus.primarySchool.rawValue)" {
            viewModel.classInfo = viewModel.primaryClassDataSource.get(index: primaryClassComboboxView.selectedIndex)?.id ?? ""
        }
        if viewModel.learningStatus == "\(LearningStatus.secondarySchool.rawValue)" {
            viewModel.classInfo = viewModel.secondaryClassDataSource.get(index: secondaryClassComboboxView.selectedIndex)?.id ?? ""
        }
        viewModel.differentSubject = differentSubjectTextField.text ?? ""
        viewModel.bestSubject = bestSubjectTextField.text ?? ""
        viewModel.achievement = achievementTextField.text ?? ""
        viewModel.workOther = workOtherTextField.text ?? ""
        viewModel.healthOther = healthOtherTextField.text ?? ""
        viewModel.personalityOther = personalityOtherTextField.text ?? ""
        viewModel.hobbieOther = hobbieOtherTextField.text ?? ""
        viewModel.dreamOther = dreamOtherTextField.text ?? ""
        
        childNameErrorLabel.text = viewModel.childNameErrorTitle
        dateOfBirthErrorLabel.text = viewModel.dateOfBirthErrorTitle
        learningStatusErrorLabel.text = viewModel.learningStatusErrorTitle
        
        return viewModel.isFormValid
    }
    
    func getViewModel() -> Step2ViewModel {
        return viewModel
    }
}

// MARK: - Setup
extension Step2TableViewController {
    private func setLearningStatusCheckboxView() {
        childhoodCheckboxView.value = "\(LearningStatus.childhood.rawValue)"
        outSchoolCheckboxView.value = "\(LearningStatus.outSchool.rawValue)"
        handicapCheckboxView.value = "\(LearningStatus.handicap.rawValue)"
        preSchoolCheckboxView.value = "\(LearningStatus.preSchool.rawValue)"
        primarySchoolCheckboxView.value = "\(LearningStatus.primarySchool.rawValue)"
        secondarySchoolCheckboxView.value = "\(LearningStatus.secondarySchool.rawValue)"
        
        learningStatusCheckBoxViews = [
            childhoodCheckboxView,
            outSchoolCheckboxView,
            handicapCheckboxView,
            preSchoolCheckboxView,
            primarySchoolCheckboxView,
            secondarySchoolCheckboxView
        ]
        
        learningStatusCheckBoxViews.forEach { [weak self] checkboxView in
            guard let self = self else { return }
            checkboxView.selectedCompletion = {
                guard self.currentLearningStatusCheckBoxView != checkboxView else {
                    self.currentLearningStatusCheckBoxView?.isSelected = true
                    return
                }
                
                self.currentLearningStatusCheckBoxView?.isSelected = false
                self.currentLearningStatusCheckBoxView = checkboxView
                
                if checkboxView != self.primarySchoolCheckboxView && checkboxView != self.secondarySchoolCheckboxView {
                    self.primaryClassComboboxView.isEnable = false
                    self.secondaryClassComboboxView.isEnable = false
                    self.primaryClassComboboxView.refresh()
                    self.secondaryClassComboboxView.refresh()
                    return
                }
                
                if checkboxView == self.primarySchoolCheckboxView {
                    self.primaryClassComboboxView.show()
                    self.primaryClassComboboxView.isEnable = true
                    self.secondaryClassComboboxView.isEnable = false
                    self.secondaryClassComboboxView.refresh()
                    return
                }
                
                if checkboxView == self.secondarySchoolCheckboxView {
                    self.secondaryClassComboboxView.show()
                    self.primaryClassComboboxView.isEnable = false
                    self.secondaryClassComboboxView.isEnable = true
                    self.primaryClassComboboxView.refresh()
                    return
                }
            }
        }
    }
    
    private func setDataCheckboxListView() {
        subjectCheckboxListView.setDataSource(viewModel.listSubject)
        leaningCapacityCheckboxListView.setDataSource(viewModel.listLearningCapacity)
        houseWorkCheckboxListView.setDataSource(viewModel.listHouseWork)
        healthCheckboxListView.setDataSource(viewModel.listHealth)
        personalityCheckboxListView.setDataSource(viewModel.listPersonality)
        hobbieCheckboxListView.setDataSource(viewModel.listHobbie)
        dreamCheckboxListView.setDataSource(viewModel.listDream)
    }
    
    private func setupComboboxView() {
        primaryClassComboboxView.config(with: viewModel.primaryClassDataSource.map { $0.value }, selectedIndex: -1, isEnable: false)
        secondaryClassComboboxView.config(with: viewModel.secondaryClassDataSource.map { $0.value }, selectedIndex: -1, isEnable: false)
    }
}

// MARK: - Get info
extension Step2TableViewController {
    func setupViewModelFromParent(_ parentViewModel: CreateChildProfileViewModel) {
        guard let model = parentViewModel.childProfileModel else {
            return
        }
        
        viewModel.listSubject = model.favouriteSubjectModel?.listObject ?? []
        viewModel.listLearningCapacity = model.learningCapacityModel?.listObject ?? []
        viewModel.listHouseWork = model.houseworkModel?.listObject ?? []
        viewModel.listHealth = model.healthModel?.listObject ?? []
        viewModel.listPersonality = model.personalityModel?.listObject ?? []
        viewModel.listHobbie = model.hobbyModel?.listObject ?? []
        viewModel.listDream = model.dreamModel?.listObject ?? []
        
        if parentViewModel.isEditMode {
            setupEditMode(model)
        }
    }
    
    private func setupEditMode(_ model: ChildProfileModel) {
        viewModel.childName = model.name
        viewModel.nickName = model.nickName
        viewModel.gender = model.gender
        viewModel.dateOfBirth = DateHelper.stringFromServerDateToShortDate(model.dateOfBirth)
        viewModel.learningStatus = model.leaningStatus
        viewModel.classInfo = model.classInfo
        viewModel.differentSubject = model.favouriteSubjectModel?.otherValue ?? ""
        viewModel.bestSubject = model.favouriteSubjectModel?.otherValue2 ?? ""
        viewModel.achievement = model.learningCapacityModel?.otherValue ?? ""
        viewModel.workOther = model.houseworkModel?.otherValue ?? ""
        viewModel.healthOther = model.healthModel?.otherValue ?? ""
        viewModel.personalityOther = model.personalityModel?.otherValue ?? ""
        viewModel.hobbieOther = model.hobbyModel?.otherValue ?? ""
        viewModel.dreamOther = model.dreamModel?.otherValue ?? ""
        
        _ = view
        genderSegmentedControl.selectedSegmentIndex = viewModel.gender == 0 ? 1 : 0
        dateOfBirthDatePickerView.config(with: DateHelper.dateFromShortDate(viewModel.dateOfBirth))
        childNameTextField.text = viewModel.childName
        nickNameTextField.text = viewModel.nickName
        differentSubjectTextField.text = viewModel.differentSubject
        bestSubjectTextField.text = viewModel.bestSubject
        achievementTextField.text = viewModel.achievement
        workOtherTextField.text = viewModel.workOther
        healthOtherTextField.text = viewModel.healthOther
        personalityOtherTextField.text = viewModel.personalityOther
        hobbieOtherTextField.text = viewModel.hobbieOther
        dreamOtherTextField.text = viewModel.dreamOther
        
        currentLearningStatusCheckBoxView = learningStatusCheckBoxViews.first(where: { $0.value == viewModel.learningStatus})
        currentLearningStatusCheckBoxView?.isSelected = true
        if currentLearningStatusCheckBoxView == primarySchoolCheckboxView {
            primaryClassComboboxView.config(with: viewModel.primaryClassDataSource.map { $0.value },
                                    selectedIndex: Utilities.getIndex(from: viewModel.primaryClassDataSource, with: viewModel.classInfo))
        }
        
        if currentLearningStatusCheckBoxView == secondarySchoolCheckboxView {
            secondaryClassComboboxView.config(with: viewModel.secondaryClassDataSource.map { $0.value },
                                            selectedIndex: Utilities.getIndex(from: viewModel.secondaryClassDataSource, with: viewModel.classInfo))
        }
    }
}


