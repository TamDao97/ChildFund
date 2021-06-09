//
//  CheckboxCollectionViewCell.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class CheckboxCollectionViewCell: UICollectionViewCell {
    var objectInputModel: ObjectInputModel? {
        didSet {
            guard let model = objectInputModel else {
                return
            }
            checkboxView.title = model.name
            checkboxView.isSelected = model.isCheck
            
            guard isShowTextField && !model.otherName.isEmpty else {
                textField.isHidden = true
                return
            }
            
            textField.isHidden = false
            textField.placeholder = model.otherName
            textField.text = model.otherValue
            textField.delegate = self
        }
    }
    
    private var isShowTextField = false
    
    private var textField: UITextField = {
        let textField = UITextField()
        textField.translatesAutoresizingMaskIntoConstraints = false
        textField.keyboardType = .numberPad
        textField.font = UIFont.systemFont(ofSize: 12)
        textField.textAlignment = .right
        textField.isHidden = true
        return textField
    }()
    
    private var checkboxView: CheckboxView = {
        let checkboxView = CheckboxView()
        checkboxView.translatesAutoresizingMaskIntoConstraints = false
        return checkboxView
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
        setupCheckboxView()
        setupTextField()
    }
    
    private func setupCheckboxView() {
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
                let model = self.objectInputModel
            else {
                return
            }
            model.isCheck = self.checkboxView.isSelected
        }
    }
    
    private func setupTextField() {
        addSubview(textField)
        
        NSLayoutConstraint.activate([
            textField.widthAnchor.constraint(equalToConstant: 50),
            textField.trailingAnchor.constraint(equalTo: trailingAnchor),
            textField.centerYAnchor.constraint(equalTo: centerYAnchor)
            ])
        
        textField.superview?.bringSubviewToFront(textField)
    }
    
    func configure(model: ObjectInputModel, isShowTextField: Bool = false) {
        self.isShowTextField = isShowTextField
        objectInputModel = model
    }
}

extension CheckboxCollectionViewCell: UITextFieldDelegate {
    func textFieldDidEndEditing(_ textField: UITextField) {
        guard let text = textField.text else { return }
        objectInputModel?.otherValue = text
    }
    
    func textField(_ textField: UITextField, shouldChangeCharactersIn range: NSRange, replacementString string: String) -> Bool {
        guard let text = textField.text else { return false }
        
        let count = text.count + string.count - range.length
        return count <= 4
    }
    
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        textField.resignFirstResponder()
        return true
    }
}
