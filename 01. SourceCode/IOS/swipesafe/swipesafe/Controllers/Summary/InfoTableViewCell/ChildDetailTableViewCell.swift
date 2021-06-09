//
//  ChildDetailTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/25/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ChildDetailTableViewCell: UITableViewCell {
    @IBOutlet weak var titleLabel: UILabel!
    
    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var genderLabel: UILabel!
    @IBOutlet weak var birthDayLabel: UILabel!
    @IBOutlet weak var ageLabel: UILabel!
    @IBOutlet weak var levelLabel: UILabel!
    @IBOutlet weak var fullAddressLabel: UILabel!
    @IBOutlet weak var dateActionLabel: UILabel!
    
    
    @IBOutlet weak var genderContainerView: UIView!
    @IBOutlet weak var birthdayContainerView: UIStackView!
    @IBOutlet weak var ageContainerView: UIStackView!
    
    @IBOutlet weak var abuseTableView: UITableView!
    @IBOutlet weak var abuseTableViewHeightConstraint: NSLayoutConstraint!
    
    weak var delegate: SelectUpdateReportDelegate?
    var childIndex: Int = 0
    var abuses: [ChildAbuseModel] = []
    var abuseTableViewHeight: CGFloat {
        return AbuseDetailTableViewCell.defaultHeight * CGFloat(abuses.count)
    }
    
    override func awakeFromNib() {
        super.awakeFromNib()
        
        abuseTableView.dataSource = self
        abuseTableView.delegate = self
        abuseTableView.register(UINib(nibName: AbuseDetailTableViewCell.className, bundle: nil),
                                forCellReuseIdentifier: AbuseDetailTableViewCell.className)
    }
    
    func config(childIndex: Int) {
        guard let childModel = AppData.shared.getChild(at: childIndex) else {
            return
        }
        
        self.childIndex = childIndex
        titleLabel.text = SummaryContentType.child.title + " #\(childIndex + 1)"
        abuses = childModel.abuses
        abuseTableView.reloadData()
        abuseTableViewHeightConstraint.constant = abuseTableViewHeight
        
        nameLabel.text = childModel.name == "" ? Strings.Step1.unknownChildName : childModel.name
        genderLabel.text = childModel.genderName
        genderContainerView.isHidden = childModel.gender.isEmpty
        levelLabel.text = childModel.levelName
        fullAddressLabel.text = childModel.fullAddress
        dateActionLabel.text = DateHelper.convert(dateString: childModel.dateAction,
                                                  fromFormat: DateFormatString.serverDateFormat,
                                                  toFormat: DateFormatString.ddMMyyyyHHmm)
        
        birthdayContainerView.isHidden = childModel.birthDay.isEmpty
        birthDayLabel.text = DateHelper.stringFromShortDateToLocalDate(childModel.birthDay)
        if let age = childModel.age {
            ageContainerView.isHidden = false
            ageLabel.text = String(age)
        } else {
            ageContainerView.isHidden = true
        }
    }
    
    @IBAction func updateAction(_ sender: Any) {
        delegate?.updateAction(type: .child, index: childIndex)
    }
}

extension ChildDetailTableViewCell: UITableViewDataSource, UITableViewDelegate {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return abuses.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: AbuseDetailTableViewCell.className, for: indexPath) as! AbuseDetailTableViewCell
        cell.config(title: abuses[indexPath.row].name)
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return AbuseDetailTableViewCell.defaultHeight
    }
}
