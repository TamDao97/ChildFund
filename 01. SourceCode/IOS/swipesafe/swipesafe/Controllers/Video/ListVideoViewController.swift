//
//  ListVideoViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ListVideoViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func closeAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBOutlet weak var playListTableView: UITableView!
    
    let viewModel = ListVideoViewModel()
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == VideoItemViewController.className, let index = sender as? Int {
            let videoViewController = segue.destination as! VideoItemViewController
            videoViewController.videoTitle = viewModel.playListItemViewModels[index].title
            videoViewController.videoId = viewModel.playListItemViewModels[index].id
        }
    }
    
    override func setupView() {
        getPlaylist()
        configTableView()
    }
    
    override func setupTitle() {
        title = Strings.videoTitle
    }
    
    private func getPlaylist() {
        showHUD()
        viewModel.getPlaylist { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            self.playListTableView.reloadData()
        }
    }
    
    private func configTableView() {
        playListTableView.dataSource = self
        playListTableView.delegate = self
        
        playListTableView.tableFooterView = UIView()
        playListTableView.estimatedRowHeight = 80
        playListTableView.rowHeight = UITableView.automaticDimension
    }
}

extension ListVideoViewController: UITableViewDataSource, UITableViewDelegate {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return viewModel.playListItemViewModels.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: VideoItemTableViewCell.className, for: indexPath) as! VideoItemTableViewCell
        cell.configure(viewModel: viewModel.playListItemViewModels[indexPath.row])
        return cell
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        
        let videoViewController = VideoItemViewController.storyboardInstance(identifier: VideoItemViewController.className,
                                                                                     with: HomeViewController.className)
        videoViewController.containerViewController = containerViewController
        videoViewController.videoTitle = viewModel.playListItemViewModels[indexPath.row].title
        videoViewController.videoId = viewModel.playListItemViewModels[indexPath.row].id
        containerViewController?.pushViewController(videoViewController)
    }
}
