//
//  RightViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class RightViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func closeAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBOutlet weak var rightTableView: UITableView!
    var rightCellViewModels: [RightCellViewModel] = []
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == RightDetailViewController.className, let index = sender as? Int {
            let rightDetailViewController = segue.destination as! RightDetailViewController
            rightDetailViewController.content = rightCellViewModels[index]
        }
    }

    override func setupView() {
        setupData()
        configTableView()
    }
    
    override func setupTitle() {
        title = Strings.rightTitle
    }
    
    private func setupData() {
        rightCellViewModels = [
            RightCellViewModel(id: 0, title: Strings.right1, date: Strings.dummyDate, fileUrl: Strings.right1Url),
            RightCellViewModel(id: 1, title: Strings.right2, date: Strings.dummyDate, fileUrl: Strings.right2Url),
            RightCellViewModel(id: 2, title: Strings.right3, date: Strings.dummyDate, fileUrl: Strings.right3Url),
            RightCellViewModel(id: 3, title: Strings.right4, date: Strings.dummyDate, fileUrl: Strings.right4Url),
            RightCellViewModel(id: 4, title: Strings.right5, date: Strings.dummyDate, fileUrl: Strings.right5Url)
        ]
    }
    
    private func configTableView() {
        rightTableView.dataSource = self
        rightTableView.delegate = self
        
        rightTableView.tableFooterView = UIView()
        rightTableView.estimatedRowHeight = 80
        rightTableView.rowHeight = UITableView.automaticDimension
    }
    
    private func downloadFile(link: String, fileName: String) {
        guard let url = URL(string: link) else { return }
        
        let urlSession = URLSession(configuration: .default, delegate: self, delegateQueue: OperationQueue())
        
        let downloadTask = urlSession.downloadTask(with: url)
        downloadTask.taskDescription = fileName
        
        showHUD()
        downloadTask.resume()
    }
}

extension RightViewController: URLSessionDownloadDelegate {
    func urlSession(_ session: URLSession, downloadTask: URLSessionDownloadTask, didFinishDownloadingTo location: URL) {
        guard let id = downloadTask.taskDescription else { return }
        let documentsPath = FileManager.default.urls(for: .documentDirectory, in: .userDomainMask)[0]
        let destinationURL = documentsPath.appendingPathComponent("File\(id).pdf")
        try? FileManager.default.removeItem(at: destinationURL)
        do {
            try FileManager.default.copyItem(at: location, to: destinationURL)
            DispatchQueue.main.async { [weak self] in
                self?.performSegue(withIdentifier: RightDetailViewController.className, sender: Int(id))
            }
        } catch let error {
            print("Copy Error: \(error.localizedDescription)")
        }
        dimissHUD()
    }
    
    func urlSession(_ session: URLSession, task: URLSessionTask, didCompleteWithError error: Error?) {
        guard let errorContent = error?.localizedDescription else { return }
        AlertController.shared.showErrorMessage(message: errorContent, completionHandler: {})
        dimissHUD()
    }
}
extension RightViewController: UITableViewDataSource, UITableViewDelegate {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return rightCellViewModels.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: RightTableViewCell.className, for: indexPath) as! RightTableViewCell
        cell.configure(viewModel: rightCellViewModels[indexPath.row])
        return cell
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        let rightContent = rightCellViewModels[indexPath.row]
        if rightContent.fileName.fileExist {
            let rightDetailViewController = RightDetailViewController.storyboardInstance(identifier: RightDetailViewController.className,
                                                                                         with: HomeViewController.className)
            rightDetailViewController.containerViewController = containerViewController
            rightDetailViewController.content = rightCellViewModels[rightContent.id]
            containerViewController?.pushViewController(rightDetailViewController)
        } else {
            downloadFile(link: rightContent.fileUrl, fileName: "\(rightContent.id)")
        }
    }
}

