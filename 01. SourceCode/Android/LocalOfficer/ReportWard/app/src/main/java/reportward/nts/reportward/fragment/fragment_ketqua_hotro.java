package reportward.nts.reportward.fragment;

import android.app.Dialog;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
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
import reportward.nts.reportward.adapter.AdapterKetQuaHoTro;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.KetQuaHoTroModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;
import reportward.nts.reportward.model.SearchBaoCaoModel;

public class fragment_ketqua_hotro extends Fragment {
    private View view;
    private AdapterKetQuaHoTro adapterKetQuaHoTro;
    private List<KetQuaHoTroModel> listKetQuaHoTro;
    private RecyclerView recyclerView;
    private EditText spnYearFrom, spnYearTo;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_ketqua_hotro, container, false);
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

        Button btnTimKiemKetQuaHoTro = view.findViewById(R.id.btnTimKiemKetQuaHoTro);
        btnTimKiemKetQuaHoTro.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                InitView();
            }
        });

        return view;
    }

    private void InitView() {

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


        recyclerView = view.findViewById(R.id.listKetQuaHoTro);
        recyclerView.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerView.setHasFixedSize(true);

        SearchBaoCaoModel searchBaoCaoModel = new SearchBaoCaoModel();
        searchBaoCaoModel.maTinh = loginProfileModel.tinh;
        searchBaoCaoModel.maHuyen = loginProfileModel.quanHuyen;
        searchBaoCaoModel.maXa = loginProfileModel.xaPhuong;
        searchBaoCaoModel.tuNgay = DateUtils.ConvertDMYToYMD(spnYearFrom.getText().toString());
        searchBaoCaoModel.denNgay = DateUtils.ConvertDMYToYMD(spnYearTo.getText().toString());

        getResultHelp(searchBaoCaoModel);
    }

    private void showDialogDetailResult(KetQuaHoTroModel ketQuaHoTroModel) {
        final Dialog dialog = new Dialog(getContext());
        dialog.setContentView(R.layout.popup_detail_ketquahotro);
        dialog.setTitle(ketQuaHoTroModel.doiTuong);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;

        TextView txtDoiTuongTre = dialog.findViewById(R.id.txtDoiTuongTre);
        txtDoiTuongTre.setText(ketQuaHoTroModel.doiTuong);

        TextView txtCoHoTroChamSoc = dialog.findViewById(R.id.txtCoHoTroChamSoc);
        txtCoHoTroChamSoc.setText(ketQuaHoTroModel.coHoTroChamSocSoLuong);

        TextView txtCoHoTroXaHoi = dialog.findViewById(R.id.txtCoHoTroXaHoi);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.coHoTroTroGiupXaHoiSoLuong);

        TextView txtCoHoTroTamLy = dialog.findViewById(R.id.txtCoHoTroTamLy);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.coHoTroTroGiupTamLySoLuong);

        TextView txtHoTroPhapLy = dialog.findViewById(R.id.txtHoTroPhapLy);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.coHoTroTroGiupPhapLySoLuong);

        TextView txtHoTroGiaoDuc = dialog.findViewById(R.id.txtHoTroGiaoDuc);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.coHoTroTroGiupGiaoDucSoLuong);

        TextView txtHoTroTaiChinh = dialog.findViewById(R.id.txtHoTroTaiChinh);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.coHoTroTroGiupTaiChinhSoLuong);

        TextView txtHoTroKhac = dialog.findViewById(R.id.txtHoTroKhac);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.coHoTroTroGiupKhacSoLuong);

        TextView txtGiaDinhKhongMuon = dialog.findViewById(R.id.txtGiaDinhKhongMuon);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.koHoTroGiaDinhKhongMuonSoLuong);

        TextView txtNanNhanChuyenDi = dialog.findViewById(R.id.txtNanNhanChuyenDi);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.koHoTroNanNhanChuyenDiSoLuong);

        TextView txtKhongHoTroKhac = dialog.findViewById(R.id.txtKhongHoTroKhac);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.koHoTroKhacSoLuong);

        TextView txtXacMinhKhongCo = dialog.findViewById(R.id.txtXacMinhKhongCo);
        txtCoHoTroXaHoi.setText(ketQuaHoTroModel.xacMinhKhongCoSoLuong);


        dialog.show();
    }

    private void getResultHelp(SearchBaoCaoModel searchBaoCaoModel) {
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(searchBaoCaoModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }

        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.ketQuaHoTro))
                .addHeaders("Authorization",loginProfileModel.accessToken)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultModel<List<KetQuaHoTroModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<KetQuaHoTroModel>>>() {
                            }.getType());
                            listKetQuaHoTro = resultObject.data;

                            adapterKetQuaHoTro = new AdapterKetQuaHoTro(getContext(), listKetQuaHoTro);
                            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
                            recyclerView.setAdapter(adapterKetQuaHoTro);

                            adapterKetQuaHoTro.SetOnItemClickListener(new AdapterKetQuaHoTro.OnItemClickListener() {
                                @Override
                                public void showDialogDetail(View view, int position, KetQuaHoTroModel obj) {
                                    showDialogDetailResult(obj);
                                }
                            });


                        } catch (Exception ex) {

                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }
}
