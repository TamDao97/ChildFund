//
//  ListChildProfileViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ListChildProfileViewController: BaseViewController {
    
    @IBOutlet weak var childProfileTableView: UITableView!
    @IBOutlet weak var statusLabel: UILabel!
    @IBOutlet weak var totalItemLabel: UILabel!
    
    private let viewModel = ListChildProfileViewModel()
    
    private var isLoadMode: Bool = false
    private var emptyView = UIView()
    private var loadingMoreContentView: UIView = {
        let loadingMoreContentView = UIView(frame: CGRect(x: 0, y: 0, width: UIScreen.main.bounds.width, height: 46))
        loadingMoreContentView.backgroundColor = .clear
        let activityIndicatorView = UIActivityIndicatorView(style: .gray)
        activityIndicatorView.center = loadingMoreContentView.center
        activityIndicatorView.startAnimating()
        loadingMoreContentView.addSubview(activityIndicatorView)
        return loadingMoreContentView
    }()
    private let refreshControl = UIRefreshControl()
    
    override func setupView() {
        getListChildProfile()
        setupTableView()
    }
    
    override func setupTitle() {
        title = ScreenTitle.listChildProfile
    }
    
    override func refreshView() {
        parent?.title = title
    }
    
    override func setupNavigationBar() {
        let searchBarButtonItem = UIBarButtonItem(image: ImageNames.search.image, style: .plain, target: self, action: #selector(showSearchView))
        parent?.navigationItem.rightBarButtonItem = searchBarButtonItem
    }
    
    @objc private func showSearchView() {
        if let mainViewController = parent as? MainViewController,
            mainViewController.isMenuShow {
            mainViewController.closeMenu()
        }
        
        let searchViewModel = SearchViewModel(code: viewModel.searchCondition.childCode,
                                              name: viewModel.searchCondition.name,
                                              address: viewModel.searchCondition.address)
        SearchViewController.show(from: self, viewModel: searchViewModel) { [weak self] searchViewModel in
            guard let self = self else { return }
            self.viewModel.searchCondition.childCode = searchViewModel.code
            self.viewModel.searchCondition.name = searchViewModel.name
            self.viewModel.searchCondition.address = searchViewModel.address
            
            self.refreshControl.beginRefreshing()
            self.getListChildProfile()
        }
    }
    
    private func setupTableView() {
        isLoadMode = true
        
        childProfileTableView.refreshControl = refreshControl
        refreshControl.addTarget(self, action: #selector(refreshData), for: .valueChanged)
        
        childProfileTableView.tableFooterView = UIView()
        childProfileTableView.register(UINib(nibName: ChildProfileTableViewCell.className, bundle: nil),
                                       forCellReuseIdentifier: ChildProfileTableViewCell.className)
        childProfileTableView.dataSource = self
        childProfileTableView.delegate = self
    }
    
    @objc private func refreshData() {
        getListChildProfile()
    }
    
    private func getListChildProfile(isLoadMore: Bool = false) {
        if isLoadMore {
            viewModel.searchCondition.pageNumber += 1
        } else {
            viewModel.searchCondition.pageNumber = 1
        }
        
        viewModel.search() { [weak self] result in
            guard let self = self else { return }
            self.refreshControl.endRefreshing()
            self.loadMoreContent(false)
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                self.statusLabel.isHidden = false
                self.isLoadMode = false
                return
            }
            
            self.isLoadMode = self.viewModel.recentItemCount != 0
            self.statusLabel.isHidden = true
            self.childProfileTableView.reloadData()
            self.totalItemLabel.text = "\(self.viewModel.totalItem)"
        }
    }
}

extension ListChildProfileViewController: UITableViewDataSource, UITableViewDelegate {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return viewModel.listChildProfileCellViewModel.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: ChildProfileTableViewCell.className,
                                                 for: indexPath) as! ChildProfileTableViewCell
        cell.delegate = self
        cell.config(with: viewModel.listChildProfileCellViewModel[indexPath.row], index: indexPath.row)
        return cell
    }
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        guard
            isLoadMode,
            indexPath.row == viewModel.listChildProfileCellViewModel.count - 1,
            childProfileTableView.tableFooterView == emptyView
        else {
            return
        }
        loadMoreContent(isLoadMode)
        
        guard viewModel.listChildProfileCellViewModel.count > 0 else {
            return
        }
        
        getListChildProfile(isLoadMore: true)
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 84
    }
}

// MARK: - Helpers
extension ListChildProfileViewController {
    func loadMoreContent(_ isLoadMore: Bool) {
        if isLoadMore {
            childProfileTableView.contentInset = UIEdgeInsets.zero
            childProfileTableView.tableFooterView = self.loadingMoreContentView
        } else {
            childProfileTableView.contentInset = UIEdgeInsets.zero
            childProfileTableView.tableFooterView = emptyView
        }
    }
}

extension ListChildProfileViewController: ChildProfileCellDelegate {
    func report(index: Int) {
        ChildReportViewController.show(viewController: self) { [weak self] reportContent in
            guard let self = self else { return }
            self.viewModel.currentReportContent = reportContent
            self.viewModel.currentChildId = self.viewModel.listChildProfileCellViewModel[index].id
            
            self.showHUD()
            self.viewModel.addReport() { [weak self] result in
                guard let self = self else { return }
                self.dimissHUD()
                guard result == .success else {
                    self.showMessage(title: self.viewModel.errorResponseContent)
                    return
                }
                self.showMessage(title: Strings.reportSuccessAdd)
            }
        }
    }
    
    func edit(index: Int) {
        let childProfileId = self.viewModel.listChildProfileCellViewModel[index].id
        let editProfileChildProfileViewController = CreateChildProfileViewController.instanceWithEditMode(childProfileId: childProfileId)
        navigationController?.pushViewController(editProfileChildProfileViewController, animated: true)
    }
}
