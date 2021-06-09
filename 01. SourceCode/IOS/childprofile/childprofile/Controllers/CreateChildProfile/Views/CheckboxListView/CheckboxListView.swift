//
//  CheckboxListView.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class CheckboxListView: UIView {
    private var dataSource: [ObjectInputModel] = []
    private var isShowTextField: Bool = false
    
    var collectionView: UICollectionView = {
        let collectionViewFlowLayout = UICollectionViewFlowLayout()
        let collectionView = UICollectionView(frame: .zero, collectionViewLayout: collectionViewFlowLayout)
        collectionView.translatesAutoresizingMaskIntoConstraints = false
        collectionView.backgroundColor = .clear
        collectionView.isScrollEnabled = false
        return collectionView
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
        setupCollectionView()
    }
    
    func setDataSource(_ values: [ObjectInputModel], isShowTextField: Bool = false) {
        self.isShowTextField = isShowTextField
        dataSource = values
        collectionView.reloadData()
    }
}

// MARK: Collection view
extension CheckboxListView {
    private func setupCollectionView() {
        collectionView.dataSource = self
        collectionView.delegate = self
        collectionView.allowsSelection = false
        
        collectionView.register(CheckboxCollectionViewCell.self,
                                forCellWithReuseIdentifier: CheckboxCollectionViewCell.className)
        
        addSubview(collectionView)
        NSLayoutConstraint.activate([
            collectionView.leadingAnchor.constraint(equalTo: leadingAnchor),
            collectionView.trailingAnchor.constraint(equalTo: trailingAnchor),
            collectionView.topAnchor.constraint(equalTo: topAnchor),
            collectionView.bottomAnchor.constraint(equalTo: bottomAnchor)
            ])
    }
}

// MARK: CollectionView datasource
extension CheckboxListView: UICollectionViewDataSource {
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return dataSource.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: CheckboxCollectionViewCell.className,
                                                      for: indexPath) as! CheckboxCollectionViewCell
        cell.configure(model: dataSource[indexPath.row], isShowTextField: isShowTextField)
        return cell
    }
}

// MARK: CollectionView layout delegate
extension CheckboxListView: UICollectionViewDelegateFlowLayout {
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        return CGSize(width: collectionView.bounds.width / 2 - 4, height: 25)
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 12
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumInteritemSpacingForSectionAt section: Int) -> CGFloat {
        return 8
    }
}
