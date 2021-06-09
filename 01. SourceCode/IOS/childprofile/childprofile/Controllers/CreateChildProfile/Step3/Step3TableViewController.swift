//
//  Step3TableViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class Step3TableViewController: StepViewController {
    @IBOutlet weak var totalBrothersTextField: UITextField!
    @IBOutlet weak var totalSistersTextField: UITextField!
    @IBOutlet weak var liveParentCheckboxListView: CheckboxListView!
    @IBOutlet weak var notLiveParentCheckboxListView: CheckboxListView!
    @IBOutlet weak var notLiveParentOtherTextField: UITextField!
    @IBOutlet weak var liveWhoCheckboxListView: CheckboxListView!
    @IBOutlet weak var liveWhoOtherTextField: UITextField!
    @IBOutlet weak var whoWriteLetterCheckboxListView: CheckboxListView!
    @IBOutlet weak var liveWriteLetterOtherTextField: UITextField!
    
    @IBOutlet weak var memberFamilyTableView: UITableView!
    @IBOutlet weak var heightMemberFamilyTableViewConstraint: NSLayoutConstraint!
    let defaultMemberFamilyCellHeight: CGFloat = 86
    let memberFamilyCellHeight: CGFloat = 70
    
    weak var containerViewController: CreateChildProfileViewController?
    private let viewModel = Step3ViewModel()
    
    override func setupView() {
        setupTableView()
        setDataCheckboxListView()
    }
    
    override func refreshView() {
        containerViewController?.setupTitleLabel(Strings.step3Title.uppercased())
    }
    
    @IBAction func addFamilyMemberButtonWasTouched(_ sender: Any) {
        let viewController = MemberFamilyViewController.instance()
        viewController.callerViewController = self
        present(viewController, animated: true, completion: nil)
    }
    
    override func updateFormToViewModel() -> Bool {
        viewModel.notLiveParent = notLiveParentOtherTextField.text ?? ""
        viewModel.liveWhoOther = liveWhoOtherTextField.text ?? ""
        viewModel.liveWriteLetterOther = liveWriteLetterOtherTextField.text ?? ""
        
        return true
    }
    
    func getViewModel() -> Step3ViewModel {
        return viewModel
    }
}

// MARK: - Setup
extension Step3TableViewController {
    private func setDataCheckboxListView() {
        liveParentCheckboxListView.setDataSource(viewModel.listLiveParent)
        notLiveParentCheckboxListView.setDataSource(viewModel.listNotLiveParent, isShowTextField: true)
        liveWhoCheckboxListView.setDataSource(viewModel.listLiveWho)
        whoWriteLetterCheckboxListView.setDataSource(viewModel.listWhowWriteLetter)
    }
}

// MARK: - Get info
extension Step3TableViewController {
    func setupViewModelFromParent(_ parentViewModel: CreateChildProfileViewModel) {
        guard let model = parentViewModel.childProfileModel else {
            return
        }
        
        viewModel.listLiveParent = model.livingWithParentModel?.listObject ?? []
        viewModel.listNotLiveParent = model.notLivingWithParentModel?.listObject ?? []
        viewModel.listLiveWho = model.livingWithOtherModel?.listObject ?? []
        viewModel.listWhowWriteLetter = model.letterWriteModel?.listObject ?? []
        viewModel.listFamilyMember = model.listFamilyMember
        
        if parentViewModel.isEditMode {
            setupEditMode(model)
        }
    }
    
    private func setupEditMode(_ model: ChildProfileModel) {
        _ = view
        viewModel.listFamilyMember.forEach { familyMember in
            familyMember.dateOfBirth = DateHelper.stringFromServerDateToShortDate(familyMember.dateOfBirth)
        }
        reloadFamilyMemberData()
        
        viewModel.notLiveParent = model.notLivingWithParentModel?.otherValue ?? ""
        viewModel.liveWhoOther = model.livingWithOtherModel?.otherValue ?? ""
        viewModel.liveWriteLetterOther = model.letterWriteModel?.otherValue ?? ""
        
        notLiveParentOtherTextField.text = viewModel.notLiveParent
        liveWhoOtherTextField.text = viewModel.liveWhoOther
        liveWriteLetterOtherTextField.text = viewModel.liveWriteLetterOther
    }
}

