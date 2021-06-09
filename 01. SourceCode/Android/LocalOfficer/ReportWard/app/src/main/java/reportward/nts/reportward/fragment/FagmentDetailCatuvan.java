package reportward.nts.reportward.fragment;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.text.HtmlCompat;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.AdapterDetaiNghiPham;
import reportward.nts.reportward.adapter.AdapterDetailCatuvan;
import reportward.nts.reportward.adapter.AdapterTimeline;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.CaTuVanResultModel;
import reportward.nts.reportward.model.CaTuVanSearch;
import reportward.nts.reportward.model.ChiTietCaModel;
import reportward.nts.reportward.model.DoiTuongXamHaiViewModel;
import reportward.nts.reportward.model.LienHeChiTietModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;
import reportward.nts.reportward.model.ThongTinTreViewModel;

public class FagmentDetailCatuvan extends Fragment {
    private View view;
    private RecyclerView listChild, listNghiPham, listLanLienHe;
    private AdapterDetailCatuvan adapterDetailCatuvan;
    private AdapterDetaiNghiPham adapterDetaiNghiPham;
    private TextView txtMaHoSo, txtLoaiHoSo, txtCacVanDe, txtNguonThongTin, txtDoiTuong, txtHoTen,
            txtGioiTinh, txtNamSinh, txtDanToc, txtDiaChi, txtSoDienThoai, txtNgayTao, txtNgayCapNhat,
            txtPhanLoaiCa, txtTrangThaiCa;
    public String soHoSo;
    private List<ThongTinTreViewModel> listThongTinTre;
    private List<DoiTuongXamHaiViewModel> listDoiTuongXamHai;
    private TextView txtKhongCoNghiPham, txtKhongCoTre;
    private AdapterTimeline adapterTimeline;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;
    private RelativeLayout progressDialog;
    private int countResponse = 0;


    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_detail_catuvan, null);

        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        InitView();


        if (soHoSo != null) {
            progressDialog.setVisibility(View.VISIBLE);
            countResponse = 0;
            getListChild(soHoSo);
            getListNghiPham(soHoSo);
            getListLanLienHe(soHoSo);

            AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.chitietcatuvanapp + "?maSoHoSo=" + soHoSo))
                    .addHeaders("Authorization", loginProfileModel.accessToken)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            countResponse++;
                            visibilityProgressDialog();
                            try {
                                ResultModel<ChiTietCaModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<ChiTietCaModel>>() {
                                }.getType());
                                //item_total.setText(String.valueOf(resultObject.TotalItem));
                                //set data and list adapter

                                if (resultObject.status.equals("1")) {
                                    setDataInfo(resultObject.data);
                                } else {
                                    Toast.makeText(getActivity(), "Có lỗi phát sinh khi xem hồ sơ!", Toast.LENGTH_SHORT).show();
                                }

                            } catch (Exception ex) {

                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            countResponse++;
                            visibilityProgressDialog();
                        }
                    });
        }
        return view;
    }

    private void InitView() {
        progressDialog = view.findViewById(R.id.progressDialog);
        listChild = view.findViewById(R.id.listTre);
        listChild.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        listChild.setHasFixedSize(true);

        listNghiPham = view.findViewById(R.id.listNghiPham);
        listNghiPham.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        listNghiPham.setHasFixedSize(true);

        listLanLienHe = view.findViewById(R.id.listLanLienHe);
        listLanLienHe.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        listLanLienHe.setHasFixedSize(true);

        txtMaHoSo = view.findViewById(R.id.txtMaHoSo);
        txtLoaiHoSo = view.findViewById(R.id.txtLoaiHoSo);
        txtCacVanDe = view.findViewById(R.id.txtCacVanDe);
        txtNguonThongTin = view.findViewById(R.id.txtNguonThongTin);
        txtDoiTuong = view.findViewById(R.id.txtDoiTuong);
        txtHoTen = view.findViewById(R.id.txtHoTen);
        txtGioiTinh = view.findViewById(R.id.txtGioiTinh);
        txtNamSinh = view.findViewById(R.id.txtNamSinh);
        txtDanToc = view.findViewById(R.id.txtDanToc);
        txtDiaChi = view.findViewById(R.id.txtDiaChi);
        txtSoDienThoai = view.findViewById(R.id.txtSoDienThoai);
        txtNgayTao = view.findViewById(R.id.txtNgayTao);
        txtNgayCapNhat = view.findViewById(R.id.txtNgayCapNhat);
        txtPhanLoaiCa = view.findViewById(R.id.txtPhanLoaiCa);
        txtTrangThaiCa = view.findViewById(R.id.txtTrangThaiCa);

        txtKhongCoNghiPham = view.findViewById(R.id.txtKhongCoNghiPham);
        txtKhongCoTre = view.findViewById(R.id.txtKhongCoTre);
    }

    private void setDataInfo(ChiTietCaModel chiTietCaModel) {
        txtMaHoSo.setText(chiTietCaModel.Id);
        txtLoaiHoSo.setText(chiTietCaModel.LoaiHoSo);
        txtCacVanDe.setText(chiTietCaModel.CacVanDe);
        txtNguonThongTin.setText(chiTietCaModel.NguonThongTin);
        txtDoiTuong.setText(chiTietCaModel.DoiTuong);
        txtHoTen.setText(chiTietCaModel.HoTen);
        txtGioiTinh.setText(chiTietCaModel.GioiTinh);
        txtNamSinh.setText(DateUtils.ConvertMDYServerToDMY( chiTietCaModel.NamSinh));
        txtDanToc.setText(chiTietCaModel.DanToc);
        txtDiaChi.setText(chiTietCaModel.DiaChi);
        txtSoDienThoai.setText(chiTietCaModel.SoDienThoai);
        txtNgayTao.setText(DateUtils.ConvertMDYServerToDMY(chiTietCaModel.NgayTao));
        txtNgayCapNhat.setText(DateUtils.ConvertMDYServerToDMY(chiTietCaModel.NgayCapNhat));
        txtPhanLoaiCa.setText(chiTietCaModel.PhanLoaiCa);
        txtTrangThaiCa.setText(chiTietCaModel.TrangThaiCa);
    }

    private void getListChild(String soHoSo) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsTre) + "?maSoHoSo=" + soHoSo)
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            countResponse++;
                            visibilityProgressDialog();
                            ResultModel<List<ThongTinTreViewModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ThongTinTreViewModel>>>() {
                            }.getType());
                            listThongTinTre = resultObject.data;
                            adapterDetailCatuvan = new AdapterDetailCatuvan(getContext(), listThongTinTre);
                            listChild.setLayoutManager(new LinearLayoutManager(getActivity()));
                            listChild.setAdapter(adapterDetailCatuvan);

                            if (listThongTinTre.size() == 0) {
                                txtKhongCoTre.setVisibility(View.VISIBLE);
                            }
                        } catch (Exception ex) {

                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });
    }

    private void getListNghiPham(String soHoSo) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsDoiTuong) + "?maSoHoSo=" + soHoSo)
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        countResponse++;
                        visibilityProgressDialog();
                        try {
                            ResultModel<List<DoiTuongXamHaiViewModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<DoiTuongXamHaiViewModel>>>() {
                            }.getType());
                            listDoiTuongXamHai = resultObject.data;
                            adapterDetaiNghiPham = new AdapterDetaiNghiPham(getContext(), listDoiTuongXamHai);
                            listNghiPham.setLayoutManager(new LinearLayoutManager(getActivity()));
                            listNghiPham.setAdapter(adapterDetaiNghiPham);

                            if (listDoiTuongXamHai.size() == 0) {
                                txtKhongCoNghiPham.setVisibility(View.VISIBLE);
                            }
                        } catch (Exception ex) {

                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });
    }

    private void getListLanLienHe(String soHoSo) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dslanlienhetheohoso) + "?maSoHoSo=" + soHoSo)
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            countResponse++;
                            visibilityProgressDialog();
                            ResultModel<List<LienHeChiTietModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<LienHeChiTietModel>>>() {
                            }.getType());
                            adapterTimeline = new AdapterTimeline(getContext(), resultObject.data);
                            listLanLienHe.setLayoutManager(new LinearLayoutManager(getActivity()));
                            listLanLienHe.setAdapter(adapterTimeline);
                        } catch (Exception ex) {
                            String temp = ex.getMessage();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                        String temp = anError.getMessage();
                    }
                });
    }

    private void visibilityProgressDialog() {
        if (countResponse == 3) {
            progressDialog.setVisibility(View.GONE);
        }
    }
}
