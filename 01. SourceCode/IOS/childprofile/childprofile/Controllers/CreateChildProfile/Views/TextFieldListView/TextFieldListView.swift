//
//  TextFieldListView.swift
//  childprofile
//
//  Created by Thanh Luu on 1/27/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class TextFieldListView: UIView {
    private var dataSource: [ObjectInputModel] = []
    private var isShowTextField: Bool = false
    
    var tableView: UITableView = {
        let tableView = UITableView()
        tableView.translatesAutoresizingMaskIntoConstraints = false
        tableView.allowsSelection = false
        tableView.isScrollEnabled = false
        tableView.backgroundColor = .clear
        return tableView
    }()
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        initViews()
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        initViews()
    }
    
    private func initViews() {
        setupTableView()
    }
    
    func setDataSource(_ values: [ObjectInputModel]) {
        dataSource = values
        tableView.reloadData()
    }
}

// MARK: Table view
extension TextFieldListView {
    private func setupTableView() {
        tableView.dataSource = self
        tableView.delegate = self
        
        tableView.register(UINib(nibName: TextFieldTableViewCell.className, bundle: nil),
                                       forCellReuseIdentifier: TextFieldTableViewCell.className)
        
        addSubview(tableView)
        NSLayoutConstraint.activate([
            tableView.leadingAnchor.constraint(equalTo: leadingAnchor),
            tableView.trailingAnchor.constraint(equalTo: trailingAnchor),
            tableView.topAnchor.constraint(equalTo: topAnchor),
            tableView.bottomAnchor.constraint(equalTo: bottomAnchor)
            ])
    }
}

extension TextFieldListView: UITableViewDataSource {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return dataSource.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: TextFieldTableViewCell.className, for: indexPath) as! TextFieldTableViewCell
        cell.configure(with: dataSource[indexPath.row])
        cell.separatorInset = UIEdgeInsets(top: 0, left: UIScreen.main.bounds.width, bottom: 0, right: 0)
        return cell
    }
}

extension TextFieldListView: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 57
    }
}
