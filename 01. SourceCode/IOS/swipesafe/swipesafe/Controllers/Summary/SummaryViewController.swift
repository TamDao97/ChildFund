
//
//  Step1ViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

protocol SelectUpdateReportDelegate: class {
    func updateAction(type: SummaryContentType, index: Int)
}

class SummaryViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    @IBOutlet weak var summaryTableView: UITableView!
    
    var viewModel = SummaryViewModel()
    
    override func setupView() {
        setupTableView()
    }
    
    override func refreshView() {
        viewModel.refreshData()
        summaryTableView.reloadData()
    }
    
    @IBAction func backAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBAction func createReportAction(_ sender: Any) {
        self.showHUD()
        viewModel.createReport { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            AppData.shared.resetReport()
            
            let summaryNotificationViewController = SummaryNotificationViewController
                .storyboardInstance(identifier: SummaryNotificationViewController.className,
                                    with: SummaryViewController.className)
            summaryNotificationViewController.homeViewController = self.containerViewController
            self.present(summaryNotificationViewController, animated: true, completion: nil)
        }
    }
}

// MARK: First setup
extension SummaryViewController {
    private func setupTableView() {
        summaryTableView.dataSource = self
        summaryTableView.estimatedRowHeight = 44
        summaryTableView.rowHeight = UITableView.automaticDimension
        
        summaryTableView.register(UINib(nibName: ChildDetailTableViewCell.className, bundle: nil),
                                  forCellReuseIdentifier: ChildDetailTableViewCell.className)
        summaryTableView.register(UINib(nibName: PrisonerDetailTableViewCell.className, bundle: nil),
                                  forCellReuseIdentifier: PrisonerDetailTableViewCell.className)
        summaryTableView.register(UINib(nibName: DescriptionTableViewCell.className, bundle: nil),
                                  forCellReuseIdentifier: DescriptionTableViewCell.className)
        summaryTableView.register(UINib(nibName: ReporterTableViewCell.className, bundle: nil),
                                  forCellReuseIdentifier: ReporterTableViewCell.className)
    }
}

extension SummaryViewController: UITableViewDataSource {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return viewModel.summaryCellViewModels.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if viewModel.summaryCellViewModels[indexPath.row].contentType == .child {
            let cell = tableView.dequeueReusableCell(withIdentifier: ChildDetailTableViewCell.className, for: indexPath) as! ChildDetailTableViewCell
            cell.config(childIndex: viewModel.summaryCellViewModels[indexPath.row].order)
            cell.delegate = self
            return cell
        }
        
        if viewModel.summaryCellViewModels[indexPath.row].contentType == .prisoner {
            let cell = tableView.dequeueReusableCell(withIdentifier: PrisonerDetailTableViewCell.className, for: indexPath) as! PrisonerDetailTableViewCell
            cell.config(prisonerIndex: viewModel.summaryCellViewModels[indexPath.row].order)
            cell.delegate = self
            return cell
        }
        
        if viewModel.summaryCellViewModels[indexPath.row].contentType == .description {
            let cell = tableView.dequeueReusableCell(withIdentifier: DescriptionTableViewCell.className, for: indexPath) as! DescriptionTableViewCell
            cell.config(title: viewModel.reportDescription)
            cell.delegate = self
            return cell
        }
        
        if viewModel.summaryCellViewModels[indexPath.row].contentType == .reporter {
            let cell = tableView.dequeueReusableCell(withIdentifier: ReporterTableViewCell.className, for: indexPath) as! ReporterTableViewCell
            cell.config(reporterModel: viewModel.reporterInfo)
            cell.delegate = self
            return cell
        }
        
        return UITableViewCell()
    }
}

extension SummaryViewController: SelectUpdateReportDelegate {
    func updateAction(type: SummaryContentType, index: Int) {
        log("Select update type: \(type.rawValue) at index: \(index)")
        let editFormViewController = FormViewController.storyboardEditInstance(editType: type, index: index)
        editFormViewController.containerViewController = containerViewController
        containerViewController?.pushViewController(editFormViewController)
    }
}
