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
import reportward.nts.reportward.adapter.BaoCaoThuPhamTuoiAdapter;
import reportward.nts.reportward.model.BaoCaoThuPhamModel;

public class BaoCaoThuPhamDoTuoiFragment extends Fragment {
    private View view;
    private BaoCaoThuPhamTuoiAdapter baoCaoThuPhamTuoiAdapter;
    private RecyclerView recyclerView;
    private List<BaoCaoThuPhamModel> listDataView = new ArrayList<>();

    public BaoCaoThuPhamDoTuoiFragment() {
    }

    public static BaoCaoThuPhamDoTuoiFragment newInstance() {
        BaoCaoThuPhamDoTuoiFragment fragment = new BaoCaoThuPhamDoTuoiFragment();
        return fragment;
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_bc_thu_pham_tuoi, container, false);

        initComponent();

        return view;
    }

    private void initComponent() {
        recyclerView = view.findViewById(R.id.listBaoCaoDoTuoi);
        recyclerView.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerView.setHasFixedSize(true);

        baoCaoDoTuoi(listDataView);
    }

    public void baoCaoDoTuoi(List<BaoCaoThuPhamModel> listData) {
        try {
            listDataView = listData;

            baoCaoThuPhamTuoiAdapter = new BaoCaoThuPhamTuoiAdapter(getContext(), listDataView);
            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
            recyclerView.setAdapter(baoCaoThuPhamTuoiAdapter);

        } catch (Exception ex) {
        }
    }
}
