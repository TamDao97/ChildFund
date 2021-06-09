package reportward.nts.reportward.fragment;

import android.content.Intent;
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
import java.util.Date;
import java.util.List;

import reportward.nts.reportward.LoginActivity;
import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.BaoCaoDiaBanAdapter;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.BaoCaoDiaBanModel;
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;
import reportward.nts.reportward.model.SearchBaoCaoModel;

public class BaoCaoDiabanFragment extends Fragment {
    private View view;
    private BaoCaoDiaBanAdapter baoCaoDiaBanAdapter;
    private List<BaoCaoDiaBanModel> listDiaBan;
    private RecyclerView recyclerView;
    private SearchBaoCaoModel searchBaoCaoModel;
    private EditText spnYearFrom, spnYearTo;
    private RelativeLayout progressDialog;
    private String[] arrayTinh, arrayHuyen, arrayXa;
    private List<ComboboxResult> listTinh, listHuyen, listXa;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;


    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_bao_cao_dia_ban, container, false);

        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        initComponent();

        baoCaoDiaBanTre();

        return view;
    }

    private void initComponent() {
        spnYearFrom = view.findViewById(R.id.spnYearFrom);
        spnYearTo = view.findViewById(R.id.spnYearTo);
        Button btnSearch = view.findViewById(R.id.btnSearch);
        progressDialog = view.findViewById(R.id.progressDialog);

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


        recyclerView = view.findViewById(R.id.listBaoCaoDiaBan);
        recyclerView.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerView.setHasFixedSize(true);

        searchBaoCaoModel = new SearchBaoCaoModel();
        searchBaoCaoModel.maTinh = loginProfileModel.tinh;
        searchBaoCaoModel.maHuyen = loginProfileModel.quanHuyen;
        searchBaoCaoModel.maXa = loginProfileModel.xaPhuong;

        btnSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                baoCaoDiaBanTre();
            }
        });
    }

