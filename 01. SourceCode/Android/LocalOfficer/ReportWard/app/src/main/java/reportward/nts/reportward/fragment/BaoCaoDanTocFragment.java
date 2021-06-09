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
import android.widget.Button;
import android.widget.EditText;
import android.widget.RelativeLayout;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.BaoCaoDanTocAdapter;
import reportward.nts.reportward.model.TuoiGioiTinhDanTocModel;

public class BaoCaoDanTocFragment extends Fragment {
    private View view;
    private BaoCaoDanTocAdapter baoCaoDanTocAdapter;
    private RecyclerView recyclerView;
    private List<TuoiGioiTinhDanTocModel> listDataView = new ArrayList<>();

    public static BaoCaoDanTocFragment newInstance() {
        BaoCaoDanTocFragment fragment = new BaoCaoDanTocFragment();
        return fragment;
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_bao_cao_dan_toc, container, false);

        initComponent();

        return view;
    }

    private void initComponent() {
        recyclerView = view.findViewById(R.id.listBaoCaoDanToc);
        recyclerView.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerView.setHasFixedSize(true);

        baoCaoDanToc(listDataView);
    }

    public void baoCaoDanToc(List<TuoiGioiTinhDanTocModel> listData) {
        try {
            listDataView = listData;
            baoCaoDanTocAdapter = new BaoCaoDanTocAdapter(getContext(), listDataView);
            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
            recyclerView.setAdapter(baoCaoDanTocAdapter);

        } catch (Exception ex) {
        }
    }
}
