//
//  HomeViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/31/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class HomeViewController: BaseViewController {
    @IBOutlet weak var containerView: UIView!

    @IBOutlet weak var backgroundView: UIView!
    @IBOutlet weak var menuImageView: UIImageView!
    @IBOutlet weak var lineView: UIView!
    
    var childControllers: [UIViewController] = []
    var currentChildViewController: UIViewController?
    
    var defaultViewController: MainViewController?
    
    private var stackViewControllers: [UIViewController] = []
    
    override func setupView() {
        setDefaultViewController()
    }
    
    @IBAction func callSOSAction(_ sender: Any) {
        let url = URL(string: "tel://\(Constants.childSOSNumber)")
        url?.open()
    }
    
    @IBAction func openMenuAction(_ sender: Any) {
        let menuViewController = MenuViewController.storyboardInstance(identifier: MenuViewController.className,
                                                                       with: HomeViewController.className)
        menuViewController.callerViewController = self
        present(menuViewController, animated: false, completion: nil)
    }
    
    func openChildViewController(menuItem: MenuItem) {
        stackViewControllers = []
        if let viewController = defaultViewController {
            stackViewControllers.append(viewController)
        }
        
        if let _ = currentChildViewController as? FormViewController {
            AppData.shared.resetReport()
        }
        
        switch menuItem {
        case .report:
            let formViewController = FormViewController.storyboardInstance()
            formViewController.containerViewController = self
            pushViewController(formViewController)
        case .library:
            let libraryViewController = LibraryViewController.storyboardInstance(identifier: LibraryViewController.className,
                                                                                 with: HomeViewController.className)
            libraryViewController.containerViewController = self
            pushViewController(libraryViewController)
        case .phone:
            let phoneViewController = CallDialogViewController.storyboardInstance(identifier: CallDialogViewController.className,
                                                                                  with: HomeViewController.className)
            phoneViewController.containerViewController = self
            pushViewController(phoneViewController)
        case .link:
            let linkViewController = LinkViewController.storyboardInstance(identifier: LinkViewController.className,
                                                                           with: HomeViewController.className)
            linkViewController.containerViewController = self
            pushViewController(linkViewController)
        case .appInfo:
            let appInfoViewController = AppInfoViewController.storyboardInstance(identifier: AppInfoViewController.className,
                                                                                 with: HomeViewController.className)
            appInfoViewController.containerViewController = self
            pushViewController(appInfoViewController)
        default:
            return
        }
    }
}

// MARK: - First setup
extension HomeViewController {
    func setDefaultViewController() {
        defaultViewController = MainViewController.storyboardInstance(identifier: MainViewController.className,
                                                                       with: HomeViewController.className)
        defaultViewController?.containerViewController = self
        guard let viewController = defaultViewController else { return }
        pushViewController(viewController)
    }
    
    func popViewController() {
        guard stackViewControllers.count > 0 else { return }
        stackViewControllers.removeLast()
        
        guard let viewController = stackViewControllers.last else { return }
        changeChildViewController(viewController)
        setBackgroundViewState(isHomeScreen: stackViewControllers.count == 1)
    }
    
    func pushViewController(_ viewController: UIViewController) {
        stackViewControllers.append(viewController)
        
        changeChildViewController(viewController)
        setBackgroundViewState(isHomeScreen: stackViewControllers.count == 1)
    }
    
    func resetToDefaultViewController() {
        stackViewControllers = []
        guard let viewController = defaultViewController else { return }
        pushViewController(viewController)
    }
}

// MARK: - Helpers
extension HomeViewController {
    private func changeChildViewController(_ viewController: UIViewController) {
        if let oldChildViewController = self.currentChildViewController {
            remove(childViewController: oldChildViewController)
        }
        
        currentChildViewController = viewController
        
        guard let newChildViewController = currentChildViewController else {
            return
        }
        self.add(childViewController: newChildViewController, containerView: self.containerView)
    }
    
    private func setBackgroundViewState(isHomeScreen: Bool) {
        if isHomeScreen {
            lineView.backgroundColor = .white
            menuImageView.tintColor = .white
            backgroundView.isHidden = false
        } else {
            lineView.backgroundColor = Colors.textureHeader
            menuImageView.tintColor = Colors.textureHeader
            backgroundView.isHidden = true
        }
    }
}
