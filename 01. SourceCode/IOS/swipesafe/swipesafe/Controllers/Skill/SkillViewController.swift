//
//  SkillViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class SkillViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func closeAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBOutlet weak var skillTableView: UITableView!
    
    var skillCellViewModels: [SkillCellViewModel] = []
    
    override func setupView() {
        setupData()
        configTableView()
    }
    
    override func setupTitle() {
        title = Strings.skillTitle
    }
    
    private func setupData() {
        skillCellViewModels = [
            SkillCellViewModel(title: Strings.skill1Title, date: Strings.dummyDate, content: Strings.skill1Content, image: ImageNames.skill1.image, htmlFileName: Strings.skill1HtmlFileName),
            SkillCellViewModel(title: Strings.skill2Title, date: Strings.dummyDate, content: Strings.skill2Content, image: ImageNames.skill2.image, htmlFileName: Strings.skill2HtmlFileName),
            SkillCellViewModel(title: Strings.skill3Title, date: Strings.dummyDate, content: Strings.skill3Content, image: ImageNames.skill3.image, htmlFileName: Strings.skill3HtmlFileName),
            SkillCellViewModel(title: Strings.skill4Title, date: Strings.dummyDate, content: Strings.skill4Content, image: ImageNames.skill4.image, htmlFileName: Strings.skill4HtmlFileName),
        ]
    }
    
    private func configTableView() {
        skillTableView.dataSource = self
        skillTableView.delegate = self
        
        skillTableView.tableFooterView = UIView()
        skillTableView.estimatedRowHeight = 80
        skillTableView.rowHeight = UITableView.automaticDimension
    }
}

extension SkillViewController: UITableViewDataSource, UITableViewDelegate {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return skillCellViewModels.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: SkillTableViewCell.className, for: indexPath) as! SkillTableViewCell
        cell.configure(viewModel: skillCellViewModels[indexPath.row])
        return cell
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        
        let skillDetailViewController = SkillDetailViewController.storyboardInstance(identifier: SkillDetailViewController.className,
                                                                         with: HomeViewController.className)
        skillDetailViewController.containerViewController = containerViewController
        skillDetailViewController.content = skillCellViewModels[indexPath.row]
        containerViewController?.pushViewController(skillDetailViewController)
    }
}
