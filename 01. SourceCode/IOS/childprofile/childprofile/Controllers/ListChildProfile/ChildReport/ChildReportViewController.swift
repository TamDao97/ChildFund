//
//  ChildReportViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/24/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ChildReportViewController: BaseViewController {

    @IBOutlet weak var reportContentTextView: UITextView!
    @IBOutlet weak var placeHolderLabel: UILabel!
    
    var completion: ((String) -> Void)?
    
    static func show(viewController: UIViewController, completion: @escaping (String) -> Void) {
        let childReportViewController = ChildReportViewController.instance()
        childReportViewController.completion = completion
        viewController.present(childReportViewController, animated: true, completion: nil)
    }
    
    override func setupView() {
        reportContentTextView.delegate = self
        reportContentTextView.becomeFirstResponder()
    }
    
    @IBAction func reportButtonWasTouched(_ sender: Any) {
        guard reportContentTextView.text != "" else {
            self.showMessage(title: Strings.reportContentTitleError, position: .center)
            reportContentTextView.becomeFirstResponder()
            return
        }
        
        completion?(reportContentTextView.text)
        dismiss(animated: true, completion: nil)
    }
    
    @IBAction func closeButtonWasTouched(_ sender: Any) {
        dismiss(animated: true, completion: nil)
    }
}

extension ChildReportViewController: UITextViewDelegate {
    func textViewDidChange(_ textView: UITextView) {
        placeHolderLabel.isHidden = textView.text.count != 0
    }
}
