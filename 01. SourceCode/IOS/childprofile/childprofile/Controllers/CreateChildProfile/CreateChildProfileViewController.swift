//
//  CreateChildProfileViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class CreateChildProfileViewController: BaseViewController {
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var containerView: UIView!
    @IBOutlet weak var statusPageControl: UIPageControl!
    
    @IBOutlet weak var previousButton: UIButton!
    @IBOutlet weak var nextButton: UIButton!
    @IBOutlet weak var updateButton: UIButton!
    
    static func instanceWithEditMode(childProfileId: String) -> CreateChildProfileViewController {
        let viewModel = CreateChildProfileViewModel()
        viewModel.childProfileId = childProfileId
        let editChildProfileViewController = CreateChildProfileViewController.instance()
        editChildProfileViewController.viewModel = viewModel
        return editChildProfileViewController
    }
    
    lazy var step1ViewController: Step1TableViewController = {
        let step1ViewController = Step1TableViewController.instance()
        step1ViewController.containerViewController = self
        return step1ViewController
    }()
    
    lazy var step2ViewController: Step2TableViewController = {
        let step2ViewController = Step2TableViewController.instance()
        step2ViewController.containerViewController = self
        return step2ViewController
    }()
    
    lazy var step3ViewController: Step3TableViewController = {
        let step3ViewController = Step3TableViewController.instance()
        step3ViewController.containerViewController = self
        return step3ViewController
    }()
    
    lazy var step4ViewController: Step4TableViewController = {
        let step4ViewController = Step4TableViewController.instance()
        step4ViewController.containerViewController = self
        return step4ViewController
    }()
    
    var childControllers: [StepViewController] = []
    var currentChildViewController: StepViewController?
    var viewModel = CreateChildProfileViewModel()
    
    override func setupView() {
        setupChildViewControllers()
        getChildProfileModel()
        setupGesture()
        updateStateControlButton()
    }
    
    override func setupTitle() {
        title = viewModel.title
    }
    
    override func refreshView() {
        parent?.title = title
    }
    
    func setupTitleLabel(_ string: String) {
        titleLabel.text = string
    }
    
    @IBAction func previousButtonWasTouched(_ sender: Any) {
        setPreviousViewController()
    }
    
    @IBAction func nextButtonWasTouched(_ sender: Any) {
        setNextViewController()
    }
    
    @IBAction func updateButtonWasTouched(_ sender: Any) {
        if !viewModel.isEditMode {
            insertChildProfile()
        } else {
            updateChildProfile()
        }
    }
}

extension CreateChildProfileViewController {
    private func getChildProfileModel() {
        if viewModel.isEditMode {
            self.showHUD()
        }
        viewModel.get() { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            self.step1ViewController.setupViewModelFromParent(self.viewModel)
            self.step2ViewController.setupViewModelFromParent(self.viewModel)
            self.step3ViewController.setupViewModelFromParent(self.viewModel)
            self.step4ViewController.setupViewModelFromParent(self.viewModel)
        }
    }
    
    private func insertChildProfile() {
        guard step4ViewController.updateFormToViewModel() else { return }
        viewModel.setModelFromChildViewModel(step1ViewModel: step1ViewController.getViewModel(),
                                             step2ViewModel: step2ViewController.getViewModel(),
                                             step3ViewModel: step3ViewController.getViewModel(),
                                             step4ViewModel: step4ViewController.getViewModel())
        showHUD()
        viewModel.insert() { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            guard let mainViewController = self.parent as? MainViewController else {
                return
            }
            mainViewController.showMessage(title: self.viewModel.successMessage)
            mainViewController.reloaChildProfileViewController()
        }
    }
    
    private func updateChildProfile() {
        guard step4ViewController.updateFormToViewModel() else { return }
        viewModel.setModelFromChildViewModel(step1ViewModel: step1ViewController.getViewModel(),
                                             step2ViewModel: step2ViewController.getViewModel(),
                                             step3ViewModel: step3ViewController.getViewModel(),
                                             step4ViewModel: step4ViewController.getViewModel())
        showHUD()
        viewModel.update() { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            self.showMessage(title: self.viewModel.updateSuccessMessage)
            self.navigationController?.popViewController(animated: true)
        }
    }
    
    private func setupChildViewControllers() {
        childControllers = [step1ViewController,
                            step2ViewController,
                            step3ViewController,
                            step4ViewController]
        
        changeChildViewController(step1ViewController)
    }
    
    private func setupGesture() {
        let swipeToLeft = UISwipeGestureRecognizer(target: self, action: #selector(handleSwipeToLeft(sender:)))
        swipeToLeft.direction = .left
        let swipeToRight = UISwipeGestureRecognizer(target: self, action: #selector(handleSwipeToRight(sender:)))
        swipeToRight.direction = .right
        view.addGestureRecognizer(swipeToLeft)
        view.addGestureRecognizer(swipeToRight)
    }
    
    @objc func handleSwipeToLeft(sender: UISwipeGestureRecognizer) {
        setNextViewController()
    }
    
    @objc func handleSwipeToRight(sender: UISwipeGestureRecognizer) {
        setPreviousViewController()
    }
}

// MARK: - Helpers
extension CreateChildProfileViewController {
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
    
    private func setNextViewController() {
        guard
            statusPageControl.currentPage != childControllers.count - 1,
            let stepViewController = currentChildViewController,
            stepViewController.updateFormToViewModel()
            else {
                return
        }
        statusPageControl.currentPage += 1
        updateStateControlButton()
        changeChildViewController(childControllers[statusPageControl.currentPage])
    }
    
    private func setPreviousViewController() {
        if statusPageControl.currentPage == 0 {
            return
        }
        statusPageControl.currentPage -= 1
        updateStateControlButton()
        changeChildViewController(childControllers[statusPageControl.currentPage])
    }
    
    private func updateStateControlButton() {
        previousButton.isEnabled = statusPageControl.currentPage != 0
        nextButton.isEnabled = statusPageControl.currentPage != childControllers.count - 1
        updateButton.isEnabled = statusPageControl.currentPage == childControllers.count - 1
    }
}
