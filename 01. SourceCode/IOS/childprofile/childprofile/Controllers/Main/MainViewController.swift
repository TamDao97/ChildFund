//
//  MainViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/11/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class MainViewController: BaseViewController {
    @IBOutlet weak var containerView: UIView!
    @IBOutlet weak var backgroundMenuView: UIView!
    @IBOutlet weak var menuView: MenuView!
    
    @IBOutlet weak var widthMenuViewConstraint: NSLayoutConstraint!
    @IBOutlet weak var leadingMenuViewConstraint: NSLayoutConstraint!
    
    var menuBarButtonItem: UIBarButtonItem!
    var searchBarButtonItem: UIBarButtonItem!
    let defaultMenuViewWidth: CGFloat = 285
    
    var currentChildViewController: UIViewController?
    
    var isMenuShow: Bool = false {
        didSet {
            toggleShowMenu()
        }
    }
    
    override func setupView() {
        setupMenuView()
        setDefaultChildViewController()
    }
    
    override func setupNavigationBar() {
        menuBarButtonItem = UIBarButtonItem(image: ImageNames.menu.image, style: .plain, target: self, action: #selector(toggleMenu))
        navigationItem.leftBarButtonItem = menuBarButtonItem
    }
}

// MARK: - Setup view
extension MainViewController {
    private func setupMenuView() {
        widthMenuViewConstraint.constant = defaultMenuViewWidth
        leadingMenuViewConstraint.constant = -defaultMenuViewWidth
        
        menuView.menuSelectedHandler = { [weak self] indexSelected in
            guard let self = self else { return }
            guard let screenMenu = ScreenMenu(rawValue: indexSelected) else {
                return
            }
            
            self.isMenuShow = false
            
            switch screenMenu {
            case .logout:
                self.logout()
            default:
                guard let newChildViewController = screenMenu.viewController else {
                    return
                }
                self.changeChildViewController(newChildViewController)
            }
        }
    }
    
    private func setDefaultChildViewController() {
        let listProfileViewController = ListChildProfileViewController.instance()
        changeChildViewController(listProfileViewController)
    }
    
    func reloaChildProfileViewController() {
        let childProfileViewController = CreateChildProfileViewController.instance()
        changeChildViewController(childProfileViewController)
    }
}

// MARK: - Feature
extension MainViewController {
    private func toggleShowMenu() {
        if isMenuShow {
            closeKeyboard()
            leadingMenuViewConstraint.constant = 0
            menuBarButtonItem.image = ImageNames.close.image
        } else {
            leadingMenuViewConstraint.constant = -menuView.frame.width
            menuBarButtonItem.image = ImageNames.menu.image
        }
        
        UIView.animate(withDuration: 0.25, animations: { [weak self] in
            guard let self = self else { return }
            self.view.layoutIfNeeded()
            self.backgroundMenuView.alpha = self.isMenuShow ? 0.3 : 0
        })
    }
    
    @objc private func toggleMenu() {
        isMenuShow = !isMenuShow
    }
    
    @IBAction func closeMenuViewAction(_ sender: Any) {
        closeMenu()
    }
    
    func closeMenu() {
        isMenuShow = false
    }
    
    private func changeChildViewController(_ viewController: UIViewController) {
        if let oldChildViewController = self.currentChildViewController {
            self.remove(childViewController: oldChildViewController)
        }
        
        self.currentChildViewController = viewController
        
        guard let newChildViewController = self.currentChildViewController else {
            return
        }
        self.add(childViewController: newChildViewController, containerView: self.containerView)
    }
    
    func logout() {
        Setting.isLogin.value = false
        
        let loginViewController = LoginViewController.instance()
        let navigationController = UINavigationController(rootViewController: loginViewController)
        present(navigationController, animated: true, completion: nil)
    }
    
    func reloadProfileImage() {
        menuView.reloadImage()
    }
}
