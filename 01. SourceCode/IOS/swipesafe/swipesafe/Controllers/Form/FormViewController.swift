//
//  FormViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/4/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class FormViewController: BaseViewController {
    @IBOutlet weak var containerView: UIView!
    
    @IBOutlet weak var nextTitleLabel: UILabel!
    @IBOutlet weak var nextImageView: UIImageView!
    
    weak var containerViewController: HomeViewController?
    
    var childControllers: [StepViewController] = []
    var currentChildViewController: StepViewController?
    var viewModel = FormViewModel()
    
    // Edit mode
    var editType: SummaryContentType?
    var index: Int?
    var isEditMode: Bool {
        return editType != nil
    }
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        guard segue.identifier == DialogExitFormViewController.className else { return }
        let dialogExitFormViewController = segue.destination as? DialogExitFormViewController
        dialogExitFormViewController?.formViewController = self
    }
    
    static func storyboardEditInstance(editType: SummaryContentType, index: Int) -> FormViewController {
        let editFormViewController = FormViewController.storyboardInstance()
        editFormViewController.editType = editType
        editFormViewController.index = index
        return editFormViewController
    }
    
    lazy var step1ViewController: Step1ViewController = {
        let step1ViewController = Step1ViewController.storyboardInstance()
        step1ViewController.containerViewController = self
        return step1ViewController
    }()
    
    lazy var step2ViewController: Step2ViewController = {
        let step2ViewController = Step2ViewController.storyboardInstance()
        step2ViewController.containerViewController = self
        return step2ViewController
    }()
    
    lazy var step3ViewController: Step3ViewController = {
        let step3ViewController = Step3ViewController.storyboardInstance()
        step3ViewController.containerViewController = self
        return step3ViewController
    }()
    
    lazy var step4ViewController: Step4ViewController = {
        let step4ViewController = Step4ViewController.storyboardInstance()
        step4ViewController.containerViewController = self
        return step4ViewController
    }()
    
    // MARK: - Lifecycle
    override func setupView() {
        setupNextButton()
        setupChildViewControllers()
    }
    
    override func refreshView() {
        navigationController?.setNavigationBarHidden(true, animated: false)
        // Refresh step4 state when back from summary
        if !isEditMode, viewModel.pageIndex == childControllers.count - 1 {
            step4ViewController.contentViewController?.refreshFromReporterData()
        }
    }
    
    @IBAction func previousAction(_ sender: Any) {
        guard !isEditMode else {
            containerViewController?.popViewController()
            return
        }
        
        guard viewModel.pageIndex != 0 else {
            // Call dialog
            performSegue(withIdentifier: DialogExitFormViewController.className, sender: nil)
            return
        }
        
        viewModel.pageIndex -= 1
        let visibleViewController = childControllers[viewModel.pageIndex]
        if visibleViewController == step3ViewController {
            step3ViewController.refreshValueFromAppData()
        }
        changeChildViewController(visibleViewController)
    }
    
    @IBAction func nextAction(_ sender: Any) {
        guard
            let currentChildViewController = self.currentChildViewController,
            currentChildViewController.updateFormToViewModel()
        else {
            return
        }
        
        guard !isEditMode else {
            dismiss(animated: false, completion: nil)
            return
        }
        
        guard viewModel.pageIndex != childControllers.count - 1 else {
            gotoSummary()
            return
        }
        
        viewModel.pageIndex += 1
        changeChildViewController(childControllers[viewModel.pageIndex])
    }
}

// MARK: - First setup
extension FormViewController {
    private func setupNextButton() {
        if isEditMode {
            nextTitleLabel.text = Strings.saveButtonTitle
            nextImageView.image = ImageNames.save.image
        } else {
            nextTitleLabel.text = Strings.nextButtonTitle
            nextImageView.image = ImageNames.next.image
        }
    }
    
    private func setupChildViewControllers() {
        guard !isEditMode else {
            guard let editType = self.editType else { return }
            switch editType {
            case .child:
                step1ViewController.updateIndex = index
                changeChildViewController(step1ViewController)
            case .prisoner:
                step2ViewController.updateIndex = index
                changeChildViewController(step2ViewController)
            case .description:
                step3ViewController.isEditMode = true
                changeChildViewController(step3ViewController)
            case .reporter:
                step4ViewController.isEditMode = true
                changeChildViewController(step4ViewController)
            }
            return
        }
        
        childControllers = [step1ViewController,
                            step2ViewController,
                            step3ViewController,
                            step4ViewController]
        changeChildViewController(step1ViewController)
    }
}

// MARK: - Functions
extension FormViewController {
    func handlerExitForm(isConfirm: Bool) {
        guard isConfirm else { return }
        AppData.shared.resetReport()
        
        containerViewController?.popViewController()
    }
    
    private func gotoSummary() {
        let summaryViewController = SummaryViewController.storyboardInstance()
        summaryViewController.containerViewController = containerViewController
        containerViewController?.pushViewController(summaryViewController)
    }
}

// MARK: - Helpers
extension FormViewController {
    private func changeChildViewController(_ viewController: StepViewController) {
        if let oldChildViewController = self.currentChildViewController {
            self.remove(childViewController: oldChildViewController)
        }
        
        self.currentChildViewController = viewController
        
        guard let newChildViewController = self.currentChildViewController else {
            return
        }
        self.add(childViewController: newChildViewController, containerView: self.containerView)
    }
}

class DialogExitFormViewController: UIViewController {
    weak var formViewController: FormViewController?
    
    @IBAction func confirmAction(_ sender: Any) {
        closeDialog(isConfirm: true)
    }
    
    @IBAction func cancelAction(_ sender: Any) {
        closeDialog(isConfirm: false)
    }
    
    private func closeDialog(isConfirm: Bool) {
        self.dismiss(animated: true) { [weak self] in
            self?.formViewController?.handlerExitForm(isConfirm: isConfirm)
        }
    }
}