//    /***
//     * Lấy dữ liệu cho combobox
//     * @param authorizationKey
//     */
//    private void getDataCombobox(String authorizationKey) {
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dsVanDe))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        String tamp = "";
//                        ResultModel<List<ItemObjectInSubModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ItemObjectInSubModel>>>() {
//                        }.getType());
//                        listVanDe = resultObject.data;
//
//                        vanDeListAdapter = new VanDeListAdapter(getActivity(), listVanDe, listVanDeChooseTemp);
//                        rvVanDe.setAdapter(vanDeListAdapter);
//                        vanDeListAdapter.SetOnItemClickListener(new VanDeListAdapter.OnItemClickListener() {
//                            @Override
//                            public void onItemClick(View view, int position, ItemObjectInSubModel childObjectTypeModel) {
//
//                            }
//                        });
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dsDoiTuongGoi))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listDoiTuong = resultObject.data;
//
//                        arrayDoiTuong = new String[listDoiTuong.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listDoiTuong) {
//                            arrayDoiTuong[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dsloaihoso))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listLoaiCa = resultObject.data;
//
//                        arrayLoaiCa = new String[listLoaiCa.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listLoaiCa) {
//                            if (index == 0) {
//                                spnLoaiCa.setTag(item.id);
//                                spnLoaiCa.setText(item.text);
//                            }
//                            arrayLoaiCa[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dsnguonthongtin))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listNguonThongTin = resultObject.data;
//
//                        arrayNguonThongTin = new String[listNguonThongTin.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listNguonThongTin) {
//                            arrayNguonThongTin[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dsDanToc))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listDanToc = resultObject.data;
//
//                        arrayDanToc = new String[listDanToc.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listDanToc) {
//                            arrayDanToc[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dsphanloaihoso))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listPhanLoaiHoSo = resultObject.data;
//
//                        arrayPhanLoaiHoSo = new String[listPhanLoaiHoSo.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listPhanLoaiHoSo) {
//                            if (index == 1) {
//                                spnPhanLoaiHoSo.setTag(item.id);
//                                spnPhanLoaiHoSo.setText(item.text);
//                            }
//
//                            arrayPhanLoaiHoSo[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dsmoitruongxamhai))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listMoiTruongXamHai = resultObject.data;
//
//                        arrayMoiTruongXamHai = new String[listMoiTruongXamHai.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listMoiTruongXamHai) {
//                            arrayMoiTruongXamHai[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dstrangthaihoso))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listTrangThaiHoSo = resultObject.data;
//
//                        arrayTrangThaiHoSo = new String[listTrangThaiHoSo.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listTrangThaiHoSo) {
//                            if (index == 0) {
//                                spnTrangThaiHoSo.setTag(item.id);
//                                spnTrangThaiHoSo.setText(item.text);
//                            }
//                            arrayTrangThaiHoSo[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.dstinhtp))
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listTinh = resultObject.data;
//
//                        arrayTinh = new String[listTinh.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listTinh) {
//                            if (item.id.equals(global.loginProfileModel.tinh)) {
//                                spnTinhNguoiGoi.setText(item.text);
//                                spnTinhNguoiGoi.setTag(item.id);
//                            }
//                            arrayTinh[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//
//        getDataDistrict(authorizationKey, global.loginProfileModel.tinh);
//
//        getDataWrap(authorizationKey, global.loginProfileModel.quanHuyen);
//    }
//
//    Runnable runnableDataDistrict = new Runnable() {
//        @Override
//        public void run() {
//            getDataDistrict(global.loginProfileModel.accessToken, spnTinhNguoiGoi.getTag().toString());
//            spnHuyenNguoiGoi.setTag("");
//            spnHuyenNguoiGoi.setText("");
//            spnXaNguoiGoi.setTag("");
//            spnXaNguoiGoi.setText("");
//        }
//    };
//
//    private void getDataDistrict(String authorizationKey, String tinhId) {
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.DSQuanHuyen) + "?maTinh=" + tinhId)
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listHuyen = resultObject.data;
//
//                        arrayHuyen = new String[listHuyen.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listHuyen) {
//                            if (item.id.equals(global.loginProfileModel.quanHuyen)) {
//                                spnHuyenNguoiGoi.setText(item.text);
//                                spnHuyenNguoiGoi.setTag(item.id);
//                            }
//                            arrayHuyen[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//    }
//
//    Runnable runnableDataWrap = new Runnable() {
//        @Override
//        public void run() {
//            getDataWrap(global.loginProfileModel.accessToken, spnHuyenNguoiGoi.getTag().toString());
//            spnXaNguoiGoi.setTag("");
//            spnXaNguoiGoi.setText("");
//        }
//    };
//
//    private void getDataWrap(String authorizationKey, String huyenId) {
//        AndroidNetworking.get(Utils.GetUrlApi(LinkApi.DSPhuongXa) + "?maHuyen=" + huyenId)
//                .addHeaders("Authorization", authorizationKey)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsJSONObject(new JSONObjectRequestListener() {
//                    @Override
//                    public void onResponse(JSONObject response) {
//                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
//                        }.getType());
//                        listXa = resultObject.data;
//
//                        arrayXa = new String[listXa.size()];
//                        int index = 0;
//                        for (ComboboxResult item : listXa) {
//                            if (item.id.equals(global.loginProfileModel.xaPhuong)) {
//                                spnXaNguoiGoi.setText(item.text);
//                                spnXaNguoiGoi.setTag(item.id);
//                            }
//                            arrayXa[index] = item.text;
//                            index++;
//                        }
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                    }
//                });
//    }

    private void baoCaoDiaBanTre() {
        progressDialog.setVisibility(View.VISIBLE);
        searchBaoCaoModel.tuNgay = DateUtils.ConvertDMYToYMD(spnYearFrom.getText().toString());
        searchBaoCaoModel.denNgay = DateUtils.ConvertDMYToYMD(spnYearTo.getText().toString());

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(searchBaoCaoModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }

        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.diabantre))
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultModel<List<BaoCaoDiaBanModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<BaoCaoDiaBanModel>>>() {
                            }.getType());
                            listDiaBan = resultObject.data;

                            baoCaoDiaBanAdapter = new BaoCaoDiaBanAdapter(getContext(), listDiaBan);
                            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
                            recyclerView.setAdapter(baoCaoDiaBanAdapter);
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
