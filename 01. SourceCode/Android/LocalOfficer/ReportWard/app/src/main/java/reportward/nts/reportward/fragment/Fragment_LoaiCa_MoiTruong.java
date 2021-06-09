package reportward.nts.reportward.fragment;

import android.content.SharedPreferences;
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

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.AdapterLoaiCaMoiTruong;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.LoaiCaMoiTruongModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;
import reportward.nts.reportward.model.SearchBaoCaoModel;

public class Fragment_LoaiCa_MoiTruong extends Fragment {
    private View view;
    private RecyclerView recyclerView;
    private AdapterLoaiCaMoiTruong adapterLoaiCaMoiTruong;
    List<LoaiCaMoiTruongModel> listLoaiCaMoiTruong;
    private Button btnTimKiemLoaiCaMoiTruong;
    private EditText spnYearFrom, spnYearTo;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_loaica_moitruong, container, false);
        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }
        spnYearFrom = view.findViewById(R.id.spnYearFrom);
        spnYearTo = view.findViewById(R.id.spnYearTo);

        SimpleDateFormat format = new SimpleDateFormat("dd/MM/yyyy");
        spnYearTo.setText(format.format(new Date()));
        format = new SimpleDateFormat("yyyy");
        spnYearFrom.setText("01/01/" + format.format(new Date()));
        InitView();
        btnTimKiemLoaiCaMoiTruong = view.findViewById(R.id.btnTimKiemLoaiCaMoiTruong);
        btnTimKiemLoaiCaMoiTruong.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                InitView();
            }
        });
        return view;
    }

    private void InitView() {
        recyclerView = view.findViewById(R.id.listLoaiCaMoiTruong);
        recyclerView.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerView.setHasFixedSize(true);


        spnYearFrom.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) view);
            }
        });

        spnYearTo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) view);
            }
        });

        SearchBaoCaoModel searchBaoCaoModel = new SearchBaoCaoModel();
        searchBaoCaoModel.maTinh = loginProfileModel.tinh;
        //searchBaoCaoModel.maHuyen = global.loginProfileModel.quanHuyen;
        //searchBaoCaoModel.maXa = global.loginProfileModel.xaPhuong;
        searchBaoCaoModel.tuNgay = DateUtils.ConvertDMYToYMD(spnYearFrom.getText().toString());
        searchBaoCaoModel.denNgay = DateUtils.ConvertDMYToYMD(spnYearTo.getText().toString());
        getLoaiCaMoiTruong(searchBaoCaoModel);
    }

    private void getLoaiCaMoiTruong(SearchBaoCaoModel searchBaoCaoModel) {
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(searchBaoCaoModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }

        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.loaiCaMoiTruong))
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultModel<List<LoaiCaMoiTruongModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<LoaiCaMoiTruongModel>>>() {
                            }.getType());
                            listLoaiCaMoiTruong = resultObject.data;

                            adapterLoaiCaMoiTruong = new AdapterLoaiCaMoiTruong(getContext(), listLoaiCaMoiTruong);
                            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
                            recyclerView.setAdapter(adapterLoaiCaMoiTruong);
                        } catch (Exception ex) {

                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }
}
