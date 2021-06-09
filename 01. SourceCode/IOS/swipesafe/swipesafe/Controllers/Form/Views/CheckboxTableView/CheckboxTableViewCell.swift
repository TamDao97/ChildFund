//
//  CheckboxTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/16/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class CheckboxTableViewCell: UITableViewCell {
    static let defaultCellHeight: CGFloat = 35
    
    var childAbuseModel: ChildAbuseModel? {
        didSet {
            guard let model = childAbuseModel else { return }
            
            checkboxView.title = model.name
            checkboxView.isSelected = model.isCheck
        }
    }
    
    private let checkboxView: CheckboxView = {
        let checkboxView = CheckboxView()
        checkboxView.translatesAutoresizingMaskIntoConstraints = false
        return checkboxView
    }()
    
    override init(style: UITableViewCell.CellStyle, reuseIdentifier: String?) {
        super.init(style: style, reuseIdentifier: reuseIdentifier)
        
        // Checkbox view
        addSubview(checkboxView)
        NSLayoutConstraint.activate([
            checkboxView.leadingAnchor.constraint(equalTo: leadingAnchor),
            checkboxView.trailingAnchor.constraint(equalTo: trailingAnchor),
            checkboxView.topAnchor.constraint(equalTo: topAnchor),
            checkboxView.bottomAnchor.constraint(equalTo: bottomAnchor)
            ])
        
        checkboxView.selectedCompletion = { [weak self] in
            guard
                let self = self,
                let model = self.childAbuseModel
            else {
                return
            }
            model.isCheck = self.checkboxView.isSelected
        }
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
    }
}
