//
//  InfoUpdateTableViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/17/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class InfoUpdateTableViewController: UITableViewController {
    var infoCellViewModels: [InfoCellViewModel] = []
    
    var updateHandler: ((Int) -> Void)?
    var removeHandler: ((Int) -> Void)?
    
    var contentHeight: CGFloat {
        tableView.layoutIfNeeded()
        return tableView.contentSize.height
    }
    
    convenience init(infoCellViewModels: [InfoCellViewModel]) {
        self.init()
        self.infoCellViewModels = infoCellViewModels
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        tableView.separatorColor = .clear
        let infoCellNib = UINib(nibName: InfoUpdateTableViewCell.className, bundle: nil)
        tableView.register(infoCellNib,
                           forCellReuseIdentifier: InfoUpdateTableViewCell.className)
    }
    
    func reloadData(with infoCellViewModels: [InfoCellViewModel]) {
        self.infoCellViewModels = infoCellViewModels
        tableView.reloadData()
    }
    
    // MARK: - Table view data source
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return infoCellViewModels.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: InfoUpdateTableViewCell.className, for: indexPath) as! InfoUpdateTableViewCell
        cell.delegate = self
        cell.infoCellViewModel = infoCellViewModels[indexPath.row]
        return cell
    }
    
    // MARK: - Table view delegate
    
    override func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return InfoUpdateTableViewCell.defaultCellHeight
    }
}

extension InfoUpdateTableViewController: InfoUpdateTableViewCellDelegate {
    func update(index: Int) {
        updateHandler?(index)
    }
    
    func remove(index: Int) {
        removeHandler?(index)
    }
}
