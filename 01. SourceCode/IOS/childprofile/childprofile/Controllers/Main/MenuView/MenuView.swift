//
//  MenuView.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

enum ScreenMenu: Int {
    case listChildProfile = 0
    case createChildProfile
    case shareImage
    case profileTitle
    case userInfo
    case changePassword
    case logout
    
    var viewController: UIViewController? {
        switch self {
        case .listChildProfile:
            return ListChildProfileViewController.instance()
        case .createChildProfile:
            return CreateChildProfileViewController.instance()
        case .shareImage:
            return ShareImageViewController.instance()
        case .userInfo:
            return UserProfileViewController.instance()
        case .changePassword:
            return ChangePasswordViewController.instance()
        default:
            return nil
        }
    }
    
    var menuItem: MenuItem {
        switch self {
        case .listChildProfile:
            return MenuItem(id: self.rawValue, title: ScreenTitle.listChildProfile, imageName: ImageNames.listChildProfile)
        case .createChildProfile:
            return MenuItem(id: self.rawValue, title: ScreenTitle.createChildProfile, imageName: ImageNames.createChildProfile)
        case .shareImage:
            return MenuItem(id: self.rawValue, title: ScreenTitle.shareImage, imageName: ImageNames.albumShare)
        case .userInfo:
            return MenuItem(id: self.rawValue, title: ScreenTitle.userInfo, imageName: ImageNames.userInfo)
        case .changePassword:
            return MenuItem(id: self.rawValue, title: ScreenTitle.changePassword, imageName: ImageNames.changePassword)
        case .logout:
            return MenuItem(id: self.rawValue, title: ScreenTitle.logout, imageName: ImageNames.logout)
        case .profileTitle:
            return MenuItem(id: self.rawValue, title: Strings.menuTitle, imageName: "", isTitle: true)
        }
    }
}

class MenuView: NibView {
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var userImageView: UIImageView!
    @IBOutlet weak var menuTableView: UITableView!
    
    let menuItems: [MenuItem] = [
        ScreenMenu.listChildProfile.menuItem,
        ScreenMenu.createChildProfile.menuItem,
        ScreenMenu.shareImage.menuItem,
        ScreenMenu.profileTitle.menuItem,
        ScreenMenu.userInfo.menuItem,
        ScreenMenu.changePassword.menuItem,
        ScreenMenu.logout.menuItem
    ]
    
    var menuSelectedHandler: ((Int) -> Void)?
    
    override func xibSetup() {
        super.xibSetup()
        
        setupUserInfo()
        setupTableView()
    }
    
    private func setupUserInfo() {
        usernameLabel.text = Setting.userFullName.value
        userImageView.setImage(urlString: Setting.imagePath.value)
    }
    
    func reloadImage() {
        userImageView.setImage(urlString: Setting.imagePath.value)
    }
    
    private func setupTableView() {
        menuTableView.register(MenuItemTableViewCell.nib(), forCellReuseIdentifier: MenuItemTableViewCell.className)
        menuTableView.dataSource = self
        menuTableView.delegate = self
        
        menuTableView.tableFooterView = UIView()
    }
}

extension MenuView: UITableViewDataSource {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return menuItems.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: MenuItemTableViewCell.className, for: indexPath) as! MenuItemTableViewCell
        cell.bind(menuItem: menuItems[indexPath.row])
        return cell
    }
}

extension MenuView: UITableViewDelegate {
    func tableView(_ tableView: UITableView, shouldHighlightRowAt indexPath: IndexPath) -> Bool {
        return !menuItems[indexPath.row].isTitle
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        menuSelectedHandler?(indexPath.row)
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return menuItems[indexPath.row].isTitle ? 32 : 44
    }
}
