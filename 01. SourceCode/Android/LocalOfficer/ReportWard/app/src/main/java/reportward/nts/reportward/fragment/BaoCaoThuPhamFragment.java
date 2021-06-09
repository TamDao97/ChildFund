package reportward.nts.reportward.fragment;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.view.ViewPager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RelativeLayout;
import android.widget.TextView;

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
import reportward.nts.reportward.adapter.SectionsPagerAdapter;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.BaoCaoThuPhamModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;
import reportward.nts.reportward.model.SearchBaoCaoModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class BaoCaoThuPhamFragment extends Fragment {
    private View view;
    private SectionsPagerAdapter adapter;
    private ViewPager tpViewPager;
    private TabLayout tpTabLayout;
    private SearchBaoCaoModel searchBaoCaoModel;
    private EditText spnYearFrom, spnYearTo;
    private RelativeLayout progressDialog;
    private String urlBaoCao, title;
    private TextView txtTitle;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_bao_cao_thu_pham, container, false);

        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        urlBaoCao = this.getArguments().getString("urlApi", "");
        title = this.getArguments().getString("title", "");

        initComponent();

        baoCaoThuPham();

        // Inflate the layout for this fragment
        return view;
    }

    private void initComponent() {
        spnYearFrom = view.findViewById(R.id.spnYearFrom);
        spnYearTo = view.findViewById(R.id.spnYearTo);
        Button btnSearch = view.findViewById(R.id.btnSearch);
        progressDialog = view.findViewById(R.id.progressDialog);
        txtTitle = view.findViewById(R.id.txtTitle);
        txtTitle.setText(title);

        SimpleDateFormat format = new SimpleDateFormat("dd/MM/yyyy");
        spnYearTo.setText(format.format(new Date()));
        format = new SimpleDateFormat("yyyy");
        spnYearFrom.setText("01/01/" + format.format(new Date()));

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

        searchBaoCaoModel = new SearchBaoCaoModel();
        searchBaoCaoModel.maTinh = loginProfileModel.tinh;
        searchBaoCaoModel.maHuyen = loginProfileModel.quanHuyen;
        searchBaoCaoModel.maXa = loginProfileModel.xaPhuong;

        btnSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                baoCaoThuPham();
            }
        });

        tpViewPager = (ViewPager) view.findViewById(R.id.tpViewPager);
        setupViewPager(tpViewPager);

        tpTabLayout = (TabLayout) view.findViewById(R.id.tpTabLayout);
        tpTabLayout.setupWithViewPager(tpViewPager);
    }

    private void setupViewPager(ViewPager viewPager) {
        adapter = new SectionsPagerAdapter(getActivity().getSupportFragmentManager());
        adapter.addFragment(BaoCaoThuPhamGioiTinhFragment.newInstance(), "Giới tính");
        adapter.addFragment(BaoCaoThuPhamDoTuoiFragment.newInstance(), "Độ tuổi");
        viewPager.setAdapter(adapter);
    }

    private void baoCaoThuPham() {
        progressDialog.setVisibility(View.VISIBLE);
        searchBaoCaoModel.tuNgay = DateUtils.ConvertDMYToYMD(spnYearFrom.getText().toString());
        searchBaoCaoModel.denNgay = DateUtils.ConvertDMYToYMD(spnYearTo.getText().toString());

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(searchBaoCaoModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }

        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,urlBaoCao))
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultModel<List<BaoCaoThuPhamModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<BaoCaoThuPhamModel>>>() {
                            }.getType());

                            ((BaoCaoThuPhamGioiTinhFragment) adapter.getItem(0)).baoCaoGioiTinh(resultObject.data);
                            ((BaoCaoThuPhamDoTuoiFragment) adapter.getItem(1)).baoCaoDoTuoi(resultObject.data);

                            progressDialog.setVisibility(View.GONE);

                        } catch (Exception ex) {
                            progressDialog.setVisibility(View.GONE);
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        progressDialog.setVisibility(View.GONE);
                    }
                });
    }
}
