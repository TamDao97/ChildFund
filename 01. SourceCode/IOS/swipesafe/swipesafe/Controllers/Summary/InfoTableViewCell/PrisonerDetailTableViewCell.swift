//
//  PrisonerDetailTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/25/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class PrisonerDetailTableViewCell: UITableViewCell {
    @IBOutlet weak var titleLabel: UILabel!
    
    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var genderLabel: UILabel!
    @IBOutlet weak var birthDayLabel: UILabel!
    @IBOutlet weak var ageLabel: UILabel!
    @IBOutlet weak var phoneLabel: UILabel!
    @IBOutlet weak var relationShipLabel: UILabel!
    @IBOutlet weak var fullAddressLabel: UILabel!
    
    @IBOutlet weak var nameContainerView: UIView!
    @IBOutlet weak var genderContainerView: UIView!
    @IBOutlet weak var birthdayContainerView: UIStackView!
    @IBOutlet weak var ageContainerView: UIStackView!
    @IBOutlet weak var relationShipContainerView: UIView!
    @IBOutlet weak var phoneContainerView: UIView!
    
    weak var delegate: SelectUpdateReportDelegate?
    var prisonerIndex: Int = 0
    
    func config(prisonerIndex: Int) {
        guard let prisonerModel = AppData.shared.getPrisoner(at: prisonerIndex) else {
            return
        }
        
        self.prisonerIndex = prisonerIndex
        titleLabel.text = SummaryContentType.prisoner.title + " #\(prisonerIndex + 1)"
        
        nameLabel.text = prisonerModel.name == "" ? Strings.Step1.unknownChildName : prisonerModel.name
        nameContainerView.isHidden = prisonerModel.name.isEmpty
        
        genderLabel.text = prisonerModel.genderName
        genderContainerView.isHidden = prisonerModel.gender.isEmpty
        
        fullAddressLabel.text = prisonerModel.fullAddress
        
        relationShipLabel.text = prisonerModel.relationShipName
        relationShipContainerView.isHidden = prisonerModel.relationShipName.isEmpty
        
        phoneLabel.text = prisonerModel.phone
        phoneContainerView.isHidden = prisonerModel.phone.isEmpty
        
        birthdayContainerView.isHidden = prisonerModel.birthDay.isEmpty
        birthDayLabel.text = DateHelper.stringFromShortDateToLocalDate(prisonerModel.birthDay)
        if let age = prisonerModel.age {
            ageContainerView.isHidden = false
            ageLabel.text = String(age)
        } else {
            ageContainerView.isHidden = true
        }
    }
    
    @IBAction func updateAction(_ sender: Any) {
        delegate?.updateAction(type: .prisoner, index: prisonerIndex)
    }
}
