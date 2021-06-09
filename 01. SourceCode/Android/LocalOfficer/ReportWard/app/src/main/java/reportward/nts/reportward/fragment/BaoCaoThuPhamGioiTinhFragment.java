package reportward.nts.reportward.fragment;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.BaoCaoGioiTinhAdapter;
import reportward.nts.reportward.adapter.BaoCaoThuPhamGioiTinhAdapter;
import reportward.nts.reportward.model.BaoCaoThuPhamModel;

public class BaoCaoThuPhamGioiTinhFragment extends Fragment {
    private View view;
    private BaoCaoThuPhamGioiTinhAdapter baoCaoThuPhamGioiTinhAdapter;
    private RecyclerView recyclerView;
    private List<BaoCaoThuPhamModel> listDataView = new ArrayList<>();

    public BaoCaoThuPhamGioiTinhFragment() {
    }

    public static BaoCaoThuPhamGioiTinhFragment newInstance() {
        BaoCaoThuPhamGioiTinhFragment fragment = new BaoCaoThuPhamGioiTinhFragment();
        return fragment;
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_bc_thu_pham_gioi_tinh, container, false);

        initComponent();

        return view;
    }

    private void initComponent() {
        recyclerView = view.findViewById(R.id.listBaoCaoGioiTinh);
        recyclerView.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerView.setHasFixedSize(true);

        baoCaoGioiTinh(listDataView);
    }

    public void baoCaoGioiTinh(List<BaoCaoThuPhamModel> listData) {
        try {
            listDataView = listData;
            baoCaoThuPhamGioiTinhAdapter = new BaoCaoThuPhamGioiTinhAdapter(getContext(), listDataView);
            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
            recyclerView.setAdapter(baoCaoThuPhamGioiTinhAdapter);

        } catch (Exception ex) {
        }
    }
}
