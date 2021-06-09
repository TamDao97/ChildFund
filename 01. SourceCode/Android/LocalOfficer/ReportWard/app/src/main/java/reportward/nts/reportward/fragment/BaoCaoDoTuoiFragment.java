package reportward.nts.reportward.fragment;

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
import reportward.nts.reportward.adapter.BaoCaoDoTuoiAdapter;
import reportward.nts.reportward.model.TuoiGioiTinhDanTocModel;

public class BaoCaoDoTuoiFragment extends Fragment {
    private View view;
    private BaoCaoDoTuoiAdapter baoCaoDoTuoiAdapter;
    private RecyclerView recyclerView;
    private List<TuoiGioiTinhDanTocModel> listDataView = new ArrayList<>();

    public static BaoCaoDoTuoiFragment newInstance() {
        BaoCaoDoTuoiFragment fragment = new BaoCaoDoTuoiFragment();
        return fragment;
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_bao_cao_tuoi, container, false);

        initComponent();

        return view;
    }

    private void initComponent() {
        recyclerView = view.findViewById(R.id.listBaoCaoDoTuoi);
        recyclerView.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerView.setHasFixedSize(true);

        baoCaoDoTuoi(listDataView);
    }

    public void baoCaoDoTuoi(List<TuoiGioiTinhDanTocModel> listData) {
        try {
            listDataView = listData;

            baoCaoDoTuoiAdapter = new BaoCaoDoTuoiAdapter(getContext(), listDataView);
            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
            recyclerView.setAdapter(baoCaoDoTuoiAdapter);

        } catch (Exception ex) {
        }
    }
}