// MARK: - Member Family Handler
extension Step3TableViewController {
    private func setupTableView() {
        memberFamilyTableView.dataSource = self
        memberFamilyTableView.delegate = self
    }
    
    private func reloadFamilyMemberData() {
        memberFamilyTableView.reloadData()
        
        let brotherCount = viewModel.listFamilyMember
            .filter({
                $0.relationshipId == Constants.relationshipOlderBrother || $0.relationshipId == Constants.relationshipYoungerBrother
            }).count
        
        let sisterCount = viewModel.listFamilyMember
            .filter({
                $0.relationshipId == Constants.relationshipOlderSister || $0.relationshipId == Constants.relationshipYoungerSister
            }).count
        
        totalBrothersTextField.text = "\(brotherCount)"
        totalSistersTextField.text = "\(sisterCount)"
    }
    
    override func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        if tableView == memberFamilyTableView {
            return memberFamilyCellHeight
        }
        
        if tableView == self.tableView {
            guard indexPath.row != 0 else {
                let numberMember = viewModel.listFamilyMember.count
                heightMemberFamilyTableViewConstraint.constant = memberFamilyCellHeight * CGFloat(numberMember)
                return numberMember == 0 ? defaultMemberFamilyCellHeight : heightMemberFamilyTableViewConstraint.constant + 102
            }
            
            return super.tableView(tableView, heightForRowAt: indexPath)
        }
        
        return super.tableView(tableView, heightForRowAt: indexPath)
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        guard tableView == memberFamilyTableView else {
            return super.tableView(tableView, numberOfRowsInSection: section)
        }
        
        return viewModel.listFamilyMember.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if tableView == self.tableView {
            return super.tableView(tableView, cellForRowAt: indexPath)
        }
        let memberCell = tableView.dequeueReusableCell(withIdentifier: MembeFamilyTableViewCell.className,
                                                       for: indexPath) as! MembeFamilyTableViewCell
        let model = viewModel.listFamilyMember[indexPath.row]
        let memberCellViewModel = FamilyMemberCellViewModel(name: model.name,
                                                            gender: Gender(rawValue: model.gender)?.title ?? Gender.male.title,
                                                            relation: Utilities.getName(from: viewModel.relationShipDataSource, with: model.relationshipId),
                                                            birthday: DateHelper.stringFromShortDateToLocalDate(model.dateOfBirth),
                                                            liveWithChildren: LiveWithChild(rawValue: model.liveWithChild)?.title ?? LiveWithChild.yes.title)
        memberCell.delegate = self
        memberCell.configure(viewModel: memberCellViewModel, index: indexPath.row)
        return memberCell
    }
    
    func updateFamilyMember(viewModel: MemberFamilyViewModel) {
        if let _ = viewModel.editModel {
            reloadFamilyMemberData()
            return
        }
        
        let insertModel = viewModel.getInsertModel()
        insertModel.createBy = Setting.userId.value
        insertModel.updateBy = Setting.userId.value
        self.viewModel.listFamilyMember.append(insertModel)
        
        reloadFamilyMemberData()
        tableView.beginUpdates()
        tableView.endUpdates()
    }
}

// MARK: - FamilyMemberCellDelegate
extension Step3TableViewController: FamilyMemberCellDelegate {
    func delete(index: Int) {
        AlertController.shared.showConfirmMessage(message: Strings.deleteFamilyMember, confirm: Strings.agree, cancel: Strings.cancel) { [weak self] isConfirm in
            guard isConfirm, let self = self else { return }
            self.viewModel.listFamilyMember.remove(at: index)
            self.reloadFamilyMemberData()
            self.tableView.beginUpdates()
            self.tableView.endUpdates()
        }
    }
    
    func edit(index: Int) {
        let viewController = MemberFamilyViewController.instance()
        viewController.callerViewController = self
        viewController.viewModel.config(editModel: viewModel.listFamilyMember[index])
        present(viewController, animated: true, completion: nil)
    }
}
