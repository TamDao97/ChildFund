//
//  MainViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/2/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class AboutViewController: BaseViewController {
    @IBOutlet weak var contentTextView: UITextView!
    
    override func viewDidAppear(_ animated: Bool) {
        super.viewDidAppear(animated)
        contentTextView.setContentOffset(.zero, animated: false)
    }
    
    @IBAction func callSOSAction(_ sender: Any) {
        let url = URL(string: "tel://\(Constants.childSOSNumber)")
        url?.open()
    }
    
    @IBAction func goToFormAction(_ sender: Any) {
        let formViewController = FormViewController.storyboardInstance()
        present(formViewController, animated: false, completion: nil)
    }
}
