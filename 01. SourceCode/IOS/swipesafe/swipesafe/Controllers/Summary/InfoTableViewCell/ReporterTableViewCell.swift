//
//  ReporterTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/25/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ReporterTableViewCell: UITableViewCell {
    @IBOutlet weak var titleLabel: UILabel!
    
    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var genderLabel: UILabel!
    @IBOutlet weak var phoneLabel: UILabel!
    @IBOutlet weak var mailLabel: UILabel!
    @IBOutlet weak var relationShipLabel: UILabel!
    @IBOutlet weak var fullAddressLabel: UILabel!
    
    @IBOutlet weak var invisibleView: UIView!
    @IBOutlet weak var nameContainerView: UIView!
    @IBOutlet weak var genderContainerView: UIView!
    @IBOutlet weak var phoneContainerView: UIView!
    @IBOutlet weak var mailContainerView: UIView!
    @IBOutlet weak var relationShipContainerView: UIView!
    @IBOutlet weak var fullAddressContainerView: UIView!
    
    weak var delegate: SelectUpdateReportDelegate?
    
    func config(reporterModel: ReporterModel) {
        let reporterModel = AppData.shared.getReporterInfo()
        titleLabel.text = SummaryContentType.reporter.title
        
        guard reporterModel.type == ReporterType.open else {
            invisibleView.isHidden = false
            nameContainerView.isHidden = true
            genderContainerView.isHidden = true
            phoneContainerView.isHidden = true
            mailContainerView.isHidden = true
            relationShipContainerView.isHidden = true
            fullAddressContainerView.isHidden = true
            return
        }
        
        invisibleView.isHidden = true
        
        nameLabel.text = reporterModel.name
        genderLabel.text = reporterModel.genderName
        phoneLabel.text = reporterModel.phone
        mailLabel.text = reporterModel.email
        relationShipLabel.text = reporterModel.relationShipName
        fullAddressLabel.text = reporterModel.fullAddress
        
        nameContainerView.isHidden = reporterModel.name.isEmpty
        genderContainerView.isHidden = reporterModel.genderName.isEmpty
        phoneContainerView.isHidden = reporterModel.phone.isEmpty
        mailContainerView.isHidden = reporterModel.email.isEmpty
        relationShipContainerView.isHidden = reporterModel.relationShipName.isEmpty
        fullAddressContainerView.isHidden = reporterModel.fullAddress.isEmpty
    }
    
    @IBAction func updateAction(_ sender: Any) {
        delegate?.updateAction(type: .reporter, index: 0)
    }
}
